namespace Calculator.Test
{
    public class Tests
    {
        private string _directoryWithTestFiles { get; set; }
        private string _directoryToResultsFiles { get; set; }
        private List<int[]> _listOfResults { get; set; }

        [SetUp]
        public void Setup()
        {
            var currentDirectory = Environment.CurrentDirectory;
            var workingDirectory = Directory.GetParent(currentDirectory).Parent.FullName;
            _directoryWithTestFiles = Directory.GetParent(workingDirectory).Parent.FullName + "\\Calculator.Test\\TestFiles";
            InitializeListOfReults();
        }

        private void InitializeListOfReults()
        {
            _listOfResults = new List<int[]>();
            _listOfResults.Add(new int[20] { 12, 25, 28, 55, 38, 39, 24, 26, 41, 33, 32, 20, 44, 19, 56, 29, 47, 29, 48, 51 });
            _listOfResults.Add(new int[20] { 63, 60, 42, 59, 52, 73, 69, 71, 72, 59, 59, 80, 86, 89, 80, 74, 92, 102, 90, 90 });
            _listOfResults.Add(new int[20] { 80, 113, 99, 103, 89, 123, 111, 122, 98, 119, 134, 120, 120, 125, 127, 140, 129, 139, 156, 145 });
            _listOfResults.Add(new int[20] { 138, 143, 171, 153, 148, 156, 178, 166, 158, 161, 171, 194, 136, 198, 180, 186, 178, 183, 210, 197 });
            _listOfResults.Add(new int[20] { 412, 270, 587, 424, 405, 770, 604, 486, 628, 489, 484, 709, 774, 337, 639, 682, 400, 611, 704, 659 });
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestFileWork(int numberTest)
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems(_directoryWithTestFiles + "\\TestFile" + numberTest + ".txt");
            var listOfResult = _listOfResults[numberTest - 1];
            for (int i = 0; i < listOfResult.Length; i++)
            {
                Assert.AreEqual(listOfResult[i], result.resultSolveProblems[i].Result);
            }
        }

        [Test]
        public void TestFileWithError()
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems(_directoryWithTestFiles + "\\TestFile6.txt");
            var listOfResult = new (int result, bool error)[20]
            {
                (30, false), (0, true), (62, false), (0, true), (405, false),
                (770, false), (604, false), (0, true), (628, false), (489, false),
                (484, false), (709, false), (774, false), (337, false), (639, false),
                (0, true), (400, false), (611, false), (0, true), (0, true)
            };
            for (int i = 0; i < listOfResult.Length; i++)
            {
                Assert.AreEqual(listOfResult[i].result, result.resultSolveProblems[i].Result);
                Assert.AreEqual(listOfResult[i].error, result.resultSolveProblems[i].HasError);
            }
        }

        [Test]
        public void TestFilePathNull()
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems(null);
            Assert.AreEqual(true, result.fileNotExists);
        }

        [Test]
        public void TestFilePathNotExist()
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems("not exist");
            Assert.AreEqual(true, result.fileNotExists);
        }

        [Test]
        public void TestFilePathEmpty()
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveFileOfProblems(_directoryWithTestFiles + "\\TestFileEmpty.txt");
            Assert.AreEqual(true, result.fileNotExists);
        }

        [TestCase("(6*6)-((16/4)+2)+3", 33, false)]
        [TestCase("(10*8)-(24+6)+(5*2)", 60, false)]
        [TestCase("(12*13)-((78/6)+2)+4", 145, false)]
        [TestCase("(9*15)+(90/6)-(12*1)", 138, false)]
        [TestCase("((20*3)-(60/5)+15)(9+2)-((36-3)/3)", 682, false)]
        [TestCase("((15*3)-(45/5)+10", 0, true)]
        public void TestOrdinaryWork(string problem, int resultProblem, bool haveError)
        {
            var calculator = new BL.Calculator();
            var result = calculator.SolveProblem(problem);
            Assert.AreEqual(resultProblem, result.Result);
            Assert.AreEqual(haveError, result.HasError);
        }
    }
}