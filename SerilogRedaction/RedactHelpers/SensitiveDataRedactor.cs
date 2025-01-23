namespace SerilogRedaction.RedactHelpers
{
    public static class SensitiveDataRedactor
    {
        private static Dictionary<string, Regex> _regexPatterns = new Dictionary<string, Regex>() ;
        private static Dictionary<string, string> _maskingPatterns = new Dictionary<string, string>(); 

        public static void Initialize(SensitiveDataSettings settings)
        {   
            _regexPatterns = settings.Patterns.ToDictionary(
                keyValue => keyValue.Key,
                keyValue => new Regex(keyValue.Value, RegexOptions.Compiled)
            );

            _maskingPatterns = settings.MaskingPatterns;
        }

        public static string Redact(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // Iterate through each pattern and apply redaction
            foreach (var pattern in _regexPatterns)
            {
                var key = pattern.Key;
                var regex = pattern.Value;

                if (regex.IsMatch(input))
                {
                    if (key == "Cnic")
                    {
                        var match = regex.Match(input);
                        return string.Format(_maskingPatterns[key], input.Substring(0, 5), input[^1]);
                    }
                    else if (key == "CreditCard")
                    {
                        input = regex.Replace(input, match =>
                            string.Format(_maskingPatterns[key], match.Value.Substring(0, 4), match.Value[^4..]));
                    }
                    else if (key == "Email")
                    {
                        input = regex.Replace(input, match =>
                        {
                            var parts = match.Value.Split('@');
                            return string.Format(_maskingPatterns[key], parts[0][0], parts[1]);
                        });
                    }
                }
            }

            return input;
        }
        #region commented code 
        //private static readonly Regex CnicRegex = new(@"^\d{5}-\d{7}-\d{1}$", RegexOptions.Compiled);
        //private static readonly Regex CreditCardRegex = new(@"\b\d{4}-?\d{4}-?\d{4}-?\d{4}\b", RegexOptions.Compiled);
        //private static readonly Regex EmailRegex = new(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b", RegexOptions.Compiled);

        //public static string Redact(string input)
        //{
        //    if (string.IsNullOrEmpty(input))
        //        return input;

        //    if (CnicRegex.IsMatch(input))
        //    {
        //        return $"{input.Substring(0, 5)}-*******-{input[^1]}";
        //    }

        //    if (CreditCardRegex.IsMatch(input))
        //    {
        //        input = CreditCardRegex.Replace(input, match =>
        //            $"{match.Value.Substring(0, 4)}-****-****-{match.Value[^4..]}");
        //    }

        //    if (EmailRegex.IsMatch(input))
        //    {
        //        input = EmailRegex.Replace(input, match =>
        //        {
        //            var parts = match.Value.Split('@');
        //            return $"{parts[0][0]}****@{parts[1]}";
        //        });
        //    }

        //    return input;
        //}
        #endregion
    }// end class 

}
