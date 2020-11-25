using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    public class WordModel {
        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }
        [JsonProperty("endIndex")]
        public int EndIndex { get; set; }

        public WordModel(int start, int end) {
            StartIndex = start;
            EndIndex = end;
        }
    }
}
