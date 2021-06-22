
using backend.Models;

namespace backend.interfaces
{
    public interface IFunction
    {
        string Encrypt(string plainText, string passPhrase);
        string Decrypt(string cipherText, string passPhrase);
        User Login(string username, string password);

        User AuthenticateUser(User login);

        string GenerateJSONWebToken(User userInfo);
        string CreatePassword(string password);

        string PasswordSalt();

        string GenID(string id, string prefix);

        string encoding(string toEncode);

        string decoding(string toDecode);

        string covertLink(string link);
    }
}