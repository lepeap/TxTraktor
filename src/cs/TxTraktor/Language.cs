using System.Collections.Generic;
using System.Linq;

namespace TxTraktor
{
    public enum Language
    {
        Unknown,
        Ru
    }

    public static class LanguageExt
    {
        private static Dictionary<Language, string> LangKeyDict;
        private static Dictionary<string, Language> KeyLangDict;

        static LanguageExt()
        {
            LangKeyDict = new Dictionary<Language, string>()
            {
                {Language.Ru, "ru"}
            };
            KeyLangDict = LangKeyDict.ToDictionary(kp => kp.Value, kp => kp.Key);
        }

        public static string GetTextKey(this Language lang)
        {
            return LangKeyDict[lang];
        }

        public static Language GetEnumFromKey(string key)
        {
            if (!KeyLangDict.ContainsKey(key))
                return Language.Unknown;

            return KeyLangDict[key];
        }
    } 
}