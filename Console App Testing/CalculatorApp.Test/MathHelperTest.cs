using System.Reflection;

namespace CalculatorApp.Test
{
    public class MathHelperTest
    {
        [Fact]
        public void IsEvenTest()
        {
            var calculator = new MathFormulas(); // Use the method derived from calculator

            int x = 1; // Assign an odd number to x
            int y = 2; // Assign an even number to y

            // Assign the result of IsEven method
            var xResult = calculator.IsEven(x); 
            var yResult = calculator.IsEven(y);

            Assert.False(xResult); // It should be false because x is an odd number
            Assert.True(yResult); // It should be true because y is an even number
        }

        
        [Theory] // Theory because parameter needed
        [InlineData(1, 2, 1)] // Parameters is given here
        [InlineData(1, 3, 2)]
        public void DiffTest(int x, int y, int expectedValue)
        {
            var calculator = new MathFormulas();

            var result = calculator.Diff(x, y);

            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(new int[3] {1, 2, 3}, 6)] // Parameters is given here
        [InlineData(new int[3] {-4, -6, -10}, -20)] // Parameters is given here
        public void SumTest(int[] values, int expectedValue)
        {
            var calculator = new MathFormulas();
            var result = calculator.Sum(values);
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(1, 1, 2)] // Test data
        [InlineData(-1, -1, -2)] // Test data
        public void AddTest(int x, int y, int expectedValue)
        {
            var calculator = new MathFormulas();
            var result = calculator.Add(x, y);
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(new int[3] {1, 2, 3}, 2)] // Test data
        [InlineData(new int[3] {1, 4, 10}, 5)] // Test data
        public void AverageTest(int[] values, int expectedValue)
        {
            var calculator = new MathFormulas();
            var result = calculator.Average(values);
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [MemberData(nameof(MathFormulas.Data), MemberType = typeof(MathFormulas))] // Get testable data and set its format
        public void Add_MemberData_Test(int x, int y, int expectedValue)
        {
            var calculator = new MathFormulas();
            var result = calculator.Add(x, y);
            Assert.Equal(expectedValue, result);
        }


        [Theory(Skip = "Skipped because too many tests.")]
        [ClassData(typeof(MathFormulas))]
        public void Add_ClassData_Test(int x, int y, int expectedValue)
        {
            var calculator = new MathFormulas();
            var result = calculator.Add(x, y);
            Assert.Equal(expectedValue, result);
        }
    }
}