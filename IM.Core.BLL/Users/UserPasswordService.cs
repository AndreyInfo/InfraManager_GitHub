using System;
using System.Security.Cryptography;
using System.Text;

namespace InfraManager.BLL.Users;

public class UserPasswordService
{
    public bool ValidateLoginPassword(byte[] passwordSDWeb, string passwordDecrypted)
    {
        byte[] passwordStored = passwordSDWeb;
        if (passwordStored == null)
            return false;

        byte[] password = CalculatePassword(passwordDecrypted);
        if (passwordStored.Length != password.Length)
            return false;

        for (int i = 5; i < passwordStored.Length - 11; i++)
            if (passwordStored[i] != password[i])
                return false;

        return true;
    }

    public byte[] CalculatePassword(string password)
    {
        byte[] crypto = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(password));
        byte[] guid = Guid.NewGuid().ToByteArray();
        byte[] hash = new Byte[crypto.Length + guid.Length];
        Array.Copy(guid, 0, hash, 0, 5);
        Array.Copy(crypto, 0, hash, 5, crypto.Length);
        Array.Copy(guid, 5, hash, crypto.Length + 5, 11);
        return hash;
    }
}