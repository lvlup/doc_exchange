using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class FilePathProvider: IFilePathProvider
    {
        private readonly FilePathRepository _filePathRepository;

        public FilePathProvider(FilePathRepository fileCategoryRepository)
        {
            _filePathRepository = fileCategoryRepository;
        }

        public async Task<FilePath> Get(int id)
        {
            return await _filePathRepository.Get(id).ConfigureAwait(false);
        }

        public  async Task<bool> Add(FilePath filePath)
        {
            return await _filePathRepository.Create(filePath).ConfigureAwait(false);
        }
    }
}
