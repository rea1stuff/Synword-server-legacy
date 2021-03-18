using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class RequestExceptionHandler {
        public static IActionResult Handle(Exception exception) {
            if (new List<Type> { typeof(UserException) }.Contains(exception.GetType().BaseType)) {
                return new ContentResult() {
                    StatusCode = 400,
                    Content = exception.Message
                };
            } else {
                return new ObjectResult(exception.Message) {
                    StatusCode = 500
                };
            }
        }
    }
}
