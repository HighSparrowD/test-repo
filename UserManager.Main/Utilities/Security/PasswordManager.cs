using System.Security.Cryptography;

namespace UserManager.Main.Utilities.Security;

public static class PasswordManager
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    public static string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        byte[] hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

        string base64Hash = Convert.ToBase64String(hashBytes);

        return base64Hash;
        
    }

    public static bool VerifiPassword(string passwordHash, string password)
    {
        byte[] hashBytes = Convert.FromBase64String(passwordHash);

        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        for (int i = 0; i < KeySize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
