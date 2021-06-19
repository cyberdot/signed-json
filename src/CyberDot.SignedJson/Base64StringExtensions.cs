namespace CyberDot.SignedJson
{
    public static class Base64StringExtensions
    {
        private static readonly char[] PaddingChar = new char[]{'='};

        public static string RemovePadding(this string base64String)
        {
            return base64String.TrimEnd(PaddingChar);
        }

        public static string AddPadding(this string base64String)
        {
            var remainder = base64String.Length % 4;
            switch (remainder)
            {
                case 2:
                    return $"{base64String}==";
                case 3:
                    return $"{base64String}=";
                default:
                    return base64String;
            }
        }
    }
}