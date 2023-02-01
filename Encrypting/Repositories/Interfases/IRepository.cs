using Encrypting.Models;

namespace Encrypting.Repositories.Interfases
{
    public interface IRepository
    {

        // Task<byte[]> EncryptAsync(ContractModel model);
         Task SaveEncriptTextToDatabaseAsync(ContractModel model);
    }
}
