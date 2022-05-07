using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OS_Project
{
    public static class FatTable
    {
        public static int[] arr;
        public static string FileName = "OS.txt";
       static FatTable()
        {
            arr = new int[1024];
            arr[0] = -1; 
            arr[1] = 2;  
            arr[2] = 3;
            arr[3] = 4;
            arr[4] = -1;

            for (int i = 5; i < 1024; i++)
            {
                arr[i] = 0;
            }
        }
  

        public static void WriteFatTable()
        {
            using (FileStream file = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var seek = file.Seek(1024, SeekOrigin.Begin);
                var arrayAfterEdit = new byte[1024 * 4];
                Buffer.BlockCopy(arr, 0, arrayAfterEdit, 0, 1024);
                file.Write(arrayAfterEdit);
                file.Close();
            }
        }
        public static int[] GetFatTable()
        {
            using (FileStream file = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
            {
                file.Seek(1024, SeekOrigin.Begin);
                byte[] buffer = new byte[4096];
                file.Read(buffer, 0, buffer.Length);
                int[] retArr = new int[1024];
                Buffer.BlockCopy(buffer, 0, retArr, 0, buffer.Length);
                file.Close();
                return retArr;
            }
        }
        public static int getAvailableBlocks()
        {
            int counter = 0;
            for (int i = 0; i < GetFatTable().Length; i++)
            {
                if ((GetFatTable()[i]) == 0)
                {
                    counter++;
                }
            }
            return counter;
        }
        public static int getAvailableBlock()
        {
            for (int i = 0; i < GetFatTable().Length; i++)
            {
                if ((GetFatTable()[i]) == 0)
                {
                    return i;
                }
            }
            return 1;
        }

        public static int GetNext(int index)
        {
            int NextValue;
            for (int i = 0; i < GetFatTable().Length; i++)
            {
                if (i == index)
                {
                    NextValue = GetFatTable()[i];
                    return NextValue;
                }
            }
            return 0;
        }
        public static void setNext(int value, int Index) 
        {
            int[] NotBlocks = new int[5] { 0, 1, 2, 3, 4 };
            if (Index < 0)
            {
                
            }
            else
            {
                for (int i = 0; i < GetFatTable().Length; i++)
                {

                    if (Index != NotBlocks[0] && Index != NotBlocks[1] && Index != NotBlocks[2] && Index != NotBlocks[3] && Index != NotBlocks[4])
                    {
                        if (i == Index)
                        {
                            arr[i] = value;
                            WriteFatTable();
                            GetFatTable();
                        }
                    }
                    else
                    {

                        break;
                    }
                }
            }
        }
        static public int FreeSpaces()
        {
            int free = FatTable.getAvailableBlocks() * 1024;
            return free;
        }
    }
}
