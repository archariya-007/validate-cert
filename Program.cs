using System;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;

class CertificateValidator
{
    public static void ValidateCertificate(string certFilePath)
    {
        if (!File.Exists(certFilePath))
        {
            throw new FileNotFoundException($"Certificate file not found: {certFilePath}");
        }

        var certificate = new X509Certificate2(certFilePath);

        // Check if the certificate is valid (not expired)
        if (DateTime.Now < certificate.NotBefore || DateTime.Now > certificate.NotAfter)
        {
            throw new SecurityException($"Certificate is not valid. Validity period: {certificate.NotBefore} - {certificate.NotAfter}");
        }

        // Check if the certificate is self-signed
        if (certificate.Subject == certificate.Issuer)
        {
            Console.WriteLine("Warning: The certificate is self-signed.");
        }

        // Optionally, verify the certificate chain
        //using (var chain = new X509Chain())
        //{
        //    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online; // Check revocation status
        //    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
        //    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;

        //    if (!chain.Build(certificate))
        //    {
        //        foreach (var status in chain.ChainStatus)
        //        {
        //            Console.WriteLine($"Chain error: {status.StatusInformation}");
        //        }
        //        throw new SecurityException("Certificate chain validation failed.");
        //    }
        //}

        Console.WriteLine("Certificate is valid.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void Main(string[] args)
    {
        string certPath = @"C:\health-repos\POCs\validate-cert\justnotdpad.cer";
        // @"\\wex-share-d01.mbe.local\Certificates\gsassocert-bad.cer";


        try
        {
            ValidateCertificate(certPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Validation failed: {ex.Message}");
        }
    }
}