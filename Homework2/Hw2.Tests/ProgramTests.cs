using Hw2;
using Hw2_Console;
using Xunit;

namespace Hw2Tests
{
    public class ProgramTests
    {
        [Theory]
        [InlineData(15, 5, "+", 20)]
        [InlineData(15, 5, "-", 10)]
        [InlineData(15, 5, "*", 75)]
        [InlineData(15, 5, "/", 3)]
        public void TestAllOperations(int value1, int value2, string operation, int expectedValue)
        {
            // arrange
            var args = new[] { value1.ToString(), operation, value2.ToString() };

            //act
            Program.Main(args);
            var actual = Program.GetResult();

            //assert
            Assert.Equal(expectedValue, actual);
        }
        
        [Theory]
        [InlineData("f", "+", "3")]
        [InlineData("3", "+", "f")]
        [InlineData("a", "+", "f")]
        public void TestParserWrongValues(string val1, string operation, string val2)
        {
            // arrange
            var args = new[] { val1, operation, val2 };
            
            //assert
            Assert.Throws<ArgumentException>(() => Program.Main(args));
        }
        
        [Fact]
        public void TestParserWrongOperation()
        {
            // arrange
            var args = new[] { "3", ".", "4" };
            
            //assert
            Assert.Throws<InvalidOperationException>(() => Program.Main(args));
        }

        [Fact]
        public void TestParserWrongLength()
        {
            // arrange
            var args = new[] { "3", ".", "4", "5" };
            
            //assert
            Assert.Throws<ArgumentException>(() => Program.Main(args));
        }
    }
}