using OngProject.Core.Entities;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor y Contexto AppDbContext
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Objects and Repositories

        private readonly IBaseRepository<Activities> _activitiesRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<Comments> _commentsRepository;
        private readonly IBaseRepository<Contacts> _contactsRepository;
        private readonly IBaseRepository<Member> _memberRepository;
        private readonly IBaseRepository<News> _newsRepository;
        private readonly IBaseRepository<Organizations> _organizationsRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<Slides> _slidesRepository;
        private readonly IBaseRepository<Testimonials> _testimonialsRepository;
        private readonly IBaseRepository<User> _usersRepository;

        public IBaseRepository<Activities> ActivitiesRepository => _activitiesRepository ?? new BaseRepository<Activities>(_context);
        public IBaseRepository<Comments> CommentsRepository => _commentsRepository ?? new BaseRepository<Comments>(_context);
        public IBaseRepository<Contacts> ContactsRepository => _contactsRepository ?? new BaseRepository<Contacts>(_context);
        public IBaseRepository<Member> MemberRepository => _memberRepository ?? new BaseRepository<Member>(_context);
        public IBaseRepository<Organizations> OrganizationsRepository => _organizationsRepository ?? new BaseRepository<Organizations>(_context);
        public IBaseRepository<News> NewsRepository => _newsRepository ?? new BaseRepository<News>(_context);
        public IBaseRepository<Slides> SlidesRepository => _slidesRepository ?? new BaseRepository<Slides>(_context);
        public IBaseRepository<Testimonials> TestimonialsRepository => _testimonialsRepository ?? new BaseRepository<Testimonials>(_context);
        public IBaseRepository<User> UsersRepository => _usersRepository ?? new BaseRepository<User>(_context); 
        public IBaseRepository<Category> CategoryRepository => _categoryRepository ?? new BaseRepository<Category>(_context);

        public IBaseRepository<Role> RoleRepository  => _roleRepository ?? new BaseRepository<Role>(_context);

        #endregion

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}