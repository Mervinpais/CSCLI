namespace CSCLI
{
    public static class BoxedText
    {
        //═║╔╗╝╚ Regular Set
        public static void Info(string Text)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine($"""
                    ═INFO════════════
                     {Text}
                    """);
            Console.ResetColor();

        }
        public static void Warn(string Text, bool severe = false)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"""
                    ═WARNING═══════════════
                     {Text}
                    """);
            Console.ResetColor();
        }

        public static void Error(string Text, bool severe = false)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"""
                    ═ERROR═══════════════
                     {Text}
                    """);
            Console.ResetColor();
        }

        public static class Arrays
        {
            public static void Regular(string[] Text)
            {
                string separator = new string('═', Text.Length + 5);
                string e = string.Join(Environment.NewLine, Text);
                Console.WriteLine($"╔{separator}╗");
                foreach (string line in Text)
                {
                    Console.WriteLine($"║ {line}");
                }
                Console.WriteLine($"╚{separator}╝");
            }
            public static void Error(string[] Text, bool severe = false)
            {
                Console.ResetColor();
                string separator = new string('═', Text.Length + 5);
                string e = string.Join(Environment.NewLine, Text);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"╔{separator.Substring(separator.Length / 2)}ERROR{separator.Substring(separator.Length / 2)}╗");
                foreach (string line in Text)
                {
                    Console.WriteLine($"║ {line}");
                }
                Console.WriteLine($"╚═══{separator}═══╝");
                Console.ResetColor();
            }
        }
    }
}
