using System.Collections.Generic;
using System.Threading.Tasks;

namespace Synonymizer {
    interface ISynonymizer {
        Task<string> SynonymizeAsync(string text);
        string Synonymize(string text);
    }
}
