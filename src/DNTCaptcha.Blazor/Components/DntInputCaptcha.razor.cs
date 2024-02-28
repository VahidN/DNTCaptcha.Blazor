using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DNTCaptcha.Blazor;

/// <summary>
///     A custom captcha component. 
///     This is an interactive (Server & WebAssembly) captcha component and can't be used with the Blazor SSR.
/// </summary>
public partial class DntInputCaptcha : ComponentBase, IDisposable, IAsyncDisposable
{
    private const string ScriptPath = "./_content/DNTCaptcha.Blazor/scripts/dntCaptcha.js";
    private bool _isDisposed;
    private IJSObjectReference? _scriptModule;
    private Timer? _timer;

    private string _value = string.Empty;
    private ElementReference ReferenceToCanvas { set; get; } = default!;
    private string ElementId { set; get; } = Guid.NewGuid().ToString("N");

    /// <summary>
    ///     Additional user attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; } =
        new Dictionary<string, object>(StringComparer.Ordinal);

    /// <summary>
    ///     The captcha's number
    /// </summary>
    [Parameter]
    public string Value
    {
        get => _value;
        set => SetValue(value);
    }

    /// <summary>
    ///     Fires when a new captcha text is generated
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { set; get; }

    /// <summary>
    ///     The font size of the captcha's text.
    ///     Its default value is `25`.
    /// </summary>
    [Parameter]
    public int FontSize { set; get; } = 25;

    /// <summary>
    ///     The font name of the captcha's text.
    ///     Its default value is `Times New Roman`.
    /// </summary>
    [Parameter]
    public string FontName { set; get; } = "Times New Roman";

    /// <summary>
    ///     The font color of the captcha's text.
    ///     Its default value is `#603F83FF`.
    /// </summary>
    [Parameter]
    public string FontColor { set; get; } = "#603F83FF";

    /// <summary>
    ///     The captcha's background color.
    ///     Its default value is `#FCF6F5FF`.
    /// </summary>
    [Parameter]
    public string BackgroundColor { set; get; } = "#FCF6F5FF";

    /// <summary>
    ///     The number of displayed random lines to distort image.
    ///     Its default value is `6`.
    /// </summary>
    [Parameter]
    public int RandomLinesCount { set; get; } = 6;

    /// <summary>
    ///     The number of displayed random circles to distort image.
    ///     Its default value is `6`.
    /// </summary>
    [Parameter]
    public int RandomCirclesCount { set; get; } = 6;

    /// <summary>
    ///     The border's width of the captcha.
    ///     Its default value is `0.3`.
    /// </summary>
    [Parameter]
    public double BorderWidth { set; get; } = 0.3;

    /// <summary>
    ///     The border's color of the captcha.
    ///     Its default value is `grey`.
    /// </summary>
    [Parameter]
    public string BorderColor { set; get; } = "grey";

    /// <summary>
    ///     The number of displayed points to distort image.
    ///     Its default value is `25`.
    /// </summary>
    [Parameter]
    public int NoisePointsCount { set; get; } = 25;

    /// <summary>
    ///     The captcha's text padding.
    ///     Its default value is `11`.
    /// </summary>
    [Parameter]
    public int Padding { set; get; } = 11;

    /// <summary>
    ///     The ShadowBlur of the captcha's text.
    ///     Its default value is `7`.
    /// </summary>
    [Parameter]
    public int ShadowBlur { set; get; } = 7;

    /// <summary>
    ///     The ShadowColor of the captcha's text.
    ///     Its default value is `navy`.
    /// </summary>
    [Parameter]
    public string ShadowColor { set; get; } = "navy";

    /// <summary>
    ///     The ShadowOffsetX of the captcha's text.
    ///     Its default value is `-3`.
    /// </summary>
    [Parameter]
    public int ShadowOffsetX { set; get; } = -3;

    /// <summary>
    ///     The ShadowOffsetY of the captcha's text.
    ///     Its default value is `-3`.
    /// </summary>
    [Parameter]
    public int ShadowOffsetY { set; get; } = -3;

    /// <summary>
    ///     The timer's interval in ms to display the captcha's text at different locations.
    ///     Its default value is `2500 ms`.
    /// </summary>
    [Parameter]
    public TimeSpan TimerInterval { set; get; } = TimeSpan.FromMilliseconds(2500);

    /// <summary>
    ///     Gets or sets an absolute expiration date for the cache entry.
    ///     After that the captcha will be recalculated.
    ///     Its default value is 2 minutes.
    /// </summary>
    [Parameter]
    public TimeSpan AbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    ///     Shows thousands separators such as 100,100,100 in ShowDigits mode.
    ///     Its default value is true.
    /// </summary>
    [Parameter]
    public bool AllowThousandsSeparators { get; set; } = true;

    /// <summary>
    ///     The CSS class of the captcha's canvas.
    ///     Its default value is ``.
    /// </summary>
    [Parameter]
    public string CaptchaCanvasClass { get; set; } = "";

    /// <summary>
    ///     The CSS class of the captcha's DIV.
    ///     Its default value is `d-flex justify-content-center align-self-center ms-1`.
    /// </summary>
    [Parameter]
    public string CaptchaDivClass { get; set; } = "d-flex justify-content-center align-self-center ms-1";

    /// <summary>
    ///     The language of the captcha. It's default value is Persian.
    /// </summary>
    [Parameter]
    public NumberToWordLanguage Language { set; get; } = NumberToWordLanguage.Persian;

    /// <summary>
    ///     Display mode of the captcha's text. It's default value is NumberToWord.
    /// </summary>
    [Parameter]
    public DisplayMode DisplayMode { set; get; }

    /// <summary>
    ///     The max value of the captcha. It's default value is 9000.
    /// </summary>
    [Parameter]
    public int Max { set; get; } = 9000;

    /// <summary>
    ///     The min value of the captcha. It's default value is 1.
    /// </summary>
    [Parameter]
    public int Min { set; get; } = 1;

    /// <summary>
    ///     The refresh-button-class of the captcha. It's default value is `bi-arrow-repeat btn`.
    /// </summary>
    [Parameter]
    public string RefreshButtonClass { set; get; } = "bi-arrow-repeat btn";

    /// <summary>
    ///     The title of refresh-button of the captcha. It's default value is `refresh`.
    /// </summary>
    [Parameter]
    public string RefreshButtonTitle { set; get; } = "Refresh";

    /// <summary>
    ///     Its default value is true.
    /// </summary>
    [Parameter]
    public bool ShowRefreshButton { set; get; } = true;

    /// <summary>
    /// Child content rendered inside the button
    /// </summary>
    [Parameter] 
    public RenderFragment? RefreshButtonChildContent { get; set; }

    /// <summary>
    ///     Represents an instance of a JavaScript runtime to which calls may be dispatched.
    /// </summary>
    [Inject]
    internal IJSRuntime? JSRuntime { set; get; }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_scriptModule is not null)
            {
                await _scriptModule.DisposeAsync();
                _scriptModule = null;
            }

            GC.SuppressFinalize(this);
        }
        catch (JSDisconnectedException)
        {
            // Ignore it. It is impossible to call JavaScript when the SignalR connection is disconnected.
            // Since event listeners stop existing after a page reload there are no memory leaks.
        }
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void SetValue(string value)
    {
        if (string.Equals(_value, value, StringComparison.Ordinal))
        {
            return;
        }

        _value = value;
        if (ValueChanged.HasDelegate)
        {
            _ = ValueChanged.InvokeAsync(value);
        }
    }

    private ValueTask ShowCaptchaAsync()
    {
        if (_scriptModule is null)
        {
            throw new InvalidOperationException($"{nameof(_scriptModule)} is null.");
        }

        var (text, randomNumber) =
            CaptchaTextFactory.GetData(Min, Max, AllowThousandsSeparators, Language, DisplayMode);
        SetValue(randomNumber.ToString(CultureInfo.InvariantCulture));

        return _scriptModule.InvokeVoidAsync("drawDntCaptcha",
                                             ReferenceToCanvas,
                                             new
                                             {
                                                 text,
                                                 fontSize = FontSize,
                                                 fontName = FontName,
                                                 fontColor = FontColor,
                                                 backgroundColor = BackgroundColor,
                                                 randomLinesCount = RandomLinesCount,
                                                 randomCirclesCount = RandomCirclesCount,
                                                 noisePointsCount = NoisePointsCount,
                                                 padding = Padding,
                                                 shadowBlur = ShadowBlur,
                                                 shadowColor = ShadowColor,
                                                 shadowOffsetX = ShadowOffsetX,
                                                 shadowOffsetY = ShadowOffsetY,
                                                 timerInterval = TimerInterval.TotalMilliseconds,
                                                 borderWidth = BorderWidth,
                                                 borderColor = BorderColor,
                                             });
    }

    /// <summary>
    ///     Method invoked after each time the component has been rendered.
    /// </summary>
    /// <param name="firstRender">Set to true if this is the first time</param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await LoadScriptsAsync();

        if (firstRender)
        {
            await ShowCaptchaAsync();
            StartTimer();
        }
    }

    private async Task LoadScriptsAsync()
    {
        if (JSRuntime is null)
        {
            throw new InvalidOperationException($"{nameof(JSRuntime)} is null.");
        }

        _scriptModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", ScriptPath);
    }

    /// <summary>
    ///     Method invoked when the component is ready to start
    /// </summary>
    private void StartTimer()
    {
        if (_timer is not null)
        {
            return;
        }

        _timer = new Timer(AbsoluteExpiration.TotalMilliseconds);
        _timer.Elapsed += NotifyTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void NotifyTimerElapsed(object? source, ElapsedEventArgs e)
    {
        _ = InvokeAsync(async () => await ShowCaptchaAsync());
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            try
            {
                if (_timer is null)
                {
                    return;
                }

                _timer.Stop();
                _timer.Elapsed -= NotifyTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }
            finally
            {
                _isDisposed = true;
            }
        }
    }
}