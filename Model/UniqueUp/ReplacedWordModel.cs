using System.Collections.Generic;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model.UniqueUp {
    public class ReplacedWordModel {
        [JsonProperty("word")]
        public WordModel Word { get; set; }
        [JsonProperty("synonyms")]
        public List<string> Synonyms { get; set; }

        public ReplacedWordModel(WordModel word, List<string> synonyms) {
            Word = word;
            Synonyms = synonyms;
        }
    }
}
