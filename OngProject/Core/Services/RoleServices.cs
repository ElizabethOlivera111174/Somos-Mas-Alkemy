using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Role> GetById(int id)
        {
            return await _unitOfWork.RoleRepository.GetById(id);
        }

    }
}
