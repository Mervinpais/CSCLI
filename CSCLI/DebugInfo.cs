namespace CSCLI
{
    public static class DebugInfo
    {
        public static void All()
        {
            WindowInfo();
            ConsoleInfo();
            CursorInfo();
        }

        public static void WindowInfo()
        {
            Console.WriteLine("Console.WindowTop : " + Console.WindowTop);
            Console.WriteLine("Console.WindowHeight : " + Console.WindowHeight);
            Console.WriteLine("Console.WindowLeft : " + Console.WindowLeft);
            Console.WriteLine("Console.WindowWidth : " + Console.WindowWidth);
        }
        public static void ConsoleInfo()
        {
            Console.WriteLine("Console.ForegroundColor : " + Console.ForegroundColor);
            Console.WriteLine("Console.BackgroundColor : " + Console.BackgroundColor);
            Console.WriteLine("Console.OutputEncoding : " + Console.OutputEncoding);
            Console.WriteLine("Console.Title : " + Console.Title);
        }
        public static void CursorInfo()
        {
            Console.WriteLine("Console.CursorTop : " + Console.CursorTop);
            Console.WriteLine("Console.CursorLeft : " + Console.CursorLeft);
            Console.WriteLine("Console.CursorSize : " + Console.CursorSize);
            Console.WriteLine("Console.CursorVisible : " + Console.CursorVisible);
        }

        public static void RedirectedInfo() //ok do we need this?
        {
            Console.WriteLine("Console.IsErrorRedirected : " + Console.IsErrorRedirected);
            Console.WriteLine("Console.IsInputRedirected : " + Console.IsInputRedirected);
            Console.WriteLine("Console.IsOutputRedirected : " + Console.IsOutputRedirected);
        }

        public static void OtherInfo() //COULD be useful
        {
            Console.WriteLine("Console.IsErrorRedirected : " + Console.KeyAvailable);
            Console.WriteLine("Console.IsInputRedirected : " + Console.NumberLock);
            Console.WriteLine("Console.IsOutputRedirected : " + Console.TreatControlCAsInput);
        }
    }
}