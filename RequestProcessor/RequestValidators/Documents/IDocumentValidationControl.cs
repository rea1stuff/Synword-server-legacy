using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using SynWord_Server_CSharp.Exceptions;

namespace SynWord_Server_CSharp.RequestProcessor.RequestValidators.Documents {
    public abstract class IDocumentValidationControl : IValidationControl {
        IFormFile _file;
        string _path = ContentRootPath.Path + @"/Files/UploadedFiles/";
        public IDocumentValidationControl(IFormFile file) {
            _file = file;
        }
        protected abstract bool IsPremium();
        public void PremiumVerification() {
            if (!IsPremium()) {
                throw new NoPremiumException(); 
            }
        }

        public void FileExtensionVerification() {
            if (_file.Length < 0 || Path.GetExtension(_file.FileName) != ".docx") {
                throw new InvalidFileExtensionException();
            }
        }
        public void IsDirectoryExists() {
            if (!Directory.Exists(_path)) {
                Directory.CreateDirectory(_path);
            }
        }
        public void CreateFile(string filePath, IFormFile file) {
            using (FileStream fileStream = System.IO.File.Create(filePath)) {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
        }
    }
}
