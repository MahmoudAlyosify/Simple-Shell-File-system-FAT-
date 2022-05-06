using System;
using System.Collections.Generic;
using System.Text;

namespace OS_Project
{
    public class directoryEntry
    {
        //fields
        public char[] FileName = new char[11];
        public byte fileAttribute;
        public byte[] fileEmpty = new byte[12];
        public int fileSize;
        public int firstCluster;
        public directoryEntry()
        {

        }
        public directoryEntry( char[] fileName, byte fileAttribute, int firstCluster,int file_size)
        {
             fileSize = 0;
            
            string s = new string(fileName);

            if (fileName.Length < 11)
            {
                for (int i = fileName.Length; i < 11; i++)
                    s += (char)' ';
            }

           this.FileName = s.ToCharArray();
           this.fileAttribute = fileAttribute;
           this.firstCluster = firstCluster;
            this.fileSize = file_size;
            for (int i = 0; i < fileEmpty.Length; i++)
            {
                fileEmpty[i] = 0;
            }
        }

        public byte[] getByte()
        {
            byte[] ConvertFileNametoBYTE = Encoding.ASCII.GetBytes(FileName); //convert array of char to array of byte
            byte[] DirEntryarray = new byte[32];  // عملت اراى كبيرة من نوع بايت تخزن كل الداتا بتاعت الانتريز
            byte[] arr2 = new byte[(ConvertFileNametoBYTE.Length/2)];
            int l = 0;
            for (int i = 0; i < arr2.Length; i++)
            {
                arr2[i] = ConvertFileNametoBYTE[l]; // خزنت اسم الفايل جوه الاراى
                l++;  
            }

            for (int i = 0; i < arr2.Length; i++) 
            {
                DirEntryarray[i] = arr2[i];
            }
            DirEntryarray[11] = fileAttribute;  // حطيت الاتريبيوت جوه فايل ولا فولدر 1أو 0

            int j = 0;
            for (int i = 12; i < 24; i++)
            {
                DirEntryarray[i] = fileEmpty[j];  // خزنت الداتا بتاعت الفايل ايمبتى فى الاراى
                j++;
            }

            int c = 0;

            byte[] CovnINTtoBYTE_firstcluster = BitConverter.GetBytes(firstCluster);//convert int to array of byte
            // لأن الفيرست كلاستر من نوع انتيجير وانا عايزة اخليه 4 بايتات فعملت اراى علشان اخزنه
            for (int i = 24; i < 28; i++)
            {
                //DirEntry[i] = Convert.ToByte(firstCluster);
                DirEntryarray[i] = CovnINTtoBYTE_firstcluster[c];
                c++;
            }

            int k = 0;
            byte[] CovnINTtoBYTE_size = BitConverter.GetBytes(fileSize);//convert int to array of byte
            // لأن الفايل سايز من نوع انتيجير وانا عايزة اخليه 4 بايتات فعملت اراى علشان اخزنه
            for (int i = 28; i < 32; i++)
            {
                DirEntryarray[i] = CovnINTtoBYTE_size[k];
                k++;
            }

            return DirEntryarray;
        }

        public directoryEntry GetdirectoryEntry(byte[] arrOFbyte) //فانكشن لتحويل من الاراى اوف بايت للديركتورى انترى
        {

            char[] char11_FileName = new char[11];  // اراى اوف كاركتر لتخزين الفايل نيم
            byte[] First11byte_filename = new byte[11]; // علشان يقرأ البايتات بتاعت الفايل نيم
           
            for (int i = 0; i < 11; i++)
            {
                First11byte_filename[i] = arrOFbyte[i]; // بيقرأ البايتات من الاراى اللى راجعة
            }
            
            for (int i = 0; i < First11byte_filename.Length; i++)
            {
                char11_FileName[i] = Convert.ToChar(First11byte_filename[i]); // بيحولها لكاركتر وبيخزنها فى اراى
            }

            byte fileatt = arrOFbyte[12]; // خزن الفايل اتريبيوت

            byte[] FileEmpty = new byte[12]; // عمل اراى اوف بايت علشان يقرأ بايتات الاراى القديمة

            int j = 0;
            for (int i = 12; i < 24; i++)
            {
                FileEmpty[j] = arrOFbyte[i]; // خزن من الاراى القديمة وحطها فى الاراى القديمة
                j++;
            }


            int index = 0;
            byte[] Fcluster = new byte[4]; // علشان يقرأ الفيرست كلاستر
            for (int i = 24; i < 28; i++) 
            {
                Fcluster[index] = arrOFbyte[i]; // خزن الفيرست كلاستر فى الاراى
                index++;
            }

            int FClu = BitConverter.ToInt32(Fcluster, 0);  // خزن الفيرست كلاستر فى متغير انتيجير


            int indexSize = 0;
            byte[] FSize = new byte[4];
            for (int i = 28; i < 32; i++)
            {
                FSize[indexSize] = arrOFbyte[i]; // قرأ الفايل سايز فى اراى
                indexSize++;
            }
            
            int FileSize = BitConverter.ToInt32(FSize, 0);  //خزن السايز فى رقم انتيجر


            directoryEntry a = new directoryEntry(char11_FileName, fileatt, FClu,fileSize); //عمل متغير من الكلاس علشان يبعتله الداتا
            a.fileEmpty = FileEmpty;
            a.fileSize = FileSize;
            return a;
        }
        public directoryEntry GetdirectoryEntry() //فانكشن لو فولدر مفيهوش حاجه
        {
            directoryEntry a = new directoryEntry(FileName, fileAttribute, firstCluster,fileSize); //عمل متغير من الكلاس علشان يبعتله الداتا
            a.fileEmpty = fileEmpty;
            a.fileSize = fileSize;
            return a;
        }

    }
}
