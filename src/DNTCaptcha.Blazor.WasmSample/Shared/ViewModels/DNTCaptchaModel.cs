using System.ComponentModel.DataAnnotations;

namespace DNTCaptcha.Blazor.WasmSample.Shared.ViewModels;

public class DNTCaptchaModel
{
    [Required] public string UserName { set; get; }

    [Required] public string Password { set; get; }

    public string EnteredCaptchaText { set; get; }

    public string CaptchaText { set; get; }
}