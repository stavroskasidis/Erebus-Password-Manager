using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Erebus.Model;

namespace Erebus.DataAccess.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Vault Vault { get; set; }
    }
}
