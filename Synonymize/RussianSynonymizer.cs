using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.UniqueUp;

namespace SynWord_Server_CSharp.Synonymize {
    class RussianSynonymizer : Synonymizer {
        public override async Task<UniqueUpResponseModel> SynonymizeAsync(string text) {
            return await Task.Run(() => Synonymize(text));
        }

        public override UniqueUpResponseModel Synonymize(string text) {
            StringBuilder textBuilder = new StringBuilder(text);
            List<WordModel> words = GetWordsFromText(text);
            List<ReplacedWordModel> replaced = new List<ReplacedWordModel>();
            int replacedCount = 0;
            int difference = 0;

            for (int i = 0; i < words.Count; i++) {
                string word = textBuilder.ToString(words[i].StartIndex + difference, words[i].EndIndex + 1 - words[i].StartIndex);

                int index = BinarySearch(SynonymDictionary.russianDictionary, word.ToLower(), 0, SynonymDictionary.russianDictionary.Count - 1);

                if (index >= 0) {
                    string synonym = SynonymDictionary.russianDictionary[SynonymDictionary.russianDictionary[index].SynonymId].Word;

                    List<string> synonyms = new List<string>();
                    synonyms.Add(word);
                    synonyms.Add(synonym);

                    if (char.IsUpper(word[0])) {
                        synonym = char.ToUpper(synonym[0]) + synonym.Substring(1);
                    }

                    textBuilder.Remove(words[i].StartIndex + difference, words[i].EndIndex + 1 - words[i].StartIndex);
                    textBuilder.Insert(words[i].StartIndex + difference, synonym);

                    replaced.Add(new ReplacedWordModel(new WordModel(words[i].StartIndex + difference, words[i].StartIndex + difference + synonym.Length - 1), synonyms));

                    int wordLength = words[i].EndIndex + 1 - words[i].StartIndex;
                    int synonymLength = synonym.Length;

                    difference += synonymLength - wordLength;
                    replacedCount++;
                }
            }

            UniqueUpResponseModel uniqueUpResponse = new UniqueUpResponseModel();
            uniqueUpResponse.Text = textBuilder.ToString();
            uniqueUpResponse.Replaced = replaced.ToArray();
            uniqueUpResponse.ReplacedCount = replacedCount;

            return uniqueUpResponse;
        }

        private int BinarySearch(List<RussianSynonym> synonyms, string word, int left, int right) {
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
    }
}
