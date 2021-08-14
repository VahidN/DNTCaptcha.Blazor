using System.Security.Cryptography;
using static System.FormattableString;

namespace DNTCaptcha.Blazor
{
    /// <summary>
    /// SumOfTwoNumbers Provider
    /// </summary>
    public class SumOfTwoNumbersProvider : ICaptchaTextProvider
    {
        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public string GetText(int number, NumberToWordLanguage language)
        {
            var randomNumber = RandomNumberGenerator.GetInt32(1, number <= 1 ? 7 : number);
            var text = number > randomNumber ?
                Invariant($"{number - randomNumber} + {randomNumber}") :
                Invariant($"0 + {number}");
            return language == NumberToWordLanguage.Persian ? text.ToPersianNumbers() : text;
        }
    }
}