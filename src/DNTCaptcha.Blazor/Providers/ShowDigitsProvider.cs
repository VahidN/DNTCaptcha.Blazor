using System;
using System.Globalization;
using Microsoft.Extensions.Options;

namespace DNTCaptcha.Blazor
{
    /// <summary>
    /// display a numeric value using the equivalent text
    /// </summary>
    public class ShowDigitsProvider : ICaptchaTextProvider
    {
        private readonly bool _allowThousandsSeparators;

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        public ShowDigitsProvider(bool allowThousandsSeparators)
        {
            _allowThousandsSeparators = allowThousandsSeparators;
        }

        /// <summary>
        /// display a numeric value using the equivalent text
        /// </summary>
        /// <param name="number">input number</param>
        /// <param name="language">local language</param>
        /// <returns>the equivalent text</returns>
        public string GetText(int number, NumberToWordLanguage language)
        {
            var text = _allowThousandsSeparators ?
                            string.Format(CultureInfo.InvariantCulture, "{0:N0}", number) :
                            number.ToString(CultureInfo.InvariantCulture);
            return language == NumberToWordLanguage.Persian ? text.ToPersianNumbers() : text;
        }
    }
}