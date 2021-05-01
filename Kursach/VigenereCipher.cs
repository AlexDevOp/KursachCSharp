using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    public class VigenereCipher
    {
        const string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        readonly string letters;

        public VigenereCipher(string alphabet = null)
        {
            letters = string.IsNullOrEmpty(alphabet) ? defaultAlphabet : alphabet;
        }

        private string Vigenere(string text, string password, bool encrypting = true)
        {
            password = password.ToLower();
            int N = letters.Length;
            string result = String.Empty;

            int keyword_index = 0;
            bool isUpper;
            int offset;
            foreach (char symbol in text)
            {
                if (!Char.IsLetter(symbol) || !letters.Contains(Char.ToLower(symbol)))
                {
                    result += symbol;
                    continue;
                }

                isUpper = Char.IsUpper(symbol);

                if (encrypting)
                {
                    offset = (letters.IndexOf(symbol) +
                        letters.IndexOf(password[keyword_index])) % N;
                }
                else
                {
                    offset = (letters.IndexOf(symbol) + N -
                        letters.IndexOf(password[keyword_index])) % N;
                }

                result += isUpper ? Char.ToUpper(letters[offset]) : letters[offset];

                keyword_index++;

                if (keyword_index == password.Length)
                    keyword_index = 0;
            }

            return result;
        }

        public string Encrypt(string plainMessage, string password)
            => Vigenere(plainMessage, password);

        public string Decrypt(string encryptedMessage, string password)
            => Vigenere(encryptedMessage, password, false);
    }
}
