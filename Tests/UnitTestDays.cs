using Main;
using Main.Days;
using Main.Exercises;

namespace Tests
{
    [TestClass]
    public class UnitTestDays
    {
        private void TestPartOne(Exercise day, object result)
        {
            // Act
            var actual = day.SolvePartOne();

            // Assert
            Assert.AreEqual(result, actual);
        }

        private void TestPartTwo(Exercise day, object result)
        {
            // Act
            var actual = day.SolvePartTwo();

            // Assert
            Assert.AreEqual(result, actual);
        }

        private void TestDay(Exercise day, object partOne, object partTwo)
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
            TestDay(new Day03(), 7850L, 2581L);
        }

        [TestMethod]
        public void TestDay04()
        {
            TestDay(new Day04(), 518L, 909L);
        }

        [TestMethod]
        public void TestDay05()
        {
            TestDay(new Day05(), "FRDSQRRCD", "HRFTQVWNN");
        }

        [TestMethod]
        public void TestDay06()
        {
            TestDay(new Day06(), 1356, 2564);
        }

        [TestMethod]
        public void TestDay07()
        {
            TestDay(new Day07(), 1367870L, 549173L);
        }
    }
}