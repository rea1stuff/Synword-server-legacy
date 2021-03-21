using System;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.Model {
    [Serializable]
    class RussianSynonym : Synonym {
        public int Id { get; set; }
        public int SynonymId { get; set; }

        public RussianSynonym(string word) : base(word) {
            Word = word;
        }

        [JsonConstructor]
        public RussianSynonym(int id, string word, int synonymId) : base(word) {
            Id = id;
            Word = word;
            SynonymId = synonymId;
        }
    }
}
