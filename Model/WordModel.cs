using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    public class WordModel {
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }
        [JsonProperty("endIndex")]
        public int EndIndex { get; set; }
        [JsonIgnore]
        public bool Replaced { get; set; }

        public WordModel(int start, int end) : this(start, end, false) { }

        public WordModel(int start, int end, bool replaced) {
            StartIndex = start;
            EndIndex = end;
            Replaced = replaced;
        }
    }
}
