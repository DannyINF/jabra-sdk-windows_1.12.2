using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using System.Windows;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class ComboBoxControlViewModel : DisposeMethods
  {
    private ComboBoxControl _comboBoxControl;

    public ComboBoxControl ComboBoxControl
    {
      get { return _comboBoxControl; }
      set
      {
        _comboBoxControl = value;
        OnPropertyChanged("ComboBoxControl");
      }
    }

    private ICommand _invokeCmd;
    public ICommand InvokeCmd
    {
      get
      {
        return _invokeCmd ?? (_invokeCmd = new CommandHandler(() => InvokeSetting(), true));
      }
    }

    public void InvokeSetting()
    {
      try
      {
        IDevice device = CommonMethods.SelectedDevice;
        string ringtoneTypeSettingValue = string.Empty;
        string ringtoneVolumeSettingValue = string.Empty;

        var playringtoneGuidPair = DeviceSettingsGUIDHelper.PlayRingtoneGuidPair;

        string ringtoneVolumeGuid = string.Empty;
        // Determine the setting played - ringtone type or ringtone volume
        if (playringtoneGuidPair.TryGetValue(_comboBoxControl.GuidValue.ToString().ToUpper(), out ringtoneVolumeGuid))
        {
          // Ringtone type setting is played
          ringtoneTypeSettingValue = _comboBoxControl.CurrentValue.ToString();

          // Check if ringtone volume is changed in UI - gets added in unwritten settings
          var ringtoneVolumeSetting = SpecialHandlers.UnWrittenSettings.FirstOrDefault(setting => setting.Guid == Guid.Parse(ringtoneVolumeGuid.ToUpper()));
          if (ringtoneVolumeSetting != null)
          {
            // ringtone volume is changed- get changed value from UI control
            ComboBoxControlViewModel uiComboBoxSetting = UserControlDetails.GetComboBoxControlInformations(Guid.Parse(ringtoneVolumeGuid), MainWindowViewModel.ViewModelDeviceSettings.ExpanderControlList);
            if (uiComboBoxSetting != null)
            {
              ringtoneVolumeSettingValue = uiComboBoxSetting.ComboBoxControl.CurrentValue.ToString();
            }
          }
          else
          {
            // ringtone volume not changed- consider current value
            ringtoneVolumeSettingValue = DeviceSettingsGUIDHelper.GetSettingInformationByGuid(ringtoneVolumeGuid).CurrentValue;
          }
        }
        else
        {
          // Ringtone volume setting is played          
          var ringtoneTypeGuid = playringtoneGuidPair.FirstOrDefault(x => x.Value == _comboBoxControl.GuidValue.ToString().ToUpper()).Key;

          // Check if ringtone type is changed in UI - gets added in unwritten settings
          var ringtoneTypeSetting = SpecialHandlers.UnWrittenSettings.FirstOrDefault(setting => setting.Guid == Guid.Parse(ringtoneTypeGuid.ToUpper()));
          if (ringtoneTypeSetting != null)
          {
            // ringtone type is changed- get changed value from UI control
            ComboBoxControlViewModel uiComboBoxSetting = UserControlDetails.GetComboBoxControlInformations(Guid.Parse(ringtoneTypeGuid), MainWindowViewModel.ViewModelDeviceSettings.ExpanderControlList);
            if (uiComboBoxSetting != null)
            {
              ringtoneTypeSettingValue = uiComboBoxSetting.ComboBoxControl.CurrentValue.ToString();
            }
          }
          else
          {
            // ringtone type not changed- consider current value
            ringtoneTypeSettingValue = DeviceSettingsGUIDHelper.GetSettingInformationByGuid(ringtoneTypeGuid).CurrentValue;
          }

          ringtoneVolumeSettingValue = _comboBoxControl.CurrentValue.ToString();
        }
        device.PlayRingtone(Convert.ToByte(ringtoneVolumeSettingValue), Convert.ToByte(ringtoneTypeSettingValue));
      }
      catch
      {
        MessageBoxService.ShowMessage("Error in playing ringtone", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public ComboBoxControlViewModel()
    {
    }

    public ComboBoxControlViewModel(ISetting setting, bool deviceSupportsPlayRingtone)
    {
      Dictionary<int, string> settingValue = new Dictionary<int, string>();

      if (setting.SettingValues != null)
      {
        foreach (var val in setting.SettingValues)
        {
          settingValue.Add(Convert.ToInt32(val.Key), val.Value);
        }
      }
      bool requireSettingPadlock = setting.IsSettingProtectionEnabled ? setting.IsSettingProtected : false;
      _comboBoxControl = new ComboBoxControl
      {
        GuidValue = Guid.Parse(setting.Guid),
        Label = setting.SettingName,
        SettingValues = settingValue,
        CurrentValue = Convert.ToInt32(setting.CurrentValue),
        HelpText = setting.HelpText,
        ControlEnabled = !requireSettingPadlock,
        ControlCreated = true,
        CanInvoke = deviceSupportsPlayRingtone ? IsPlayRingtoneSupportedSetting(setting.Guid) : false,
        Protected = requireSettingPadlock,
      };
    }

    public bool IsPlayRingtoneSupportedSetting(string guid)
    {
      var playRingtoneGuidPair = DeviceSettingsGUIDHelper.PlayRingtoneGuidPair;
      return playRingtoneGuidPair.ContainsKey(guid) || playRingtoneGuidPair.ContainsValue(guid);
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
