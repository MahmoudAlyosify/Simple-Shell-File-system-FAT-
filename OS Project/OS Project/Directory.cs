using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace OS_Project
{
    
    public class directory : directoryEntry
    {

        public directory()
        {

        }
        public List<directoryEntry> directoryTable;

        public directory Parent;

        public int Fat_index;
        public directory(char[] file_Name, byte File_Attribute, int file_first_Cluster, int FileSize, directory Parent) : base(file_Name, File_Attribute, file_first_Cluster, FileSize)
        {

            if (Parent != null)
            {
                this.Parent = Parent;
            }
            directoryTable = new List<directoryEntry>();
        }

        public byte[] Direcctory_Entry_Byte = new byte[32];
        //static public List<Director
        public void Write_directory()
        {
            {
                if (firstCluster != 0)
                {
                   //بعد كل ده كل البيانات متخزنى هنا
                    byte[] Directory_table_bytes = new byte[32 * directoryTable.Count];

                    byte[] Dircetory_entry_byte = new byte[32];

                    for (int i = 0; i < directoryTable.Count; i++)
                    {
                        Dircetory_entry_byte = directoryTable[i].getByte();

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

                            int StartAfterFullsizeBLOCk = (1024 * Number_of_full_Size_Block); //هنا ضربها ف 1024 يعني مثلا لو هخزن حاجه 2000 ف هياخد في الفور لو الي فوق اول 1024 وهيكمل في دي ف هيبدا من بعد اول 1024 وهيكمل نخزين عادي

                            for (int i = 0; i < Number_of_Reminder_Data; i++)
                            {
                                list2[i] = Directory_table_bytes[StartAfterFullsizeBLOCk];
                                StartAfterFullsizeBLOCk++;
                            }
                            Directory_Table_Byte.Add(list2);
                        }

                        if (firstCluster != 0)
                        {
                            Fat_index = firstCluster;
                        }
                        else
                        {
                            Fat_index = FatTable.getAvailableBlock();//first empty block
                            firstCluster = Fat_index;
                        }

                        for (int i = 0; i < Directory_Table_Byte.Count; i++)
                        {
                            VirtualDisk.WriteBlock(Directory_Table_Byte[i], Fat_index);//في حالة ان دي اخر كلاستر هكتب فيها يبقي الي بعدها بتساوي -1
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
            }
        }

        public void Readdirectory()
        {
            if (firstCluster != 0 && FatTable.GetNext(firstCluster) != 0)//لازم يكون مليان عشان اقرا منه
            {
                Fat_index = firstCluster;
                int next;
                next = FatTable.GetNext(Fat_index);
                List<byte> lsobj = new List<byte>();
                do
                {
                    lsobj.AddRange(VirtualDisk.GetBlock(Fat_index));
                    Fat_index = next;
                    if (Fat_index != -1)
                    {
                        next = FatTable.GetNext(Fat_index);
                    }
                } while (next != -1);

                byte[] Dobj = new byte[32];   

                for (int i = 0; i < lsobj.Count; i++)
                {
                    Dobj[i % 32] = lsobj[i];

                    if ((i + 1) % 32 == 0) 
                    {
                        directoryTable.Add(GetdirectoryEntry(Dobj));
                    }
                }
            }
        }

        public int Search(string FileName)
        {
            Readdirectory();
            string filename = new string(FileName);
            if (FileName.Length < 11) 
            {
                for (int i = FileName.Length; i < 11; i++) 
                {
                    filename += " ";
                }
            }

            FileName = filename;
            for (int i = 0; i < directoryTable.Count; i++)

            {
                string directoryTABLEfileName = new string(directoryTable[i].FileName);

                if (FileName.Equals(directoryTABLEfileName)) 
                {
                    return i;
                }
            }
            return -1;
        }



        public void UpdateContent(directoryEntry direc) //بتعدل علي الدايريكتوري
        {
            Readdirectory();
            int index;
            index = Parent.Search(direc.FileName.ToString());

            if (index != -1)
            {
               directoryTable.RemoveAt(index);
               directoryTable.Insert(index, direc);
            }

        }

        public void Deletedirectory(string Fname)
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
                    Parent.Readdirectory();

                    int indexParent = Parent.Search(Fname);

                    if (indexParent != -1)
                    {
                        Parent.directoryTable.RemoveAt(indexParent);//الحاجة الي جوا البيرنت اتمسحت
                        Parent.Write_directory();
                    }
                }
            
            FatTable.WriteFatTable();
        }
     
    }
}
