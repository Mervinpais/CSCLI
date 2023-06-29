namespace CSCLI
{
    public class DirTree
    {
        public static void Gen(string dir, string indent = "", int currentDepth = 0, int maxDepth = 2, bool displayMaindir = true)
        {
            if (displayMaindir)
            {
                Console.WriteLine($"=== {dir} ===");
            }

            if (currentDepth >= maxDepth)
                return;

            try
            {
                string[] directories = Directory.GetDirectories(dir);
                foreach (string directory in directories)
                {
                    string folderName = Path.GetFileName(directory);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{indent}├─ {folderName}");

                    Gen(directory, $"{indent}│  ", currentDepth + 1, maxDepth, false); // Recursive call to process subfolders
                }

                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{indent}├─ {fileName}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{indent}├─ Error: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
