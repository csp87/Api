using System.ComponentModel.DataAnnotations;

namespace Api_Rest.Models
{
    public class Login
    {
        [Required]
        public string usr { get; set; }
        [Required]
        public string pass { get; set; }
    }
}