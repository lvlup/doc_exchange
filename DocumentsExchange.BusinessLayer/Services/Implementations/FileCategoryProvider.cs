using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataAccessLayer.Repository;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
    public class FileCategoryProvider : IFileCategoryProvider
    {
        private readonly FileCategoryRepository _fileCategoryRepository;

        public FileCategoryProvider(FileCategoryRepository fileCategoryRepository)
        {
            _fileCategoryRepository = fileCategoryRepository;
        }

        public async Task<IEnumerable<FileCategory>> GetAll()
        {
            return await _fileCategoryRepository.GetAll().ConfigureAwait(false);
        }

        public async Task<FileCategory> Get(int id)
        {
            return await _fileCategoryRepository.Get(id).ConfigureAwait(false);
        }

        public async Task<bool> Add(FileCategory fileCategory)
        {
            return await _fileCategoryRepository.Create(fileCategory).ConfigureAwait(false);
        }

        public async Task<bool> Update(FileCategory fileCategory)
        {
            return await _fileCategoryRepository.Update(fileCategory).ConfigureAwait(false);
        }

        public async Task<bool> Delete(int id)
        {
            return await _fileCategoryRepository.Delete(id).ConfigureAwait(false);
        }
    }
}
