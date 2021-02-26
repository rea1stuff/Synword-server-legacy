using Microsoft.AspNetCore.Hosting;

namespace SynWord_Server_CSharp {
    static class WebRootPath {
        private static string path;

        public static string Path {
            get { 
                return path; 
            } 
        }

        public static void Initialize(IWebHostEnvironment webHostEnvironment) {
            path = webHostEnvironment.WebRootPath;
        }
    }
}
