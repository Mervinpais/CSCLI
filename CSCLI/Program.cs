﻿using System.Diagnostics;
#nullable disable

namespace CSCLI
{
    internal class Program
    {
        public static string startingDirectory = "C:\\";
        public readonly static string NULL_VALUE = "<null>";

        static void Main(string[] args)
        {
            string tempPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), "CSCLI LOGS");
            bool runningInnerProgram = false;
            Dictionary<string, string> variables = new() { };
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
                Debug.WriteLine("Log Folder created successfully.");
            }
            else
            {
                Debug.WriteLine("Log Folder already exists.");
            }

            string currentDateAndTime = DateTime.Now.ToString("yyyy.MM.dd_HH,mm,ss");
            string LogfilePath = Path.Combine(tempPath, $"{currentDateAndTime}.txt");
            string[] LogfileContent = {
                $"[DATE {DateTime.Now}]",
                $"{DateTime.Now}: CSCLI Started"
            };

            File.WriteAllLines(LogfilePath, LogfileContent);
            BoxedText.Info("Log file created and written successfully.");

            string currentDirectory = startingDirectory;
            int currentStatus = 0;
            List<string> commandList = new() { };
            while (true)
            {
                Console.ResetColor();
                string command = "";
                if (runningInnerProgram == false)
                {
                    if (commandList.Count == 0)
                    {
                        CLI_Line.Load(currentDirectory, currentStatus);

                        command = Console.ReadLine();
                    }
                    else
                    {
                        command = commandList[0];
                        commandList.RemoveAt(0);
                    }
                }
                currentStatus = 0;
                if (command.Contains("&&"))
                {
                    try
                    {
                        List<string> tmpList = new(command.Split("&&"));
                        List<string> comandListItems = new();
                        foreach (string item in tmpList)
                        {
                            comandListItems.Add(item.Trim());
                        }
                        commandList.AddRange(comandListItems);
                    }
                    catch { }
                    continue;
                }

                if (command == null || command == "")
                {
                    command = NULL_VALUE;
                    continue;
                }
                else if (command.StartsWith("help"))
                {
                    Console.WriteLine("""
                        help: Displays information about available commands.
                        print: Writes the provided text to the console.
                        dir: Generates and displays a directory tree starting from the current directory.
                        ls: Lists files in the current directory.
                        cd: Changes the current directory to the specified directory.
                        read: Reads and displays the contents of a specified file.
                        clear: Clears the console screen.
                        fileInfo: Displays information about a specified file, such as creation time and last modified time.
                        version or -v: Displays the version of the Windows operating system.
                        run: Executes an external program specified by the provided file name.
                        grep: Searches for a specified term in all files within the current directory and displays the matching lines.
                        exit: Exits the program.
                        """);
                }
                else if (command.StartsWith("print"))
                {
                    Console.WriteLine(command.Substring(6).TrimStart());
                }
                else if (command.StartsWith("dir"))
                {
                    int depth = 2;
                    string depthInput = command.Substring(3).Trim();
                    if (!string.IsNullOrEmpty(depthInput))
                    {
                        if (!int.TryParse(depthInput, out depth))
                        {
                            BoxedText.Error($"Invalid depth value: {depthInput}");
                            continue;
                        }
                    }
                    if (depth < 0)
                    {
                        BoxedText.Error($"{depth} is a negative value, not valid for depth of search files/directories");
                        continue;
                    }
                    if (depth > 3)
                    {
                        BoxedText.Warn($"Depth of {depth} could take time to show all directories and files within depth {depth}, Do you want to continue?");
                        Console.Write("(y/n)> ");
                        char response = Console.ReadKey().KeyChar;
                        if (response == 'n')
                        {
                            BoxedText.Warn($"Declined, Default Depth (2) will be used instead of user-specified ({depth})");
                            depth = 2;
                        }
                        else if (response != 'y')
                        {
                            BoxedText.Error($"\'{response}\' is not a valid response");
                            continue;
                        }
                    }
                    DirTree.Gen(currentDirectory, maxDepth: depth);
                }
                else if (command.StartsWith("ls"))
                {
                    foreach (string item in Directory.GetFiles(currentDirectory))
                    {
                        Console.Write(item + " ");
                    }
                }
                else if (command.StartsWith("cd"))
                {
                    if (string.IsNullOrEmpty(command.Substring(2).TrimStart()))
                    {
                        BoxedText.Error("null Directory Error; No Directory Specified");
                        currentStatus = -1;
                        continue;
                    }

                    if (!command.EndsWith("\\"))
                    {
                        command = command + "\\";
                    }

                    string Dir = command.Substring(2).TrimStart();
                    string mainDir;

                    if (Dir == "..\\" || Dir == ".." || Dir == "../")
                    {
                        mainDir = Path.GetDirectoryName(currentDirectory);
                    }
                    else
                    {
                        mainDir = Path.Combine(currentDirectory, Dir);
                    }

                    if (Directory.Exists(mainDir))
                    {
                        currentDirectory = mainDir;
                    }
                    else
                    {
                        Console.WriteLine("Not a Valid Path\n");
                        currentStatus = -1;
                        continue;
                    }
                }

                else if (command.StartsWith("new"))
                {
                    if (string.IsNullOrEmpty(command.Substring(3).TrimStart()))
                    {
                        BoxedText.Error("no Arguments Error; Required argument 'Type' (dir/file)");
                        currentStatus = -1;
                        continue;
                    }

                    string[] arguments = command.Substring(3).TrimStart().Split(" ");
                    if (arguments.Length < 2)
                    {
                        BoxedText.Error("Invalid arguments. Usage: new <Type> <Name>");
                        currentStatus = -1;
                        continue;
                    }

                    string type = arguments[0].ToLower();
                    string name = arguments[1].TrimStart();

                    if (type == "dir" && !name.EndsWith("\\"))
                    {
                        name = name + "\\";
                    }

                    string path = Path.Combine(currentDirectory, name);

                    try
                    {
                        if (type == "dir")
                        {
                            Directory.CreateDirectory(path);
                        }
                        else if (type == "file")
                        {
                            File.WriteAllText(path, "");
                        }
                        else
                        {
                            BoxedText.Error($"Invalid type '{type}'. Type must be 'dir' or 'file'.");
                            currentStatus = -1;
                            continue;
                        }

                        if (type == "dir" && Directory.Exists(path))
                        {
                            BoxedText.Info($"Directory \'{name}\'");
                            currentDirectory = path;
                        }
                        else if (type == "file" && File.Exists(path))
                        {
                            BoxedText.Info("");
                        }
                        else
                        {
                            BoxedText.Error($"Unable to create {type} '{path}'.");
                            currentStatus = -1;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        BoxedText.Error($"Error creating {type} '{path}': {ex.Message}");
                        currentStatus = -1;
                    }
                }
                else if (command.StartsWith("ren"))
                {
                    if (string.IsNullOrEmpty(command.Substring(3).TrimStart()))
                    {
                        BoxedText.Error("no Arguments Error; Required arguments 'Type' (dir/file) and 'NewName'");
                        currentStatus = -1;
                        continue;
                    }

                    string[] arguments = command.Substring(3).TrimStart().Split(" ");
                    if (arguments.Length < 3)
                    {
                        BoxedText.Error("Invalid arguments. Usage: ren <Type> <CurrentName> <NewName>");
                        currentStatus = -1;
                        continue;
                    }

                    string type = arguments[0].ToLower();
                    string currentName = arguments[1].TrimStart();
                    string newName = arguments[2].TrimStart();

                    string currentPath = Path.Combine(currentDirectory, currentName);
                    string newPath = Path.Combine(currentDirectory, newName);

                    try
                    {
                        if (type == "dir")
                        {
                            if (!currentName.EndsWith("\\"))
                            {
                                currentName = currentName + "\\";
                            }

                            if (!newName.EndsWith("\\"))
                            {
                                newName = newName + "\\";
                            }

                            currentPath = Path.Combine(currentDirectory, currentName);
                            newPath = Path.Combine(currentDirectory, newName);

                            if (Directory.Exists(currentPath))
                            {
                                Directory.Move(currentPath, newPath);
                                BoxedText.Info($"Renamed directory from '{currentName}' to '{newName}'.");
                                currentDirectory = newPath;
                            }
                            else
                            {
                                BoxedText.Error($"Directory '{currentPath}' does not exist.");
                                currentStatus = -1;
                                continue;
                            }
                        }
                        else if (type == "file")
                        {
                            if (File.Exists(currentPath))
                            {
                                File.Move(currentPath, newPath);
                                BoxedText.Info($"Renamed file from '{currentName}' to '{newName}'.");
                            }
                            else
                            {
                                BoxedText.Error($"File '{currentPath}' does not exist.");
                                currentStatus = -1;
                                continue;
                            }
                        }
                        else
                        {
                            BoxedText.Error($"Invalid type '{type}'. Type must be 'dir' or 'file'.");
                            currentStatus = -1;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        BoxedText.Error($"Error renaming {type} '{currentName}': {ex.Message}");
                        currentStatus = -1;
                    }
                }
                else if (command.StartsWith("copy"))
                {
                    if (string.IsNullOrEmpty(command.Substring(4).TrimStart()))
                    {
                        BoxedText.Error("No arguments provided. Required arguments: 'Type' (dir/file), 'SourcePath', and 'DestinationPath'.");
                        currentStatus = -1;
                        continue;
                    }

                    string[] arguments = command.Substring(4).TrimStart().Split(" ");
                    if (arguments.Length < 3)
                    {
                        BoxedText.Error("Invalid arguments. Usage: copy <Type> <SourcePath> <DestinationPath>");
                        currentStatus = -1;
                        continue;
                    }

                    string type = arguments[0].ToLower();
                    string sourcePath = arguments[1].TrimStart();
                    string destinationPath = arguments[2].TrimStart();

                    string currentPath = Path.Combine(currentDirectory, sourcePath);
                    string newPath = Path.Combine(currentDirectory, destinationPath);
                    CopyFileOrDirectory(type, currentPath, newPath);
                    void CopyFileOrDirectory(string type, string sourcePath, string destinationPath)
                    {
                        try
                        {
                            if (type == "dir")
                            {
                                if (!Directory.Exists(sourcePath))
                                {
                                    BoxedText.Error($"Directory '{sourcePath}' does not exist.");
                                    currentStatus = -1;
                                    return;
                                }

                                if (!Directory.Exists(destinationPath))
                                    Directory.CreateDirectory(destinationPath);

                                foreach (string file in Directory.GetFiles(sourcePath))
                                {
                                    string fileName = Path.GetFileName(file);
                                    string destFile = Path.Combine(destinationPath, fileName);
                                    File.Copy(file, destFile, true);
                                }

                                foreach (string subdirectory in Directory.GetDirectories(sourcePath))
                                {
                                    string directoryName = Path.GetFileName(subdirectory);
                                    string destDirectory = Path.Combine(destinationPath, directoryName);
                                    CopyFileOrDirectory("dir", subdirectory, destDirectory);
                                }

                                BoxedText.Info($"Copied directory from '{sourcePath}' to '{destinationPath}'.");
                            }
                            else if (type == "file")
                            {
                                if (!File.Exists(sourcePath))
                                {
                                    BoxedText.Error($"File '{sourcePath}' does not exist.");
                                    currentStatus = -1;
                                    return;
                                }

                                File.Copy(sourcePath, destinationPath, true);
                                BoxedText.Info($"Copied file from '{sourcePath}' to '{destinationPath}'.");
                            }
                            else
                            {
                                BoxedText.Error($"Invalid type '{type}'. Type must be 'dir' or 'file'.");
                                currentStatus = -1;
                            }
                        }
                        catch (Exception ex)
                        {
                            BoxedText.Error($"Error copying {type} '{sourcePath}': {ex.Message}");
                            currentStatus = -1;
                        }
                    }
                }

                else if (command.StartsWith("del"))
                {
                    if (string.IsNullOrEmpty(command.Substring(3).TrimStart()))
                    {
                        BoxedText.Error("no Arguments Error; Required argument 'Type' (dir/file)");
                        currentStatus = -1;
                        continue;
                    }

                    string[] arguments = command.Substring(3).TrimStart().Split(" ");
                    if (arguments.Length < 2)
                    {
                        BoxedText.Error("Invalid arguments. Usage: new <Type> <Name>");
                        currentStatus = -1;
                        continue;
                    }

                    string type = arguments[0].ToLower();
                    string name = arguments[1].TrimStart();

                    if (type == "dir" && !name.EndsWith("\\"))
                    {
                        name = name + "\\";
                    }

                    string path = Path.Combine(currentDirectory, name);

                    if (type == "dir" && !Directory.Exists(path))
                    {
                        BoxedText.Error($"Directory \'{name}\' does NOT exist");
                        continue;
                    }
                    else if (type == "file" && !File.Exists(path))
                    {
                        BoxedText.Error($"File \'{path}\' does NOT exist");
                        continue;
                    }

                    try
                    {
                        if (type == "dir")
                        {
                            Directory.Delete(path, true);
                        }
                        else if (type == "file")
                        {
                            File.Delete(path);
                        }
                        else
                        {
                            BoxedText.Error($"Invalid type '{type}'. Type must be 'dir' or 'file'.");
                            currentStatus = -1;
                            continue;
                        }

                        if (type == "dir" && !Directory.Exists(path))
                        {
                            BoxedText.Info($"Directory \'{name}\' deleted");
                        }
                        else if (type == "file" && !File.Exists(path))
                        {
                            BoxedText.Info($"File \'{path}\' deleted");
                        }
                        else
                        {
                            BoxedText.Error($"Unable to Delete {type} '{path}'.");
                            currentStatus = -1;
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        BoxedText.Error($"Error creating {type} '{path}': {ex.Message}");
                        currentStatus = -1;
                    }
                }
                else if (command.StartsWith("read"))
                {
                    string arg = command.Substring(4).TrimStart();
                    if (string.IsNullOrEmpty(arg))
                    {
                        BoxedText.Error("null File Error; No File Specified");
                        currentStatus = -1;
                        continue;
                    }

                    string filePath = Path.Combine(currentDirectory, arg);

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"File Not Found '{arg}'");
                        currentStatus = -1;
                        continue;
                    }

                    Console.WriteLine($"\n==={arg}===");
                    Console.WriteLine(string.Join(Environment.NewLine, File.ReadAllLines(filePath)));
                    Console.WriteLine();
                    for (int i = 0; i < arg.Length + 6; i++)
                    {
                        Console.Write("=");
                    }
                    Console.WriteLine();
                }
                else if (command.StartsWith("clear"))
                {
                    Console.Clear();
                }
                else if (command.StartsWith("fileInfo"))
                {
                    string fileName = command.Substring(8).TrimStart();
                    if (!fileName.Contains(currentDirectory.Substring(0, 3)))
                    {
                        fileName = currentDirectory + fileName;
                    }

                    string[] e4 = { $"File Created on: {File.GetCreationTime(fileName)}",
                                    $"File Last Accessed on: {File.GetLastAccessTime(fileName)}",
                                    $"File Last edited on: {File.GetLastWriteTime(fileName)}" };

                    BoxedText.Arrays.Regular(e4);
                }
                else if (command.StartsWith("version") || command == "-v")
                {
                    OperatingSystem os = Environment.OSVersion;
                    Version version = os.Version;
                    BoxedText.Info($"Windows version: {version.Build}");
                }
                else if (command.StartsWith("run"))
                {
                    string fileName = command.Substring(3).TrimStart();
                    try
                    {
                        Process process = new Process();
                        process.StartInfo = new ProcessStartInfo(fileName);
                        Console.WriteLine($"Running \'{fileName}\'");
                        process.EnableRaisingEvents = true;
                        process.Exited += SecondProgramExited;
                        process.Start();
                        runningInnerProgram = true;
                    }
                    catch (FileNotFoundException fileNotFoundEx)
                    {
                        BoxedText.Error(fileNotFoundEx.Message);
                    }
                    catch (IOException InputOutputEx)
                    {
                        BoxedText.Error(InputOutputEx.Message);
                    }
                    catch (Exception Ex)
                    {
                        BoxedText.Error(Ex.Message);
                    }
                }
                else if (command.StartsWith("grep"))
                {
                    string searchTerm = command.Substring(5).TrimStart();
                    string[] files = Directory.GetFiles(currentDirectory);

                    foreach (string file in files)
                    {
                        string[] lines = File.ReadAllLines(file);

                        foreach (string line in lines)
                        {
                            if (line.Contains(searchTerm))
                            {
                                Console.Write($"{file}:\n");
                                int startIndex = line.IndexOf(searchTerm);
                                int searchTermLength = searchTerm.Length;

                                string word = "";
                                foreach (char line2 in line.ToCharArray())
                                {
                                    if (line2 == ' ')
                                    {
                                        Console.ResetColor();
                                        if (word.TrimStart() == searchTerm)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                        }
                                        if (word.StartsWith(" "))
                                        {
                                            Console.Write(word);
                                        }
                                        Console.ResetColor();
                                        word = " ";
                                    }
                                    else
                                    {
                                        word = word + line2;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (command.StartsWith("log"))
                {
                    List<string> logLines = new List<string>
                    {
                        "Log File Below;\n"
                    };
                    logLines.AddRange(File.ReadAllLines(LogfilePath));
                    BoxedText.Info(string.Join(Environment.NewLine, logLines));
                }
                else if (command.StartsWith("yes"))
                {
                    string main = command.Substring(3).TrimStart();
                    for (int i = 0; i < 100000; i++)
                    {
                        commandList.Add(main);
                    }
                }
                else if (command.StartsWith("var"))
                {
                    if (command.Substring(3).Trim().Length > 0)
                    {
                        string varName = command.Substring(3).TrimStart();
                        if (command.Split(" ").Length > 2)
                        {
                            string varData = varName.Split(" ")[1].TrimStart();
                            varName = varName.Split(" ")[0].Trim();
                            if (variables.ContainsKey(varName))
                            {
                                variables.Remove(varName);
                            }
                            variables.Add(varName, varData);
                        }
                        else
                        {
                            varName = varName.Trim();
                            if (variables.ContainsKey(varName))
                            {
                                Console.WriteLine($"Key: {varName}, Value: {variables[varName]}");
                            }
                            else
                            {
                                BoxedText.Error($"Variable \'{varName}\' doesnt exist!");
                            }
                        }
                    }
                }
                else if (IsValidExpression(command))
                {
                    NCalc.Expression expr = new NCalc.Expression(command);
                    Console.WriteLine(Convert.ToDouble(expr.Evaluate()));
                }
                else if (command.StartsWith("find"))
                {
                    FileFindGUI.Show(currentDirectory);
                }
                else if (command.StartsWith("exit"))
                {
                    BoxedText.Info("\n!!!EXITING CSCLI!!!");
                    Environment.Exit(0);
                }
                else
                {
                    BoxedText.Error($"Error; '{command}' is not a valid command");
                    currentStatus = -1;
                }
                string status = currentStatus == -1 ? "Failed" : "Successful";
                File.WriteAllLines(LogfilePath, new string[] { string.Join(Environment.NewLine, File.ReadAllLines(LogfilePath)), $"{DateTime.Now}: Ran \'{command}\', Args \'{string.Join(",", command.Split(" ")[1..])}\', Status: {status}" });
                Console.ResetColor();
                Console.WriteLine();
            }
            void SecondProgramExited(object sender, EventArgs e)
            {
                runningInnerProgram = false;
                Console.Clear();
            }
            bool IsValidExpression(string input)
            {
                // List of valid operators
                char[] operators = { '+', '-', '*', '/' };

                // Remove whitespace from the input
                input = input.Replace(" ", "");

                // Check if the input contains only valid characters
                foreach (char c in input)
                {
                    if (!Char.IsDigit(c) && !operators.Contains(c) && c != '(' && c != ')')
                    {
                        return false;
                    }
                }

                // Check if the input starts and ends with a digit or closing parenthesis
                if (!Char.IsDigit(input[0]) && input[0] != '(' || !Char.IsDigit(input[input.Length - 1]) && input[input.Length - 1] != ')')
                {
                    return false;
                }

                // Check if the input contains consecutive operators or parentheses
                for (int i = 0; i < input.Length - 1; i++)
                {
                    char currentChar = input[i];
                    char nextChar = input[i + 1];

                    if ((operators.Contains(currentChar) || currentChar == '(') && (operators.Contains(nextChar) || nextChar == ')'))
                    {
                        return false;
                    }
                }

                // Check if parentheses are balanced
                int parenthesesCount = 0;
                foreach (char c in input)
                {
                    if (c == '(')
                    {
                        parenthesesCount++;
                    }
                    else if (c == ')')
                    {
                        parenthesesCount--;
                        if (parenthesesCount < 0)
                        {
                            return false;
                        }
                    }
                }

                return parenthesesCount == 0;
            }

        }
    }
}
