using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Jabra_SDK_Demo.Model
{
  public class RemoteMMIv2 : INotifyPropertyChanged
  {
    private bool _remoteMMIv2Supported;
    public bool RemoteMMIv2Supported
    {
      get { return _remoteMMIv2Supported; }
      set
      {
        _remoteMMIv2Supported = value;
        OnPropertyChanged("RemoteMMIv2Supported");
      }
    }

    private ObservableCollection<string> _rMMiv2Data;
    public ObservableCollection<string> RMMiv2Data
    {
      get { return _rMMiv2Data; }
      set
      {
        _rMMiv2Data = value;
        OnPropertyChanged("RMMiv2Data");
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
    protected void OnPropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion   
  }
  public class RemoteMMIv2Details
  {
    public int DeviceId;
    public string Data;
  }
}
