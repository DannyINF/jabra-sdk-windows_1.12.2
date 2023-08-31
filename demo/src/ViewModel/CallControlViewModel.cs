using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;
using CallControl = Jabra_SDK_Demo.Model.CallControl;
using System.Reflection;

namespace Jabra_SDK_Demo.ViewModel
{
  public class CallControlViewModel : ViewModelBase
  {
    private CallControl _callControl;
    public CallControl CallControl
    {
      get { return _callControl; }
      set
      {
        _callControl = value;
        OnPropertyChanged("CallControl");
      }
    }

    private List<CallControls> _allCallControls;
    public List<CallControls> AllCallControls
    {
      get { return _allCallControls; }
      set
      {
        _allCallControls = value;
        OnPropertyChanged("AllCallControls");
      }
    }

    private ICommand _clickClearCommand;
    public ICommand ClickClearCommand
    {
      get
      {
        return _clickClearCommand ?? (_clickClearCommand = new CommandHandler(() => ClickClearAction(), _canExecute));
      }
    }
    private bool _canExecute;

    private ICommand _buttonClickCommand;
    public ICommand ButtonClickCommand
    {
      get
      {
        return _buttonClickCommand ?? (_buttonClickCommand = new CommandHandlerArguments(param => ButtonClickCommandHandler(param), _canExecute));
      }
    }

    private ICommand _buttonConnectCommand;
    public ICommand ButtonConnectCommand
    {
      get
      {
        return _buttonConnectCommand ?? (_buttonConnectCommand = new CommandHandlerArguments(param => ButtonConnectClickCommandHandler(), _canExecute));
      }
    }

    private ICommand _buttonDisconnectCommand;
    public ICommand ButtonDisconnectCommand
    {
      get
      {
        return _buttonDisconnectCommand ?? (_buttonDisconnectCommand = new CommandHandlerArguments(param => ButtonDisConnectClickCommandHandler(), _canExecute));
      }
    }

    private ICommand _readyForTelephonyChecked;
    public ICommand ReadyForTelephonyChecked
    {
      get
      {
        return _readyForTelephonyChecked ?? (_readyForTelephonyChecked = new CommandHandlerArguments(param => ReadyForTelephonyCheckCommandHandler(IntegrationService.ReadyForTelephonyChecked), _canExecute));
      }
    }

    private ObservableCollection<string> _rawHidData;

    public ObservableCollection<string> RawHidData
    {
      get { return _rawHidData; }
      set
      {
        _rawHidData = value;
        OnPropertyChanged("RawHidData");
      }
    }

    private ObservableCollection<string> _translatedData;
    public ObservableCollection<string> TranslatedData
    {
      get { return _translatedData; }
      set
      {
        _translatedData = value;
        OnPropertyChanged("TranslatedData");
      }
    }

    private DispatcherTimer busyLightTimer;

    public ICommand GetLockCommand { get; }
    public ICommand ReleaseLockCommand { get; }
    public ICommand IsLockedCommand { get; }
    public CallControlViewModel()
    {
    }

    private void GetLock()
    {
	    CallControl.Lock = CommonMethods.SelectedDevice.Lock() ? "Locked by us" : "Failed to lock";
    }

    private void ReleaseLock()
    {
	    CallControl.Lock = CommonMethods.SelectedDevice.Unlock() ? "Unlocked" : "Failed to unlock";
    }

    private void IsLocked()
    {
	    CallControl.Lock = $"Locked: {CommonMethods.SelectedDevice.IsLocked}";
    }

    public void WaitSystemToBeIdle()
    {
      if (busyLightTimer == null)
      {
        busyLightTimer = new DispatcherTimer();
        busyLightTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
        busyLightTimer.Tick += new EventHandler(BusyLightTimer_Tick);
        busyLightTimer.Start();
      }
    }

    public CallControlViewModel(IDevice device)
    {
      _canExecute = true;
      GetLockCommand = new CommandHandler(GetLock, _canExecute);
      ReleaseLockCommand = new CommandHandler(ReleaseLock, _canExecute);
      IsLockedCommand = new CommandHandler(IsLocked, _canExecute);
      RawHidData = new ObservableCollection<string>();
      TranslatedData = new ObservableCollection<string>();
      AllCallControls = new List<CallControls>();
      _callControl = new CallControl
      {
        HidWorkingStates = new Dictionary<int, string>
        {
          { Convert.ToInt32(HidState.STD_HID), HidState.STD_HID.ToString() },
          { Convert.ToInt32(HidState.GN_HID), HidState.GN_HID.ToString() }
        }
      };

      UpdateRccSettings(device.DeviceId);
      ShowRccSettings(device.DeviceId);
      IntegrationService.ShowIntegrationServices();
    }

    public void ShowRccSettings(int deviceId)
    {

      if (CommonMethods.SelectedDevice != null)
      {
        IDevice device = CommonMethods.SelectedDevice;
        var result = (from list in AllCallControls
                      where list.DeviceId == deviceId
                      select list).FirstOrDefault();
        if (result != null)
        {
          _callControl.OffHookStatus = Convert.ToInt32(result.OffHookStatus);
          _callControl.RingerStatus = Convert.ToInt32(result.RingerStatus);
          _callControl.AudioLinkStatus = Convert.ToInt32(result.AudioLinkStatus);
          _callControl.MuteStatus = Convert.ToInt32(result.MuteStatus);
          _callControl.OnHoldStatus = Convert.ToInt32(result.OnHoldStatus);
          _callControl.BusyLightStatus = Convert.ToInt32(result.BusyLightStatus);
          _callControl.OffHookStatusEnabled = device.IsHookStateSupported;
          _callControl.RingerStatusEnabled = device.IsInBandRingerSupported || device.IsOutBandRingerSupported;
          _callControl.MuteStatusEnabled = device.IsMicrophoneMuteSupported;
          _callControl.OnHoldStatusEnabled = device.IsCallOnHoldSupported;
          _callControl.AudioLinkStatusEnabled = device.IsAudioLinkSupported;
          _callControl.BusyLightStatusEnabled = device.IsSetBusylightSupported;

          if (device.DeviceConnectionType == DeviceConnectionType.Usb)
          {
            _callControl.CallControlSupported = false;
          }
          else
          {
            _callControl.CallControlSupported = true;
          }

          try
          {
            if (device.IsGnHidStdHidSupported)
            {
              _callControl.IsGnHidStdHidSupported = true;
              HidState hidState = device.GetHidWorkingState();
              _callControl.CurrentHIDWorkingStateValue = Convert.ToInt32(hidState);
              _callControl.CurrentHIDWorkingState = hidState.ToString();
            }
            else
            {
              _callControl.IsGnHidStdHidSupported = false;
              _callControl.CurrentHIDWorkingState = "Not supported";
            }
          }
          catch { }
        }
      }
    }

    public void ClickClearAction()
    {
      RawHidData.Clear();
      TranslatedData.Clear();
    }

    private void BusyLightTimer_Tick(object sender, EventArgs e)
    {
      try
      {
        if (busyLightTimer != null)
        {
          busyLightTimer.Stop();
          busyLightTimer = null;
          bool busyLightStatus = false;
          if (CommonMethods.SelectedDevice != null)
          {
            int selectedDeviceId = CommonMethods.SelectedDevice.DeviceId;
            busyLightStatus = CommonMethods.SelectedDevice.IsBusylightOn;
            var result = (from list in AllCallControls
                          where list.DeviceId == selectedDeviceId
                          select list).FirstOrDefault();
            result.BusyLightStatus = busyLightStatus;
            UpdateRccButtonStatus(busyLightStatus, ButtonStates.BusyLight, selectedDeviceId);
          }
        }
      }
      catch { }
    }

    private void ButtonClickCommandHandler(object parameter)
    {

      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      int selectedDeviceId = CommonMethods.SelectedDevice.DeviceId;

      var result = (from list in AllCallControls
                    where list.DeviceId == selectedDeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        ButtonStates btnState = ButtonStates.OffHook;
        bool value = false;

        try
        {
          switch (parameter.ToString())
          {
            case "OffHook":
              value = result.OffHookStatus = !result.OffHookStatus;
              btnState = ButtonStates.OffHook;
              CommonMethods.SelectedDevice.SetHookState(result.OffHookStatus);
              if (Convert.ToBoolean(_callControl.BusyLightStatusEnabled))
              {
                // Read busylight status in dispatcher timer
                WaitSystemToBeIdle();
              }
              break;
            case "Ringer":
              value = result.RingerStatus = !result.RingerStatus;
              btnState = ButtonStates.Ringer;
              CommonMethods.SelectedDevice.SetRinger(result.RingerStatus);
              break;
            case "Mute":
              value = result.MuteStatus = !result.MuteStatus;
              btnState = ButtonStates.Mute;
              CommonMethods.SelectedDevice.SetMicrophoneMuted(result.MuteStatus);
              break;
            case "OnHold":
              btnState = ButtonStates.OnHold;
              if (Convert.ToBoolean(_callControl.OnHoldStatus))
              {
                result.OffHookStatus = true;
                value = result.OnHoldStatus = false;
                CommonMethods.SelectedDevice.SetHookState(result.OffHookStatus);
                CommonMethods.SelectedDevice.SetCallOnHold(result.OnHoldStatus);
              }
              else
              {
                result.OffHookStatus = false;
                value = result.OnHoldStatus = true;
                CommonMethods.SelectedDevice.SetCallOnHold(result.OnHoldStatus);
                CommonMethods.SelectedDevice.SetHookState(result.OffHookStatus);
              }
              UpdateRccButtonStatus(result.OffHookStatus, ButtonStates.OffHook, selectedDeviceId);
              break;
            case "AudioLink":
              value = result.AudioLinkStatus = !result.AudioLinkStatus;
              btnState = ButtonStates.AudioLink;
              CommonMethods.SelectedDevice.SetAudioLinkState(result.AudioLinkStatus);
              break;
            case "BusyLight":
              value = result.BusyLightStatus = !result.BusyLightStatus;
              btnState = ButtonStates.BusyLight;
              CommonMethods.SelectedDevice.SetBusylightState(result.BusyLightStatus);
              break;
          }
          UpdateRccButtonStatus(value, btnState, selectedDeviceId);
        }
        catch (Exception)
        {
          MessageBoxService.ShowMessage($"Error during set {btnState}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    public void SetRccButtonStatus(ushort deviceId, string translatedInData, bool buttonInData)
    {
      ButtonStates btnState = ButtonStates.Undefined;
      bool setValue = false;

      if (AllCallControls == null) return;

      var result = (from list in AllCallControls
                    where list.DeviceId == (Convert.ToInt32(deviceId))
                    select list).FirstOrDefault();
      if (result != null)
      {
        try
        {
          switch (translatedInData.ToLower(CultureInfo.InvariantCulture))
          {
            case "offhook":
              // Bring the softphone to focus, if set as preferred softphone
              // Need to check call status by integrating Call manager logic which is currently out of scope.
              bool isInFocus = MainWindowViewModel.IntegrationServices.IsInFocus;
              if (buttonInData && isInFocus)
              {
                // Bring demo app to focus if set as "Preferred softphone" on "OffHook"-true event
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action(delegate
                {
                  Application.Current.MainWindow.WindowState = WindowState.Normal;
                  Application.Current.MainWindow.Show();
                  Application.Current.MainWindow.Activate();
                }), DispatcherPriority.Send, null);
              }
              setValue = buttonInData;
              break;
            case "mute":
              setValue = buttonInData;
              break;
            case "flash":
              setValue = buttonInData;
              break;
            case "online":
              btnState = ButtonStates.AudioLink;
              setValue = buttonInData;
              result.AudioLinkStatus = setValue;
              break;
            case "busylight":
              btnState = ButtonStates.BusyLight;
              setValue = buttonInData;
              result.BusyLightStatus = setValue;
              break;
          }
        }
        catch { }

        if (btnState != ButtonStates.Undefined)
          UpdateRccButtonStatus(setValue, btnState, Convert.ToInt32(deviceId));
      }
    }

    public void UpdateRawHidData(string message)
    {
      RawHidData?.Insert(0, message);
    }
    public void UpdateTranslatedData(string message)
    {
      TranslatedData?.Insert(0, message);
    }

    private void UpdateRccButtonStatus(bool value, ButtonStates state, int deviceId)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      if (CommonMethods.SelectedDevice.DeviceId == deviceId)
      {
        switch (state)
        {
          case ButtonStates.OffHook:
            _callControl.OffHookStatus = Convert.ToInt32(value);
            break;
          case ButtonStates.Ringer:
            _callControl.RingerStatus = Convert.ToInt32(value);
            break;
          case ButtonStates.Mute:
            _callControl.MuteStatus = Convert.ToInt32(value);
            break;
          case ButtonStates.OnHold:
            _callControl.OnHoldStatus = Convert.ToInt32(value);
            break;
          case ButtonStates.AudioLink:
            _callControl.AudioLinkStatus = Convert.ToInt32(value);
            break;
          case ButtonStates.BusyLight:
            _callControl.BusyLightStatus = Convert.ToInt32(value);
            break;
        }
      }
    }

    public void UpdateRccSettings(int deviceId)
    {
      CallControls callCtrls = new CallControls();
      callCtrls.DeviceId = deviceId;
      AllCallControls.Add(callCtrls);
    }

    private void ButtonConnectClickCommandHandler()
    {
      Guid spGuid = Guid.Empty;

      if (!Guid.TryParse(IntegrationService.ClientGuid, out spGuid))
      {
        MessageBoxService.ShowMessage($"Invalid client GUID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (String.IsNullOrEmpty(IntegrationService.ClientName))
      {
        MessageBoxService.ShowMessage($"Client name is missing", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      try
      {
        IntegrationService.IsConnectedToIntegrationService = MainWindowViewModel.IntegrationServices.ConnectClient(Guid.Parse(IntegrationService.ClientGuid), IntegrationService.ClientName, Assembly.GetExecutingAssembly().GetName().Version);

        if (IntegrationService.IsConnectedToIntegrationService)
        {
          IntegrationService.ConnectEnabled = false;
          IntegrationService.ReadyForTelephonyEnabled = true;
          IntegrationService.ShowIntegrationServices();
        }
        else
        {
          IntegrationService.ConnectEnabled = true;
          IntegrationService.ReadyForTelephonyEnabled = false;
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ButtonDisConnectClickCommandHandler()
    {
      try
      {
        MainWindowViewModel.IntegrationServices.ClientState = ClientStateId.NotReadyForTelephony;
        MainWindowViewModel.IntegrationServices.DisconnectClient(Guid.Parse(IntegrationService.ClientGuid));
        IntegrationService.IsConnectedToIntegrationService = false;
        IntegrationService.IsClientInFocus = "No";
        IntegrationService.ConnectEnabled = true;
        IntegrationService.ReadyForTelephonyChecked = false;
        IntegrationService.ReadyForTelephonyEnabled = false;
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ReadyForTelephonyCheckCommandHandler(object param)
    {
      if (Convert.ToBoolean(param))
      {
        MainWindowViewModel.IntegrationServices.ClientState = ClientStateId.ReadyForTelephony;
      }
      else
        MainWindowViewModel.IntegrationServices.ClientState = ClientStateId.NotReadyForTelephony;
    }

    #region INotifyPropertyChanged Members

#pragma warning disable CS0108 // 'CallControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'CallControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

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
