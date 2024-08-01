using System.Text.RegularExpressions;

namespace API.Helpers.Utilities
{
    public static partial class PluralizeUtility
    {
        private static readonly IList<ReplaceRule> _pluralRules = PluralRules.GetRules();
        private static readonly IList<ReplaceRule> _singularRules = SingularRules.GetRules();
        private static readonly ICollection<string> _uncountables = Uncountables.GetUncountables();
        private static readonly IDictionary<string, string> _irregularPlurals = IrregularRules.GetIrregularPlurals();
        private static readonly IDictionary<string, string> _irregularSingles = IrregularRules.GetIrregularSingulars();

        private static readonly Regex _replacementRegex = ReplacementRegex();

        public static string Pluralize(this string word)
        {
            return Transform(word, _irregularSingles, _irregularPlurals, _pluralRules);
        }

        public static string Singularize(this string word)
        {
            return Transform(word, _irregularPlurals, _irregularSingles, _singularRules);
        }

        public static bool IsSingular(this string word)
        {
            return word.Singularize() == word;
        }

        public static bool IsPlural(this string word)
        {
            return word.Pluralize() == word;
        }

        public static string Format(this string word, int count, bool inclusive = false)
        {
            var pluralized = count == 1 ?
                word.Singularize() : word.Pluralize();

            return (inclusive ? count + " " : "") + pluralized;
        }

        public static void AddPluralRule(Regex rule, string replacement)
        {
            _pluralRules.Add(new ReplaceRule
            {
                Condition = rule,
                ReplaceWith = replacement
            });
        }

        public static void AddPluralRule(string rule, string replacement)
        {
            var regexRule = SanitizeRule(rule);
            _pluralRules.Add(new ReplaceRule
            {
                Condition = regexRule,
                ReplaceWith = replacement
            });
        }

        public static void AddSingularRule(Regex rule, string replacement)
        {
            _singularRules.Add(new ReplaceRule
            {
                Condition = rule,
                ReplaceWith = replacement
            });
        }

        public static void AddSingularRule(string rule, string replacement)
        {
            var regexRule = SanitizeRule(rule);
            _singularRules.Add(new ReplaceRule
            {
                Condition = regexRule,
                ReplaceWith = replacement
            });
        }

        public static void AddUncountableRule(string word)
        {
            _uncountables.Add(word);
        }

        public static void AddUncountableRule(Regex rule)
        {
            _pluralRules.Add(new ReplaceRule
            {
                Condition = rule,
                ReplaceWith = "$0"
            });

            _singularRules.Add(new ReplaceRule
            {
                Condition = rule,
                ReplaceWith = "$0"
            });
        }

        public static void AddIrregularRule(string single, string plural)
        {
            _irregularSingles.Add(single, plural);
            _irregularPlurals.Add(plural, single);
        }

        private static Regex SanitizeRule(string rule)
        {
            return new Regex($"^{rule}$", RegexOptions.IgnoreCase);
        }

        private static string RestoreCase(string originalWord, string newWord)
        {
            // Tokens are an exact match.
            if (originalWord == newWord)
                return newWord;

            // Lower cased words. E.g. "hello".
            if (originalWord == originalWord.ToLower())
                return newWord.ToLower();

            // Upper cased words. E.g. "HELLO".
            if (originalWord == originalWord.ToUpper())
                return newWord.ToUpper();

            // Title cased words. E.g. "Title".
            if (originalWord[0] == char.ToUpper(originalWord[0]))
                return char.ToUpper(newWord[0]) + newWord[1..];

            // Lower cased words. E.g. "test".
            return newWord.ToLower();
        }

        private static string ApplyRules(string token, string originalWord, IList<ReplaceRule> rules)
        {
            // Empty string or doesn't need fixing.
            if (string.IsNullOrEmpty(token) || _uncountables.Contains(token))
                return originalWord;


            // Iterate over the sanitization rules and use the first one to match.
            // Iterate backwards since specific/custom rules can be appended
            for (var i = rules.Count - 1; i >= 0; i--)
            {
                var rule = rules[i];

                // If the rule passes, return the replacement.
                if (rule.Condition.IsMatch(originalWord))
                {
                    var match = rule.Condition.Match(originalWord);
                    var matchString = match.Groups[0].Value;
                    if (string.IsNullOrWhiteSpace(matchString))
                        return rule.Condition.Replace(originalWord, GetReplaceMethod(originalWord[match.Index - 1].ToString(), rule.ReplaceWith), 1);
                    return rule.Condition.Replace(originalWord, GetReplaceMethod(matchString, rule.ReplaceWith), 1);
                }
            }

            return originalWord;
        }

        private static MatchEvaluator GetReplaceMethod(string originalWord, string replacement)
        {
            return match =>
            {
                return RestoreCase(originalWord, _replacementRegex.Replace(replacement, m => match.Groups[int.Parse(m.Groups[1].Value)].Value));
            };
        }

        private static string Transform(string word, IDictionary<string, string> replacables,
            IDictionary<string, string> keepables, IList<ReplaceRule> rules)
        {
            if (keepables.ContainsKey(word)) return word;
            if (replacables.TryGetValue(word, out string token)) return RestoreCase(word, token);
            return ApplyRules(word, word, rules);
        }

        [GeneratedRegex("\\$(\\d{1,2})")]
        private static partial Regex ReplacementRegex();
    }

    public class ReplaceRule
    {
        public Regex Condition { get; set; }
        public string ReplaceWith { get; set; }
    }

    internal static class IrregularRules
    {
        private static readonly Dictionary<string, string> dictionary = new(StringComparer.OrdinalIgnoreCase)
            {
                // Pronouns.
                {"I", "we"},
                {"me", "us"},
                {"he", "they"},
                {"she", "they"},
                {"them", "them"},
                {"myself", "ourselves"},
                {"yourself", "yourselves"},
                {"itself", "themselves"},
                {"herself", "themselves"},
                {"himself", "themselves"},
                {"themself", "themselves"},
                {"is", "are"},
                {"was", "were"},
                {"has", "have"},
                {"this", "these"},
                {"that", "those"},
                // Words ending in with a consonant and `o`.
                {"echo", "echoes"},
                {"dingo", "dingoes"},
                {"volcano", "volcanoes"},
                {"tornado", "tornadoes"},
                {"torpedo", "torpedoes"},
                // Ends with `us`.
                {"genus", "genera"},
                {"viscus", "viscera"},
                // Ends with `ma`.
                {"stigma", "stigmata"},
                {"stoma", "stomata"},
                {"dogma", "dogmata"},
                {"lemma", "lemmata"},
                {"schema", "schemata"},
                {"anathema", "anathemata"},
                // Other irregular rules.
                {"ox", "oxen"},
                {"axe", "axes"},
                {"die", "dice"},
                {"yes", "yeses"},
                {"foot", "feet"},
                {"eave", "eaves"},
                {"goose", "geese"},
                {"tooth", "teeth"},
                {"quiz", "quizzes"},
                {"human", "humans"},
                {"proof", "proofs"},
                {"carve", "carves"},
                {"valve", "valves"},
                {"looey", "looies"},
                {"thief", "thieves"},
                {"groove", "grooves"},
                {"pickaxe", "pickaxes"},
                {"passerby","passersby" },
                {"cookie","cookies" }
            };

        public static IDictionary<string, string> GetIrregularPlurals()
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, string> item in dictionary.Reverse())
            {
                if (!result.ContainsKey(item.Value)) result.Add(item.Value, item.Key);
            }
            return result;
        }

        public static IDictionary<string, string> GetIrregularSingulars()
        {
            return dictionary;
        }
    }

    internal static class Uncountables
    {
        public static ICollection<string> GetUncountables()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase) { 
                // Singular words with no plurals.
                "adulthood",
                "advice",
                "agenda",
                "aid",
                "aircraft",
                "alcohol",
                "ammo",
                "anime",
                "athletics",
                "audio",
                "bison",
                "blood",
                "bream",
                "buffalo",
                "butter",
                "carp",
                "cash",
                "chassis",
                "chess",
                "clothing",
                "cod",
                "commerce",
                "cooperation",
                "corps",
                "debris",
                "diabetes",
                "digestion",
                "elk",
                "energy",
                "equipment",
                "excretion",
                "expertise",
                "firmware",
                "flounder",
                "fun",
                "gallows",
                "garbage",
                "graffiti",
                "headquarters",
                "health",
                "herpes",
                "highjinks",
                "homework",
                "housework",
                "information",
                "jeans",
                "justice",
                "kudos",
                "labour",
                "literature",
                "machinery",
                "mackerel",
                "mail",
                "media",
                "mews",
                "moose",
                "music",
                "mud",
                "manga",
                "news",
                "only",
                "personnel",
                "pike",
                "plankton",
                "pliers",
                "police",
                "pollution",
                "premises",
                "rain",
                "research",
                "rice",
                "salmon",
                "scissors",
                "series",
                "sewage",
                "shambles",
                "shrimp",
                "software",
                "species",
                "staff",
                "swine",
                "tennis",
                "traffic",
                "transportation",
                "trout",
                "tuna",
                "wealth",
                "welfare",
                "whiting",
                "wildebeest",
                "wildlife",
                "you"
            };
        }
    }

    internal static class PluralRules
    {
        public static IList<ReplaceRule> GetRules()
        {
            return new List<ReplaceRule>
            {
                // rules are ordered more generic first
                new() { Condition = ReplaceRuleRegex("s?$"), ReplaceWith = "s" },
                new() { Condition = ReplaceRuleRegex("[^\u0000-\u007F]$"),  ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("([^aeiou]ese)$"), ReplaceWith = "$1" },
                new() { Condition = ReplaceRuleRegex("(ax|test)is$"),  ReplaceWith = "$1es" },
                new() { Condition = ReplaceRuleRegex("(alias|[^aou]us|t[lm]as|gas|ris)$"),  ReplaceWith = "$1es" },
                new() { Condition = ReplaceRuleRegex("(e[mn]u)s?$"),  ReplaceWith = "$1s" },
                new() { Condition = ReplaceRuleRegex("([^l]ias|[aeiou]las|[ejzr]as|[iu]am)$"),  ReplaceWith = "$1" },
                new() { Condition = ReplaceRuleRegex("(alumn|syllab|vir|radi|nucle|fung|cact|stimul|termin|bacill|foc|uter|loc|strat)(?:us|i)$"), ReplaceWith = "$1i" },
                new() { Condition = ReplaceRuleRegex("(alumn|alg|vertebr)(?:a|ae)$"), ReplaceWith = "$1ae" },
                new() { Condition = ReplaceRuleRegex("(seraph|cherub)(?:im)?$"), ReplaceWith = "$1im" },
                new() { Condition = ReplaceRuleRegex("(her|at|gr)o$"), ReplaceWith = "$1oes" },
                new() { Condition = ReplaceRuleRegex("(agend|addend|millenni|dat|extrem|bacteri|desiderat|strat|candelabr|errat|ov|symposi|curricul|automat|quor)(?:a|um)$"),  ReplaceWith = "$1a" },
                new() { Condition = ReplaceRuleRegex("(apheli|hyperbat|periheli|asyndet|noumen|phenomen|criteri|organ|prolegomen|hedr|automat)(?:a|on)$"),  ReplaceWith = "$1a" },
                new() { Condition = ReplaceRuleRegex("sis$"), ReplaceWith = "ses" },
                new() { Condition = ReplaceRuleRegex("(?:(kni|wi|li)fe|(ar|l|ea|eo|oa|hoo)f)$"),  ReplaceWith = "$1$2ves" },
                new() { Condition = ReplaceRuleRegex("([^aeiouy]|qu)y$"),  ReplaceWith = "$1ies" },
                new() { Condition = ReplaceRuleRegex("([^ch][ieo][ln])ey$"),  ReplaceWith = "$1ies" },
                new() { Condition = ReplaceRuleRegex("(x|ch|ss|sh|zz)$"),  ReplaceWith = "$1es" },
                new() { Condition = ReplaceRuleRegex("(matr|cod|mur|sil|vert|ind|append)(?:ix|ex)$"),  ReplaceWith = "$1ices" },
                new() { Condition = ReplaceRuleRegex("\\b((?:tit)?m|l)(?:ice|ouse)$"),  ReplaceWith = "$1ice" },
                new() { Condition = ReplaceRuleRegex("(pe)(?:rson|ople)$"),  ReplaceWith = "$1ople" },
                new() { Condition = ReplaceRuleRegex("(child)(?:ren)?$"),  ReplaceWith = "$1ren" },
                new() { Condition = ReplaceRuleRegex("eaux$"),  ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("m[ae]n$"), ReplaceWith = "men" },
                new() { Condition = ReplaceRuleRegex("^thou$"), ReplaceWith = "you" },


                new() { Condition = ReplaceRuleRegex("pox$"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("o[iu]s$"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("deer$"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("fish$"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("sheep$"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("measles$/"), ReplaceWith = "$0" },
                new() { Condition = ReplaceRuleRegex("[^aeiou]ese$"), ReplaceWith = "$0" }
            };
        }

        private static Regex ReplaceRuleRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }

    internal static class SingularRules
    {
        public static IList<ReplaceRule> GetRules()
        {
            return new List<ReplaceRule>
            {
                // rules are ordered more generic first
                new () { Condition = ReplaceRuleRegex("s$"), ReplaceWith = ""},
                new () { Condition = ReplaceRuleRegex("(ss)$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("(wi|kni|(?:after|half|high|low|mid|non|night|[^\\w]|^)li)ves$"), ReplaceWith = "$1fe"},
                new () { Condition = ReplaceRuleRegex("(ar|(?:wo|[ae])l|[eo][ao])ves$"), ReplaceWith = "$1f"},
                new () { Condition = ReplaceRuleRegex("ies$"), ReplaceWith ="y"},
                new () { Condition = ReplaceRuleRegex("\\b([pl]|zomb|(?:neck|cross)?t|coll|faer|food|gen|goon|group|lass|talk|goal|cut)ies$"), ReplaceWith = "$1ie" },
                new () { Condition = ReplaceRuleRegex("\\b(mon|smil)ies$"), ReplaceWith = "$1ey"},
                new () { Condition = ReplaceRuleRegex("\\b((?:tit)?m|l)ice$"), ReplaceWith = "$1ouse"},
                new () { Condition = ReplaceRuleRegex("(seraph|cherub)im$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("(x|ch|ss|sh|zz|tto|go|cho|alias|[^aou]us|t[lm]as|gas|(?:her|at|gr)o|[aeiou]ris)(?:es)?$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("(analy|diagno|parenthe|progno|synop|the|empha|cri|ne)(?:sis|ses)$"), ReplaceWith = "$1sis"},
                new () { Condition = ReplaceRuleRegex("(movie|twelve|abuse|e[mn]u)s$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("(test)(?:is|es)$"), ReplaceWith = "$1is"},
                new () { Condition = ReplaceRuleRegex("(alumn|syllab|octop|vir|radi|nucle|fung|cact|stimul|termin|bacill|foc|uter|loc|strat)(?:us|i)$"), ReplaceWith = "$1us"},
                new () { Condition = ReplaceRuleRegex("(agend|addend|millenni|dat|extrem|bacteri|desiderat|strat|candelabr|errat|ov|symposi|curricul|quor)a$"), ReplaceWith = "$1um"},
                new () { Condition = ReplaceRuleRegex("(apheli|hyperbat|periheli|asyndet|noumen|phenomen|criteri|organ|prolegomen|hedr|automat)a$"), ReplaceWith = "$1on"},
                new () { Condition = ReplaceRuleRegex("(alumn|alg|vertebr)ae$"), ReplaceWith = "$1a"},
                new () { Condition = ReplaceRuleRegex("(cod|mur|sil|vert|ind)ices$"), ReplaceWith = "$1ex"},
                new () { Condition = ReplaceRuleRegex("(matr|append)ices$"), ReplaceWith = "$1ix"},
                new () { Condition = ReplaceRuleRegex("(pe)(rson|ople)$"), ReplaceWith = "$1rson"},
                new () { Condition = ReplaceRuleRegex("(child)ren$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("(eau)x?$"), ReplaceWith = "$1"},
                new () { Condition = ReplaceRuleRegex("men$"), ReplaceWith = "man" },

                new () { Condition = ReplaceRuleRegex("[^aeiou]ese$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("deer$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("fish$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("measles$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("o[iu]s$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("pox$"), ReplaceWith = "$0"},
                new () { Condition = ReplaceRuleRegex("sheep$"), ReplaceWith = "$0" }
            };
        }

        private static Regex ReplaceRuleRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}