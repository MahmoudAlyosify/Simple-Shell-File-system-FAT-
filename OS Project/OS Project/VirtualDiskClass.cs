using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace OS_Project
{

    public static class VirtualDisk
    {
        static public directory Root;

        static public string FileName = "Os.txt";
        static byte[] super = Encoding.ASCII.GetBytes("0");
        static byte[] bytes = Encoding.ASCII.GetBytes("#");
        static byte[] Data = Encoding.ASCII.GetBytes("*");
        static int position = 0;

        public static void Intialize()
        {
            if (!File.Exists(FileName))
            {
                using (FileStream file = new FileStream(FileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    for (int i = 0; i < 1024; i++)
                    {
                        file.Write(super);
                        position++;
                    }

                    file.Seek(position, SeekOrigin.Begin);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 1024; j++)
                        {
                            file.Write(bytes);
                            position++;
                        }
                    }
                    file.Seek(position, SeekOrigin.Begin);

                    for (int i = 0; i < 1019; i++)
                    {
                        for (int j = 0; j < 1024; j++)
                        {
                            file.Write(Data);
                            position++;
                        }
                    }
                    file.Close();
                }
                Root = new directory("M:\\Users\\Mahmoud".ToCharArray(), 1, 6,0, null);
                Root.Write_directory();
                FatTable.WriteFatTable();

            }
            else
            {
                int[] Fat_Table = FatTable.GetFatTable();
                Root = new directory("M:\\Users\\Mahmoud".ToCharArray(), 1, 5,0, null);
                Root.Write_directory();
                Root.Readdirectory();
            }
        }

        static public void WriteBlock(byte[] BlockData, int Fatindex)
        {
            string fileName = "OS.txt";
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                file.Seek(Fatindex * 1024, SeekOrigin.Begin);
                file.Write(BlockData, 0, BlockData.Length);
                file.Close();
            }
        }
        static public void Write_Cluster(int Cluster_index, byte[] bytes)
        {
            using (FileStream writer = File.OpenWrite((FileName)))
            {
                writer.Seek(Cluster_index * 1024, SeekOrigin.Begin);
                foreach (char ln in bytes)
                {
                    writer.WriteByte((byte)ln);

                }
                writer.Flush();
            }

        }
        static public byte[] GetBlock(int index)
        {
            string FileName = "OS.txt";
            byte[] TheReturnData = new byte[1024];
            FileStream file = File.OpenRead(FileName);
            file.Seek(index * 1024, SeekOrigin.Begin);
            file.Read(TheReturnData, 0, TheReturnData.Length);
            file.Close();
            return TheReturnData;
        }
        static public void printBlock(int index)
        {
            for (int i = 0; i < 1024; i++)
            {
                Console.Write((char)GetBlock(index)[i]);
            }
        }
    }
}
