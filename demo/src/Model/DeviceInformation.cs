using System.ComponentModel;
using System.Windows.Media;

namespace Jabra_SDK_Demo.Model
{
  public class DeviceInformation : INotifyPropertyChanged
  {
    private int _deviceId = -1;
    public int DeviceId
    {
      get { return _deviceId; }
      set
      {
        _deviceId = value;
        OnPropertyChanged("DeviceId");
      }
    }

    private string _serialNumber = string.Empty;
    public string SerialNumber
    {
      get { return _serialNumber; }
      set
      {
        _serialNumber = value;
        OnPropertyChanged("SerialNumber");
      }
    }

    private string _batteryStatus;
    public string BatteryStatus
    {
      get { return _batteryStatus; }
      set
      {
        _batteryStatus = value;
        OnPropertyChanged("BatteryStatus");
      }
    }

    private bool _hasExtraBatteryUnits;
    public bool HasExtraBatteryUnits
    {
      get { return _hasExtraBatteryUnits; }
      set
      {
        _hasExtraBatteryUnits = value;
        OnPropertyChanged("HasExtraBatteryUnits");
      }
    }

    private string _batteryStatusUnits;
    public string BatteryStatusUnits
    {
      get { return _batteryStatusUnits; }
      set
      {
        _batteryStatusUnits = value;
        OnPropertyChanged("BatteryStatusUnits");
      }
    }

    private string _deviceName;
    public string DeviceName
    {
      get { return _deviceName; }
      set
      {
        _deviceName = value;
        OnPropertyChanged("DeviceName");
      }
    }

    private ImageSource _imageSource;
    public ImageSource ImageSource
    {
      get { return _imageSource; }
      set
      {
        _imageSource = value;
        OnPropertyChanged("ImageSource");
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
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
}
