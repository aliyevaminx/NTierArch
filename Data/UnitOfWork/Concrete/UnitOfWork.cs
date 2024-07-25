using Data.Contexts;
using Data.Repository.Concrete;
using Data.UnitOfWork.Abstract;


namespace Data.UnitOfWork.Concrete;

public class UnitOfWork : IUnitOfWork
{
    public readonly GroupRepository Groups;
    public readonly StudentRepository Students;
    private readonly AppDbContext _context;

    public UnitOfWork()
    {
        _context = new AppDbContext();
        Groups = new GroupRepository(_context);
        Students = new StudentRepository(_context);
        
    }

    public void Commit()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {

        }
    }
}
