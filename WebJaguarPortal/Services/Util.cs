using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Cryptography;
using System.Text;

namespace WebJaguarPortal.Services
{
    internal class Util
    {

        internal static string GenerateHash(string input)
        {
            const int iterations = 10000;
            const int saltSize = 16;
            const int hashSize = 32;

            // Gera um salt aleatório
            byte[] salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Cria o hash usando o PBKDF2
            byte[] hash = new Rfc2898DeriveBytes(input, salt, iterations, HashAlgorithmName.SHA256).GetBytes(hashSize);

            // Concatena o salt com o hash
            byte[] hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            // Retorna o hash como uma string
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyHash(string password, string hashedPassword)
        {
            const int iterations = 10000;
            const int saltSize = 16;
            const int hashSize = 32;
            // Obtém o salt a partir do hash armazenado
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            // Calcula o hash da senha fornecida com o mesmo salt
            byte[] computedHash = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256).GetBytes(hashSize);

            // Compara os hashes
            for (int i = 0; i < hashSize; i++)
            {
                if (hashBytes[i + saltSize] != computedHash[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal static string GenerateKey(int length)
        {
            byte[] randomBytes = new byte[length / 2];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        internal static string GenerateBase64(int bytesLength)
        {
            byte[] randomBytes = new byte[bytesLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

        public static string GenerateMD5(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}

