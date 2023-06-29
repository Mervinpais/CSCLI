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
            foreach (string file in filesFound)
            {
                string[] wordInstances = file.Split(ItemName);
                foreach (string e in wordInstances)
                {
                    Console.ResetColor();
                    Console.Write(e);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(ItemName);
                    Console.ResetColor();
                }
                Console.SetCursorPosition(Console.CursorLeft - ItemName.Length, Console.CursorTop);
                for (int i = 0; i < ItemName.Length; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
