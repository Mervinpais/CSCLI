namespace CSCLI
{
    public static class FileFindGUI_OLD
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
            int state = 0;
            string itemName = "";
            string itemType = "Any";
            List<int> Selectedlines = new() { };
            List<string> Foundlines = new() { };
            int selectedItem = 0;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            while (true)
            {
            stuff:
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                if (state == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write($"Item Name: [{itemName}]\n");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                if (state == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write($"Item Type: <{itemType}>\n");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkGray;
                if (state == 2)
                {
                    Console.WriteLine($"[Found files in {currentDir}]======");
                }
                else
                {
                    Console.WriteLine($"[{currentDir}]======");
                }

                foreach (string file in Directory.GetFiles(currentDir))
                {
                    if (state == 2)
                    {
                        string fileName = file.Substring(file.LastIndexOf("\\"));
                        int ContainsPeriod = itemName.LastIndexOf(".");
                        if (ContainsPeriod == 0)
                        {
                            fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                        }
                        fileName = fileName.Substring(1);
                        string fileNameSearch = fileName;
                        if (fileName.Length >= itemName.Length)
                        {
                            fileNameSearch = fileNameSearch.Substring(0, itemName.Length);
                        }
                        if (fileNameSearch == itemName)
                        {
                            Console.WriteLine("    " + file);
                            Foundlines.Add(file);
                            Selectedlines.Add(Console.CursorTop);
                        }
                    }
                    else if (state >= 3)
                    {
                        Console.SetCursorPosition(Console.CursorLeft, Selectedlines[0]);
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        int counter = 0;
                        while (keyInfo.Key != ConsoleKey.Enter)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            foreach (string line2 in Foundlines)
                            {
                                Console.Clear();
                                if (counter == selectedItem)
                                {
                                    Console.BackgroundColor = ConsoleColor.Black;
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                else
                                {
                                    Console.ResetColor();
                                }
                                Console.WriteLine(line2);
                                counter++;
                            }
                            if (keyInfo.Key == ConsoleKey.DownArrow)
                            {
                                state++;
                                if (Foundlines.Count <= selectedItem)
                                {
                                    selectedItem = 0;
                                }
                            }
                            keyInfo = Console.ReadKey(true);
                        }
                        if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            state = 0;
                            goto stuff;
                        }
                    }
                    else
                    {
                        Console.WriteLine("    " + file);
                    }


                }
                if (state >= 3)
                {
                    state = 0;
                    itemName = "";
                }
                if (state == 2)
                {
                    state = 3;
                }

                Console.SetCursorPosition(0, 0);
                if (state < 3 && state < 2)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        state += 1;
                    }

                    if (state == 0)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Write(">");
                        Console.SetCursorPosition(12, 0);
                        itemName += key.KeyChar;
                    }
                    else if (state == 1)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.Write(">");
                        Console.SetCursorPosition(12, 1);
                        if (key.Key == ConsoleKey.RightArrow)
                        {
                            if (itemType == "Any")
                            {
                                itemType = "File";
                            }
                            else if (itemType == "File")
                            {
                                itemType = "Dir";
                            }
                            else if (itemType == "Dir")
                            {
                                itemType = "Any";
                            }
                        }
                    }
                }
                else
                {

                }
            }
        }
    }
}
