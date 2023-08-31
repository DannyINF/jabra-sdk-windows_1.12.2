using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JabraSDK;

namespace Jabra_SDK_Demo.Model
{
  public enum AccessChannel
  {
    Public = 0,
    Test,
    Beta
  }

  public class FirmwareUpdates
  {
    public int DeviceId { get; set; }
    public bool ProgressVisibility { get; set; }
    public string Message { get; set; }
    public string DownloadCancel { get; set; }
    public List<IFirmware> FirmwareInformation { get; set; }
    public int Progress { get; set; }
    public bool FileDownloaded { get; set; }
    public Version Version { get; set; }
    public bool FileDownloadInProgress { get; set; }
    public IFirmware SelectedItem { get; set; }
    public int SelectedIndex { get; set; }
    public string IsFwLockEnabled { get; set; }
  }

  public class FirmwareUpdate : INotifyPropertyChanged
  {
    public FirmwareUpdate()
    {

    }

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

    private bool _progressVisibility;
    public bool ProgressVisibility
    {
      get { return _progressVisibility; }
      set
      {
        _progressVisibility = value;
        OnPropertyChanged("ProgressVisibility");
      }
    }

    private string _message;
    public string Message
    {
      get { return _message; }
      set
      {
        _message = value;
        OnPropertyChanged("Message");
      }
    }

    private string _downloadCancel = "Download";
    public string DownloadCancel
    {
      get { return _downloadCancel; }
      set
      {
        _downloadCancel = value;
        OnPropertyChanged("DownloadCancel");
      }
    }


    private List<IFirmware> _firmwareInformation;
    public List<IFirmware> FirmwareInformation
    {
      get { return _firmwareInformation; }
      set
      {
        _firmwareInformation = value;
        OnPropertyChanged("FirmwareInformation");
      }
    }

    private IFirmware _selectedItem;
    public IFirmware SelectedItem
    {
      get
      {
        return _selectedItem;
      }
      set
      {
        _selectedItem = value;
        OnPropertyChanged("SelectedItem");
      }
    }

    private int _progress = 0;
    public int Progress
    {
      get { return _progress; }
      set
      {
        _progress = value;
        OnPropertyChanged("Progress");
      }
    }

    private bool _fileDownloaded;
    public bool FileDownloaded
    {
      get { return _fileDownloaded; }
      set
      {
        _fileDownloaded = value;
        OnPropertyChanged("FileDownloaded");
      }
    }

    private Version _version;
    public Version Version
    {
      get { return _version; }
      set
      {
        _version = value;
        OnPropertyChanged("Version");
      }
    }

    private bool _fileDownloadInProgress;
    public bool FileDownloadInProgress
    {
      get { return _fileDownloadInProgress; }
      set
      {
        _fileDownloadInProgress = value;
        OnPropertyChanged("FileDownloadInProgress");
      }
    }

    private int _selectedIndex;
    public int SelectedIndex
    {
      get { return _selectedIndex; }
      set
      {
        _selectedIndex = value;
        OnPropertyChanged("SelectedIndex");
      }
    }

    private string _isFwLockEnabled;
    public string IsFwLockEnabled
    {
      get { return _isFwLockEnabled; }
      set
      {
        _isFwLockEnabled = value;
        OnPropertyChanged("IsFwLockEnabled");
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
