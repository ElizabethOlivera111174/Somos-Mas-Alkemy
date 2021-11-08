using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace OngProject.Core.Interfaces.IServices
{
    public interface IUserServices
    {
        Task<IEnumerable<UserDTO>> GetAll();
        Task<Result> Register(UserInsertDTO userInsertDTO);
        Task<User> GetByEmail(string email);
        int GetUserId(string token);
        Task<UserInfoDTO> GetById(int userId);
        Task<Result> Delete(string token );
        Task<Result> Update(UserUpdateDTO userUpdateDTO,string token);
    }
}