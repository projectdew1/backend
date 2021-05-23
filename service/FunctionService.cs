using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.interfaces;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.service
{
    public class FunctionService : IFunction
    {

        private readonly ApiDBContext _context;
        private readonly IConfiguration _config;

        public FunctionService(ApiDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public string GenerateJSONWebToken(User userInfo)
        {
            var Keys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(Keys, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:Expires"]));

            var claims = new[] {
                new Claim("user",userInfo.Username)
            };

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: expires,
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public User AuthenticateUser(User login)
        {
            User user = Login(login.Username, login.Password);
            return user;
        }
        public User Login(string username, string password)
        {
            User user = null;
            var current = _context.Users;
            try
            {
                var account = current.Where(r => r.Username == username).First();
                var values = Decrypt(account.Password, account.Salt);
                user = account;
                if (account == null || values != password)
                {
                    return null;
                }
                account.LastLogin = DateTime.Now;
                _context.SaveChanges();
                return user;
            }
            catch (System.Exception)
            {

                return null;
            }

        }

        public string PasswordSalt()
        {
            var salt = RandomString(11);
            return salt;
        }
        public string CreatePassword(string password)
        {
            var salt = RandomString(11);
            var result = Encrypt(password, salt);
            return result;
        }
        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        /// password decode encode
        private const string initVector = "webdeveloperbyde"; // 16 Byte
        private const int keysize = 256;

        //passPhrase = salt
        //plainText = password
        //cipherText = passwordhash
        public string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged();



            symmetricKey.Mode = CipherMode.CBC;

            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string cipherText, string passPhrase)
        {

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public string GenID(string id, string prefix)
        {
            if (id == "")
            {
                return prefix + "000001";
            }
            else
            {
                var numID = id.Substring(1, id.Length - 1);
                var number = int.Parse(numID) + 1;
                var getNum = "00000" + number.ToString();
                var get = getNum.Substring(getNum.Length - 6, 6);
                return prefix + get;

            }
        }
    }
}