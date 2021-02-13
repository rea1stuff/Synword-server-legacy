﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model
{
    public class UserDataModel
    {
        public string uid { get; set; }
        public bool isPremium { get; set; }
        public int uniqueCheckRequests { get; set; }
        public int uniqueUpRequests { get; set; }
        public int documentUniqueUpRequests { get; set; }
        public int documentUniqueCheckRequests { get; set; }
        public int documentMaxSymbolLimit { get; set; }
        public int uniqueCheckMaxSymbolLimit { get; set; }
        public int uniqueUpMaxSymbolLimit { get; set; }
        public string creationDate { get; set; }
    }
}