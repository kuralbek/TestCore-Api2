using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestCore_Api
{
    [Table("st_role")]
    public class Roles
    {
        [Key]
        public int IdRole { get; set; }

        public string NameRole { get; set; }
    }
}
