﻿//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;
//using System.Security.Cryptography;
//using System.Text;

//namespace Encrypting
//{
//    public class AesEncryption
//    {
//        private static readonly byte[] _emptyKey = new byte[32];

//        // TODO: consider moving regions in separate classes or partial classes

//        #region methods with password

//        /// <summary>
//        /// Encrypts string and returns string. Salt and IV will be embedded to encrypted string.
//        /// Can later be decrypted with <see cref="DecryptWithPassword(string, string, ReportAndCancellationToken)"/>
//        /// IV and salt are generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits) and key size will be 32 bytes (256 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">String to encrypt</param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Encrypted string</returns>
//        public static string EncryptWithPassword(string dataToEncrypt, string password, ReportAndCancellationToken token = null)
//        {
//            byte[] data = Encoding.UTF8.GetBytes(dataToEncrypt);
//            byte[] result = EncryptWithPassword(data, password, token);
//            return Convert.ToBase64String(result);
//        }

//        /// <summary>
//        /// Decrypts string with embedded salt and IV that are encrypted with <see cref="EncryptWithPassword(byte[], string, ReportAndCancellationToken)"/>
//        /// </summary>
//        /// <param name="dataToDecrypt">string to decrypt</param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Decrypted string</returns>
//        public static string DecryptWithPassword(string dataToDecrypt, string password, ReportAndCancellationToken token = null)
//        {
//            byte[] data = Convert.FromBase64String(dataToDecrypt);
//            byte[] result = DecryptWithPassword(data, password, token);
//            return Encoding.UTF8.GetString(result);
//        }

//        /// <summary>
//        /// Encrypts byte array and returns byte array. Salt and IV will be embedded in result.
//        /// Can be later decrypted using <see cref="DecryptWithPassword(byte[], string, ReportAndCancellationToken)"/>
//        /// IV and salt are generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits) and key size will be 32 bytes (256 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">Bytes to encrypt</param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Encrypted bytes</returns>
//        public static byte[] EncryptWithPassword(byte[] dataToEncrypt, string password, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToEncrypt, (inStream, outStream) => EncryptWithPassword(inStream, password, outStream, token));

//        /// <summary>
//        /// Decrypts byte array with embedded salt and IV that is encrypted with <see cref="EncryptWithPassword(byte[], string, ReportAndCancellationToken)"/>
//        /// </summary>
//        /// <param name="dataToDecrypt"></param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Decrypted bytes</returns>
//        public static byte[] DecryptWithPassword(byte[] dataToDecrypt, string password, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToDecrypt, (inStream, outStream) => DecryptWithPassword(inStream, password, outStream, token));

//        /// <summary>
//        /// Reads data from stream (parameter dataToEncrypt) and writes encrypted data to stream (parameter destination).
//        /// Can be later decrypted using <see cref="DecryptWithPassword(Stream, string, Stream, ReportAndCancellationToken)"/>.
//        /// Salt and IV will be embedded to resulting stream.
//        /// IV and salt are generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits) and key size will be 32 bytes (256 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">Stream containing data to encrypt</param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="destination">Stream to which to write encrypted data</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void EncryptWithPassword(Stream dataToEncrypt, string password, Stream destination, ReportAndCancellationToken token = null)
//        {
//            byte[] salt;
//            var ph = new PasswordHasher(32);
//            byte[] key = ph.HashPasswordAndGenerateSalt(password, out salt);
//            EncryptAndEmbedIv(dataToEncrypt, key, destination, salt, token);
//        }

//        /// <summary>
//        /// Reads data from stream (parameter dataToEncrypt) and writes encrypted data to stream (parameter destination) asynchronously.
//        /// Can be later decrypted using <see cref="DecryptWithPasswordAsync(Stream, string, Stream, ReportAndCancellationToken)"/>.
//        /// Salt and IV will be embedded to resulting stream.
//        /// IV and salt are generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits) and key size will be 32 bytes (256 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">Stream containing data to encrypt</param>
//        /// <param name="password">Password that is used for generating key for encryption/decryption</param>
//        /// <param name="destination">Stream to which to write encrypted data</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Task to await</returns>
//        public static Task EncryptWithPasswordAsync(Stream dataToEncrypt, string password, Stream destination, ReportAndCancellationToken token = null)
//        {
//            byte[] salt;
//            var ph = new PasswordHasher(32);
//            byte[] key = ph.HashPasswordAndGenerateSalt(password, out salt);
//            return EncryptAndEmbedIvAsync(dataToEncrypt, key, destination, salt, token);
//        }

//        /// <summary>
//        /// Decrypts the with password data that was encrypted with <see cref="EncryptWithPassword(Stream, string, Stream, ReportAndCancellationToken)"/>
//        /// </summary>
//        /// <param name="dataToDecrypt">The data to decrypt.</param>
//        /// <param name="password">The password.</param>
//        /// <param name="destination">The destination stream.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void DecryptWithPassword(Stream dataToDecrypt, string password, Stream destination, ReportAndCancellationToken token = null)
//            => DecryptWithEmbeddedIv(dataToDecrypt, null, destination, password, token);

//        /// <summary>
//        /// Decrypts the with password asynchronously data that was encrypted with <see cref="EncryptWithPasswordAsync(Stream, string, Stream, ReportAndCancellationToken)"/>
//        /// </summary>
//        /// <param name="dataToDecrypt">The data to decrypt.</param>
//        /// <param name="password">The password.</param>
//        /// <param name="destination">The destination stream.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static Task DecryptWithPasswordAsync(Stream dataToDecrypt, string password, Stream destination, ReportAndCancellationToken token = null)
//            => DecryptWithEmbeddedIvAsync(dataToDecrypt, null, destination, password, token);

//        #endregion

//        #region methods with embedded iv

//        /// <summary>
//        /// Encrypts bytes and embeds IV. Can be decrypted with <see cref="DecryptWithEmbeddedIv(byte[], byte[], ReportAndCancellationToken)"/>
//        /// IV is generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits) and key size will be 32 bytes (256 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">Bytes to encrypt</param>
//        /// <param name="key">Key that will be used for encryption/decryption</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Byte array, encrypted data with embedded IV</returns>
//        public static byte[] EncryptAndEmbedIv(byte[] dataToEncrypt, byte[] key, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToEncrypt, (inStream, outStream) => EncryptAndEmbedIv(inStream, key, outStream, token));

//        /// <summary>
//        /// Decrypts bytes with embedded IV encrypted with <see cref="EncryptAndEmbedIv(byte[], byte[], ReportAndCancellationToken)"/>
//        /// </summary>
//        /// <param name="dataToDecrypt">Bytes, data with embedded IV, to decrypt</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Byte array, encrypted data</returns>
//        public static byte[] DecryptWithEmbeddedIv(byte[] dataToDecrypt, byte[] key, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToDecrypt, (inStream, outStream) => DecryptWithEmbeddedIv(inStream, key, outStream, token));

//        /// <summary>
//        /// Encrypts and embeds IV into result. Data is read from stream and encrypted data is wrote to stream.
//        /// Can be decrypted with <see cref="DecryptWithEmbeddedIv(Stream, byte[], Stream, ReportAndCancellationToken)"/>
//        /// IV is generated by <see cref="CryptoRandom"/> which is using System.Security.Cryptography.Rfc2898DeriveBytes.
//        /// IV size is 16 bytes (128 bits).
//        /// </summary>
//        /// <param name="dataToEncrypt">Stream containing data to encrypt</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="destination"></param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void EncryptAndEmbedIv(Stream dataToEncrypt, byte[] key, Stream destination, ReportAndCancellationToken token = null)
//            => EncryptAndEmbedIv(dataToEncrypt, key, destination, null, token);

//        private static void EncryptAndEmbedIv(Stream dataToEncrypt, byte[] key, Stream destination, byte[] salt, ReportAndCancellationToken token = null)
//        {
//            byte[] iv = CryptoRandom.NextBytesStatic(16);
//            Encrypt(new CryptoRequest
//            {
//                EmbedIV = true,
//                InData = dataToEncrypt,
//                OutData = destination,
//                IV = iv,
//                Key = key,
//                EmbedSalt = salt != null,
//                Salt = salt,
//                Token = token
//            });
//        }

//        private static Task EncryptAndEmbedIvAsync(Stream dataToEncrypt, byte[] key, Stream destination, byte[] salt, ReportAndCancellationToken token = null)
//        {
//            byte[] iv = CryptoRandom.NextBytesStatic(16);
//            return EncryptAsync(new CryptoRequest
//            {
//                EmbedIV = true,
//                InData = dataToEncrypt,
//                OutData = destination,
//                IV = iv,
//                Key = key,
//                EmbedSalt = salt != null,
//                Salt = salt,
//                Token = token
//            });
//        }

//        /// <summary>
//        /// Decrypts data with embedded IV, that is encrypted with <see cref="EncryptAndEmbedIv(Stream, byte[], Stream, ReportAndCancellationToken)"/>, into result. 
//        /// Data is read from stream and decrypted data is wrote to stream.
//        /// </summary>
//        /// <param name="dataToDecrypt">Stream containing data to decrypt.</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="destination">Stream to which decrypted data will be wrote.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void DecryptWithEmbeddedIv(Stream dataToDecrypt, byte[] key, Stream destination, ReportAndCancellationToken token = null)
//            => DecryptWithEmbeddedIv(dataToDecrypt, key, destination, null, token);

//        private static void DecryptWithEmbeddedIv(Stream dataToDecrypt, byte[] key, Stream destination, string password, ReportAndCancellationToken token = null)
//        {
//            Decrypt(new CryptoRequest
//            {
//                EmbedIV = true,
//                InData = dataToDecrypt,
//                OutData = destination,
//                Key = key,
//                Password = password,
//                EmbedSalt = password != null,
//                Token = token
//            });
//        }

//        private static Task DecryptWithEmbeddedIvAsync(Stream dataToDecrypt, byte[] key, Stream destination, string password, ReportAndCancellationToken token = null)
//        {
//            return DecryptAsync(new CryptoRequest
//            {
//                EmbedIV = true,
//                InData = dataToDecrypt,
//                OutData = destination,
//                Key = key,
//                Password = password,
//                EmbedSalt = password != null,
//                Token = token
//            });
//        }

//        #endregion

//        #region base methods

//        /// <summary>
//        /// Encrypts data from byte array to byte array.
//        /// </summary>
//        /// <param name="dataToEncrypt"></param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Encrypted data in for of bytes</returns>
//        public static byte[] Encrypt(byte[] dataToEncrypt, byte[] key, byte[] iv, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToEncrypt, (inStream, outStream) =>
//            Encrypt(new CryptoRequest
//            {
//                EmbedIV = true,
//                InData = inStream,
//                OutData = outStream,
//                IV = iv,
//                Key = key,
//                Token = token
//            }));

//        /// <summary>
//        /// Decrypts data from byte array to byte array.
//        /// </summary>
//        /// <param name="dataToDecrypt"></param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Decrypted data in for of bytes</returns>
//        public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key, byte[] iv, ReportAndCancellationToken token = null)
//            => HandleByteToStream(dataToDecrypt, (inStream, outStream) => Decrypt(inStream, key, iv, outStream, token));

//        /// <summary>
//        /// Encrypts data from stream to stream.
//        /// </summary>
//        /// <param name="dataToEncrypt">Stream with data to decrypt.</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="destination">Stream to which encrypted data will be wrote.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void Encrypt(Stream dataToEncrypt, byte[] key, byte[] iv, Stream destination, ReportAndCancellationToken token = null)
//            => Encrypt(new CryptoRequest
//            {
//                SkipValidations = false,
//                InData = dataToEncrypt,
//                OutData = destination,
//                Key = key,
//                IV = iv,
//                Token = token
//            });

//        /// <summary>
//        /// Encrypts data from stream to stream asynchronously.
//        /// </summary>
//        /// <param name="dataToEncrypt">Stream with data to decrypt.</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="destination">Stream to which encrypted data will be wrote.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Task to await</returns>
//        public static Task EncryptAsync(Stream dataToEncrypt, byte[] key, byte[] iv, Stream destination, ReportAndCancellationToken token = null)
//            => EncryptAsync(new CryptoRequest
//            {
//                SkipValidations = false,
//                InData = dataToEncrypt,
//                OutData = destination,
//                Key = key,
//                IV = iv,
//                Token = token
//            });

//        /// <summary>
//        /// Decrypts data from stream to stream.
//        /// </summary>
//        /// <param name="dataToDecrypt">Stream with data to encrypt.</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="destination">Stream to which decrypted data will be wrote.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        public static void Decrypt(Stream dataToDecrypt, byte[] key, byte[] iv, Stream destination, ReportAndCancellationToken token = null)
//            => Decrypt(new CryptoRequest
//            {
//                IV = iv,
//                Key = key,
//                InData = dataToDecrypt,
//                OutData = destination,
//                SkipValidations = false,
//                Token = token
//            });

//        /// <summary>
//        /// Decrypts data from stream to stream asynchronously.
//        /// </summary>
//        /// <param name="dataToDecrypt">Stream with data to encrypt.</param>
//        /// <param name="key">Key that will be used for encryption/decryption, must be 16, 24 or 32 bytes long.</param>
//        /// <param name="iv">Initialization vector, must be 16 bytes</param>
//        /// <param name="destination">Stream to which decrypted data will be wrote.</param>
//        /// <param name="token">Optional token for progress reporting and canceling operation.</param>
//        /// <returns>Task to await</returns>
//        public static Task DecryptAsync(Stream dataToDecrypt, byte[] key, byte[] iv, Stream destination, ReportAndCancellationToken token = null)
//            => DecryptAsync(new CryptoRequest
//            {
//                IV = iv,
//                Key = key,
//                InData = dataToDecrypt,
//                OutData = destination,
//                SkipValidations = false,
//                Token = token
//            });

//        internal static void Encrypt(CryptoRequest request)
//        {
//            CryptoContainer container = request.ValidateEncryption();
//            ReportAndCancellationToken token = request.Token ?? new ReportAndCancellationToken();
//            token.CanReportProgress = request.InData.CanSeek;

//            using (var aes = GetAes())
//            using (var encryptor = GetEncryptorAndSetAes(aes, request))
//            {
//                int bufferSize = aes.BlockSize;
//                if (token.CanReportProgress ?? false)
//                {
//                    token.NumberOfIterations = (int)Math.Ceiling(request.InData.Length / (double)bufferSize);
//                }
//                CryptoStream cs = new CryptoStream(request.OutData, encryptor, CryptoStreamMode.Write);
//                byte[] buffer = new byte[bufferSize];
//                int read = 0;
//                int iterationCount = 0;
//                while ((read = request.InData.Read(buffer, 0, bufferSize)) > 0)
//                {
//                    cs.Write(buffer, 0, read);
//                    cs.Flush();

//                    if (token.IsCanceled)
//                    {
//                        break;
//                    }
//                    iterationCount++;
//                    token.ReportProgressInternal(iterationCount);
//                }
//                if (token.IsCanceled)
//                {
//                    return;
//                }
//                cs.FlushFinalBlock();
//            }
//            if (!request.SkipValidations)
//            {
//                container.WriteChecksAndEmbeddedData();
//            }
//        }

//        internal static async Task EncryptAsync(CryptoRequest request)
//        {
//            CryptoContainer container = request.ValidateEncryption();
//            ReportAndCancellationToken token = request.Token ?? new ReportAndCancellationToken();
//            token.CanReportProgress = request.InData.CanSeek;

//            using (var aes = GetAes())
//            using (var encryptor = GetEncryptorAndSetAes(aes, request))
//            {
//                int bufferSize = aes.BlockSize;
//                if (token.CanReportProgress ?? false)
//                {
//                    token.NumberOfIterations = (int)Math.Ceiling(request.InData.Length / (double)bufferSize);
//                }
//                CryptoStream cs = new CryptoStream(request.OutData, encryptor, CryptoStreamMode.Write);
//                byte[] buffer = new byte[bufferSize];
//                int read = 0;
//                int iterationCount = 0;
//                while ((read = await request.InData.ReadAsync(buffer, 0, bufferSize)) > 0)
//                {
//                    await cs.WriteAsync(buffer, 0, read);
//                    await cs.FlushAsync();

//                    if (token.IsCanceled)
//                    {
//                        break;
//                    }
//                    iterationCount++;
//                    token.ReportProgressInternal(iterationCount);
//                }
//                if (token.IsCanceled)
//                {
//                    return;
//                }
//                cs.FlushFinalBlock();
//            }
//            if (!request.SkipValidations)
//            {
//                await container.WriteChecksAndEmbeddedDataAsync();
//            }
//        }

//        internal static void Decrypt(CryptoRequest request)
//        {
//            CryptoContainer container = request.ValidateDecrypt(request);
//            ReportAndCancellationToken token = request.Token ?? new ReportAndCancellationToken();
//            token.CanReportProgress = request.InData.CanSeek;

//            using (var aes = GetAes())
//            using (ICryptoTransform decryptor = GetDecryptorAndSetAes(aes, request))
//            {
//                int bufferSize = aes.BlockSize;
//                if (token.CanReportProgress ?? false)
//                {
//                    token.NumberOfIterations = (int)Math.Ceiling(request.InData.Length / (double)bufferSize);
//                }
//                CryptoStream cs = new CryptoStream(request.OutData, decryptor, CryptoStreamMode.Write);
//                byte[] buffer = new byte[bufferSize];
//                int read = 0;
//                int iterationCount = 0;
//                while ((read = request.InData.Read(buffer, 0, bufferSize)) > 0)
//                {
//                    cs.Write(buffer, 0, read);
//                    cs.Flush();

//                    if (token.IsCanceled)
//                    {
//                        break;
//                    }
//                    iterationCount++;
//                    token.ReportProgressInternal(iterationCount);
//                }
//                if (token.IsCanceled)
//                {
//                    return;
//                }
//                cs.FlushFinalBlock();
//            }
//        }

//        internal static async Task DecryptAsync(CryptoRequest request)
//        {
//            CryptoContainer container = request.ValidateDecrypt(request);
//            ReportAndCancellationToken token = request.Token ?? new ReportAndCancellationToken();
//            token.CanReportProgress = request.InData.CanSeek;

//            using (var aes = GetAes())
//            using (ICryptoTransform decryptor = GetDecryptorAndSetAes(aes, request))
//            {
//                int bufferSize = aes.BlockSize;
//                if (token.CanReportProgress ?? false)
//                {
//                    token.NumberOfIterations = (int)Math.Ceiling(request.InData.Length / (double)bufferSize);
//                }
//                CryptoStream cs = new CryptoStream(request.OutData, decryptor, CryptoStreamMode.Write);
//                byte[] buffer = new byte[bufferSize];
//                int read = 0;
//                int iterationCount = 0;
//                while ((read = await request.InData.ReadAsync(buffer, 0, bufferSize)) > 0)
//                {
//                    await cs.WriteAsync(buffer, 0, read);
//                    await cs.FlushAsync();

//                    if (token.IsCanceled)
//                    {
//                        break;
//                    }
//                    iterationCount++;
//                    token.ReportProgressInternal(iterationCount);
//                }
//                if (token.IsCanceled)
//                {
//                    return;
//                }
//                cs.FlushFinalBlock();
//            }
//        }

//        #endregion

//        #region validations

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">The key</param>
//        /// <param name="iv">The IV</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedData(byte[] encryptedData, byte[] key, byte[] iv)
//            => HandleByteToStream(encryptedData, (stream) => ValidateEncryptedData(stream, key, iv));

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">The key</param>
//        /// <param name="iv">The IV</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedData(Stream encryptedData, byte[] key, byte[] iv)
//            => ValidateEncryptedData(new CryptoRequest
//            {
//                InData = encryptedData,
//                IV = iv,
//                Key = key
//            });

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">Key used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        [Obsolete("Method name changed to ValidateEncryptedDataWithEmbeddedIv, this method will be removed in v5")]
//        public static ValidationResult ValidateEncryptedDataWithEmbededIv(byte[] encryptedData, byte[] key)
//            => ValidateEncryptedDataWithEmbeddedIv(encryptedData, key);

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">Key used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedDataWithEmbeddedIv(byte[] encryptedData, byte[] key)
//            => HandleByteToStream(encryptedData, (stream) => ValidateEncryptedDataWithEmbededIv(stream, key));

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">Key used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        [Obsolete("Method name changed to ValidateEncryptedDataWithEmbeddedIv, this method will be removed in v5")]
//        public static ValidationResult ValidateEncryptedDataWithEmbededIv(Stream encryptedData, byte[] key)
//            => ValidateEncryptedDataWithEmbeddedIv(encryptedData, key);

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="key">Key used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedDataWithEmbeddedIv(Stream encryptedData, byte[] key)
//            => ValidateEncryptedData(new CryptoRequest
//            {
//                InData = encryptedData,
//                Key = key,
//                EmbedIV = true
//            });

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="password">Password used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedDataWithPassword(string encryptedData, string password)
//            => ValidateEncryptedDataWithPassword(Convert.FromBase64String(encryptedData), password);

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="password">Password used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedDataWithPassword(byte[] encryptedData, string password)
//            => HandleByteToStream(encryptedData, (stream) => ValidateEncryptedDataWithPassword(stream, password));

//        /// <summary>
//        /// Validates the encrypted data.
//        /// </summary>
//        /// <param name="encryptedData">The encrypted data</param>
//        /// <param name="password">Password used for encryption</param>
//        /// <returns>ValidationResult</returns>
//        public static ValidationResult ValidateEncryptedDataWithPassword(Stream encryptedData, string password)
//            => ValidateEncryptedData(new CryptoRequest
//            {
//                InData = encryptedData,
//                EmbedIV = true,
//                EmbedSalt = true,
//                Password = password
//            });

//        internal static ValidationResult ValidateEncryptedData(CryptoRequest request)
//            => CryptoContainer.CreateForDecryption(request).ReadAndValidateDataForDecryption();

//        #endregion

//        internal static byte[] HandleByteToStream(byte[] data, Action<Stream, Stream> action)
//        {
//            byte[] result;
//            using (Stream inStream = new MemoryStream())
//            using (Stream outStream = new MemoryStream())
//            {
//                inStream.Write(data, 0, data.Length);
//                inStream.Flush();
//                inStream.Position = 0;
//                action(inStream, outStream);
//                outStream.Position = 0;
//                result = new byte[outStream.Length];
//                outStream.Read(result, 0, result.Length);
//            }
//            return result;
//        }

//        internal static ValidationResult HandleByteToStream(byte[] data, Func<Stream, ValidationResult> func)
//        {
//            ValidationResult result;
//            using (Stream stream = new MemoryStream())
//            {
//                stream.Write(data, 0, data.Length);
//                stream.Flush();
//                stream.Position = 0;
//                result = func(stream);
//            }
//            return result;
//        }

//        private static Aes GetAes() => Aes.Create();

//        private static ICryptoTransform GetEncryptorAndSetAes(Aes aes, CryptoRequest request)
//        {
//            aes.IV = request.IV;
//            aes.Key = request.Key;
//            // aes.Padding = PaddingMode.PKCS7;
//            // aes.BlockSize = 128;

//            return aes.CreateEncryptor();
//        }

//        private static ICryptoTransform GetDecryptorAndSetAes(Aes aes, CryptoRequest request)
//        {
//            aes.IV = request.IV;
//            aes.Key = request.Key;
//            // aes.Padding = PaddingMode.PKCS7;
//            // aes.BlockSize = 128;
//            return aes.CreateDecryptor();
//        }
//    }
//}
//    }
//}
