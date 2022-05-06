using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
namespace OS_Project
{
    public class Commands
    {

        public static List<string> Date = new List<string>();
        public static Boolean isused, import_isused;
        public string[] CommandArg;
        public Commands(string command)
        {
            CommandArg = command.Split(" ");

            if (CommandArg.Length == 1)
            {
                Command(CommandArg);
            }
            else if (CommandArg.Length == 2)
            {
                Command2Arg(CommandArg);
            }
            else if (CommandArg.Length == 3)
            {
                Command3Arg(CommandArg);
            }
        }
        static void Command(string[] CommandArray)
        {
            DateTime aDate = DateTime.Now;
            string[] fileEntries = Directory.GetFiles(@"E:\");
            List<string> fileImport = new List<string>();
            if (CommandArray[0].ToLower() == "quit")
            {
                Environment.Exit(0);
            }
            else if (CommandArray[0].ToLower() == "cls")
            {
                Console.Clear();
            }
            else if (CommandArray[0].ToLower() == "dir")//dir - List the contents of directory
            {

                int count_num_files = 0;
                int count_num_Directory = 0;
                int size_of_each_file = 0;
                for (int i = 0; i < Program.Currentdirectory.directoryTable.Count; i++)
                {
                    if (Program.Currentdirectory.directoryTable[i].fileAttribute == 1)
                    {

                        Console.WriteLine("         " + Program.Currentdirectory.directoryTable[i].fileSize + "  " + new string(Program.Currentdirectory.directoryTable[i].FileName));

                        count_num_files++;
                        size_of_each_file += Program.Currentdirectory.directoryTable[i].fileSize;
                    }
                    else if (Program.Currentdirectory.directoryTable[i].fileAttribute == 16)
                    {
                        Console.WriteLine("         <DIR>    " + "  " + new string(Program.Currentdirectory.directoryTable[i].FileName));
                        count_num_Directory++;
                    }
                }
                Console.WriteLine("                     " + count_num_files + "  File(s)     " + size_of_each_file);
                Console.WriteLine("                     " + count_num_Directory + "  DIR(s)      " + FatTable.get_free_spaces() + "  bytes free");

            }
            else if (CommandArray[0].ToLower() == "help")
            {
                Console.WriteLine(" -cd       Change the current default directory to another.");
                Console.WriteLine(" -cls      Clear the screen.");
                Console.WriteLine(" -dir      List the contents of directory.");
                Console.WriteLine(" -help     Display the user manual using the more filter.");
                Console.WriteLine(" -quit     Quit the shell.");
                Console.WriteLine(" -copy     Copies one or more files to another location.");
                Console.WriteLine(" -del      Deletes one or more files.");
                Console.WriteLine(" -md       Creates a directory.");
                Console.WriteLine(" -rd       Removes a directory.");
                Console.WriteLine(" -rename   Renames a file.");
                Console.WriteLine(" -type     Displays the contents of a text file.");
                Console.WriteLine(" -import   import text file(s) from your computer.");
                Console.WriteLine(" -export   export text file(s) to your computer.");

            }


            else
            {
                Console.WriteLine("\'" + CommandArray[0] + "\'" + " is not recognized as an internal or external command,operable program or batch file.");
            }

        }
        static void Command2Arg(string[] CommandArray2Arg)
        {


            if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "cd")
            {
                Console.WriteLine("cd   Change the current default directory to. If the argument is not present, report the current directory. If the directory does not exist an appropriate error should be reported.");
            }
            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "cls")
            {
                Console.WriteLine("cls   Clear the screen.");
            }
            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "dir")
            {
                Console.WriteLine("dir   List the contents of directory.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "help")
            {
                Console.WriteLine("help   Display the user manual using the more filter.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "quit")
            {
                Console.WriteLine("quit   Quit the shell.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "copy")
            {
                Console.WriteLine("copy   Copies one or more files to another location.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "del")
            {
                Console.WriteLine("del   Deletes one or more files.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "md")
            {
                Console.WriteLine("md   Creates a directory.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "rd")
            {
                Console.WriteLine("rd   Removes a directory.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "rename")
            {
                Console.WriteLine("rename   Renames a file.");
            }

            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "type")
            {
                Console.WriteLine("type   Displays the contents of a text file.");
            }
            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "import")
            {
                Console.WriteLine("import   import text file(s) from your computer.");
            }
            else if (CommandArray2Arg[0].ToLower() == "help" && CommandArray2Arg[1].ToLower() == "export")
            {
                Console.WriteLine("export   export text file(s) to your computer.");
            }

            else if (CommandArray2Arg[0].ToLower() == "md")
            {
                if (CommandArray2Arg[1].Contains(":"))
                {  //  string dir = @"C:\test\Aaron";
                    if (!Directory.Exists(CommandArray2Arg[1]))
                    {
                        Directory.CreateDirectory(CommandArray2Arg[1]);
                    }
                }
                else if (!CommandArray2Arg[1].Contains(":"))
                {
                    isused = true;
                    if (Program.Currentdirectory.Search(CommandArray2Arg[1]) == -1)
                    {
                        directoryEntry d = new directoryEntry(CommandArray2Arg[1].ToCharArray(), 0x10, 0, 0);
                        Program.Currentdirectory.directoryTable.Add(d);
                        Program.Currentdirectory.Write_directory();
                        if (Program.Currentdirectory.Parent != null)
                        {
                            Program.Currentdirectory.Parent.UpdateContent(Program.Currentdirectory.GetdirectoryEntry());
                            Program.Currentdirectory.Parent.Write_directory();
                        }
                        DateTime aDate2 = DateTime.Now;
                        Date.Add(aDate2.ToString("MM / dd / yyyy  HH:mm  tt"));

                    }

                    else
                    {
                        Console.WriteLine("A subdirectory or file " + CommandArray2Arg[1] + " is already exists.");
                    }
                }
            }

            else if (CommandArray2Arg[0] == "del")
            {
                if (CommandArray2Arg[1].Contains(":"))
                {

                }
                else
                {
                    int index = Program.Currentdirectory.Search(CommandArray2Arg[1].ToString());
                    if (index != -1)
                    {
                        if (Program.Currentdirectory.directoryTable[index].fileAttribute == 1)
                        {
                            int first = Program.Currentdirectory.directoryTable[index].firstCluster;
                            int size = Program.Currentdirectory.directoryTable[index].fileSize;
                            string content = "";
                            FileEntryClass d1 = new FileEntryClass(CommandArray2Arg[1].ToCharArray(), 0x01, first, size, Program.Currentdirectory, content);
                            d1.DeleteFile();

                        }

                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the file specified.");
                    }
                }
            }
            else if (CommandArray2Arg[0].ToLower() == "rd") //remove directory
            {
                int index = Program.Currentdirectory.Search(CommandArray2Arg[1].ToString());
                if (index != -1)
                {
                    int size = Program.Currentdirectory.directoryTable[index].fileSize;

                    int FirstCluster = Program.Currentdirectory.directoryTable[index].firstCluster; //علشان اشوف الفولدر موجود ولا
                    directory dir = new directory(CommandArray2Arg[1].ToCharArray(), 0x10, FirstCluster, size, Program.Currentdirectory);
                    dir.Deletedirectory(CommandArray2Arg[1].ToString());
                    Date.RemoveAt(index);
                }
                else
                {
                    Console.WriteLine("Folder not exist");
                }
            }
            else if (CommandArray2Arg[0] == "import")
            {
                if (File.Exists(CommandArray2Arg[1]))
                {
                    string name_txt = Path.GetFileName(CommandArray2Arg[1]);
                    string content_txt = File.ReadAllText(CommandArray2Arg[1]);
                    int size_txt = content_txt.Length;
                    int index = Program.Currentdirectory.Search(name_txt);
                    if (index == -1)
                    {
                        if (size_txt > 0)
                        {
                            Program.Currentdirectory.firstCluster = FatTable.getAvailableBlock();
                        }
                        else { }
                        FileEntryClass d = new FileEntryClass(name_txt.ToCharArray(), 0x01, 0, size_txt, Program.Currentdirectory, content_txt);
                        d.WriteFileContent();
                        directoryEntry d1 = new directoryEntry(name_txt.ToCharArray(), 0x01, 0, size_txt);
                        Program.Currentdirectory.directoryTable.Add(d1);
                        Program.Currentdirectory.Write_directory();
                    }

                }
            }
            else if (CommandArray2Arg[0].ToLower() == "cd") //change directory'
            {
                int index = Program.Currentdirectory.Search(CommandArray2Arg[1].ToString());//بسيرش علي الدايريكتوري الي انا عايز اروحله 

                if (index != -1)
                {
                    byte attribute = Program.Currentdirectory.directoryTable[index].fileAttribute;
                    if (attribute == 16)
                    {
                        int FirstCluster = Program.Currentdirectory.directoryTable[index].firstCluster; //علشان اشوف الفولدر موجود ولا
                        directory dir = new directory(CommandArray2Arg[1].ToCharArray(), 1, FirstCluster, 0, Program.Currentdirectory);//بديلة معلومات الي الدايريكتوري الي عايز اروحله
                        Program.Currentdirectory = dir;  //هنا خليته يشاور ع الدايريكتوري الي عايز اروحله
                        Program.path = Program.path + "\\" + CommandArray2Arg[1].ToString();   //غيرت الباس
                        Program.Currentdirectory.Readdirectory();
                    }
                }
                else
                {
                    Console.WriteLine("Specified folder is not exist.");
                }
            }
            else if (CommandArray2Arg[0].ToLower() == "type")
            {
                int index = Program.Currentdirectory.Search(CommandArray2Arg[1].ToString());
                if (index != -1)
                {

                    int first = Program.Currentdirectory.directoryTable[index].firstCluster;
                    int size = Program.Currentdirectory.directoryTable[index].fileSize;
                    string content = "";
                    string content_txt = File.ReadAllText("F:\\MM.txt");
                    FileEntryClass d1 = new FileEntryClass(CommandArray2Arg[1].ToCharArray(), 0x01, first, size, Program.Currentdirectory, content);
                    d1.ReadFileContent();
                    Console.WriteLine(d1.FileContent);
                    Console.WriteLine(content_txt);

                }

                else
                {
                    Console.WriteLine("The system cannot find the file specified.");
                }

            }
            else
            {
                Console.WriteLine(CommandArray2Arg[0] + " It's not a valid command.");
                Console.WriteLine("Please use valid Command ^__^ ");
            }
        }
    
        static void Command3Arg(string[] CommandArray3Arg)
        {
            if (CommandArray3Arg[0].ToLower() == "rename")
            {
                int index = Program.Currentdirectory.Search(CommandArray3Arg[1].ToString());
                int index2 = Program.Currentdirectory.Search(CommandArray3Arg[2].ToString());

                if (index != -1)
                {
                    if (index2 == -1)
                    {
                        directoryEntry d1 = Program.Currentdirectory.directoryTable[index];

                        d1.FileName = CommandArray3Arg[2].ToCharArray();
                        //  Program.current_Directory.Directory_Table.RemoveAt(index);

                    }
                    else
                    {
                        Console.WriteLine("A subdirectory or file " + CommandArray3Arg[2] + " is already exists.");
                    }

                }
                else
                {

                    Console.WriteLine("The system cannot find the file specified.");
                }
            }
            else if (CommandArray3Arg[0] == "export")
            {
                int index = Program.Currentdirectory.Search(CommandArray3Arg[1].ToString());
                if (index != -1)
                {
                    if (System.IO.Directory.Exists(CommandArray3Arg[2]))
                    {
                        int first = Program.Currentdirectory.directoryTable[index].firstCluster;
                        int size = Program.Currentdirectory.directoryTable[index].fileSize;
                        string content = "";
                        FileEntryClass d1 = new FileEntryClass(CommandArray3Arg[1].ToCharArray(), 0x01, first, size, Program.Currentdirectory, content);
                        d1.ReadFileContent();
                        StreamWriter write = new StreamWriter(CommandArray3Arg[2] + "\\" + CommandArray3Arg[1]);
                        write.Write(d1.FileContent);
                        write.Flush();
                        write.Close();
                    }
                    else
                    {
                        Console.WriteLine("The system cannot find the file specified.");
                    }
                }

                else
                {
                    Console.WriteLine("The system cannot find the file specified.");
                }


            }

            else if (CommandArray3Arg[0] == "copy")
            {
                int index = Program.Currentdirectory.Search(CommandArray3Arg[1].ToString());
                if (index != -1)
                {
                    if (System.IO.Directory.Exists(CommandArray3Arg[2]))
                    {
                        if (Program.Currentdirectory.ToString() != CommandArray3Arg[2])
                        {
                            char ask;
                            Console.WriteLine("Do you want to overide (y/n)");
                            ask = Convert.ToChar(Console.ReadLine());
                            if (ask == 'y')
                            {
                                int first = Program.Currentdirectory.directoryTable[index].firstCluster;
                                int size = Program.Currentdirectory.directoryTable[index].fileSize;

                                directoryEntry d1 = new directoryEntry(CommandArray3Arg[1].ToCharArray(), 0x01, first, size);
                                directory d = new directory();
                                //المفروض هنا  اوصل لل و اضيف file جوا directory  directoy المعمول 
                                Program.Currentdirectory.directoryTable.Add(d);
                            }
                        }
                        else
                        {
                            Console.WriteLine("The system cannot find the file specified.");
                        }
                    }

                    else
                    {
                        Console.WriteLine("The system cannot find the file specified.");
                    }

                }

            }
            else
            {
                Console.WriteLine(CommandArray3Arg[0] + " It's not a valid command.");
                Console.WriteLine("Please use valid Command ^__^ ");
            }

        }

    }
}
