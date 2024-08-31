using System.Runtime.InteropServices;
using System.Text;

namespace nthLink.SDK.Extension
{
    public static class StringExtension
    {
        public static string ToRegexString(this string value)
        {
            var sb = new StringBuilder();
            foreach (var t in value)
            {
                var escapeCharacters = new[] { '\\', '*', '+', '?', '|', '{', '}', '[', ']', '(', ')', '^', '$', '.' };
                if (escapeCharacters.Any(s => s == t))
                    sb.Append('\\');

                sb.Append(t);
            }

            return sb.ToString();
        }
        public static IntPtr ToIntPtr(this string str, Encoding? encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] buffer = encoding.GetBytes(str);

            IntPtr intPtr = Marshal.AllocHGlobal(buffer.Length);

            for (int i = 0; i < buffer.Length; i++)
            {
                Marshal.WriteByte(intPtr + i, buffer[i]);
            }

            return intPtr;
        }
    }
}
