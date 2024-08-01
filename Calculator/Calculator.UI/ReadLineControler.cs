using System.Text;

namespace Calculator.UI
{
    public static class ReadLineControler
    {
        public static (string stringResult, StatusOperation status) ReadLineWithStatusOperation(TypeOperation typeOperation)
        {
            if (typeOperation == TypeOperation.Problem)
            {
                return ReadLinProblem();
            }
            else if (typeOperation == TypeOperation.FilePath)
            {
                return ReadLineFilePath();
            }
            else if (typeOperation == TypeOperation.DirectoryPathOrFileName)
            {
                return ReadLineDirectoryOrFileName();
            }
            else
            {
                throw new Exception();
            }
        }

        private static (string stringResult, StatusOperation status) ReadLinProblem()
        {
            var result = "";
            var status = StatusOperation.Exit;
            var buffer = new StringBuilder();
            var typeOfOperations = new char[7] { '*', '/', '+', '-', '(', ')', ' ' };
            var info = Console.ReadKey(true);

            while (true)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        buffer.Remove(buffer.Length - 1, 1);
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                }
                else if (Int32.TryParse(new string(info.KeyChar, 1), out var count) && count >= 0)
                {
                    if (count > 0 || (buffer.Length > 0 && count == 0))
                    {
                        Console.Write(info.KeyChar);
                        buffer.Append(info.KeyChar);
                    }
                }
                else if (typeOfOperations.Contains(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    buffer.Append(info.KeyChar);
                }
                //end buttons 
                else if (info.Key == ConsoleKey.Enter && buffer.Length != 0)
                {
                    result = buffer.ToString();
                    status = StatusOperation.Enter;
                    break;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    result = "";
                    status = StatusOperation.Exit;
                    break;
                }
                else if (info.Key == ConsoleKey.Tab)
                {
                    result = "";
                    status = StatusOperation.SwitchOperation;
                    Console.CursorLeft = 0;
                    Console.Write(result.PadRight(buffer.Length));
                    Console.CursorLeft = 0;
                    break;
                }
                //
                info = Console.ReadKey(true);
            }

            return (result, status);
        }

        private static (string stringResult, StatusOperation status) ReadLineFilePath()
        {
            var result = "";
            var status = StatusOperation.Exit;
            var buffer = new StringBuilder();
            var info = Console.ReadKey(true);

            while (true)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        buffer.Remove(buffer.Length - 1, 1);
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                }
                //end buttons 
                else if (info.Key == ConsoleKey.Enter && buffer.Length != 0)
                {
                    result = buffer.ToString();
                    status = StatusOperation.Enter;
                    break;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    result = "";
                    status = StatusOperation.Exit;
                    break;
                }
                else if (info.Key == ConsoleKey.Tab)
                {
                    result = "";
                    status = StatusOperation.SwitchOperation;
                    Console.CursorLeft = 0;
                    Console.Write(result.PadRight(buffer.Length));
                    Console.CursorLeft = 0;
                    break;
                }
                //
                else
                {
                    Console.Write(info.KeyChar);
                    buffer.Append(info.KeyChar);
                }
                info = Console.ReadKey(true);
            }

            return (result, status);
        }

        public static (bool isNeedToCreateFile, StatusOperation status) ReadLineSaveFileQuestion()
        {
            bool result;
            var status = StatusOperation.Exit;
            var info = Console.ReadKey(true);

            while (true)
            {
                if (Int32.TryParse(new string(info.KeyChar, 1), out var count))
                {
                    if (count == 1)
                    {
                        result = true;
                        Console.Write(info.KeyChar);
                        status = StatusOperation.Enter;
                        break;
                    }
                    else if (count == 0)
                    {
                        result = false;
                        Console.Write(info.KeyChar);
                        status = StatusOperation.Enter;
                        break;
                    }
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    result = false;
                    status = StatusOperation.Exit;
                    break;
                }
                info = Console.ReadKey(true);
            }
            Console.WriteLine();
            return (result, status);
        }

        private static (string stringResult, StatusOperation status) ReadLineDirectoryOrFileName()
        {

            var result = "";
            var status = StatusOperation.Exit;
            var buffer = new StringBuilder();
            var info = Console.ReadKey(true);

            while (true)
            {
                if (info.Key == ConsoleKey.Backspace)
                {
                    if (buffer.Length > 0)
                    {
                        buffer.Remove(buffer.Length - 1, 1);
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                }
                //end buttons 
                else if (info.Key == ConsoleKey.Enter && buffer.Length != 0)
                {
                    result = buffer.ToString();
                    status = StatusOperation.Enter;
                    break;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    result = "";
                    status = StatusOperation.Exit;
                    break;
                }
                //
                else
                {
                    Console.Write(info.KeyChar);
                    buffer.Append(info.KeyChar);
                }
                info = Console.ReadKey(true);
            }

            return (result, status);
        }

    }
}
