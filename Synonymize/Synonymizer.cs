using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model;
using SynWord_Server_CSharp.Model.UniqueUp;

namespace SynWord_Server_CSharp.Synonymize {
    public abstract class Synonymizer {
        public abstract Task<UniqueUpResponseModel> SynonymizeAsync(string text);

        public abstract UniqueUpResponseModel Synonymize(string text);

        protected virtual List<WordModel> GetWordsFromText(string text) {
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
