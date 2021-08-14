using System.Security.Cryptography;

namespace DNTCaptcha.Blazor
{
    /// <summary>
    /// SumOfTwoNumbersToWords Provider
    /// </summary>
    public class SumOfTwoNumbersToWordsProvider : ICaptchaTextProvider
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
            var toWordProvider = new HumanReadableIntegerProvider();
            var text = number > randomNumber ?
                $"{toWordProvider.NumberToText(number - randomNumber, language)} + {toWordProvider.NumberToText(randomNumber, language)}" :
                $"{toWordProvider.NumberToText(0, language)} + {toWordProvider.NumberToText(number, language)}";
            return language == NumberToWordLanguage.Persian ? text.ToPersianNumbers() : text;
        }
    }
}