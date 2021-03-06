﻿using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// The class is intended for translating a message into.
    /// </summary>
    public class Translator
    {
        /// <summary>
        /// The dictionary stores letters and their translation.
        /// </summary>
        private Dictionary<char, string> dictionary = new Dictionary<char, string>
        {
            {'а', "a"},
            {'б', "b"},
            {'в', "v"},
            {'г', "g"},
            {'д', "d"},
            {'е', "e"},
            {'ж', "zh"},
            {'з', "z"},
            {'и', "i"},
            {'й', "i"},
            {'к', "k"},
            {'л', "l"},
            {'м', "m"},
            {'н', "n"},
            {'о', "o"},
            {'п', "p"},
            {'р', "r"},
            {'с', "s"},
            {'т', "t"},
            {'у', "u"},
            {'ф', "f"},
            {'х', "h"},
            {'ц', "c"},
            {'ч', "ch"},
            {'ш', "sh"},
            {'щ', "sh"},
            {'ъ', ""},
            {'ы', "i"},
            {'ь', ""},
            {'э', "e"},
            {'ю', "yu"},
            {'я', "ya"},
            {'a', "а"},
            {'b', "б"},
            {'c', "ц"},
            {'d', "д"},
            {'e', "е"},
            {'f', "ф"},
            {'g', "г"},
            {'h', "х"},
            {'i', "и"},
            {'j', "дж"},
            {'k', "к"},
            {'l', "л"},
            {'m', "м"},
            {'n', "н"},
            {'o', "о"},
            {'p', "п"},
            {'q', "кв"},
            {'r', "р"},
            {'s', "с"},
            {'t', "т"},
            {'u', "у"},
            {'v', "в"},
            {'w', "в"},
            {'x', "кс"},
            {'y', "и"},
            {'z', "з"},
            {',', ","},
            {'.', "."},
            {'-', "-"},
            {'_', "_"},
            {'/', "/"},
            {'\\', "\\"},
            {'(', "("},
            {')', ")"},
            {' ', " "},
        };

        /// <summary>
        /// The method translates the message into another language.
        /// </summary>
        /// <returns>translated message</returns>
        public string TranslateMessage(string message)
        {
            message = message.ToLower();

            for (int i = 0; i < message.Length; i++)
                message = message.Replace(message[i].ToString(), dictionary[message[i]]);

            return message;
        }
    }
}
