using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using JabraSDK;

namespace Jabra_SDK_Demo.Converter
{
  class CoverterItemsSource2Enabled : MarkupExtension, IValueConverter
  {

    public CoverterItemsSource2Enabled()
    {
      TrueValue = true;
      FalseValue = false;
    }

    public bool TrueValue { get; set; }
    public bool FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is IEnumerable)
      {
        var enumerable = (IEnumerable)value;
        foreach (var item in enumerable)
        {
          return true;
        }
      }
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
