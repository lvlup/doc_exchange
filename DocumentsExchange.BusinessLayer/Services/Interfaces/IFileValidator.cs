using System.IO;
using System.Linq;

namespace DocumentsExchange.BusinessLayer.Services.Interfaces
{
    public interface IFileValidator
    {
        bool Validate(string fileName);
    }

    class FileValidator : IFileValidator
    {
        private readonly string[] _extensions = new string[]
        {
            ".jpg",
            ".doc",
            ".pdf",
        };

        public bool Validate(string fileName)
        {
            return !string.IsNullOrEmpty(fileName) && _extensions.Contains(Path.GetExtension(fileName));
        }
    }
}
