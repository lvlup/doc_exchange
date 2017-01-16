using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class FileProvider: IFileProvider
    {
        private readonly FileRepository _fileRepository;

        public FileProvider(FileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public async Task<IEnumerable<File>> GetFilesWithCategory(int orgId, int fileCategoryId)
        {
            return await _fileRepository.GetFilesWithCategory(orgId, fileCategoryId).ConfigureAwait(false);
        }

        public async Task<File> Get(int id)
        {
            return await _fileRepository.Get(id).ConfigureAwait(false);
        }

        public async Task<bool> Add(File file)
        {
            return await _fileRepository.Create(file).ConfigureAwait(false);
        }

        public async Task<bool> Delete(int id)
        {
            return await _fileRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
