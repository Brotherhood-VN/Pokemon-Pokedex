using System.Text;
using System.Text.Json;

namespace API.Helpers.Extensions
{
    public static class StrExtension
    {
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool IsBlank(this string strValue)
        {
            return string.IsNullOrWhiteSpace(strValue);
        }

        public static bool IsNotBlank(this string strValue)
        {
            return !strValue.IsBlank();
        }

        public static T JsonDeserializeNullSafe<T>(this string value) where T : class
        {
            if (value.IsBlank())
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(value);
        }
    }
}