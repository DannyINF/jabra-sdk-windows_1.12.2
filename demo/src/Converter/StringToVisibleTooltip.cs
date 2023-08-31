using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jabra_SDK_Demo.Converter
{
	public class StringToVisibleTooltip : MarkupExtension, IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value != null && ((string)value).Length > 0) //empty string does not need tooltip
				return true;
			else
				return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
