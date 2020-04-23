using System.Text;

namespace Kanbersky.Queue.Core.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string GetString(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}
