using Main;
using Main.Days;
using Main.Exercises;

namespace Tests
{
    [TestClass]
    public class UnitTestDays
    {
        public void TestPartOne(Exercise day, long result)
        {
            // Act
            var actual = day.SolvePartOne();

            // Assert
            Assert.AreEqual(result, actual);
        }

        public void TestPartTwo(Exercise day, long result)
        {
            // Act
            var actual = day.SolvePartTwo();

            // Assert
            Assert.AreEqual(result, actual);
        }


        [TestMethod]
        public void TestDay01()
        {
            // Arrange
            var day = new Day01();
            day.SetUp();

            // Act / Assert
            TestPartOne(day, 67658);
            TestPartTwo(day, 200158);

            day.TearDown();
        }

        [TestMethod]
        public void TestDay02()
        {
            // Arrange
            var day = new Day02();
            day.SetUp();

            // Act / Assert
            TestPartOne(day, 12645);
            TestPartTwo(day, 11756);

            day.TearDown();
        }
    }
}