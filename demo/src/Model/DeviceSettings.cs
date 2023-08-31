using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Jabra_SDK_Demo.Model
{
  public class FileUploadProgress
  {
    public int DeviceId { get; set; }
    public bool ProgressVisibility { get; set; }
    public string Message { get; set; }
    public int Progress { get; set; }
  }

  public class DeviceSettings : INotifyPropertyChanged
  {
    private bool _restoreDefault = false;
    public bool RestoreDefault
    {
      get { return _restoreDefault; }
      set
      {
        _restoreDefault = value;
        OnPropertyChanged("RestoreDefault");
      }
    }

    private bool _enableCancel = false;
    public bool EnableCancel
    {
      get { return _enableCancel; }
      set
      {
        _enableCancel = value;
        OnPropertyChanged("EnableCancel");
      }
    }

    private bool _dFUmode = true;
    public bool DFUmode
    {
      get { return _dFUmode; }
      set
      {
        _dFUmode = value;
        OnPropertyChanged("DFUmode");
      }
    }

    private bool _uploadRingtone = false;
    public bool UploadRingtone
    {
      get { return _uploadRingtone; }
      set
      {
        _uploadRingtone = value;
        OnPropertyChanged("UploadRingtone");
      }
    }

    private bool _uploadImage = false;
    public bool UploadImage
    {
      get { return _uploadImage; }
      set
      {
        _uploadImage = value;
        OnPropertyChanged("UploadImage");
      }
    }

    private bool _wizardMode = false;
    public bool WizardMode
    {
      get { return _wizardMode; }
      set
      {
        _wizardMode = value;
        OnPropertyChanged("WizardMode");
      }
    }

    private bool _fullWizardMode = false;
    public bool FullWizardMode
    {
      get { return _fullWizardMode; }
      set
      {
        _fullWizardMode = value;
        OnPropertyChanged("FullWizardMode");
      }
    }

    private bool _limitedWizardMode = false;
    public bool LimitedWizardMode
    {
      get { return _limitedWizardMode; }
      set
      {
        _limitedWizardMode = value;
        OnPropertyChanged("LimitedWizardMode");
      }
    }

    private bool _setDateTime = false;
    public bool SetDateTime
    {
      get { return _setDateTime; }
      set
      {
        _setDateTime = value;
        OnPropertyChanged("SetDateTime");
      }
    }

    private DateTime _dateTimeVal = DateTime.Now;
    public DateTime DateTimeVal
    {
      get { return _dateTimeVal; }
      set
      {
        _dateTimeVal = value;
        OnPropertyChanged("DateTimeVal");
      }
    }

        private DateTime _timestampVal = DateTime.Now;
        public DateTime TimestampVal
        {
            get { return _timestampVal; }
            set {
                _timestampVal = value;
                OnPropertyChanged("TimestampVal");
            }
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

    private JabraSDK.WizardMode _currentWizardMode = 0;

    public JabraSDK.WizardMode CurrentWizardMode
    {
      get { return _currentWizardMode; }
      set
      {
        _currentWizardMode = value;
        OnPropertyChanged("CurrentWizardMode");
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
