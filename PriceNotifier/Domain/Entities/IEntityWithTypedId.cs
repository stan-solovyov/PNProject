namespace Domain.Entities
{
    public interface IEntityWithTypedId<T>
    {
        T Id { get; set; }
    }
}
