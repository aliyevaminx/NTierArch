using Core.Entities;
using Data.Repository.Base;


namespace Data.Repository.Abstract;

public interface IGroupRepository : IRepository<Group>
{
    Group GetByName(string name);
}
