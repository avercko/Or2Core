﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Orchard.OpenId.Settings
{
    public class OpenIdSettings
    {
        public bool TestingModeEnabled { get; set; }
        public TokenFormat AccessTokenFormat { get; set; }
        public string Authority { get; set; }
        public IEnumerable<string> Audiences { get; set; }
        public StoreLocation? CertificateStoreLocation { get; set; }
        public StoreName? CertificateStoreName { get; set; }
        public string CertificateThumbPrint { get; set; }

        public enum TokenFormat
        {
            Encrypted = 0,
            JWT = 1
        }
    }
}
