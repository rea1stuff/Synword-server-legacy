using SynWord_Server_CSharp.Model.Request;
using System;
using System.Collections.Generic;
using System.IO;

namespace SynWord_Server_CSharp.Logging {
    static class RequestLogger {
        public static void LogRequestStatus(RequestType type, Dictionary<string, dynamic> otherInfo, RequestStatus status) {
            string otherInfoString = "";

            foreach (KeyValuePair<string, dynamic> pair in otherInfo) {
                otherInfoString += pair.Key + ": " + pair.Value + ". ";
            }

            string message = "[" + DateTime.Now + "] " + "Request: " + type.Name + ". " + otherInfoString + "Status: " + status.Name + ".";

            Console.WriteLine(message);
            Log(type, message);
        }

        public static void LogException(RequestType type, Dictionary<string, dynamic> otherInfo, string exceptionMessage) {
            string otherInfoString = "";

            foreach (KeyValuePair<string, dynamic> pair in otherInfo) {
                otherInfoString += pair.Key + ": " + pair.Value + ". ";
            }

            string message = "[" + DateTime.Now + "] " + "Request: " + type.Name + ". " + otherInfoString + "Exception: " + exceptionMessage + ".";

            Console.WriteLine(message);
            Log(type, message);
        }

        private static void Log(RequestType type, string message) {
            string path = "../SynWord-Server-CSharp/Log/" + type.Name + "Log.txt";

            using StreamWriter streamWriter = new StreamWriter(path, true, System.Text.Encoding.Default);
            streamWriter.WriteLine(message);
        }
    }
}
