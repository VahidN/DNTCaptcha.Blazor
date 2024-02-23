## DNTCaptcha.Blazor

[![DNTCaptcha.Blazor](https://github.com/VahidN/DNTCaptcha.Blazor/workflows/.NET%20Core%20Build/badge.svg)](https://github.com/VahidN/DNTCaptcha.Blazor)

`DNTCaptcha.Blazor` is a captcha generator for the Blazor based applications. It uses the standard [HTML5 Canvas API](https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API) to draw the captcha and that's why it's compatible with both Blazor WASM and Server apps. It no longer uses the `System.Drawing.Common` package which has serious cross platform and stability issues.

## Install via NuGet

To install DNTCaptcha.Blazor, run the following command in the Package Manager Console:

[![Nuget](https://img.shields.io/nuget/v/DNTCaptcha.Blazor)](http://www.nuget.org/packages/DNTCaptcha.Blazor/)

```powershell
PM> Install-Package DNTCaptcha.Blazor
```

You can also view the [package page](http://www.nuget.org/packages/DNTCaptcha.Blazor/) on NuGet.

## Usage

After installing the DNTCaptcha.Blazor package, add the following definition to the \_Imports.razor file:

```csharp
@using DNTCaptcha.Blazor
```

Then to use it, add its new tag and settings to [your view](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor.WasmSample/Client/Pages/Components/Login.razor).
Here you will need [two properties](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor.WasmSample/Shared/ViewModels/LoginViewModel.cs) and one `Compare` attribute to work with it:

```csharp
[Required]
[Compare(nameof(CaptchaText1), ErrorMessage = "The entered `Security Number` is not correct.")]
public string EnteredCaptchaText1 { set; get; }

public string CaptchaText1 { set; get; }
```

```xml
<EditForm Model="Model" OnValidSubmit="DoLoginAsync">
    <InputText name="SecurityNumber" @bind-Value="Model.EnteredCaptchaText1" />
    <DntInputCaptcha @bind-Value="Model.CaptchaText1" />
```

The `EnteredCaptchaText1` will receive the entered text from the user and the `CaptchaText1` contains the automatically generated captcha code.
Now the `Compare` attribute will compare these two values during the `OnValidSubmit` event to provide an standard and clean validation.

*Note:* Using the `Compare` attribute will make the whole process nicer, but it's not mandatory. You can omit it and then do the comparison manually on the form's submit `(if (userLoginViewModel.CaptchaText1.Equals(userLoginViewModel.EnteredCaptchaText1)){  })`. Now if the inputs don't match, [redraw the captcha](https://github.com/VahidN/DNTCaptcha.Blazor/blob/main/src/DNTCaptcha.Blazor.WasmSample/Client/Pages/Components/Login.razor.cs#L20) with the new data. Also to protect the form against brute-forces, you can use the `AbsoluteExpiration` parameter to control the refresh frequency of the captcha.

## Supported Languages

You can find all of the currently supported languages [here](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor/Contracts/NumberToWordLanguage.cs).
To add a new language, kindly contribute by editing the following files:

- [Language.cs](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor/Contracts/NumberToWordLanguage.cs)
- [HumanReadableIntegerProvider.cs](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor/Providers/HumanReadableIntegerProvider.cs)

## Samples

![DNTCaptcha.Blazor](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor.WasmSample/DntInputCaptcha.png)

- [Blazor WASM Sample](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor.WasmSample/)
- [Blazor Server Sample](https://github.com/VahidN/DNTCaptcha.Blazor/tree/main/src/DNTCaptcha.Blazor.ServerSample/)

## Demo

[You can see a demo of this component with all of its different supported DisplayModes here](https://vahidn.github.io/DNTCaptcha.Blazor)
