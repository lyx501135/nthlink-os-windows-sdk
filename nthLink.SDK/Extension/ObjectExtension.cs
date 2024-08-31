namespace nthLink.SDK.Extension
{
    public static class ObjectExtension
    {
        public static string ToStr(this object? obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                string? result = obj.ToString();

                return result == null ? string.Empty : result;
            }
        }

        public static T Unwrap<T>(this T? t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }
            else
            {
                return t;
            }
        }
    }
}
