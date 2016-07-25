using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Erebus.Model
{
    public class Account : PersistableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public bool HasAuthenticator { get; set; }
        public string AuthenticatorRestorationCode { get; set; }
        public virtual VaultFolder VaultFolder { get; set; }
    }
}
