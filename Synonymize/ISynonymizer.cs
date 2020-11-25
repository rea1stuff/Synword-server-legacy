using System.Collections.Generic;
using System.Threading.Tasks;
using SynWord_Server_CSharp.Model.UniqueUp;

namespace SynWord_Server_CSharp.Synonymize {
    interface ISynonymizer {
        Task<UniqueUpResponseModel> SynonymizeAsync(string text);
        UniqueUpResponseModel Synonymize(string text);
    }
}
