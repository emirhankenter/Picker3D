using System;
using System.Collections.Generic;
using Mek.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mek.Localization
{
    public enum Language
    {
        Turkish = 0,
        English = 1,
        Italian = 2
    }
    [CreateAssetMenu(menuName = "Localization")]
    public class Localization : ScriptableObject
    {
        [Serializable] public class TranslationDictionary : SerializableDictionary<string, LanguageDictionary> { }
        [Serializable] public class LanguageDictionary : SerializableDictionary<Language, string> { }

        public TranslationDictionary Dictionary;

        [Button]
        public void ReadCSV()
        {
            TextAsset csv = Resources.Load<TextAsset>("LocalizationDictionary");

            string[] data = csv.text.Split(new char[] { '\n' });

            string[] firstColumn = data[0].Split(new char[] {','});

            var languages = new List<Language>();

            for (int i = 1; i < firstColumn.Length; i++)
            {
                if (Enum.TryParse(firstColumn[i], out Language language))
                {
                    languages.Add(language);
                }
            }

            for (int i = 1; i < data.Length; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                if (data.Length == 0) continue;
                var key = row[0];
                if (Dictionary.TryGetValue(key, out LanguageDictionary dict))
                {
                }
                else
                {
                    dict = new LanguageDictionary();
                    Dictionary.Add(key, dict);
                }
                for (int j = 1; j < row.Length; j++)
                {
                    if (Enum.TryParse(firstColumn[j], out Language language))
                    {
                        if (dict.TryGetValue(language, out string translation))
                        {
                            dict[language] = row[j];
                        }
                        else
                        {
                            dict.Add(language, row[j]);
                        }
                    }
                }
                //for (int j = 1; j < row.Length; j++)
                //{
                //    if (Dictionary.TryGetValue(key, out LanguageDictionary dict))
                //    {
                //        foreach (var language in languages)
                //        {
                //            if (dict.TryGetValue(language, out string translation))
                //            {
                //                translation = row[j];
                //            }
                //            else
                //            {
                //                dict.Add(language, row[j]);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        var languageDict = new LanguageDictionary();

                //        foreach (var language in languages)
                //        {
                //            languageDict.Add(language, row[j]);
                //        }

                //        Dictionary.Add(key, languageDict);
                //    }
                //}
            }
        }
    }
}
