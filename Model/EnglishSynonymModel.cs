using Newtonsoft.Json;
using System.Collections.Generic;

namespace SynWord_Server_CSharp.Model {
    public class EnglishSynonym : Synonym {
        public List<string> Synonyms { get; set; }

        public EnglishSynonym() : base("") {
            Synonyms = new List<string>();
        }

        public EnglishSynonym(string word, List<string> synonyms) : base(word) {
            Word = word;
            Synonyms = synonyms;
        }
    }
}
