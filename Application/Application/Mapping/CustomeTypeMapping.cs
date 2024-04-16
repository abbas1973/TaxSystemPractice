using Application.DTOs;
using Application.Features.Web.Users;
using AutoMapper;
using Domain.Entities;
using Utilities;
using Utilities.Files;

namespace Base.Application.Mapping
{
    #region UserCreateCommand =>  User
    public class UserCreateCommand_To_User : ITypeConverter<UserCreateCommand,  User>
    {
        public  User Convert( 
            UserCreateCommand src,
            User dst,
            ResolutionContext context)
        {
            var model = new  User()
            {
                FirstName = src.FirstName,
                LastName = src.LastName,
                Username = src.Username,
                Password = src.Password.GetHash(),
                CityId = src.CityId,
                IsEnabled = true,
                Mobile = src.Mobile,
                Roles = src.RoleIds.Select(id => new UserRole() { RoleId = id }).ToList()
            };
            return model;
        }
    }
    #endregion

}
