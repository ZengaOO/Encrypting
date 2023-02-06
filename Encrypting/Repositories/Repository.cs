using Encrypting.Data;
using Encrypting.Models;
using Encrypting.Repositories.Interfases;
using Encrypting.Services;
using Microsoft.AspNetCore.Mvc;

namespace Encrypting.Repository
{
    public class Repository : IRepository

    {
        private readonly ApplicationDbContext _db;
        public Repository(
        ApplicationDbContext db)
        {
            _db = db;
        }

      
        //public async Task<byte[]> EncryptAsync(ContractModel model)
        //{
        //    _db.ContractModels.Add(model);
        //    await _db.SaveChangesAsync();
          
        //}

        public async Task SaveEncriptTextToDatabaseAsync(ContractModel model)
        {
            _db.ContractModels.Add(model); 
            await _db.SaveChangesAsync();
        }
    }
}
