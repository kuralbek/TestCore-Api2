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

        [Required]
        public int roleId { get; set; } =2; //defoult all users set role customer (2)

        public string RefreshToken { get; set; } = string.Empty;    

        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
