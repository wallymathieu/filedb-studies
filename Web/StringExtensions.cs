namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// First letter to lower case
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FirstLetterToLower(this string value)
        {
            return value.Length >= 1 ? value.Substring(0, 1).ToLower() + value.Substring(1) : value;
        }
    }
}