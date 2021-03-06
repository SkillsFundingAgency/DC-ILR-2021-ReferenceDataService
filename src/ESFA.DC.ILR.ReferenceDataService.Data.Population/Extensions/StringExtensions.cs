﻿using System;

namespace ESFA.DC.ILR.ReferenceDataService.Data.Population.Extensions
{
    public static class StringExtensions
    {
        public static bool CaseInsensitiveEquals(this string source, string data)
        {
            if (source == null && data == null)
            {
                return true;
            }

            return source?.Equals(data, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        public static string ToUpperCase(this string source)
        {
            return source?.ToUpperInvariant();
        }
    }
}
