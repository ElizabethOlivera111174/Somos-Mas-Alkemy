using OngProject.Core.Entities;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories.IRepository
{
    public interface IUnitOfWork 
    {
        IBaseRepository<Activities> ActivitiesRepository { get; }
        IBaseRepository<Slides> SlidesRepository { get; }
        IBaseRepository<Category> CategoryRepository {get; }       
        IBaseRepository<Contacts> ContactsRepository { get; }
        IBaseRepository<News> NewsRepository { get; }
        IBaseRepository<User> UsersRepository { get; }  
        IBaseRepository<Member> MemberRepository { get; }
        IBaseRepository<Comments> CommentsRepository { get; }
        IBaseRepository<Organizations> OrganizationsRepository { get; }
        IBaseRepository<Testimonials> TestimonialsRepository { get; }


        void Dispose();

        void SaveChanges();

        Task SaveChangesAsync();

    }
}
