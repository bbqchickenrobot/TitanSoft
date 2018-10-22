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

        public static void UpdateFromViewModel(this MemberModel member, UserViewModel model)
        {
            member.Id = model.Id;
            member.AboutMe = model.AboutMe;
            member.Address1 = model.Address1;
            member.City = model.City;
            member.Firstame = model.Lastame;
            member.Lastame = model.Lastame;
            member.Headline = model.Headline;
            member.PostalCode = model.PostalCode;
            member.State = model.State;
        }
    }
}

