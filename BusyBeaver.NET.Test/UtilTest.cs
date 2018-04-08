using NUnit.Framework;

namespace BusyBeaver.NET.Test
{
    [TestFixture]
    public class UtilTest
    {

        [Test]
        public static void FormatsCharWithCombiningLowLine()
        {
            char c = 'a';

            string underlined = Util.FormatWithCombiningLowLine(c);
            
            Assert.That(
                ("̲a" == underlined) || ("a̲" == underlined)
            );
            
        }
        
        [Test]
        public static void FormatsDigitWithCombiningLowLine()
        {
            char digit = '0';

            string underlinedDigit = Util.FormatWithCombiningLowLine(digit);
            
            Assert.That(
                () => ("̲0" == underlinedDigit) || ("0̲" == underlinedDigit)
            );
        }
        
        [Test]
        public static void FormatsStringWithCombiningLowLine()
        {
            string text = "Underline Me";

            string underlined = Util.FormatWithCombiningLowLine(text);
            
            Assert.That(
                () => ("̲U̲n̲d̲e̲r̲l̲i̲n̲e̲ ̲M̲e" == underlined) ||
                      ("U̲n̲d̲e̲r̲l̲i̲n̲e̲ ̲M̲e̲" == underlined)
            );
        }
        
    }
}