using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Synonymize {
    interface ISynonymizer {
        Task<string> SynonymizeAsync(string text);
        string Synonymize(string text);
    }
}
