using System.ComponentModel.DataAnnotations;

namespace TestCore_Api.ContactDir
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public string ContactName { get; set; }

        public string ContactValue { get; set; }
        
        public bool isMarked { get; set; }

    }
}
