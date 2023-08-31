using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using DeviceSettings = Jabra_SDK_Demo.Model.DeviceSettings;
using JabraSDK;
using Microsoft.Win32;
using System.Text;
using Jabra_SDK_Demo.View;
using System.Windows.Threading;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class DeviceSettingsViewModel : ViewModelBase
  {
    #region Properties

    private const string AuthMessage = "Not a valid User Id.\nPress 'Yes' to go to 'Authorization' tab.";
    private const string wavExtension = ".wav";
    private const string mp3Extension = ".mp3";

    private DeviceSettings _deviceSettings;
    private InitialArguments _initialArguments;

    public DeviceSettings DeviceSettings
    {
      get { return _deviceSettings; }
      set
      {
        _deviceSettings = value;
        OnPropertyChanged("DeviceSettings");
      }
    }

    private ObservableCollection<ExpanderControlViewModel> _expanderControlList = new ObservableCollection<ExpanderControlViewModel>();
    public ObservableCollection<ExpanderControlViewModel> ExpanderControlList
    {
      get { return _expanderControlList; }
      set
      {
        _expanderControlList = value;
        OnPropertyChanged("ExpanderControlList");
      }
    }

    #endregion

    #region ICommand   

    private ICommand _windowLoaded;

    public ICommand WindowLoaded
    {
      get
      {
        return _windowLoaded ?? (_windowLoaded = new CommandHandler(() => WindowLoadedAction(), true));
      }
    }

    private void WindowLoadedAction()
    {
      SpecialHandlers.SettingsLoaded = true;
    }

    private ICommand _clickClearCommand;
    public ICommand ClickClearCommand
    {
      get
      {
        return _clickClearCommand ?? (_clickClearCommand = new CommandHandler(() => ClickClearAction(), true));
      }
    }


    private ICommand _clickRestoreDefaultCommand;
    public ICommand ClickRestoreDefaultCommand
    {
      get
      {
        return _clickRestoreDefaultCommand ?? (_clickRestoreDefaultCommand = new CommandHandler(() => ClickRestoreDefaultAction(), true));
      }
    }

    private ICommand _clickApplyCommand;
    public ICommand ClickApplyCommand
    {
      get
      {
        return _clickApplyCommand ?? (_clickApplyCommand = new CommandHandler(() => ClickApplyAction(), true));
      }
    }

    private ICommand _clickUploadRingtoneCommand;
    public ICommand ClickUploadRingtoneCommand
    {
      get
      {
        return _clickUploadRingtoneCommand ?? (_clickUploadRingtoneCommand = new CommandHandler(() => ClickUploadRingtoneFile(), true));
      }
    }

    private ICommand _clickUploadImageCommand;
    public ICommand ClickUploadImageCommand
    {
      get
      {
        return _clickUploadImageCommand ?? (_clickUploadImageCommand = new CommandHandler(() => ClickUploadImageFile(), true));
      }
    }

    private ICommand _clickSetDateTimeCommand;
    public ICommand ClickSetDateTimeCommand
    {
      get
      {
        return _clickSetDateTimeCommand ?? (_clickSetDateTimeCommand = new CommandHandler(() => ClickSetDateTime(false), true));
      }
    }

    private ICommand _clickSetLocalDateTimeCommand;
    public ICommand ClickSetLocalDateTimeCommand
    {
      get
      {
        return _clickSetLocalDateTimeCommand ?? (_clickSetLocalDateTimeCommand = new CommandHandler(() => ClickSetDateTime(true), true));
      }
    }

    private ICommand _clickReadTimestampCommand;
    public ICommand ClickReadTimestampCommand
    {
      get
      {
        return _clickReadTimestampCommand ?? (_clickReadTimestampCommand = new CommandHandler(() => ClickReadTimestamp(), true));
      }
    }

    private ICommand _clickSetTimestampCommand;
    public ICommand ClickSetTimestampCommand
    {
      get
      {
        return _clickSetTimestampCommand ?? (_clickSetTimestampCommand = new CommandHandler(() => ClickSetTimestamp(), true));
      }
    }


    private ICommand _clickSetWizardModeCommand;
    public ICommand ClickSetWizardModeCommand
    {
      get
      {
        return _clickSetWizardModeCommand ?? (_clickSetWizardModeCommand = new CommandHandler(() => ClickSetWizardMode(), true));
      }
    }

    private ICommand _clickGetWizardModeCommand;
    public ICommand ClickGetWizardModeCommand
    {
      get
      {
        return _clickGetWizardModeCommand ?? (_clickGetWizardModeCommand = new CommandHandler(() => ClickGetWizardMode(), true));
      }
    }

    #endregion

    #region Constructor

    public DeviceSettingsViewModel()
    {
    }
    public DeviceSettingsViewModel(IDevice device, List<ISetting> setting)
    {
      _deviceSettings = new DeviceSettings();
      _initialArguments = new InitialArguments()
      {
        Device = device,
        Setting = setting,
      };
      ShowDeviceSettings(device, setting);
    }

    #endregion

    public void ClickRestoreDefaultAction()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      try
      {
        CommonMethods.SelectedDevice.FactoryReset();
        SpecialHandlers.UnWrittenSettings.Clear();
        SpecialHandlers.UnWrittenSettingsDeviceId = -1;
        SpecialHandlers.DisplayFisrtTab = true;
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Factory Reset operation failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void ClickApplyAction()
    {
      NativeCommonLibrary.UpdateSettingsToDevice(true);
    }

    public void ClickClearAction()
    {
      if (SpecialHandlers.UnWrittenSettings.Count > 0)
      {
        if (SpecialHandlers.EnableDisableButton)
        {
          var result = MessageBoxService.AskForConfirmation("Apply device settings before exiting?", "Apply Changes?", MessageBoxButton.YesNo, MessageBoxImage.Information);
          if (result == MessageBoxResult.Yes)
          {
            NativeCommonLibrary.UpdateSettingsToDevice(true);
          }
          else if (result == MessageBoxResult.No)
          {
            SpecialHandlers.UnWrittenSettings.Clear();
            SpecialHandlers.UnWrittenSettingsDeviceId = -1;
            SpecialHandlers.DisplayFisrtTab = true;
            SpecialHandlers.EnableDisableButton = false;
          }
        }
        else
        {
          SpecialHandlers.DisplayFisrtTab = true;
        }
      }
      else
      {
        SpecialHandlers.DisplayFisrtTab = true;
      }
    }

    public void ClickUploadRingtoneFile()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      var result = (from list in MainWindowViewModel.AllFileUploadDetails
                    where list.DeviceId == device.DeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        result.Progress = 0;
        result.Message = String.Empty;

        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Ringtone Files (*.mp3;*.wav)|*.mp3;*.wav";
        bool? success = openFileDialog.ShowDialog();

        string audioFilePath = string.Empty;
        if (success == true)
        {
          try
          {
            DirectoryInfo dInfo = new DirectoryInfo(openFileDialog.FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

            // Store the converted file to resource folder
            audioFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JabraSDK", "Resources");
            audioFilePath += @"\gnWaveFile" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".wav";
            File.Create(audioFilePath).Close();

            int maxLength = 27; //27s
            IAudioFile audioFileParam = device.GetAudioFileParametersForUpload();
            int bytesPerSample = audioFileParam.BitsPerSample / 8;
            int maxByteLength = maxLength * audioFileParam.SampleRate * bytesPerSample; //maxByteLength not to exceed fileParam.maxFileSize, truncate any longer

            // Convert input audio file(.mp3/.wav) to uncompressed wav format as specified
            if (openFileDialog.FileName.EndsWith(mp3Extension) || openFileDialog.FileName.EndsWith(wavExtension))
            {
              AudioUtilities.ConvertToWav(openFileDialog.FileName, audioFileParam, maxByteLength, audioFilePath);
            }
            else throw new ArgumentException("Invalid filename", openFileDialog.FileName);

            DeviceStatus ret = device.UploadWavRingtone(audioFilePath);

            if (ret != DeviceStatus.ReturnOk)
            {
              result.ProgressVisibility = false;
              MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
              result.ProgressVisibility = true;
            }
          }
          catch (Exception ex)
          {
            MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
          finally
          {
            File.Delete(audioFilePath);
          }
        }
      }
    }

    public void ClickUploadImageFile()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Image Files(*.bmp)|*.bmp";
      bool? success = openFileDialog.ShowDialog();

      if (success == true)
      {
        try
        {
          DirectoryInfo dInfo = new DirectoryInfo(openFileDialog.FileName);
          DirectorySecurity dSecurity = dInfo.GetAccessControl();
          dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
          dInfo.SetAccessControl(dSecurity);

          DeviceStatus ret = device.UploadImage(openFileDialog.FileName);

          if (ret == DeviceStatus.ReturnOk)
          {
            MessageBoxService.ShowMessage("File uploaded successfully to device", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
          }
          else
          {
            MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
        catch (Exception ex)
        {
          MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    public void ClickSetDateTime(bool isLocalDateTime)
    {

      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      try
      {
        DateTime dt = DateTime.Now;
        DeviceStatus ret = DeviceStatus.SystemError;

        ret = isLocalDateTime ? device.SetDateTime(dt) : device.SetDateTime(_deviceSettings.DateTimeVal);

        if (ret != DeviceStatus.ReturnOk)
        {
          MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
          MessageBoxService.ShowMessage("Date and time set successfully to device", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ReloadSettings()
    {
      SpecialHandlers.SettingsLoaded = false;
      MainWindowViewModel.ViewModelDeviceSettings = new DeviceSettingsViewModel(_initialArguments.Device, _initialArguments.Setting);

      MainWindowViewModel.DeviceSettingsViewModel = MainWindowViewModel.ViewModelDeviceSettings;
      Application.Current.Dispatcher.Invoke(DispatcherPriority.ApplicationIdle, new Action(() =>
      {
        SpecialHandlers.SettingsLoaded = true;
      }));
    }

    public void ShowDeviceSettings(IDevice device, List<ISetting> setting)
    {
      _deviceSettings.EnableCancel = false;
      SpecialHandlers.EnableDisableButton = false;
      if (!device.IsInFirmwareUpdateMode)
      {
        _deviceSettings.EnableCancel = true;
        _deviceSettings.RestoreDefault = device.IsFactoryResetSupported;
        _deviceSettings.UploadRingtone = device.IsUploadRingtoneSupported;
        _deviceSettings.UploadImage = device.IsUploadImageSupported;
        _deviceSettings.SetDateTime = device.IsSetDateTimeSupported;
        _deviceSettings.FullWizardMode = device.IsFeatureSupported(DeviceFeature.FullWizardMode);
        _deviceSettings.LimitedWizardMode = device.IsFeatureSupported(DeviceFeature.LimitedWizardMode);
        _deviceSettings.WizardMode = (_deviceSettings.FullWizardMode || _deviceSettings.LimitedWizardMode);
        if (_deviceSettings.WizardMode) { _deviceSettings.CurrentWizardMode = device.GetWizardMode(); }

        var settingsInfo = device.GetSettings();

        if (device.ErrorStatus != ErrorStatus.NoError)
        {
          MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
          if (settingsInfo != null)
          {
            var result = (from val in SpecialHandlers.SettingsInformation
                          where val.DeviceId == device.DeviceId
                          select val).FirstOrDefault();
            if (result != null)
            {
              SpecialHandlers.SettingsInformation.Remove(result);
            }
            SpecialHandlers.SettingsInformation.Add(new SettingsInformation() { DeviceId = device.DeviceId, DeviceSettings = settingsInfo });
            ShowSettings(settingsInfo);
          }
        }
      }
      else
      {
        _deviceSettings.DFUmode = false;
      }
    }



    private void ShowSettings(List<ISetting> settingsInfo)
    {
      CompositeCollection compositeCollection = new CompositeCollection();

      ExpanderControlViewModel expander = new ExpanderControlViewModel();
      expander.UserControlList = new CompositeCollection();
      string groupName = string.Empty;

      bool deviceSupportsPlayRingtone = CommonMethods.SelectedDevice.IsFeatureSupported(DeviceFeature.PlayRingtone);

      var devsettings = (from b in settingsInfo
                         group b by b.GroupName into g
                         select g).ToList();
      foreach (var devset in devsettings)
      {
        foreach (var settings in devset)
        {
          if (groupName != settings.GroupName)
          {
            if (_expanderControlList.Count > 0)
            {
              expander.UserControlList = compositeCollection;
              expander = new ExpanderControlViewModel();
              compositeCollection = new CompositeCollection();
            }
            var set = (from devsett in settingsInfo
                       where devsett.GroupName == settings.GroupName
                       select devsett).ToList();
            if (set.Count > 0)
            {
              expander = new ExpanderControlViewModel(settings);
              _expanderControlList.Add(expander);
            }
          }

          groupName = settings.GroupName;

          switch (settings.ControlType)
          {
            case ControlType.Button:
              compositeCollection.Add(new ButtonControlViewModel(settings));
              break;
            case ControlType.ComboBox:
            case ControlType.DropDown:
              compositeCollection.Add(new ComboBoxControlViewModel(settings, deviceSupportsPlayRingtone));
              break;
            case ControlType.Label:
              compositeCollection.Add(new StaticTextControlViewModel(settings));
              break;
            case ControlType.TextBox:
              compositeCollection.Add(new TextControlViewModel(settings));
              break;
            case ControlType.RadioButton:
            case ControlType.ToggleButton:
              compositeCollection.Add(new RadioButtonControlViewModel(settings));
              break;
            case ControlType.Unknown:
              break;
          }
        }
      }
      expander.UserControlList = compositeCollection;

      if (CommonMethods.SelectedDevice != null)
        DependencyFeatures.SetupControlDependencies(CommonMethods.SelectedDevice.DeviceId, settingsInfo, ExpanderControlList);
    }

    public void ShowProgress(FileUploadProgressEventArgs args)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;

      var result = (from list in MainWindowViewModel.AllFileUploadDetails
                    where list.DeviceId == args.DeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        result.Progress = args.Percentage;
        result.Message = args.Message;
        if (args.Status == UploadEventStatus.Completed)
        {
          MessageBoxService.ShowMessage("File uploaded successfully to device", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
          result.ProgressVisibility = false;
          ReloadSettings();
        }
        else if (args.Status == UploadEventStatus.Error)
        {
          MessageBoxService.ShowMessage("Error during file upload", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          result.ProgressVisibility = false;
        }
      }
      ShowFileUploadDetails(args.DeviceId);
    }

    public void ShowFileUploadDetails(int deviceId)
    {
      if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == deviceId)
      {
        var result = (from list in MainWindowViewModel.AllFileUploadDetails
                      where list.DeviceId == deviceId
                      select list).SingleOrDefault();
        if (result != null)
        {
          _deviceSettings.DeviceId = result.DeviceId;
          _deviceSettings.ProgressVisibility = result.ProgressVisibility;
          _deviceSettings.Progress = result.Progress;
          _deviceSettings.Message = result.Message;
        }
      }
    }

    public void ClickReadTimestamp()
    {
      if (CommonMethods.SelectedDevice != null)
      {
        try
        {
          IDevice device = CommonMethods.SelectedDevice;
          MessageBoxService.ShowMessage("Stored timestamp is: " + device.GetTimestamp(), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
          MessageBoxService.ShowMessage(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    public void ClickSetTimestamp()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      try
      {
        DateTime dt = DateTime.Now;
        DeviceStatus ret = DeviceStatus.SystemError;

        ret = device.SetTimestamp(_deviceSettings.TimestampVal);

        if (ret != DeviceStatus.ReturnOk)
        {
          MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
          MessageBoxService.ShowMessage("Timestamp set successfully in device", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void ClickSetWizardMode()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      try
      {
        JabraSDK.WizardMode selectedWizardMode = _deviceSettings.CurrentWizardMode;
        DeviceStatus ret = DeviceStatus.SystemError;

        ret = device.SetWizardMode(selectedWizardMode);

        if (ret != DeviceStatus.ReturnOk)
        {
          MessageBoxService.ShowMessage(device.GetErrorMessage(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
          MessageBoxService.ShowMessage("WizardMode set successfully in device", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }

    }

    public void ClickGetWizardMode()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      _deviceSettings.CurrentWizardMode = device.GetWizardMode();
      switch (_deviceSettings.CurrentWizardMode)
      {
        case JabraSDK.WizardMode.FullWizardMode:
          MessageBoxService.ShowMessage("WizardMode is set to: " + "Run full setup wizard on next power-on", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
          break;
        case JabraSDK.WizardMode.LimitedWizardMode:
          MessageBoxService.ShowMessage("WizardMode is set to: " + "Run limited setup wizard on next power-on", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
          break;
        case JabraSDK.WizardMode.NoWizardMode:
          MessageBoxService.ShowMessage("WizardMode is set to: " + "Do not run setup wizard on next power-on", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
          break;
        default:
          MessageBoxService.ShowMessage("BADVAL", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
          break;
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

    private class InitialArguments
    {
      public IDevice Device { get; set; }
      public List<ISetting> Setting { get; set; }
    }
  }
}
