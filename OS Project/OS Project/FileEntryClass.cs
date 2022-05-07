using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OS_Project
{
    public class FileEntryClass : DirectoryEntry
    {
        public string FileContent;
        public Directory Parent;
        public int Last_index;
        public int Fat_index;
        public FileEntryClass(char[] fileName, byte fileAttribute, int firstCluster, int fileSize, Directory Parent, string FileContent) : base(fileName, fileAttribute, firstCluster, fileSize)
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
            byte[] file = Encoding.ASCII.GetBytes(FileContent);
            for (int i = 0; i < FileContent.Length; i++)
            {
                Content[i] = file[i]; //////////////*********!!!!!!!!!!
            }

            int Number_OF_Required_Blocks = (int)Math.Ceiling(Content.Length / 1024.0m);
            int Number_of_full_Size_Block = (int)Math.Floor(Content.Length / 1024.0m);
            int Number_of_Reminder_Data = ((int)Content.Length % 1024);


            if (Number_OF_Required_Blocks <= FatTable.getAvailableBlocks())
            {
                List<byte[]> l = new List<byte[]>();
                if (Number_of_full_Size_Block > 0)
                {
                    int count = 0;
                    for (int j = 0; j < Number_of_full_Size_Block; j++)
                    {
                        byte[] list = new byte[1024];
                        for (int i = 0; i < 1024; i++)
                        {
                            list[i] = Content[count];
                            count++;
                        }
                        l.Add(list);
                    }
                }
                if (Number_of_Reminder_Data > 0)
                {
                    int ic = 1024 * Number_of_full_Size_Block;  
                    byte[] list = new byte[1024];
                    for (int i = 0; i < Number_of_Reminder_Data; i++)
                    {
                        list[i] = Content[ic];
                        ic++;
                    }
                    l.Add(list);
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

                for (int i = 0; i < l.Count; i++)
                {
                    VirtualDisk.WriteBlock(l[i], Fat_index);
                    FatTable.setNext(-1, Fat_index);
                    if (Last_index != -1)
                    {
                        FatTable.setNext(Last_index, Fat_index);
                    }
                    FatTable.WriteFatTable();
                    Last_index = Fat_index;
                    Fat_index = FatTable.getAvailableBlock();
                }
            }
            FatTable.WriteFatTable();
        }
        public string ReadFileContent()
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
                        string s = new string(Encoding.Default.GetString(d));
                        FileContent += s;
                    }
                }
            }
            return FileContent;
        }
        public void DeleteFile(string fileName)
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
                    int indexParent = Parent.Search(fileName);
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
