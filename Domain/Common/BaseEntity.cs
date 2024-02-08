namespace Domain.Common;

public abstract class BaseEntity
{
    protected BaseEntity(int id)
    {
        _id = id;
    }

     private readonly int _id;
}
