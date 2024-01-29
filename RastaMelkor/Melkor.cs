using System;
using System.Security.Cryptography;

namespace RastaMelkor
{
    public class Melkor
    {
        private readonly byte[] _entropy = { 0x90, 0x91, 0x92, 0x93 };
        private byte[] _encryptedAssembly;

        public void Init(ref byte[] file)
        {
            _encryptedAssembly = ProtectAssembly(ref file);
        }

        public void InvokeAndUnload(string methodName)
        {
            byte[] decryptedAssembly = UnprotectAssembly(ref _encryptedAssembly);
            try
            {
                var appDomain = LoadAppDomainModule(methodName, "rasta", ref decryptedAssembly);
                UnloadAppDomain(ref appDomain);
            }
            finally
            {
                Array.Clear(decryptedAssembly, 0, decryptedAssembly.Length);
            }
        }

        private byte[] ProtectAssembly(ref byte[] fileBytes)
        {
            try
            {
                byte[] encrypted = ProtectedData.Protect(fileBytes, _entropy, DataProtectionScope.LocalMachine);
                return encrypted;
            }
            finally
            {
                Array.Clear(fileBytes, 0, fileBytes.Length);
            }
        }

        private byte[] UnprotectAssembly(ref byte[] fileBytes)
        {
            byte[] decrypted = ProtectedData.Unprotect(fileBytes, _entropy, DataProtectionScope.LocalMachine);
            return decrypted;
        }

        private AppDomain LoadAppDomainModule(string methodName, string appDomainName, ref byte[] assembly)
        {
            AppDomain appDomain = AppDomain.CreateDomain(appDomainName, null, null, null, false);
            DomainProxy domainProxy = (DomainProxy)appDomain.CreateInstanceAndUnwrap(typeof(DomainProxy).Assembly.FullName, typeof(DomainProxy).FullName);
            domainProxy.LoadAndInvoke(assembly, methodName);

            return appDomain;
        }

        private void UnloadAppDomain(ref AppDomain oDomain)
        {
            AppDomain.Unload(oDomain);
        }
    }
}
