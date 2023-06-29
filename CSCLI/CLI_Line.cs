#nullable disable
namespace CSCLI
{
    internal class CLI_Line
    {
        public static void Load(string currentDirectory, int lastStatus = 0)
        {
            foreach (string line in File.ReadAllLines("C:\\Users\\mervi\\Desktop\\CSCLI\\CSCLI\\config.txt"))
            {
                Console.ResetColor();
                string[] lineSplit = line.Trim().Split(',');
                if (lineSplit[0] == "showPath" && lineSplit.Length >= 3)
                {
                    if (lineSplit[1].Trim() == "1")
                    {
                        Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[2], true);
                        Console.Write(currentDirectory);
                        Console.Write(">");
                    }
                }
                if (lineSplit[0] == "showTime" && lineSplit.Length >= 3)
                {
                    if (lineSplit[1].Trim() == "1")
                    {
                        if (lineSplit.Length == 3)
                        {
                            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[2].Trim(), true);
                        }
                        else if (lineSplit.Length == 4)
                        {
                            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[2].Trim(), true);
                            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[3].Trim(), true);
                        }
                        DateTime now = DateTime.Now;
                        Console.Write(now.ToString("yyyy-MM-dd hh:mm:ss") + " ");

                        // Determine the AM/PM part
                        string amPm = now.ToString("tt");

                        // Change console color based on AM or PM
                        if (amPm == "AM")
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }
                        else if (amPm == "PM")
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        // Write the AM/PM part with the modified color
                        Console.Write(" " + amPm + " ");

                        // Reset console color to default
                        Console.ResetColor();
                    }
                }
                if (lineSplit[0] == "showStatus" && lineSplit.Length >= 3)
                {
                    if (lineSplit[1].Trim() == "1")
                    {
                        if (lastStatus == 0)
                        {
                            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[2].Split('-')[0], true);
                            Console.Write(" " + '\u2713' + " ");
                        }
                        else if (lastStatus == -1)
                        {
                            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), lineSplit[2].Split('-')[1], true);
                            Console.Write(" " + 'X' + " ");
                        }
                    }
                }
            }
            Console.ResetColor();
            Console.Write(" ");
        }

        public static void configScreen()
        {
            List<string> lines = new List<string>();
            Console.WriteLine(string.Join(Environment.NewLine, getConfig()) + "\n");
            while (true)
            {
                Console.Write(">");
                string com = Console.ReadLine();
                if (com == "showTime")
                {
                    com = Console.ReadLine();
                    if (com == "1")
                    {
                        lines.Add("showTime = 1");
                    }
                    else if (com == "0")
                    {
                        lines.Add("showTime = 0");
                    }
                }
                else if (com == "showPath")
                {
                    com = Console.ReadLine();
                    if (com == "1")
                    {
                        lines.Add("showPath = 1");
                    }
                    else if (com == "0")
                    {
                        lines.Add("showPath = 0");
                    }
                }
                else if (com == "")
                {
                    Console.WriteLine(string.Join(Environment.NewLine, getConfig()) + "\n");
                }
                else if (com == ".")
                {
                    break;
                }
            }
            File.WriteAllLines("config.txt", lines.ToArray());
        }

        public static string[] getConfig()
        {
            bool showTime = false;
            bool showPath = false;
            foreach (var line in File.ReadAllLines("config.txt"))
            {
                if (line.StartsWith("showTime"))
                {
                    if (line.EndsWith("=1")) showTime = true;
                    else showTime = false;
                }
                if (line.StartsWith("showPath"))
                {
                    if (line.EndsWith("=1")) showPath = true;
                    else showPath = false;
                }
            }
            return new string[] {
                $"showPath; {showPath}",
                $"showTime; {showTime}"};
        }
    }
}
