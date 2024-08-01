
namespace Calculator.BL
{
    public class ResultSolveProblem
    {
        public readonly string Problem;
        public readonly int Result;
        public readonly bool HasError;
        public readonly string ErrorMessage;
        public readonly string FullProblem;

        public ResultSolveProblem(string problem, int result, bool haveError, string errorMessage)
        {
            Problem = problem;
            Result = result;
            HasError = haveError;
            ErrorMessage = errorMessage;
            if (!haveError)
            {
                FullProblem = problem + " = " + result;
            }
            else
            {
                FullProblem = problem + " = " + errorMessage;
            }

        }
    }
}