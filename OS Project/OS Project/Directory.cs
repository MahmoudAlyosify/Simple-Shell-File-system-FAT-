using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace OS_Project
{
    public class Directory : DirectoryEntry
    {
        public List<DirectoryEntry> DirectoryTable;
        public Directory Parent;
        public int Fat_index;
        public Directory(char[] fileName, byte fileAttribute, int firstCluster,int fileSize, Directory Parent) : base(fileName, fileAttribute, firstCluster, fileSize)
        {

            DirectoryTable = new List<DirectoryEntry>();
            if (Parent != null)
            {
                this.Parent = Parent;
            }

        }
        public void printTable()
        {
            for (int i=0; i< DirectoryTable.Count; i++)
            {
                Console.WriteLine(DirectoryTable[i]);
                Console.WriteLine(DirectoryTable.Count);
            }
        }
        public void Write_Directory()
        {   
                byte[] Directory_table_bytes = new byte[32 * DirectoryTable.Count];//بعد كل ده كل البيانات متخزنى هنا
                byte[] Dircetory_entry_byte = new byte[32];
                for (int i = 0; i < DirectoryTable.Count; i++)
                {
                    Dircetory_entry_byte = DirectoryTable[i].getByte();

                    for (int j = i * 32, c = 0; c < 32; c++, j++)
                    {
                        Directory_table_bytes[j] = Dircetory_entry_byte[c];  //بيخزن كل الداتا بتاعت الانترى جوه الاراى
                    }
                }

                int Number_OF_Required_Blocks = (int)Math.Ceiling(Directory_table_bytes.Length / 1024.0m);
                int Number_of_full_Size_Block = (int)Math.Floor(Directory_table_bytes.Length / 1024.0m); //(لازم يتعمل كده علشان الفلور بتأخد نوعين داتا تايب (ديسمال) و (فلوت 


                int Number_of_Reminder_Data = ((int)Directory_table_bytes.Length % 1024);
                int Last_index = -1;
            if (Number_OF_Required_Blocks <= FatTable.getAvailableBlocks())
            {
                List<byte[]> Directory_Table_Byte = new List<byte[]>();
                int count = 0;
                for (int j = 0; j < Number_of_full_Size_Block; j++)
                {
                    byte[] list = new byte[1024];
                    for (int i = 0; i < 1024; i++)
                    {
                        list[i] = Directory_table_bytes[count];
                        count++;
                    }
                    Directory_Table_Byte.Add(list);
                }
                if (Number_of_Reminder_Data > 0)
                {
                    byte[] list2 = new byte[1024];
                    int ic = (1024 * Number_of_full_Size_Block);
                    for (int i = 0; i < Number_of_Reminder_Data; i++)
                    {
                        list2[i] = Directory_table_bytes[ic];
                        ic++;
                    }

                    Directory_Table_Byte.Add(list2);
                }
                if (firstCluster != 0)  //المفروض هستدعى الفيرست كلاستر ازاى؟؟
                {
                    Fat_index = firstCluster;
                }
                else
                {
                    Fat_index = FatTable.getAvailableBlock();
                    firstCluster = Fat_index;
                }

                for (int i = 0; i < Directory_Table_Byte.Count; i++)
                {
                    VirtualDisk.WriteBlock(Directory_Table_Byte[i], Fat_index);
                    FatTable.setNext(-1, Fat_index);

                    if (Last_index != -1)
                    {
                        FatTable.setNext(Last_index, Fat_index);  ///!!!!!**
                    }
                    FatTable.WriteFatTable();
                    Last_index = Fat_index;
                    Fat_index = FatTable.getAvailableBlock();
                }
                //if (Last_index != -1)
                //{
                //    FatTable.setNext(Last_index, Fat_index);  ///!!!!!**
                //}
                //FatTable.WriteFatTable();
                //Last_index = Fat_index;
                //Fat_index = FatTable.getAvailableBlock();
            }
                FatTable.WriteFatTable();
            
        }

        public void ReadDirectory()
        {
            if (firstCluster != 0 && FatTable.GetNext(firstCluster) != 0)
            {
                Fat_index = firstCluster;
                int next;
                next = FatTable.GetNext(Fat_index);
                List<byte> ls = new List<byte>();
                do
                {
                    ls.AddRange(VirtualDisk.GetBlock(Fat_index));
                    Fat_index = next;
                    if (Fat_index != -1)
                    {
                        next = FatTable.GetNext(Fat_index);
                    }
                } while (next != -1);
            
                byte[] d = new byte[32];
            for (int i = 0; i < ls.Count; i++)
            {
                d[i % 32] = ls[i];
                if ((i + 1) % 32 == 0)
                {
                    DirectoryTable.Add(GetDirectoryEntry(d));
                }
            }
            }
        }

        public int Search(string FileName)
        {
            ReadDirectory();
            string s = new string(FileName);
            if (FileName.Length < 11)
            {
                for (int i = FileName.Length; i < 11; i++)
                    s += " ";
            }
            FileName = s;
            //int x = s.Length;

            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                string ss= new string(DirectoryTable[i].FileName);
                if (FileName.Equals(ss) )
                {
                    return i;
                }
            }
            return -1;
        }


        public void UpdateContent(DirectoryEntry d)
        {
            ReadDirectory();
            int index;
            index = Parent.Search(d.FileName.ToString());
            if (index != -1)
            {
                DirectoryTable.RemoveAt(index);
                DirectoryTable.Insert(index, d);
            }
        }
        public void DeleteDirectory(string FileDelete)
        {
            if (firstCluster != 0)
            {
                int index = firstCluster;
                int next = FatTable.GetNext(index);
                do
                {
                    FatTable.setNext(0, index);
                    index = next;
                    if (index != -1)
                    {
                        next = FatTable.GetNext(index);
                    }
                } while (index != -1);
            }
                if (Parent != null)
                {
                    Parent.ReadDirectory();
                    int indexParent = Parent.Search(FileDelete);
                    if (indexParent != -1)
                    {
                        Parent.DirectoryTable.RemoveAt(indexParent);
                        Parent.Write_Directory();
                    }
                }
            FatTable.WriteFatTable();
        }
    }
}
