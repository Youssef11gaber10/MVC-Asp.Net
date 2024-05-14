using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();//vice versa
            // .ForMember(D => D.Name, O => O.MapFrom(S => S.empName);//if the props names are diffrent
            //.ForMember(D => D.Image.FileName, O => O.MapFrom(S => S.ImageName));
        }

    }

}
