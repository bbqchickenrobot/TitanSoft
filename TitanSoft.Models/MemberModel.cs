using System;
using Raven.Identity;

namespace TitanSoft.Models
{
    public class MemberModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}
