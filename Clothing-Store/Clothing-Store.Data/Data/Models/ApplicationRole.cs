using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Data.Data.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
           : this(null)
        {
        }

        public ApplicationRole(string name)
            : base(name)
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public bool isActive { get; set; } = true;
    }
}
