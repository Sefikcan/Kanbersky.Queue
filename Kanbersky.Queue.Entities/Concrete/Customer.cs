using Kanbersky.Queue.Core.Entity;
using Kanbersky.Queue.Entities.Abstract;

namespace Kanbersky.Queue.Entities.Concrete
{
    public class Customer : BaseEntity, IEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
