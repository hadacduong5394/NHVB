using System.ComponentModel.DataAnnotations;

namespace cvmksite.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string CurrenPassword { get; set; }

        [Required]
        public string NewPassWord { get; set; }
    }
}