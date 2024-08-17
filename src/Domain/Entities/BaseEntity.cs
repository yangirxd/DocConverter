namespace DocConverter.Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid Guid { get; protected set; } = Guid.NewGuid();
    }
}
