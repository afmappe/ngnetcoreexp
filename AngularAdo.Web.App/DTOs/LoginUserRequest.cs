using System.ComponentModel.DataAnnotations;

namespace AngularAdo.Web.App.DTOs
{
    public class LoginUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
