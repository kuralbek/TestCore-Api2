using System.ComponentModel.DataAnnotations;

namespace TestCore_Api
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public byte[] passwordHash { get; set; }

        [Required]
        public byte[] passwordSalt { get; set; }
    }
}
