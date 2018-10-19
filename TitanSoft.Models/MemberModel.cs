using System;
using Raven.Identity;

namespace TitanSoft.Models
{
    public class MemberModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Headline { get; set; }
        public string AboutMe { get; set; }
    }
}
