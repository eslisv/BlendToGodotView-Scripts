namespace EVLibrary.Extensions
{
    public static class StringExtensions
    {
        private const int INDENT_SIZE = 2;

        public static string Indent(this string str, int depth)
        {
            return new string(' ', depth * INDENT_SIZE) + str;
        }

        public static string VisibleIndent(this string str, int depth)
        {
            if (depth > 0)
            {
                return VisibleIndent("| " + str, depth - 1);
            }
            return Indent(str, depth);
        }
    }
}
