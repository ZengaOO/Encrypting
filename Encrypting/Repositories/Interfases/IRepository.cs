using Encrypting.Models;

namespace Encrypting.Repositories.Interfases
{
    public interface IRepository
    {

         Task EncryptAsync(ContractModel model);
        //Task DecryptAsync(ContractModel model);
         Task SaveEncriptTextToDatabaseAsync(ContractModel model);
    }
}
