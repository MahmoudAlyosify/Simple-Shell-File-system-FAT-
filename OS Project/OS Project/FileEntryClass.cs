using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS_Project
{
    class FileEntryClass : directoryEntry   //بتاعة الدايريكتوري كده ناقص نعمل ال ميسودس
    {
        public string FileContent="";

        directory Parent;

        public int Last_index = -1;

        public int Fat_index;
        public FileEntryClass(char[] fileName, byte fileAttribute, int firstCluster,int filesize , directory Parent, string FileContent) : base(fileName, fileAttribute, firstCluster,filesize)
        {
            this.FileContent = FileContent;
            if (Parent != null)
            {
                this.Parent = Parent;
            }
        }


        public void WriteFileContent()
        {
            byte[] Content = new byte[FileContent.Length];

            for (int i = 0; i < FileContent.Length; i++)
            {
                Content[i] = (byte)FileContent[i];
            }

            int Number_OF_Required_Blocks = (int)Math.Ceiling(Content.Length / 1024.0m);

            int Number_of_full_Size_Block = (int)Math.Floor(Content.Length / 1024.0m);

            int Number_of_Reminder_Data = ((int)Content.Length % 1024);


            if (Number_OF_Required_Blocks <= FatTable.getAvailableBlocks())
            {
                int count = 0;

                List<byte[]> list1 = new List<byte[]>();

                for (int j = 0; j < Number_of_full_Size_Block; j++)
                {
                    byte[] list = new byte[1024];
                    for (int i = 0; i < 1024; i++)
                    {
                        list[i] = Content[count];
                        count++;
                    }

                    list1.Add(list);//حطيت الكونينت في ليست
                }


                if (Number_of_Reminder_Data > 0)
                {
                    byte[] list = new byte[1024];
                    for (int i = 0; i < Number_of_Reminder_Data; i++)
                    {
                        list[i] = Content[count];
                        count++;
                    }
                    list1.Add(list);
                }



                if (firstCluster != 0)  
                {
                    Fat_index = firstCluster;
                }
                else
                {
                    Fat_index = FatTable.getAvailableBlock();
                    firstCluster = Fat_index;
                }

                for (int i = 0; i < list1.Count; i++)
                {

                    VirtualDisk.WriteBlock(list1[i], Fat_index);
                    FatTable.setNext(-1, Fat_index);
                }

                if (Last_index != -1)
                {
                    FatTable.setNext(Last_index, Fat_index);
                }

                Last_index = Fat_index;
                Fat_index = FatTable.getAvailableBlock();
            }
            FatTable.WriteFatTable();
        }


        public void ReadFileContent()
        {
            if (firstCluster != 0 && FatTable.GetNext(firstCluster) != 0)
            {
                // List<directoryEntry> DT = new List<directoryEntry>();
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
                        FileContent += (d).ToString();
                    } 
                }
            }
        }

       

        public void DeleteFile()
        {
            if (firstCluster != 0)
            {
                int Fat_index = firstCluster;
                int next = FatTable.GetNext(Fat_index);
                do
                {
                    FatTable.setNext(Fat_index, 0);
                    Fat_index = next;
                    if (Fat_index != -1)
                    {
                        next = FatTable.GetNext(Fat_index);
                    }

                } while (next != -1);
                if (Parent != null)
                {

                    Parent.Readdirectory();

                    Fat_index = Parent.Search(Convert.ToString(Parent.FileName));


                    if (Fat_index != -1)
                    {
                        // parent.Directory_table.RemoveAt(Fat_index);
                        Parent.directoryTable.RemoveAt(Fat_index);
                        Parent.Write_directory();
                    }

                    FatTable.WriteFatTable();
                }

            }
        }
    }
}

