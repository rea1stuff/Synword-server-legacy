using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Exceptions {
    public class InvalidFileExtensionException : UserException {
        const string message = "invalidFileExtension";

        public InvalidFileExtensionException() : base(message) { }
    }
}
