namespace API.Helpers.Utilities
{
    public static class GenerateCodeUtility
    {
        public static string RandomAscii(int length)
        {
            const string ASCIIChars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(ASCIIChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            Random random = new();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomStringUpper(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Identity(string code, int length = 5)
        {
            return (Convert.ToInt32(code) + 1).ToString().PadLeft(length, '0');
        }
    }
}