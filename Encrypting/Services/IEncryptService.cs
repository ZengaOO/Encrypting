namespace Encrypting.Services
{
    public interface IEncryptService
    {
        Task<byte[]> EncryptAsync(string clearText, string enteredText);
    }
}
