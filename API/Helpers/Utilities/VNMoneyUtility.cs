using System.Text.RegularExpressions;

namespace API.Helpers.Utilities
{
    // https://github.com/princ3od/VietnamNumber/blob/main/VietnamNumber/VietnamNumber.cs
    public static class VNMoneyUtility
    {
        private static readonly string[] ZeroLeftPadding = { "", "00", "0" };

        private static readonly string[] Digits =
        {
            "không",
            "một",
            "hai",
            "ba",
            "bốn",
            "năm",
            "sáu",
            "bảy",
            "tám",
            "chín"
        };

        private static readonly string[] MultipleThousand =
        {
            "",
            "nghìn",
            "triệu",
            "tỷ",
            "nghìn tỷ",
            "triệu tỷ",
            "tỷ tỷ"
        };

        private static IEnumerable<string> Chunked(this string str, int chunkSize) => Enumerable
            .Range(0, str.Length / chunkSize)
            .Select(i => str.Substring(i * chunkSize, chunkSize));

        private static bool ShouldShowZeroHundred(this string[] groups) =>
            groups.Reverse().TakeWhile(it => it == "000").Count() < groups.Length - 1;

        private static void Deconstruct<T>(this IReadOnlyList<T> items, out T t0, out T t1, out T t2)
        {
            t0 = items.Count > 0 ? items[0] : default;
            t1 = items.Count > 1 ? items[1] : default;
            t2 = items.Count > 2 ? items[2] : default;
        }

        private static string ReadTriple(string triple, bool showZeroHundred)
        {
            var (a, b, c) = triple.Select(ch => int.Parse(ch.ToString())).ToArray();

            return a switch
            {
                0 when b == 0 && c == 0 => "",
                0 when showZeroHundred => "không trăm " + ReadPair(b, c),
                0 when b == 0 => Digits[c],
                0 => ReadPair(b, c),
                _ => Digits[a] + " trăm " + ReadPair(b, c)
            };
        }

        private static string ReadPair(int b, int c)
        {
            return b switch
            {
                0 => c == 0 ? "" : " lẻ " + Digits[c],
                1 => "mười " + c switch
                {
                    0 => "",
                    5 => "lăm",
                    _ => Digits[c]
                },
                _ => Digits[b] + " mươi " + c switch
                {
                    0 => "",
                    1 => "mốt",
                    4 => "bốn",
                    5 => "lăm",
                    _ => Digits[c]
                }
            };
        }

        // "10" -> "mười"
        public static string ToVietnameseWords(this long n)
        {
            if (n == 0L) return "không";
            if (n < 0L) return "âm " + (-n).ToVietnameseWords().ToLower();

            var s = n.ToString();
            var groups = (ZeroLeftPadding[s.Length % 3] + s).Chunked(3).ToArray();
            var showZeroHundred = groups.ShouldShowZeroHundred();

            var index = -1;
            var rawResult = groups.Aggregate("", (acc, e) =>
            {
                checked
                {
                    index++;
                }

                var readTriple = ReadTriple(e, showZeroHundred && index > 0);
                var multipleThousand = (string.IsNullOrWhiteSpace(readTriple)
                    ? ""
                    : (MultipleThousand.ElementAtOrDefault(groups.Length - 1 - index) ?? ""));
                return $"{acc} {readTriple} {multipleThousand} ";
            });

            return Regex
                .Replace(rawResult, "\\s+", " ")
                .Trim() + " đồng";
        }

        // "09" -> "không chín"
        public static string ToVietnameseSingleWords(this string n)
        {
            var raw = "";
            foreach (var s in n)
            {
                if (int.TryParse(s.ToString(), out int num))
                    raw += Digits[num] + " ";
            }
            return Regex
                .Replace(raw, "\\s+", " ")
                .Trim();
        }

        /// <summary>
        /// Làm tròn số tiền lẻ ở phần ngàn
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static decimal RoundTheAmount(this decimal money)
        {
            return ((int)money / 1000 + 1) * 1000;
        }
    }
}