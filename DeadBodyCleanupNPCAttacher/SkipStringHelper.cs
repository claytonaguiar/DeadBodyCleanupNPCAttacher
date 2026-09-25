using Mutagen.Bethesda.Synthesis.Settings;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringCompareSettings
{
    public class StringCompareSetting
    {
        [SynthesisTooltip("String keyword name which search for")]
        public string? Name { get; set; }

        [SynthesisSettingName("Compare type")]
        [SynthesisTooltip("Compare type, how to compare")]
        public CompareType Compare { get; set; } = CompareType.Contains;

        [SynthesisTooltip("Case insensitive compare, comparing ignore case")]
        public bool IgnoreCase { get; set; } = true;

        [SynthesisTooltip("Commentary for the strings. Just to understand")]
        public string? Comment { get; set; }
    }

    public class StringCompareSettingContainer
    {
        [SynthesisSettingName("String")]
        [SynthesisTooltip("Click to open string parameters")]
        public StringCompareSetting? StringSetting { get; set; }
    }

    public enum CompareType
    {
        Equals,
        StartsWith,
        Contains,
        EndsWith,
        Regex,
    }

    public static class StringCompareHelpers
    {
        public static bool IsUsingSkipList { get; set; } = false;

        public static bool IsInSkipList(this string? inputString, HashSet<StringCompareSettingContainer> list)
        {
            // Corrigido: Aborta se NÃO estiver usando skip list
            if (!IsUsingSkipList) return false; 
            if (string.IsNullOrWhiteSpace(inputString)) return false;

            foreach (var setting in list)
            {
                var s = setting.StringSetting;
                if (string.IsNullOrWhiteSpace(s?.Name)) continue;

                var comparison = s.IgnoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

                bool isMatch = s.Compare switch
                {
                    CompareType.Contains => inputString.Contains(s.Name, comparison),
                    CompareType.Equals => string.Equals(inputString, s.Name, comparison),
                    CompareType.StartsWith => inputString.StartsWith(s.Name, comparison),
                    CompareType.EndsWith => inputString.EndsWith(s.Name, comparison),
                    CompareType.Regex => SafeRegexMatch(inputString, s.Name, s.IgnoreCase),
                    _ => false
                };

                if (isMatch) return true;
            }

            // Corrigido: Retorna falso se percorreu toda a lista de exceções e o NPC não se encaixou em nenhuma
            return false; 
        }

        private static bool SafeRegexMatch(string inputString, string pattern, bool ignoreCase)
        {
            try
            {
                var options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
                return Regex.IsMatch(inputString, pattern, options);
            }
            catch
            {
                return false;
            }
        }
    }
}