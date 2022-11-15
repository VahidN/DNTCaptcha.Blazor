using System;
using System.Text.Json;
using System.Threading.Tasks;
using DNTCaptcha.Blazor.WasmSample.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DNTCaptcha.Blazor.WasmSample.Client.Pages.Components;

public partial class Login : ComponentBase
{
    private LoginViewModel Model { set; get; } = new();

    private async Task DoLoginAsync()
    {
        await Task.Delay(500);
        Console.WriteLine(JsonSerializer.Serialize(Model));

        // Clear the form, Redraw the captcha
        Model = new LoginViewModel();
        Model.CaptchaText1 = string.Empty; // How to redraw the captcha
    }
}