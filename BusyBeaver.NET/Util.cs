namespace BusyBeaver.NET
{
    public static class Util
    {
        public static string FormatWithUnderscore(char c)
        {
            //underscore: u0332
            var underscored = "_" + c + '_';
            return underscored;
        }
    }
}