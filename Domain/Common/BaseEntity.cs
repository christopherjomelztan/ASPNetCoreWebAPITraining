namespace Domain.Common;

public abstract class BaseEntity
{
    protected BaseEntity(int id)
    {
        Id = id;
    }

     public int Id { get; private set;}
}
