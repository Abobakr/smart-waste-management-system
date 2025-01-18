
namespace DomainDataEntities
{
    public interface IEntityObjectState
    {
        EntityObjectState ObjectState { get; set; }
    }

    public enum EntityObjectState
    {
        Added,
        Modified,
        Deleted,
        Unchanged
    }
}
/*
 * :IEntityObjectState
 * 
 * 
 * public EntityObjectState ObjectState { get; set; }
*/