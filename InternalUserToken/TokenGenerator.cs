using System;

namespace SynWord_Server_CSharp.InternalUserToken {
    public class TokenGenerator {
        public static string Generate() {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
