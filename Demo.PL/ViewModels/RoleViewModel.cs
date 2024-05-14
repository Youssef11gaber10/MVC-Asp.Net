using System;

namespace Demo.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        public string RoleName { get; set; }

        public RoleViewModel()
        {
            Id=  Guid.NewGuid().ToString();//this id is auto generated, there is no need to id is be has idenity like users it just for identify roles
            //also if you wanna create user you will do its id with same way
        }
    }
}
