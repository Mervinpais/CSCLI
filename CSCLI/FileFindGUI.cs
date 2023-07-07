using System.Diagnostics;

namespace CSCLI
{
    public static class FileFindGUI
    {
        public static void Show(string path)
        {
            string currentDir = path;
            /*   DESIGN PATTERN;
                    Item Name [----------------------------]
                    Item Type: <option:Any/File/Dir>

                    [Dir]======
                    - dir1
                    - dir2
                    - file1
                    - file2
                    - file3
                */
            //Console.BackgroundColor = ConsoleColor.DarkGray;
            //Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
            Console.Clear();
            Console.Write("Item Name: ");
            string searchKeyword = Console.ReadLine();
            Console.WriteLine($"\n=== Files Found in {currentDir} ===");
            searchKeyword = searchKeyword.ToLower();
            List<string> filesFound = new List<string>();
            foreach (string file in Directory.GetFiles(currentDir))
            {
                if (file.ToLower().Contains(searchKeyword))
                {
                    filesFound.Add(file);
                }
            }

            int pageSize = Console.WindowHeight - 2; // Adjusting for some text on top
            int totalPages = (int)Math.Ceiling((double)filesFound.Count / pageSize);
            int currentPage = 1;
            int selectedLine = 0;
            int counter = 0;

            ConsoleKeyInfo consoleKeyInfo;
            do
            {
                Console.Clear();
                Console.Write($"Item Name: {searchKeyword}");
                Console.WriteLine($"\n=== Files Found in {currentDir} ===");

                int startIndex = (currentPage - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, filesFound.Count);

                if (endIndex < Console.WindowHeight)
                {
                    endIndex = Console.WindowHeight - 5;
                }


                for (int i = startIndex; i < filesFound.Count; i++)
                {
                    string file = filesFound[i];

                    string[] fileNameSplit = file.Split(searchKeyword);
                    if (counter == selectedLine)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("> ");
                    }
                    else
                    {
                        Console.ResetColor();
                    }

                    foreach (string fileNameWithoutSearchKeyword in fileNameSplit)
                    {
                        if (selectedLine != counter)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.Write(fileNameWithoutSearchKeyword);

                        if (selectedLine != counter)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        Console.Write(searchKeyword);
                        Console.ResetColor();
                    }

                    counter++;
                    Console.SetCursorPosition(Console.CursorLeft - searchKeyword.Length, Console.CursorTop);
                    for (int j = 0; j < searchKeyword.Length; j++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine($"\n=== Page {currentPage}/{totalPages} ===");
                consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedLine--;
                    if (selectedLine < 0)
                    {
                        selectedLine = counter - 1;
                    }
                }
                else if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedLine++;
                    if (selectedLine >= counter)
                    {
                        selectedLine = 0;
                    }
                }
                else if (consoleKeyInfo.Key == ConsoleKey.LeftArrow)
                {
                    currentPage--;
                    if (currentPage < 1)
                    {
                        currentPage = totalPages;
                    }
                    selectedLine = 0;
                    counter = 0;
                }
                else if (consoleKeyInfo.Key == ConsoleKey.RightArrow)
                {
                    currentPage++;
                    if (currentPage > totalPages)
                    {
                        currentPage = 1;
                    }
                    selectedLine = 0;
                    counter = 0;
                }
                counter = 0;
            } while (consoleKeyInfo.Key != ConsoleKey.Enter);
            string selectedFile = filesFound[selectedLine];

            selectedLine = 0;
            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(0, 0);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine($"\nWhat would you like to do with {selectedFile}?");

                string[] options = {
                        "Open",
                        "Read",
                        "Write",
                        "Delete"
                };

                for (int i = 0; i < options.Length; i++)
                {
                    string option = options[i];
                    if (i == selectedLine)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else { Console.ResetColor(); }

                    Console.Write("                              ");
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(option + "\n");
                }
                consoleKeyInfo = Console.ReadKey();

                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedLine--;
                    if (selectedLine < 0)
                    {
                        selectedLine = 4 - 1;
                    }
                }
                else if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedLine++;
                    if (selectedLine >= 4)
                    {
                        selectedLine = 0;
                    }
                }
            }
            while (consoleKeyInfo.Key != ConsoleKey.Enter);

            if (selectedLine == 0)
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(selectedFile);
                process.EnableRaisingEvents = true;
                try
                {
                    process.Start();
                }
                catch
                {
                    Console.WriteLine($"Unable to run file \'{selectedFile}\'");
                }
            }
        }
    }
}
