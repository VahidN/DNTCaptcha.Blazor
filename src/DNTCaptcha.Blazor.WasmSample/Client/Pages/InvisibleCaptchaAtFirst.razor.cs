using System;
using System.Text.Json;
using System.Threading.Tasks;
using DNTCaptcha.Blazor.WasmSample.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DNTCaptcha.Blazor.WasmSample.Client.Pages;

public partial class InvisibleCaptchaAtFirst : ComponentBase
{
    private bool _firstSignIn = true;
    private DNTCaptchaModel Model { set; get; } = new();

    private async Task DoLoginAsync()
    {
        await Task.Delay(50);
        Console.WriteLine(JsonSerializer.Serialize(Model));

        if (Model.UserName == "Vahid" && Model.Password == "1234" &&
            !string.IsNullOrWhiteSpace(Model.CaptchaText) &&
            Model.CaptchaText.Equals(Model.EnteredCaptchaText))
        {
            Console.WriteLine("Good job!");
            RedrawTheCaptcha();
        }
        else
        {
            Console.WriteLine("Try again.");
            _firstSignIn = false;
            RedrawTheCaptcha();
        }
    }

    private void RedrawTheCaptcha()
    {
        // Clear the form, Redraw the captcha
        Model = new DNTCaptchaModel();
        Model.CaptchaText = string.Empty; // How to redraw the captcha
    }
}