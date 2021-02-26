using SynWord_Server_CSharp.Model.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SynWord_Server_CSharp.Logging {
    static class RequestLogger {
        private static readonly BlockingCollection<RequestLog> queue;

        static RequestLogger() {
            queue = new BlockingCollection<RequestLog>();
        }

        public static void Add(RequestLog requestLog) {
            queue.Add(requestLog);
        }

        public static Task AddAsync(RequestLog requestLog) {
            return Task.Run(() => queue.Add(requestLog));
        }

        public static bool TryAdd(RequestLog requestLog) {
            return queue.TryAdd(requestLog);
        }

        public static bool TryAdd(RequestLog requestLog, int millisecondsTimeout) {
            return queue.TryAdd(requestLog, millisecondsTimeout);
        }

        public static void Logging() {
            while (true) {
                if (queue.TryTake(out RequestLog currentLog)) {
                    if (currentLog is RequestStatusLog statusLog) {
                        LogRequestStatus(statusLog.Type, statusLog.OtherInfo, statusLog.RequestStatus);
                    } else if (currentLog is RequestExceptionLog exceptionLog) {
                        LogException(exceptionLog.Type, exceptionLog.OtherInfo, exceptionLog.ExceptionMessage);
                    }
                }
            }
        }

        private static void LogRequestStatus(RequestType type, Dictionary<string, dynamic> otherInfo, RequestStatus status) {
            string otherInfoString = "";

            string path = "../SynWord-Server-CSharp/Log/" + type.Name + "Log.txt";

            foreach (KeyValuePair<string, dynamic> pair in otherInfo) {
                otherInfoString += pair.Key + ": " + pair.Value + ".\n";
            }

            string message = "[" + DateTime.Now + "] " + "Request: " + type.Name + ".\n" + otherInfoString + "Status: " + status.Name + ".\n";

            Console.WriteLine(message);
            Log(path, message);
        }

        private static void LogException(RequestType type, Dictionary<string, dynamic> otherInfo, string exceptionMessage) {
            string otherInfoString = "";

            string path = "../SynWord-Server-CSharp/Log/ExceptionLog.txt";

            foreach (KeyValuePair<string, dynamic> pair in otherInfo) {
                otherInfoString += pair.Key + ": " + pair.Value + ".\n";
            }

            string message = "[" + DateTime.Now + "] " + "Request: " + type.Name + ".\n" + otherInfoString + "Exception: " + exceptionMessage + ".\n";

            Console.WriteLine(message);
            Log(path, message);
        }

        private static void Log(string path, string message) {
            using StreamWriter streamWriter = new StreamWriter(path, true, System.Text.Encoding.Default);
            streamWriter.WriteLine(message);
        }
    }
}
