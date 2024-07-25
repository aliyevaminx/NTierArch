using Core.Entities;
using Data.Contexts;
using Data.Repository.Abstract;
using Data.Repository.Base;


namespace Data.Repository.Concrete;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public Group GetByName(string name)
    {
        return _context.Groups.FirstOrDefault(g => g.Name == name);
    }
}
