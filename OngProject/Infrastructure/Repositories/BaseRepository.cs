using Microsoft.EntityFrameworkCore;
using OngProject.Common;
using OngProject.Core.Entities;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        #region Objects and Constructor
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _entity;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }
        #endregion
        public async Task<IEnumerable<T>> GetAll()
        {
            var response = await _entity.Where(x => x.IsDeleted == false).ToListAsync();
            return response;
        }
        public async Task<T> GetById(int id)
        {
            var response = await _entity.FindAsync(id);
            return response;
        }
        public async Task<T> Insert(T entity)
        {
            entity.CreatedAt = DateTime.Now;
            entity.IsDeleted = false;

            var response = await _entity.AddAsync(entity);
            return response.Entity;
        }
        public async Task<Result> Update(T entity)
        {
            if(entity==null)
            {
                return new Result().Fail("El id no existe");
            }
            entity.CreatedAt = DateTime.Now;
            _context.Set<T>().Update(entity);
            return new Result().Success($"Se ha actualizado correctamente");
        }
        public async Task<Result> Delete(int id)
        {
            var entity = await _entity.FindAsync(id);
            if (entity == null)
            {
                return new Result().Fail("El id no existe");
            }

            entity.IsDeleted = true;
            entity.CreatedAt = DateTime.Now;

            _entity.Update(entity);
            return new Result().Success($"Se ha borrado correctamente");
        }
        #region 
        /// <summary>
        /// Si la entidad existe y no ha sido borrada regresa un bool. 
        /// Si no esta registrado el Id, o ha sido borrada con baja logica, regresa null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        #endregion
        public bool EntityExists(int id)
        {
            return _entity.Any(x => x.Id == id && x.IsDeleted == false);
        }

        #region 
        /// <summary>
        /// Metodo para realizar una busqueda por medio de una expresion LINQ
        /// </summary>
        /// <remarks>
        /// <para>Ejemplo:
        /// var request= _unitOfWork.(repositorioDeLaEntidad).FindByCondition(x=> x.campo del objeto de la Db == Campo contra el que quiero comparar);</para>
        /// <para>Cuando se quiere agregar una entidad ligada por una FK, se debe de incluir despues de la frase, con una coma</para>
        /// <para>Ejemplo: .FindByCondition(query LINQ, y=> y.EntidadVinculadaPorunaFK)</para>
        /// <para> Ejemplo concreto:
        /// var request= _unitOfWork.UsersRepository.FindByCondition(x=> x.Email == email, y=> y.Role);</para>
        /// <para> Esto me va a traer un IEnumerable de User, y en el campo Role va a traer el Rol asignado a ese Usuario.
        /// De ahi solamente restaria aplicar logica para hacerlo lista, o para lo que necesiten</para>
        /// 
        /// </remarks>
        /// <returns>
        /// IEnumerable de la entidad que se este ocupando, filtrada. Pueden agregarse mas propiedades. 
        /// Por defecto trae solo las que IsDeleted==false
        /// </returns> 
        #endregion
        public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> predicate,
                                                          params Expression<Func<T, object>>[] includeProperties)
        {
            var response = _entity
                .Where(x => x.IsDeleted == false)
                .Where(predicate);

            foreach (var includeProperty in includeProperties)
            {
                response = response.Include(includeProperty);
            }

            return await response.ToListAsync();

        }
        public async Task<User> GetByEmail(string email)
        {
            IQueryable<User> query = _context.Users.Include(u => u.Role);
            var user = await query.Where(x => x.Email.ToUpper() == email.ToUpper() && x.IsDeleted == false).FirstOrDefaultAsync();
            return user;
        }
        public async Task<IEnumerable<T>> GetPageAsync(Expression<Func<T, object>> order, int limit, int page)
        {
            return await _entity.Where(x => !x.IsDeleted)
                .OrderBy(order)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _entity.CountAsync();
        }
    }
}
