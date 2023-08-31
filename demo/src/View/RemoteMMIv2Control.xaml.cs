using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Jabra_SDK_Demo.View
{
  /// <summary>
  /// Interaction logic for RemoteMMIv2Control.xaml
  /// </summary>
  public partial class RemoteMMIv2Control : UserControl
  {
    public RemoteMMIv2Control()
    {
      InitializeComponent();
    }

    private void CmbBtnType_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
    {

    }
  }
}
