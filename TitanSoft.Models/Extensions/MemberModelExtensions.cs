namespace TitanSoft.Models
{
    public static class MemberModelExtensions
    {
        public static UserViewModel ToViewModel(this MemberModel member)
        {
            var model = new UserViewModel
            {
                Id = member.Id,
                AboutMe = member.AboutMe,
                Address1 = member.Address1,
                City = member.City,
                Firstname = member.Lastame,
                Lastname = member.Lastame,
                Headline = member.Headline,
                PostalCode = member.PostalCode,
                State = member.State
            };
            return model;
        }
    }
}

