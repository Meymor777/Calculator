using Calculator.BL;

namespace Calculator.UI
{
    public static class PaintToConsoleControler
    {

        public static void PaintProblem(ResultSolveProblem result)
        {
            Console.WriteLine(result.FullProblem);
        }

        public static void PaintFileProblems((List<ResultSolveProblem> resultSolveProblems, bool fileNotExists, string errorMessage) result, string filePath)
        {
            if (!result.fileNotExists)
            {
                Console.WriteLine(filePath + " :");
                Console.WriteLine();
                foreach (var item in result.resultSolveProblems)
                {
                    Console.WriteLine(item.FullProblem);
                }
            }
            else
            {
                Console.WriteLine("{0} - {1}", filePath, result.errorMessage);
            }
        }

    }
}
