using Microsoft.AspNetCore.Identity;

namespace salonytics.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
    }
}
