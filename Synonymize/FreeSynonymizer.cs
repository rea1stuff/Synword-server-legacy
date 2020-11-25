using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.UniqueUp;

namespace SynWord_Server_CSharp.Synonymize {
    class FreeSynonymizer : ISynonymizer {
        public async Task<UniqueUpResponseModel> SynonymizeAsync(string text) {
            return await Task.Run(() => Synonymize(text));
        }

        public UniqueUpResponseModel Synonymize(string text) {
            StringBuilder textBuilder = new StringBuilder(text);
            List<WordModel> words = GetWordsFromText(text);
            List<WordModel> replaced = new List<WordModel>();
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

                    replaced.Add(new WordModel(words[i].StartIndex + difference, words[i].StartIndex + difference + synonym.Length - 1));

                    int wordLength = words[i].EndIndex + 1 - words[i].StartIndex;
                    int synonymLength = synonym.Length;

                    difference += synonymLength - wordLength;
                }
            }

            UniqueUpResponseModel uniqueUpResponse = new UniqueUpResponseModel();
            uniqueUpResponse.Text = textBuilder.ToString();
            uniqueUpResponse.Replaced = replaced.ToArray();

            return uniqueUpResponse;
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

        private List<WordModel> GetWordsFromText(string text) {
            List<WordModel> words = new List<WordModel>();

            int start = -1;

            for (int i = 0; i < text.Length; i++) {
                if (start < 0) {
                    if (!new char[] { ' ', '\n', '\r' }.Contains(text[i]) && !char.IsPunctuation(text[i])) {
                        start = i;
                    }
                } else {
                    if (new char[] { ' ', '\n', '\r' }.Contains(text[i]) || char.IsPunctuation(text[i])) {
                        words.Add(new WordModel(start, i - 1));
                        start = -1;
                    }
                }
            }

            if (start >= 0) {
                words.Add(new WordModel(start, text.Length - 1));
            }

            return words;
        }
    }
}
