using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();//vice versa
             // .ForMember(D => D.Name, O => O.MapFrom(S => S.empName);//if the props names are diffrent
             //.ForMember(D => D.Image.FileName, O => O.MapFrom(S => S.ImageName));
        }
    }
}
