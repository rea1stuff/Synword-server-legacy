using Microsoft.AspNetCore.Mvc;
using SynWord_Server_CSharp.Exceptions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SynWord_Server_CSharp.RequestProcessor.RequestHandlers {
    public class RequestExceptionHandler {
        public static IActionResult Handle(Exception exception) {
            if (new List<Type> { typeof(UserException) }.Contains(exception.GetType().BaseType)) {
                string content;

                if (exception.Data != null) {
                    content = JsonConvert.SerializeObject(exception.Data);
                } else {
                    content = exception.Message;
                }

                return new ContentResult() {
                    StatusCode = 400,
                    Content = content
                };
            } else {
                return new ObjectResult(exception.Message) {
                    StatusCode = 500
                };
            }
        }
    }
}
