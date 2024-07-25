using Core.Entities.Base;


namespace Core.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; }
    public int Limit { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Student> Students { get; set; }
}
