using System;
using System.Collections.Generic;
using System.Globalization;

namespace Sam.Validator
{

    internal static class Localizer
    {
        public static string Get(string key, params object[] args)
        {
            var culture = CultureInfo.CurrentUICulture.Name.Split("-")[0].ToLower(); 
            
            if (ValidationMessages.Messages.TryGetValue(key, out var translations))
            {
                if (translations.TryGetValue(culture, out var message))
                {
                    return string.Format(message, args);
                }
                else
                {
                    if (translations.TryGetValue("en", out message))
                    {
                        return string.Format(message, args);
                    }
                }

            }

            return key;
        }
    }
}