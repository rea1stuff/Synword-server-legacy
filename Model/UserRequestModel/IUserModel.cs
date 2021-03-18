using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model {
    public interface IUserModel {
        public string Uid { get; set; }
        public string Text { get; set; }
    }
}
