using System;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    [Serializable]
    class Synonym {
        public int Id { get; set; }
        public string Word { get; set; }
        public int SynonymId { get; set; }

        public Synonym(string word) {
            Word = word;
        }

        [JsonConstructor]
        public Synonym(int id, string word, int synonymId) {
            Id = id;
            Word = word;
            SynonymId = synonymId;
        }
    }
}
