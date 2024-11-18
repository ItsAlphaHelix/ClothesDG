namespace ClothesDG.Data.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Roles = new HashSet<IdentityUserRole<string>>();
        }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
    }
}
