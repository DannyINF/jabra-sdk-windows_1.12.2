using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabra_SDK_Demo.Helpers
{
  public class RemoteMMIv2Setting
  {
    public int DeviceId { get; set; }
    public int Type { get; set; }
    public List<int> InputActionId { get; set; }
    public int PriorityId { get; set; }
    public string ConfiguredFunction { get; set; }
    public bool IsOutputConfigured { get; set; }
    public int RedOutputValue { get; set; }
    public int GreenOutputValue { get; set; }
    public int BlueOutputValue { get; set; }
    public int SequenceId { get; set; }
  }
}
