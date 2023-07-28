namespace MoviesAppWeb.Helper
{
    public static class StringHelper
    {
        public static string Truncate(string input, int maxLength)
        {
            return input.Length > maxLength ? input.Substring(0, maxLength) : input;
        }
    }
}
