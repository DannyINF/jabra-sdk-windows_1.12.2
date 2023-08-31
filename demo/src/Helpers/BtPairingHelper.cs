using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using JabraSDK;

namespace Jabra_SDK_Demo.Helpers
{
  public class BtPairingHelper
  {
    public bool IsPaired { get; set; }
    public string ConnectedDeviceName { get; set; }
    public string ConnectedDisconnected { get; set; }
    public bool HeadsetConnected { get; set; }
  }

  public class BtDevice
  {
    public string Name { get; set; }
    public MacAddress BtAddress { get; set; }
    public bool IsConnected { get; set; }
    public bool IsClearPairingAllowed { get; set; }
    public ImageSource ConnectedImageSource { get; set; }

  }
}
