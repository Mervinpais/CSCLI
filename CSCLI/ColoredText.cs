namespace CSCLI
{
    public static class ColoredText
    {
        public static void WriteLine(object text, ConsoleColor color)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void WriteLine(object text, ConsoleColor fore_color, ConsoleColor back_color)
        {
            Console.ResetColor();
            Console.ForegroundColor = fore_color;
            Console.BackgroundColor = back_color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        public static void Write(object text, ConsoleColor color)
        {
            Console.ResetColor();
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        public static void Write(object text, ConsoleColor fore_color, ConsoleColor back_color)
        {
            Console.ResetColor();
            Console.ForegroundColor = fore_color;
            Console.BackgroundColor = back_color;
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
