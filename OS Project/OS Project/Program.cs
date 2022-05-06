using System;
using System.Text;

namespace OS_Project
{
    class Program
    {
        public static directory Currentdirectory;
        public static string path = "";

        static void Main(string[] args)
        { 
            Console.WriteLine("Microsoft Windows [Version 10.0.22000.613]");
            Console.WriteLine("(c) Microsoft Corporation. All rights reserved.\n");
            VirtualDisk.Intialize();
            Currentdirectory = VirtualDisk.Root;
            string path2 = new string(Currentdirectory.FileName);
            path = path2;
            while (true)
            {
                Console.Write(path);
                Console.Write(">");
                string Commmand = Console.ReadLine();
                Commands com = new Commands(Commmand);
            }
        }

    }
}