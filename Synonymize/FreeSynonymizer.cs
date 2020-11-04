using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Synonymize {
    class FreeSynonymizer : ISynonymizer {
        public async Task<string> SynonymizeAsync(string text) {
            return await Task.Run(() => Synonymize(text));
        }

        public string Synonymize(string text) {
            StringBuilder textBuilder = new StringBuilder(text);
            List<Word> words = GetWordsFromText(text);
            int difference = 0;

            for (int i = 0; i < words.Count; i++) {
                string word = textBuilder.ToString(words[i].StartIndex + difference, words[i].EndIndex + 1 - words[i].StartIndex);

                int index = BinarySearch(SynonymDictionary.dictionary, word.ToLower(), 0, SynonymDictionary.dictionary.Count - 1);

                if (index >= 0) {
                    string synonym = SynonymDictionary.dictionary[SynonymDictionary.dictionary[index].SynonymId].Word;
                    if (char.IsUpper(word[0])) {
                        synonym = char.ToUpper(synonym[0]) + synonym.Substring(1);
                    }

                    textBuilder.Remove(words[i].StartIndex + difference, words[i].EndIndex + 1 - words[i].StartIndex);
                    textBuilder.Insert(words[i].StartIndex + difference, synonym);

                    int wordLength = words[i].EndIndex + 1 - words[i].StartIndex;
                    int synonymLength = synonym.Length;

                    difference += synonymLength - wordLength;
                }
            }

            return textBuilder.ToString();
        }

        private int BinarySearch(List<Synonym> synonyms, string word, int left, int right) {
            if (left <= right) {
                int midle = left + (right - left) / 2;

                if (synonyms[midle].Word == word) {
                    return synonyms[midle].Id;
                } else {
                    if (string.Compare(synonyms[midle].Word, word) < 0) {
                        return BinarySearch(synonyms, word, midle + 1, right);
                    } else {
                        return BinarySearch(synonyms, word, left, midle - 1);
                    }
                }
            } else {
                return -1;
            }
        }

        private List<Word> GetWordsFromText(string text) {
            List<Word> words = new List<Word>();

            int start = -1;

            for (int i = 0; i < text.Length; i++) {
                if (start < 0) {
                    if (!new char[] { ' ', '\n', '\r' }.Contains(text[i]) && !char.IsPunctuation(text[i])) {
                        start = i;
                    }
                } else {
                    if (new char[] { ' ', '\n', '\r' }.Contains(text[i]) || char.IsPunctuation(text[i])) {
                        words.Add(new Word(start, i - 1));
                        start = -1;
                    }
                }
            }

            if (start >= 0) {
                words.Add(new Word(start, text.Length - 1));
            }

            return words;
        }
    }
}
