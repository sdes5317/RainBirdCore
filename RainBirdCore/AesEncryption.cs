using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RainBirdCore
{
    public class AesEncryption
    {
        private static readonly CipherMode _cipherMode = CipherMode.CBC;
        private static readonly PaddingMode _paddingMode = PaddingMode.Zeros;

        public static string Decryptor(byte[] data, string password)
        {
            byte[] aesKey = GetSha256(password);

            var aesIv = new byte[16];
            data.ToList().CopyTo(32, aesIv, 0, 16);

            var sendMessage = new byte[data.Length - 48];
            data.ToList().CopyTo(48, sendMessage, 0, data.Length - 48);

            var decodeData = GetAesDecryptor(aesKey, aesIv, sendMessage);

            var s = Encoding.UTF8.GetString(decodeData);
            return s.TrimEnd('\x10', '\x0A', '\x00');
        }

        private static byte[] GetAesDecryptor(byte[] aesKey, byte[] aesIv, byte[] receiveMessage)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIv;
                aes.Mode = _cipherMode;
                aes.Padding = _paddingMode;

                var decryptor = aes.CreateDecryptor(aesKey, aesIv);
                return decryptor.TransformFinalBlock(receiveMessage, 0, receiveMessage.Length);
            }
        }

        private static byte[] GetAesEncryptor(byte[] aesKey, byte[] aesIv, byte[] sendMessage)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIv;
                aes.Mode = _cipherMode;
                aes.Padding = _paddingMode;

                var encryptor = aes.CreateEncryptor(aesKey, aesIv);
                return encryptor.TransformFinalBlock(sendMessage, 0, sendMessage.Length);
            }
        }

        private static byte[] GetSha256(string encodeString)
        {
            //長度32bytes=256bits
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(encodeString));
            }
        }

        public static byte[] Encryptor(string sendCmd, string password, byte[] aesIv = null)
        {
            var cmdBytes = Encoding.UTF8.GetBytes(sendCmd).ToList();
            cmdBytes.Add(0x00);
            cmdBytes.Add(0x10);

            aesIv = aesIv ?? Guid.NewGuid().ToByteArray();
            var aesKey = GetSha256(password);
            var cmdSha256 = GetSha256(sendCmd);
            var cmdAes = GetAesEncryptor(aesKey, aesIv, cmdBytes.ToArray());

            var packet = new List<byte>();
            packet.AddRange(cmdSha256);
            packet.AddRange(aesIv);
            packet.AddRange(cmdAes);
            return packet.ToArray();
        }
    }
}
