using System.Text;

namespace Calculator.BL
{
    public class Calculator
    {
        public ResultSolveProblem SolveProblem(string problem)
        {
            if (problem == null)
            {
                return new ResultSolveProblem(problem, 0, true, "Exception. Wrong input.");
            }
            problem = problem.Replace(" ", "");

            var result = DivideProblem(problem);
            var numbers = result.numbers;
            var operations = result.operations;
            if (numbers.Count == 1 && operations.Count == 0)
            {
                return new ResultSolveProblem(problem, numbers[0], false, "");
            }
            else if (numbers.Count == 0 || operations.Count == 0 || numbers.Count <= operations.Count)
            {
                return new ResultSolveProblem(problem, 0, true, "Exception. Wrong input.");
            }

            var priorityOperations = new List<char[]>();
            priorityOperations.Add(new char[2] { '*', '/' });
            priorityOperations.Add(new char[2] { '+', '-' });

            foreach (var typeOfOperation in priorityOperations)
            {
                for (int i = 0; i < operations.Count; i++)
                {
                    if (typeOfOperation.Contains(operations[i]))
                    {
                        var resultOperation = MakeOperation(numbers[i], numbers[i + 1], operations[i]);
                        if (resultOperation.hasError)
                        {
                            return new ResultSolveProblem(problem, 0, true, "Exception. Wrong input.");
                        }
                        numbers[i] = resultOperation.result;
                        numbers.RemoveAt(i + 1);
                        operations.RemoveAt(i);
                        i--;
                    }
                }
            }
            return new ResultSolveProblem(problem, numbers[0], false, "");
        }


        private (List<int> numbers, List<char> operations) DivideProblem(string problem)
        {
            var calculatorData = new CalculatorData();
            foreach (var item in problem)
            {
                calculatorData.Item = item;
                if (calculatorData.StartSubProblem)
                {
                    if (!TryDivideSubProblem(calculatorData))
                    {
                        return (new List<int>(), new List<char>());
                    }
                }
                else if (calculatorData.Item == '(')
                {
                    StartSubProblem(calculatorData);
                }
                else if (Char.IsNumber(calculatorData.Item))
                {
                    DivideNumber(calculatorData);
                }
                else if (calculatorData.TypeOfOperations.Contains(calculatorData.Item))
                {
                    DivideOperation(calculatorData);
                }
                else
                {
                    return (new List<int>(), new List<char>());
                }
                calculatorData.PreviousItem = calculatorData.Item;
            }
            if (calculatorData.CountOpenParentheses != 0)
            {
                return (new List<int>(), new List<char>());
            }
            return (calculatorData.Numbers, calculatorData.Operations);
        }
        private bool TryDivideSubProblem(CalculatorData calculatorData)
        {
            if (calculatorData.Item == '(')
            {
                calculatorData.CountOpenParentheses++;
            }
            else if (calculatorData.Item == ')')
            {
                calculatorData.CountOpenParentheses--;
            }

            if (calculatorData.CountOpenParentheses != 0)
            {
                calculatorData.SubProblem.Append(calculatorData.Item);
            }
            else
            {
                var resultSubProblem = SolveProblem(calculatorData.SubProblem.ToString());
                if (resultSubProblem.HasError)
                {
                    return false;
                }
                calculatorData.Numbers.Add(resultSubProblem.Result);
                calculatorData.StartSubProblem = false;
                calculatorData.PreviousIsNumber = true;
            }
            return true;
        }
        private void StartSubProblem(CalculatorData calculatorData)
        {
            if (calculatorData.PreviousItem != '\0' && (Char.IsNumber(calculatorData.PreviousItem) || calculatorData.PreviousItem == ')'))
            {
                calculatorData.Operations.Add('*');
            }
            calculatorData.SubProblem.Clear();
            calculatorData.StartSubProblem = true;
            calculatorData.CountOpenParentheses = 1;
            calculatorData.PreviousIsNumber = false;
        }
        private void DivideNumber(CalculatorData calculatorData)
        {
            if (!calculatorData.PreviousIsNumber)
            {
                var number = Convert.ToInt32(calculatorData.Item.ToString());
                calculatorData.Numbers.Add(number);
            }
            else
            {
                calculatorData.Numbers[calculatorData.Numbers.Count - 1] =
                  calculatorData.Numbers[calculatorData.Numbers.Count - 1] * 10 + Convert.ToInt32(calculatorData.Item.ToString());
            }
            calculatorData.PreviousIsNumber = true;
        }
        private void DivideOperation(CalculatorData calculatorData)
        {
            if (calculatorData.PreviousItem == '\0' && calculatorData.Item == '-')
            {
                calculatorData.Numbers.Add(0);
            }
            calculatorData.Operations.Add(calculatorData.Item);
            calculatorData.PreviousIsNumber = false;
        }


        private (int result, bool hasError) MakeOperation(int number1, int number2, char operation)
        {
            if (operation == '*')
            {
                number1 *= number2;
            }
            else if (operation == '/')
            {
                if (number2 == 0)
                {
                    return (0, true);
                }
                number1 /= number2;
            }
            else if (operation == '+')
            {
                number1 += number2;
            }
            else if (operation == '-')
            {
                number1 -= number2;
            }
            else
            {
                return (0, true);
            }
            return (number1, false);
        }
        public (List<ResultSolveProblem> resultSolveProblems, bool fileNotExists, string errorMessage) SolveFileOfProblems(string filePath)
        {
            var result = new List<ResultSolveProblem>();
            var resultChekFile = FileControler.FileISCorrect(filePath);
            if (!resultChekFile.isCorrect)
            {
                return (result, true, resultChekFile.errorMessage);
            }

            // Open the stream and read it back.
            using (var streamReader = File.OpenText(filePath))
            {
                var textString = "";
                while ((textString = streamReader.ReadLine()) != null)
                {
                    result.Add(SolveProblem(textString));
                }
            }
            return (result, false, "");
        }
        public void CreateResultFile(List<ResultSolveProblem> resultSolveProblems, string path)
        {
            // Create the file.
            using (FileStream fs = File.Create(path))
            {
                var text = string.Join("\n", resultSolveProblems.Select(result => result.FullProblem).ToList());
                char[] value = text.ToCharArray();
                fs.Write(Encoding.UTF8.GetBytes(value), 0, value.Length);
            }
        }


        private class CalculatorData
        {
            public List<int> Numbers { get; set; }
            public List<char> Operations { get; set; }
            public bool PreviousIsNumber { get; set; }
            public char[] TypeOfOperations { get; set; }
            public char Item { get; set; }
            public char PreviousItem { get; set; }
            public StringBuilder SubProblem { get; set; }
            public bool StartSubProblem { get; set; }
            public int CountOpenParentheses { get; set; }

            public CalculatorData()
            {
                Numbers = new List<int>();
                Operations = new List<char>();
                PreviousIsNumber = false;
                TypeOfOperations = new char[4] { '*', '/', '+', '-' };
                Item = '\0';
                PreviousItem = '\0';
                SubProblem = new StringBuilder();
                StartSubProblem = false;
                CountOpenParentheses = 0;
            }

        }
    }
}
