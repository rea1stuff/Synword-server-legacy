using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Synonymizer {
    static class SynonymDictionary {
        static public List<Synonym> synonyms;

        static public void InitializeDictionary() {
            synonyms = JsonConvert.DeserializeObject<List<Synonym>>(File.ReadAllText("E:\\test.json"));
        }
    }
}
