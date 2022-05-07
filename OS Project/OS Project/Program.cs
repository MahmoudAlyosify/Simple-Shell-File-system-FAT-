using System;
using System.Text;

namespace OS_Project
{
    public static class Program
    {
        public static Directory CurrentDirectory;

        public static string Path = "";

        public static void Main(string[] args)
        {
            Console.WriteLine("Microsoft Windows [Version 10.0.22000.613]");
            Console.WriteLine("(c) Microsoft Corporation. All rights reserved.");
            Console.WriteLine();
            Console.WriteLine();

            VirtualDisk.Intialize();
         
            CurrentDirectory = VirtualDisk.Root;
            string s = new string(CurrentDirectory.FileName);
            Path = s;

            while (true)
            {
                Console.Write(Path);
                Console.Write(">");
                string Commmand = Console.ReadLine();
                Commands com = new Commands(Commmand);
            }
        }

    }
}