﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jabra_SDK_Demo.Converter
{
	public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
	{
		public BooleanToVisibilityConverter()
		{
			TrueValue = Visibility.Visible;
			FalseValue = Visibility.Collapsed;
		}

		public Visibility TrueValue { get; set; }
		public Visibility FalseValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool val = System.Convert.ToBoolean(value);
			return val ? TrueValue : FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return TrueValue.Equals(value) ? true : false;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
