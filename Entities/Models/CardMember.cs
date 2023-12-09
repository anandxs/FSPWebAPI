using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class CardMember
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(Card))]
        public Guid CardId { get; set; }
        public Card Card { get; set; }
        [ForeignKey(nameof(User))]
        public string? MemberId { get; set; }
        public User? Member { get; set; }
    }
}
