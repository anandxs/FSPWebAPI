namespace Entities.Exceptions
{
    public sealed class CardNotFoundException : NotFoundException
    {
        public CardNotFoundException(Guid id) : base($"Card with id : {id} not found.")
        {
        }
    }
}
