using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Jabra_SDK_Demo.Helpers;
using JabraSDK;

namespace Jabra_SDK_Demo.Model
{
  public class CallControls
  {
    public int DeviceId { get; set; }
    public bool OffHookStatus { get; set; }
    public bool RingerStatus { get; set; }
    public bool MuteStatus { get; set; }
    public bool OnHoldStatus { get; set; }
    public bool AudioLinkStatus { get; set; }
    public bool BusyLightStatus { get; set; }
  }

  public enum ButtonStates
  {
    Undefined,
    OffHook,
    Ringer,
    Mute,
    OnHold,
    AudioLink,
    BusyLight
  }

  public class CallControl : INotifyPropertyChanged
  {
    public CallControl()
    {

    }

    private string _lock;
    public string Lock
    {
	    get => _lock;
	    set
	    {
		    _lock = value;
        OnPropertyChanged("Lock");
	    }
    }

    private int _offHookStatus;
    public int OffHookStatus
    {
      get { return _offHookStatus; }
      set
      {
        _offHookStatus = value;
        OnPropertyChanged("OffHookStatus");
      }
    }

    private int _ringerStatus;
    public int RingerStatus
    {
      get { return _ringerStatus; }
      set
      {
        _ringerStatus = value;
        OnPropertyChanged("RingerStatus");
      }
    }

    private int _muteStatus;
    public int MuteStatus
    {
      get { return _muteStatus; }
      set
      {
        _muteStatus = value;
        OnPropertyChanged("MuteStatus");
      }
    }

    private int _onHoldStatus;
    public int OnHoldStatus
    {
      get { return _onHoldStatus; }
      set
      {
        _onHoldStatus = value;
        OnPropertyChanged("OnHoldStatus");
      }
    }

    private int _audioLinkStatus;
    public int AudioLinkStatus
    {
      get { return _audioLinkStatus; }
      set
      {
        _audioLinkStatus = value;
        OnPropertyChanged("AudioLinkStatus");
      }
    }

    private int _busyLightStatus;
    public int BusyLightStatus
    {
      get { return _busyLightStatus; }
      set
      {
        _busyLightStatus = value;
        OnPropertyChanged("BusyLightStatus");
      }
    }


    private bool _offHookStatusEnabled;
    public bool OffHookStatusEnabled
    {
      get { return _offHookStatusEnabled; }
      set
      {
        _offHookStatusEnabled = value;
        OnPropertyChanged("OffHookStatusEnabled");
      }
    }

    private bool _ringerStatusEnabled;
    public bool RingerStatusEnabled
    {
      get { return _ringerStatusEnabled; }
      set
      {
        _ringerStatusEnabled = value;
        OnPropertyChanged("RingerStatusEnabled");
      }
    }


    private bool _muteStatusEnabled;
    public bool MuteStatusEnabled
    {
      get { return _muteStatusEnabled; }
      set
      {
        _muteStatusEnabled = value;
        OnPropertyChanged("MuteStatusEnabled");
      }
    }


    private bool _onHoldStatusEnabled;
    public bool OnHoldStatusEnabled
    {
      get { return _onHoldStatusEnabled; }
      set
      {
        _onHoldStatusEnabled = value;
        OnPropertyChanged("OnHoldStatusEnabled");
      }
    }


    private bool _audioLinkStatusEnabled;
    public bool AudioLinkStatusEnabled
    {
      get { return _audioLinkStatusEnabled; }
      set
      {
        _audioLinkStatusEnabled = value;
        OnPropertyChanged("AudioLinkStatusEnabled");
      }
    }

    private bool _busyLightStatusEnabled;
    public bool BusyLightStatusEnabled
    {
      get { return _busyLightStatusEnabled; }
      set
      {
        _busyLightStatusEnabled = value;
        OnPropertyChanged("BusyLightStatusEnabled");
      }
    }

    private bool _callControlSupported;
    public bool CallControlSupported
    {
      get { return _callControlSupported; }
      set
      {
        _callControlSupported = value;
        OnPropertyChanged("CallControlSupported");
      }
    }

    private bool _isGnHidStdHidSupported;
    public bool IsGnHidStdHidSupported
    {
      get { return _isGnHidStdHidSupported; }
      set
      {
        _isGnHidStdHidSupported = value;
        OnPropertyChanged("IsGnHidStdHidSupported");
      }
    }

    private string _currentHIDWorkingState = "Not supported";
    public string CurrentHIDWorkingState
    {
      get { return _currentHIDWorkingState; }
      set
      {
        _currentHIDWorkingState = value;
        OnPropertyChanged("CurrentHIDWorkingState");
      }
    }

    private int _currentHIDWorkingStateValue;
    public int CurrentHIDWorkingStateValue
    {
      get { return _currentHIDWorkingStateValue; }
      set
      {
        _currentHIDWorkingStateValue = value;
        _currentHIDWorkingState = HidWorkingStates?[value];
        try
        {
          CommonMethods.SelectedDevice.SetHidWorkingState((HidState)value);
        }
        catch (Exception e)
        {
          MessageBoxService.ShowMessage($"Error in setting HID working state:{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        OnPropertyChanged("CurrentHIDWorkingStateValue");
        OnPropertyChanged("CurrentHIDWorkingState");
      }
    }

    private Dictionary<int, string> _hidWorkingStates;

    public Dictionary<int, string> HidWorkingStates
    {
      get { return _hidWorkingStates; }
      set
      {
        _hidWorkingStates = value;
        OnPropertyChanged("HidWorkingStates");
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