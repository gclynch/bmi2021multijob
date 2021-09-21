using Microsoft.VisualStudio.TestTools.UnitTesting;
using BMICalculator;

namespace bmiUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            BMI bmi = new BMI() { WeightStones = 12, WeightPounds = 0, HeightFeet = 5, HeightInches = 10 };
            Assert.AreEqual(bmi.BMICategory, BMICategory.Normal);
        }

        [TestMethod]
        public void TestMethod2()
        {
            BMI bmi = new BMI() { WeightStones = 15, WeightPounds = 0, HeightFeet = 5, HeightInches = 10 };
            Assert.AreEqual(bmi.BMICategory, BMICategory.Obese);
        }
    }
}
