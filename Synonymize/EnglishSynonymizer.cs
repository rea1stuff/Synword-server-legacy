using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.UniqueUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Synonymize {
    class EnglishSynonymizer : Synonymizer {
        public override async Task<UniqueUpResponseModel> SynonymizeAsync(string text) {
            return await Task.Run(() => Synonymize(text));
        }

        public override UniqueUpResponseModel Synonymize(string text) {
            StringBuilder textBuilder = new StringBuilder(text);
            List<WordModel> words = GetWordsFromText(text);
            List<ReplacedWordModel> replaced = new List<ReplacedWordModel>();
            int replacedCount = 0;

            for (int z = 3; z > 0; z--) {
                for (int i = 0; i < words.Count; i++) {
                    if (!words[i].Replaced) {
                        List<WordModel> intervalWords = new List<WordModel>();

                        bool isContainsReplaced = false;

                        for (int j = i; j < i + z && j < words.Count; j++) {
                            if (words[j].Replaced) {
                                isContainsReplaced = true;
                                break;
                            }

                            intervalWords.Add(words[j]);
                        }

                        if (isContainsReplaced) {
                            continue;
                        }

                        string word = "";

                        for (int j = 0; j < intervalWords.Count; j++) {
                            word += textBuilder.ToString(intervalWords[j].StartIndex, intervalWords[j].EndIndex + 1 - intervalWords[j].StartIndex);

                            if (j < (intervalWords.Count - 1)) {
                                word += " ";
                            }
                        }

                        int index = BinarySearch(SynonymDictionary.englishDictionary, word.ToLower(), 0, SynonymDictionary.englishDictionary.Count - 1);

                        if (index >= 0) {
                            List<string> synonyms = new List<string>(SynonymDictionary.englishDictionary[index].Synonyms);

                            if (synonyms.Count > 0) {
                                string synonym = synonyms[new Random().Next(synonyms.Count)];
                                synonyms.Insert(0, word);

                                if (char.IsUpper(word[0])) {
                                    synonym = char.ToUpper(synonym[0]) + synonym.Substring(1);
                                }

                                textBuilder.Remove(intervalWords[0].StartIndex, intervalWords[intervalWords.Count - 1].EndIndex + 1 - intervalWords[0].StartIndex);
                                textBuilder.Insert(intervalWords[0].StartIndex, synonym);

                                replaced.Add(new ReplacedWordModel(new WordModel(words[i].StartIndex, words[i].StartIndex + synonym.Length - 1), synonyms));

                                int wordLength;

                                if (intervalWords.Count == 1) {
                                    wordLength = intervalWords[0].EndIndex + 1 - intervalWords[intervalWords.Count - 1].StartIndex;
                                } else {
                                    wordLength = intervalWords[intervalWords.Count - 1].EndIndex - intervalWords[0].StartIndex + 1;
                                }

                                int synonymLength = synonym.Length;

                                int difference = synonymLength - wordLength;
                                replacedCount++;

                                words.Insert(i, new WordModel(words[i].StartIndex + difference, words[i].StartIndex + difference + synonym.Length - 1, true));

                                words.RemoveRange(i + 1, intervalWords.Count);

                                for (int j = i + 1; j < words.Count; j++) {
                                    words[j].StartIndex += difference;
                                    words[j].EndIndex += difference;
                                }
                            }
                        }
                    }
                }
            }

            UniqueUpResponseModel uniqueUpResponse = new UniqueUpResponseModel();
            uniqueUpResponse.Text = textBuilder.ToString();
            uniqueUpResponse.Replaced = replaced.ToArray();
            uniqueUpResponse.ReplacedCount = replacedCount;

            return uniqueUpResponse;
        }

        private int BinarySearch(List<EnglishSynonym> synonyms, string word, int left, int right) {
            if (left <= right) {
                int midle = left + (right - left) / 2;

                if (synonyms[midle].Word == word) {
                    return midle;
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
