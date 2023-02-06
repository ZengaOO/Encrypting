using Encrypting.Models;

namespace Encrypting.Services
{
    public interface IEncryptService
    {
        Task<byte[]> EncryptAsync(string clearText, string enteredText);
       
        Task<string> DecryptAsync(byte[] encrypted, string enteredText);
    }
}
