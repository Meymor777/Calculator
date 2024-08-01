using Calculator.BL;

namespace Calculator.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WriteInstructionInConsole();
            EnterArgs(args);
            var typeOperation = TypeOperation.Problem;

            while (true)
            {
                var message = $"Enter [{typeOperation}]:";
                Console.WriteLine(message.PadRight(50));
                var resultReadLine = ReadLineControler.ReadLineWithStatusOperation(typeOperation);

                if (resultReadLine.status == StatusOperation.Exit)
                {
                    break;
                }
                else if (resultReadLine.status == StatusOperation.SwitchOperation)
                {
                    typeOperation = typeOperation == TypeOperation.Problem ? TypeOperation.FilePath : TypeOperation.Problem;
                    Console.CursorTop--;
                    continue;
                }
                else if (resultReadLine.status == StatusOperation.Enter)
                {
                    if (typeOperation == TypeOperation.Problem)
                    {
                        EnterProblem(resultReadLine);
                    }
                    else if (typeOperation == TypeOperation.FilePath)
                    {
                        if (!EnterFilePathWithBreak(resultReadLine))
                        {
                            return;
                        }
                    }
                    Console.WriteLine();
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private static void WriteInstructionInConsole()
        {
            var emptyString = "";
            Console.WriteLine("INSTRUCTIONS:");
            Console.WriteLine(emptyString.PadLeft(75, '-'));
            Console.WriteLine("Press ENTER to get result from entered problem or file path");
            Console.WriteLine("Press TAB to change input between problem and file path");
            Console.WriteLine("Press ESC to close program");
            Console.WriteLine(emptyString.PadLeft(75, '-'));
            Console.WriteLine();
        }

        private static void EnterProblem((string stringResult, StatusOperation status) resultReadLine)
        {
            var problem = resultReadLine.stringResult;
            var calculator = new BL.Calculator();
            var result = calculator.SolveProblem(problem);
            Console.CursorLeft = 0;
            PaintToConsoleControler.PaintProblem(result);
        }

        private static bool EnterFilePathWithBreak((string stringResult, StatusOperation status) resultReadLine)
        {
            var filePath = resultReadLine.stringResult;
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems(filePath);
            Console.CursorLeft = 0;
            PaintToConsoleControler.PaintFileProblems(result, filePath);
            if (!result.fileNotExists)
            {
                return CreateFileInConsoleWithBreak(result.resultSolveProblems);
            }
            return true;
        }

        private static bool CreateFileInConsoleWithBreak(List<ResultSolveProblem> resultSolveProblems)
        {
            while (true)
            {
                Console.WriteLine();
                Console.Write("Do you want to save this result? (1 - Yes, 0 - No): ");
                var saveDialodResult = ReadLineControler.ReadLineSaveFileQuestion();
                if (saveDialodResult.status == StatusOperation.Exit)
                {
                    return false;
                }

                if (saveDialodResult.isNeedToCreateFile)
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter directory path:");
                    var directoryEnterResult = ReadLineControler.ReadLineWithStatusOperation(TypeOperation.DirectoryPathOrFileName);
                    if (directoryEnterResult.status == StatusOperation.Enter)
                    {
                        if (FileControler.DirectoryISCorrect(directoryEnterResult.stringResult))
                        {
                            Console.WriteLine();
                            Console.WriteLine("Enter file name:");
                            var fileNameEnterResult = ReadLineControler.ReadLineWithStatusOperation(TypeOperation.DirectoryPathOrFileName);
                            if (fileNameEnterResult.status == StatusOperation.Enter)
                            {
                                var newFilePath = directoryEnterResult.stringResult + "\\" + fileNameEnterResult.stringResult + ".txt";
                                var calculator = new BL.Calculator();
                                calculator.CreateResultFile(resultSolveProblems, newFilePath);
                                Console.WriteLine();
                                break;
                            }
                            else if (fileNameEnterResult.status == StatusOperation.Exit)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.CursorTop--;
                            Console.WriteLine("{0} - directory doesn't exist", directoryEnterResult.stringResult);
                        }
                    }
                    else if (directoryEnterResult.status == StatusOperation.Exit)
                    {
                        return false;
                    }
                }
                else
                {
                    break;
                }
            }
            return true;
        }

        private static void EnterArgs(string[] args)
        {
            var typeOperation = TypeOperation.FilePath;
            foreach (var filePath in args)
            {
                var message = $"Enter [{typeOperation}]:";
                Console.WriteLine(message.PadRight(50));
                var calculator = new BL.Calculator();
                var result = calculator.SolveFileOfProblems(filePath);
                PaintToConsoleControler.PaintFileProblems(result, filePath);
                Console.WriteLine();
            }
        }
    }
}
