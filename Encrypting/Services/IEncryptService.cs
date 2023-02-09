using Encrypting.Models;

namespace Encrypting.Services
{
    public interface IEncryptService
    {
        Task<byte[]> EncryptAsync(string clearText);
       
        Task<string> DecryptAsync(byte[] encryptedText);
    }
}
