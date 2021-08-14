using System.ComponentModel.DataAnnotations;

namespace DNTCaptcha.Blazor.WasmSample.Shared.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { set; get; }

        [Required]
        public string Password { set; get; }

        [Required]
        [Compare(nameof(CaptchaText1), ErrorMessage = "The entered `Security Number` is not correct.")]
        public string EnteredCaptchaText1 { set; get; }

        public string CaptchaText1 { set; get; }

        [Required]
        [Compare(nameof(CaptchaText2), ErrorMessage = "The entered `Security Number` is not correct.")]
        public string EnteredCaptchaText2 { set; get; }

        public string CaptchaText2 { set; get; }

        [Required]
        [Compare(nameof(CaptchaText3), ErrorMessage = "The entered `Security Number` is not correct.")]
        public string EnteredCaptchaText3 { set; get; }

        public string CaptchaText3 { set; get; }

        [Required]
        [Compare(nameof(CaptchaText4), ErrorMessage = "The entered `Security Number` is not correct.")]
        public string EnteredCaptchaText4 { set; get; }

        public string CaptchaText4 { set; get; }
    }
}