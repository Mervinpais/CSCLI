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
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.Write("Item Name: ");
            string ItemName = Console.ReadLine();
            Console.WriteLine($"\n=== Files Found in {currentDir} ===");
            ItemName = ItemName.ToLower();
            List<string> filesFound = new List<string>();
            foreach (string file in Directory.GetFiles(currentDir))
            {
                if (file.ToLower().Contains(ItemName))
                {
                    filesFound.Add(file);
                }
            }
            //Console.WriteLine(string.Join(Environment.NewLine, filesFound.ToArray()));
            ConsoleKeyInfo consoleKeyInfo = new ConsoleKeyInfo();
            int selectedLine = 0;
            int counter = 0;
            while (consoleKeyInfo.Key != ConsoleKey.Enter)
            {
                Console.Clear();
                Console.Write($"Item Name: {ItemName}");
                Console.WriteLine($"\n=== Files Found in {currentDir} ===");

                foreach (string file in filesFound)
                {
                    string[] wordInstances = file.Split(ItemName);
                    if (counter == selectedLine)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("> ");

                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    foreach (string e in wordInstances)
                    {
                        if (selectedLine != counter) { Console.BackgroundColor = ConsoleColor.DarkGray; Console.ForegroundColor = ConsoleColor.White; }

                        else { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }

                        Console.Write(e);

                        if (selectedLine != counter) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }

                        else { Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White; }

                        Console.Write(ItemName);
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    counter++;

                    Console.SetCursorPosition(Console.CursorLeft - ItemName.Length, Console.CursorTop);
                    for (int i = 0; i < ItemName.Length; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedLine--;
                    if (selectedLine < 0)
                    {
                        selectedLine = counter;
                    }
                }
                else if (consoleKeyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedLine++;
                    if (selectedLine > counter)
                    {
                        selectedLine = 0;
                    }
                }
                counter = 0;
            }
        }
    }
}
