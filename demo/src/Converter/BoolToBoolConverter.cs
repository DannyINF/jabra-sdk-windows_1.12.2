using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Jabra_SDK_Demo.Converter
{
  public class BoolToBoolConverter : MarkupExtension, IValueConverter
  {
    public BoolToBoolConverter()
    {
      TrueValue = true;
      FalseValue = false;
    }

    public bool TrueValue { get; set; }
    public bool FalseValue { get; set; }

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