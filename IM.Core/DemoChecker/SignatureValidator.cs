using System;
using System.Security.Cryptography;
using System.Text;

namespace InfraManager.DemoChecker;
//TODO: Вынести в отдельную сборку (eg Inframanager.EditionRestriction)
public static class SignatureValidator
{
    private static readonly string _publicKey = @"MIIBojANBgkqhkiG9w0BAQEFAAOCAY8AMIIBigKCAYEAjaClMlydP47QsN+E9rrdyhuC+9eeBpsy3iqlRLkFkL0HaK2kjiIsaFeXjtW2MLGwd2GDU2e78L3IqTu9xtNjTxhcGANx+ysn+sgJJjqu4JVHE/0/UaCqletusGS6Zjs02VXWKsfllosbR3LGm4d9/hr15Y5g1vw1qSKZS2MyTEDppu9DysZ3eb76FmkenmMxrVficslNKzZK199hMnnbcz+NFRIYfbiUtLAcqXeB2AyWmy5gvN+mOgUEhd5egvVvwsbCAl1ZNipY9Jnk1KCsZtVlfuPUgQroY/I7iRIzAmsojzTegbLF/2cF+m8838KTX8njIQ+C6YLrTQ4xC+koA7JE88WdyqAiT4YJCNCc+6ZlVS7KR0MOdF8EIO8VhTT4WznV7XIjLgxsB8BSQn5KtaRUqNtditsZzhUceKoE+s3ZGVkGpA4bGp2RSBCvheuw657E9vJEG4vgUIXujo+hOojJK3PPxysSEtwduMdJweMxnCwzcOKd4gdMeUMOup07AgMBAAE=";
    private static readonly byte[] _publicKeyBytes = Convert.FromBase64String(_publicKey);
    
    public static bool VerifyData(DemoLicenceObject licenceObject)
    {
        bool isValid;
        using (var rsa = RSA.Create(3072))
        {
            var encoder = new UTF8Encoding();
            var originalData = encoder.GetBytes(licenceObject.TimeTo);

            try
            {
                rsa.ImportSubjectPublicKeyInfo(_publicKeyBytes, out var readBytes);
                
                var signature = Convert.FromBase64String(licenceObject.Signature);
                isValid = rsa.VerifyData(originalData, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        return isValid;
    }
}