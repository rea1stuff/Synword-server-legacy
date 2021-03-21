using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model.UniqueUp {
    public class ReplacedWordModel {
        public WordModel Word { get; set; }
        public List<string> Synonyms { get; set; }

        public ReplacedWordModel(WordModel word, List<string> synonyms) {
            Word = word;
            Synonyms = synonyms;
        }
    }
}
