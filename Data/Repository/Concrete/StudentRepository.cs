using Core.Entities;
using Data.Contexts;
using Data.Repository.Abstract;
using Data.Repository.Base;

namespace Data.Repository.Concrete;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(AppDbContext context) : base(context)
    {
    }
}
