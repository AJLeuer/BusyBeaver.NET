using System;
using System.Text;
using Mono.CSharp;

namespace BusyBeaver.NET
{
    public static class Util
    {
        public static string FormatWithCombiningLowLine(string s)
        {
            StringBuilder underscored = new StringBuilder();
            
            foreach (char c in s)
            {
                string underscoredChar = FormatWithCombiningLowLine(c); //underscore codepoint: u0332
                underscored.Append(underscoredChar);
            }
            
            return underscored.ToString();
        }

        public static string FormatWithCombiningLowLine(char c)
        {
            //return '\u0332' + string.Concat(c);
            return string.Concat(c) + '\u0332'; //underscore codepoint: u0332
        }

        public static char ConvertDigitToCircledSymbol(ushort digit)
        {
            switch (digit)
            {
                case 0:
                    return '⓪';
                case 1:
                    return '①';
                case 2:
                    return '②';
                case 3:
                    return '③';
                case 4:
                    return '④';
                case 5:
                    return '⑤';
                case 6:
                    return '⑥';
                case 7:
                    return '⑦';
                case 8:
                    return '⑧';
                case 9:
                    return '⑨';
                default:
                    throw new ArgumentException("Argument to ConvertDigitToCircledSymbol() must be single digit between 0 and 9");
            }
        }

        public static char ConvertDigitToCircledSymbol(char digit)
        {
            ushort digitInt = UInt16.Parse(digit.ToString());
            return ConvertDigitToCircledSymbol(digitInt);
        }
    }
}