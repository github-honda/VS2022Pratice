/*

20240514

ref:
https://learn.microsoft.com/en-us/dotnet/api/system.net.security.sslstream?view=net-8.0&redirectedfrom=MSDN
\CodeHelper\cs\KeyWord\SslStreamClass.txt
SslTcpClientSample
SslTcpServerSample
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FormBase.DTCPIP
{
    internal class SslTcpClientSample
    {
        //private readonly static Hashtable certificateErrors = new Hashtable();

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate? certificate,
              X509Chain? chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
        private static void DisplayUsage()
        {
            Console.WriteLine("To start the client specify:");
            Console.WriteLine("clientSync machineName [serverName]");
            Environment.Exit(1);
        }

        static void DisplaySecurityServices(SslStream stream)
        {
            /*

            public override bool IsAuthenticated { get; }
                true if successful authentication occurred; otherwise, false.
                若通過驗證, 則為 true.
            public override bool IsServer { get; }
                true if the local endpoint was successfully authenticated as the server side of the authenticated connection; otherwise false.
                若本地端已通過伺服器驗證可連線, 則為 true
            public override bool IsSigned { get; }
                true if the data is signed before being transmitted; otherwise false.
                若已簽名傳輸的資料, 則為 true.
            public override bool IsEncrypted { get; }
                true if data is encrypted before being transmitted over the network and decrypted when it reaches the remote endpoint; otherwise false.
                若已加密傳輸的資料, 則為 true.
            public override bool IsMutuallyAuthenticated { get; }
                true if both server and client have been authenticated; otherwise false.
                若伺服器與本地端都已驗證, 則為 true.
            */
            Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
            Console.WriteLine("IsSigned: {0}", stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
            Console.WriteLine("Is mutually authenticated: {0}", stream.IsMutuallyAuthenticated);
        }
    }
}
