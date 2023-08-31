using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
  public class DeviceLogging : INotifyPropertyChanged
  {
    private bool _deviceLoggingSupported;
    public bool DeviceLoggingSupported
    {
      get { return _deviceLoggingSupported; }
      set
      {
        _deviceLoggingSupported = value;
        OnPropertyChanged("DeviceLoggingSupported");
      }
    }

    private bool _isCheckBoxChecked;
    public bool IsCheckBoxChecked
    {
      get { return _isCheckBoxChecked; }
      set
      {
        _isCheckBoxChecked = value;
        OnPropertyChanged("IsCheckBoxChecked");
      }
    }

    private ObservableCollection<string> _data;
    public ObservableCollection<string> Data
    {
      get { return _data; }
      set
      {
        _data = value;
        OnPropertyChanged("Data");
      }
    }

    private bool _deviceLoggingEnabled;
    public bool DeviceLoggingEnabled
    {
      get { return _deviceLoggingEnabled; }
      set
      {
        _deviceLoggingEnabled = value;
        OnPropertyChanged("DeviceLoggingEnabled");
      }
    }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    // On PropertyChnaged Event
    protected void OnPropertyChanged(string name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(name));
      }
    }
    #endregion
  }

  public class LoggingDetails
  {
    public int DeviceId;
    public string Data;
  }
}
