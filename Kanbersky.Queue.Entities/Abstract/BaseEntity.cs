using Kanbersky.Queue.Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace Kanbersky.Queue.Entities.Abstract
{
    public class BaseEntity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
