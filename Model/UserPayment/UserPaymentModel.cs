using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Model.UserPayment
{
    public class UserPaymentModel
    {
        public String accessToken { get; set; }
        public String inAppItemId { get; set; }
        public String purchaseToken { get; set; }
    }
}
