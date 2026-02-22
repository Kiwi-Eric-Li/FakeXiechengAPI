using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
