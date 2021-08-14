using System;
using System.Security.Cryptography;

namespace DNTCaptcha.Blazor
{
    /// <summary>
    /// Convert a number into text
    /// </summary>
    public static class CaptchaTextFactory
    {
        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="min">The min value of the captcha</param>
        /// <param name="max">The max value of the captcha</param>
        /// <param name="allowThousandsSeparators">Shows thousands separators such as 100,100,100 in ShowDigits mode</param>
        /// <param name="language">local language</param>
        /// <param name="displayMode">Display mode of the captcha's text</param>
        /// <returns>the equivalent text</returns>
        public static (string Text, int RandomNumber) GetData(
                int min,
                int max,
                bool allowThousandsSeparators,
                NumberToWordLanguage language,
                DisplayMode displayMode)
        {
            var number = RandomNumberGenerator.GetInt32(min, max);
            var text = displayMode switch
            {
                DisplayMode.NumberToWord => new HumanReadableIntegerProvider().GetText(number, language),
                DisplayMode.ShowDigits => new ShowDigitsProvider(allowThousandsSeparators).GetText(number, language),
                DisplayMode.SumOfTwoNumbers => new SumOfTwoNumbersProvider().GetText(number, language),
                DisplayMode.SumOfTwoNumbersToWords => new SumOfTwoNumbersToWordsProvider().GetText(number, language),
                _ => throw new InvalidOperationException($"Service of type {displayMode} is not implemented."),
            };
            return (text, number);
        }
    }
}