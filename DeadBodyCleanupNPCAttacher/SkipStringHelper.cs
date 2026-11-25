using Mutagen.Bethesda.Synthesis.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StringCompareSettings
{
    public class StringCompareSetting
    {
        [SynthesisTooltip("Keyword or pattern to search for")]
        public string? Name { get; set; }

        [SynthesisSettingName("Compare type")]
        [SynthesisTooltip("How the keyword should be matched")]
        public CompareType Compare { get; set; } = CompareType.Contains;

        [SynthesisTooltip("Ignore uppercase/lowercase differences when matching")]
        public bool IgnoreCase { get; set; } = true;

        [SynthesisTooltip("Optional note to help you remember why this entry was added")]
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

        // Cache para compilar cada padrão Regex apenas uma vez durante a execução do patcher
        private static readonly ConcurrentDictionary<(string Pattern, bool IgnoreCase), Regex?> _regexCache = new();

        public static bool IsInSkipList(this string? inputString, HashSet<StringCompareSettingContainer> list)
        {
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

            return false;
        }

        private static bool SafeRegexMatch(string inputString, string pattern, bool ignoreCase)
        {
            var regex = _regexCache.GetOrAdd((pattern, ignoreCase), key =>
            {
                try
                {
                    var options = RegexOptions.Compiled | (key.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
                    return new Regex(key.Pattern, options);
                }
                catch
                {
                    // Armazena null no cache para evitar tentar recompilar um padrão inválido nos próximos NPCs
                    return null;
                }
            });

            return regex?.IsMatch(inputString) ?? false;
        }
    }
}
