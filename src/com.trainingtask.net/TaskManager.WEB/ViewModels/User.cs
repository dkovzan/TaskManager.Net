using System.ComponentModel.DataAnnotations;

namespace TaskManager.WEB.ViewModels
{
    public class ExistingUser
    {
        [Display(Name = "Login", ResourceType = typeof(Resources.CommonResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "LoginRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "LoginLong")]
        public string Login { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.CommonResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordLong")]
        public string Password { get; set; }
    }

    public class NewUser
    {
        [Display(Name = "Login", ResourceType = typeof(Resources.CommonResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "LoginRequired")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "LoginLong")]
        public string Login { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources.CommonResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordRequired")]
        [MinLength(6, ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordShort")]
        [MaxLength(200, ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordLong")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.CommonResource))]
        [Required(ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.CommonResource), ErrorMessageResourceName = "PasswordsDontMatch")]
        public string ConfirmPassword { get; set; }
    }
}