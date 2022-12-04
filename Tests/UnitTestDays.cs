using Main;
using Main.Days;
using Main.Exercises;

namespace Tests
{
    [TestClass]
    public class UnitTestDays
    {
        private void TestPartOne(Exercise day, long result)
        {
            // Act
            var actual = day.SolvePartOne();

            // Assert
            Assert.AreEqual(result, actual);
        }

        private void TestPartTwo(Exercise day, long result)
        {
            // Act
            var actual = day.SolvePartTwo();

            // Assert
            Assert.AreEqual(result, actual);
        }

        private void TestDay(Exercise day, long partOne, long partTwo)
        {
            // Arrange
            day.SetUp();

            // Act / Assert
            TestPartOne(day, partOne);
            TestPartTwo(day, partTwo);

            day.TearDown();
        }


        [TestMethod]
        public void TestDay01()
        {
            TestDay(new Day01(), 67658, 200158);
        }

        [TestMethod]
        public void TestDay02()
        {
            TestDay(new Day02(), 12645, 11756);
        }

        [TestMethod]
        public void TestDay03()
        {
            TestDay(new Day03(), 7850, 2581);
        }

        [TestMethod]
        public void TestDay04()
        {
            TestDay(new Day04(), 518, 909);
        }
    }
}