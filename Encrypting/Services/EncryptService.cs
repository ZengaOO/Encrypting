﻿using Encrypting.Models;
using Encrypting.Repositories.Interfases;
using Encrypting.Repository;
using System.Security.Cryptography;
using System.Text;

namespace Encrypting.Services
{
    public class EncryptService : IEncryptService
    {
       

        public async Task<byte[]> EncryptAsync(string clearText, string enteredText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(enteredText);
            aes.IV = IV;

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);

            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
            await cryptoStream.FlushFinalBlockAsync();

            return output.ToArray();
        }

        private byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
           // var hashMethod = HashAlgorithmName.SHA256;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

       
        private byte[] IV =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };

        public async Task<string> DecryptAsync(byte[] encrypted, string enteredText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(enteredText);
            aes.IV = IV;

            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);

            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }
    }

}
