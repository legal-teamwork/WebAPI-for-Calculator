using System.Diagnostics;

namespace Tests
{
    class Calculator
    {
        public string Calculate(string expression)
        {
            File.WriteAllText("input.txt", expression);
            Process.Start("Parser.exe").WaitForExit();
            return File.ReadAllText("output.txt").Trim();
        }
    }

    public class Tests
    {
        Calculator calculator;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Directory.SetCurrentDirectory("..\\..\\..\\..");
        }

        [SetUp]
        public void Setup()
        {
            calculator = new Calculator();
        }

        private void Test(string expression, string expected)
        {
            Assert.That(calculator.Calculate(expression), Is.EqualTo(expected));
        }

        [Test]
        public void CorrectInput()
        {
            Test("1+2", "3");
            Test("3*2", "6");
            Test("3/2", "1.5");
            Test("1+2+3", "6");
            Test("5-2-1", "2");
            Test("4*3*2", "24");
            Test("12/3/2", "2");

            Test("1-2", "-1");
            Test("3*(-2)", "-6");
            Test("3/(-2)", "-1.5");


            Test("-(1+2)", "-3");
            Test("(1+2)-(1+2)", "0");
            Test("-((1+2)-(3+2))", "2");
            Test("1+(2)", "3");
            Test("1+(-2)", "-1");

            Test("+1+1", "2");
            Test("+1-1", "0");
            Test("1", "1");
            Test("+1", "1");
            Test("-1", "-1");
            Test("(1)", "1");
            Test("(+1)", "1");
            Test("(-1)", "-1");
            Test("((1))", "1");
            Test("(+(-1))", "-1");
            Test("(-(+1))", "-1");
            Test("(-(-1))", "1");
            Test("-(-(-1))", "-1");
            Test("(((((((-1)))))))", "-1");

        }

        [Test]
        public void IncorrectInput()
        {
            Test("+", "Err");
            Test("-", "Err");
            Test("*", "Err");
            Test("/", "Err");
            Test("1+", "Err");
            Test("1-", "Err");
            Test("1*", "Err");
            Test("1/", "Err");
            Test("1+1+", "Err");
            Test("+1+1+", "Err");


            Test("()", "Err");
            Test("(+)", "Err");
            Test("(-)", "Err");
            Test("(*)", "Err");
            Test("(/)", "Err");

            Test("(", "Err");
            Test(")", "Err");
            Test("((1+1)", "Err");
            Test("(1+1))", "Err");
            Test(")1+1", "Err");
            Test("(1+1", "Err");
            Test("1(+1)", "Err");
            Test("-1+()1+1", "Err");
            Test("(((((((-1))))))", "Err");
            Test("3*-2", "Err");
            Test("3/-2", "Err");
            Test("3--2", "Err");
        }
    }
}
