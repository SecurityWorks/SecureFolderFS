﻿using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace SecureFolderFS.WinUI.ValueConverters
{
    internal sealed class GenericEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int enumValue = (int)value;

            if (parameter is string strParam)
            {
                var enumConversionValues = strParam.Split(',').ToDictionary(k => System.Convert.ToInt32(k.Split('-')[0]), v => System.Convert.ToInt32(v.Split('-')[1]));
                
                if (enumConversionValues.TryGetValue((int)value, out var convertedValue))
                {
                    enumValue = convertedValue;
                }
                // else.. use value from the cast above
            }

            var returnEnum = Enum.Parse(targetType, Enum.GetName(targetType, enumValue));
            return returnEnum;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}