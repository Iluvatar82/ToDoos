using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Framework.Converter.Tests
{
    [TestClass()]
    public class ColorConverterTests
    {
        [TestMethod()]
        public void GetColorTest_White()
        {
            var colorString = "#ffffff";
            var color = ColorConverter.GetColor(colorString);

            Assert.IsNotNull(color);
            Assert.AreEqual(255, color.A);
            Assert.AreEqual(255, color.R);
            Assert.AreEqual(255, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod()]
        public void GetColorTest_Blue()
        {
            var colorString = "#0000ff";
            var color = ColorConverter.GetColor(colorString);

            Assert.IsNotNull(color);
            Assert.AreEqual(255, color.A);
            Assert.AreEqual(0, color.R);
            Assert.AreEqual(0, color.G);
            Assert.AreEqual(255, color.B);
        }

        [TestMethod()]
        public void GetColorTest_String_Wrong_Prefix()
        {
            var colorString = "_0000ff";
            Assert.ThrowsException<ArgumentException>(() => ColorConverter.GetColor(colorString));
        }

        [TestMethod()]
        public void GetColorTest_String_Insufficient_Color_Values()
        {
            var colorString = "#00f";
            Assert.ThrowsException<ArgumentException>(() => ColorConverter.GetColor(colorString));
        }

        [TestMethod()]
        public void GetColorTest_String_No_Hex_Character()
        {
            var colorString = "#0000fg";
            Assert.ThrowsException<ArgumentException>(() => ColorConverter.GetColor(colorString));
        }

        [TestMethod()]
        public void GetStringTest_Ok()
        {
            var color = Color.Black;
            var colorString = ColorConverter.GetString(color);

            Assert.IsNotNull(colorString);
            Assert.AreEqual('#', colorString[0]);
            Assert.AreEqual(7, colorString.Length);
            Assert.AreEqual("00", colorString[1..3]);
            Assert.AreEqual("00", colorString[3..5]);
            Assert.AreEqual("00", colorString[5..7]);
        }
    }
}