using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using Jabra_SDK_Demo.ViewModel;
using JabraSDK;

namespace Jabra_SDK_Demo.Helpers
{
  public static class NativeCommonLibrary
  {

    [DllImport("user32")]
    internal static extern void LockWorkStation();

    public static void UpdateSettingsToDevice(bool isTabChange)
    {
      if (SpecialHandlers.UnWrittenSettings.Count > 0)
      {
        IDevice unWrittenSettingsDevice = (from device in MainWindowViewModel.AvailableDevices
                                           where device.DeviceId == SpecialHandlers.UnWrittenSettingsDeviceId
                                           select device).FirstOrDefault();
        if (unWrittenSettingsDevice != null)
        {
          try
          {
            ObservableCollection<ExpanderControlViewModel> expanderControlList = MainWindowViewModel.ViewModelDeviceSettings.ExpanderControlList;
            ObservableCollection<SettingsInformation> settingsInformation = SpecialHandlers.SettingsInformation;
            List<ISetting> set = new List<ISetting>();
            foreach (var settings in SpecialHandlers.UnWrittenSettings)
            {
              int currentValue = 0;
              RadioButtonControlViewModel uiRadioButtonSetting = null;
              TextControlViewModel uiTextSetting = null;
              ButtonControlViewModel uiButtonSetting = null;
              ComboBoxControlViewModel uiComboBoxSetting = UserControlDetails.GetComboBoxControlInformations(settings.Guid, expanderControlList);

              if (uiComboBoxSetting != null)
              {
                currentValue = uiComboBoxSetting.ComboBoxControl.CurrentValue;
              }
              else
              {
                uiRadioButtonSetting = UserControlDetails.GetRadioButtonControlInformations(settings.Guid, expanderControlList);
                if (uiRadioButtonSetting == null)
                {
                  uiTextSetting = UserControlDetails.GetTextControlInformations(settings.Guid, expanderControlList);
                  if (uiTextSetting == null)
                  {
                    uiButtonSetting = UserControlDetails.GetButtonControlInformations(settings.Guid, expanderControlList);
                  }
                }
              }

              var settingsInformations = (from det in settingsInformation
                                          where det.DeviceId == unWrittenSettingsDevice.DeviceId
                                          from sett in det.DeviceSettings
                                          where sett.Guid.ToLower() == settings.Guid.ToString().ToLower()
                                          select sett).FirstOrDefault();

              if (uiRadioButtonSetting != null && settingsInformations.SettingValues != null && settingsInformations.SettingValues.Count > 0)
              {
                string selected = uiRadioButtonSetting.RadioButtonControl.RadioOnIsCheck ? uiRadioButtonSetting.RadioButtonControl.RadioOn : uiRadioButtonSetting.RadioButtonControl.RadioOff;
                var key = (from val in uiRadioButtonSetting.RadioButtonControl.SettingValues
                           where val.Value.ToLower() == selected.ToLower()
                           select val).SingleOrDefault();

                currentValue = key.Key;
              }
              if (uiTextSetting != null)
              {
                settingsInformations.CurrentValue = uiTextSetting.TextControl.CurrentValue;
              }
              else if (uiButtonSetting != null)
              {
                settingsInformations.CurrentValue = uiButtonSetting.ButtonControl.CurrentValue;
              }
              else
                settingsInformations.CurrentValue = currentValue.ToString();
              set.Add(settingsInformations);
            }


            DeviceStatus ret = unWrittenSettingsDevice.SetSettings(set);

            if (ret == DeviceStatus.ProtectedSettingWrite)
            {
              MessageBoxService.ShowMessage("Failed to apply settings to the device \nFew settings are password protected on the device", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (ret != DeviceStatus.ReturnOk && ret != DeviceStatus.DeviceRebooted)
            {
              List<IFailedSetting> failedSettings = unWrittenSettingsDevice.GetFailedSettings();
              string message = String.Empty;
              int count = 1;
              foreach (var value in failedSettings)
              {
                message += count + ". " + value.Name + "\n";
                count++;
              }
              MessageBoxService.ShowMessage("Failed to apply below settings to the device - \n" + message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
          catch (Exception ex)
          {
            MessageBoxService.ShowMessage("Exception while applying settings to the device\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
          finally
          {
            SpecialHandlers.UnWrittenSettings.Clear();
            SpecialHandlers.UnWrittenSettingsDeviceId = -1;
            SpecialHandlers.EnableDisableButton = false;
            if (isTabChange)
            {
              SpecialHandlers.DisplayFisrtTab = true;
            }
          }
        }
      }
    }

    public static void SetResetConfigurableButtons(bool isSetConfigurableButton, ushort deviceId, bool isWindowClosing)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      bool isActionChanged = false;
      if (SpecialHandlers.ButtonConfigValues.Count > 0 & isSetConfigurableButton)
      {
        foreach (var values in SpecialHandlers.UnwrittenConfigurableButtons)
        {
          var set = (from e in SpecialHandlers.ButtonConfigValues
                     where e.DeviceId == values.DeviceId
                     where e.Id == values.Id
                     where e.ButtonEventId == values.ButtonEventId
                     select e).FirstOrDefault();
          if (set != null)
          {
            set.ConfigurationActionValue = values.ConfigurationActionValue;
            isActionChanged = true;
          }
        }
        foreach (var set in SpecialHandlers.ButtonConfigValues)
        {
          var removeSet = (from e in SpecialHandlers.UnwrittenConfigurableButtons
                           where e.DeviceId == set.DeviceId
                           where e.Id == set.Id
                           where e.ButtonEventId == set.ButtonEventId
                           select e).FirstOrDefault();
          SpecialHandlers.UnwrittenConfigurableButtons.Remove(removeSet);
        }
      }
      else
      {
        if (!isSetConfigurableButton && !isWindowClosing)
        {
          var result = (from e in SpecialHandlers.UnwrittenConfigurableButtons
                        where e.DeviceId == deviceId
                        select e).ToList();
          foreach (var set in result)
          {
            var removeSet = (from e in SpecialHandlers.ButtonConfigValues
                             where e.DeviceId == set.DeviceId
                             where e.Id == set.Id
                             select e).FirstOrDefault();
            if (removeSet == null)
            {
              SpecialHandlers.UnwrittenConfigurableButtons.Remove(set);
            }
          }
        }
      }
      if (isWindowClosing)
      {
        var result = (from e in SpecialHandlers.ButtonConfigValues
                      where e.DeviceId == deviceId
                      select e).ToList();
        foreach (var val in result)
        {
          SpecialHandlers.UnwrittenConfigurableButtons.Add(val);
        }
      }
      if (SpecialHandlers.UnwrittenConfigurableButtons.Count > 0)
      {
        try
        {
          List<IConfigurableButton> configurationEvents = new List<IConfigurableButton>();
          for (int i = 0; i < SpecialHandlers.UnwrittenConfigurableButtons.Count; i++)
          {
            //configurationEvents.Add(new ConfigurableButton() {  = SpecialHandlers.UnwrittenConfigurableButtons[i].ButtonTypeKey, ButtonEventTypeKey = SpecialHandlers.UnwrittenConfigurableButtons[i].ButtonEventKey });
            //todo to be handled for multiple Actions
            Dictionary<int, string> actions = new Dictionary<int, string>();
            actions.Add(SpecialHandlers.UnwrittenConfigurableButtons[i].ButtonEventId, SpecialHandlers.UnwrittenConfigurableButtons[i].ButtonEventName);
            configurationEvents.Add(new ConfigurableButton { Id = SpecialHandlers.UnwrittenConfigurableButtons[i].Id, Actions = actions });

          }

          if (isSetConfigurableButton)
          {
            try
            {
              device.SetConfigurableButtons(configurationEvents);
              foreach (var value in SpecialHandlers.UnwrittenConfigurableButtons)
              {
                SpecialHandlers.ButtonConfigValues.Add(value);
              }
            }
            catch (Exception ex)
            {
              MessageBoxService.ShowMessage("Failed to Update Remote MMI.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
          else
          {
            if (SpecialHandlers.ButtonConfigValues.Count > 0)
            {
              if (isWindowClosing)
              {
                try
                {
                  device.ResetConfigurableButtons(configurationEvents);
                }
                catch (Exception ex)
                {
                  MessageBoxService.ShowMessage("Failed to clear Remote MMI.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              }
              else
              {
                try
                {
                  device.ResetConfigurableButtons(configurationEvents);
                  foreach (var values in SpecialHandlers.UnwrittenConfigurableButtons)
                  {
                    var value = (from e in SpecialHandlers.ButtonConfigValues
                                 where e.DeviceId == values.DeviceId
                                 where e.Id == values.Id
                                 where e.ButtonEventId == values.ButtonEventId
                                 select e).FirstOrDefault();
                    SpecialHandlers.ButtonConfigValues.Remove(value);
                  }
                }
                catch (Exception ex)
                {
                  MessageBoxService.ShowMessage("Failed to clear Remote MMI.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              }
            }
            else
            {
              MessageBoxService.ShowMessage("Please Configure the Remote MMI", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
        }
        catch (Exception )
        {

        }
        finally
        {
          SpecialHandlers.UnwrittenConfigurableButtons.Clear();
          SpecialHandlers.DisplayFisrtTab = true;
        }
      }
      else
      {
        if (!isActionChanged)
        {
          MessageBoxService.ShowMessage("Please Configure the Remote MMI", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
          SpecialHandlers.DisplayFisrtTab = true;
      }
    }

    public static void DisplayButtonConfig(ButtonConfigurationEventArgs buttonConfigurationEventArgs)
    {
      try
      {
        string value = (from e in SpecialHandlers.ButtonConfigValues
                        where e.DeviceId == buttonConfigurationEventArgs.DeviceId
                        where e.ButtonEventId == buttonConfigurationEventArgs.ButtonEventTypeKey
                        where e.Id == buttonConfigurationEventArgs.ButtonTypekey
                        select e.ConfigurationActionValue).FirstOrDefault();
        if (!string.IsNullOrEmpty(value))
        {
          switch (value.ToString())
          {
            case "Lock Screen":
              LockWorkStation();
              break;
            case "Open Browser":
              System.Diagnostics.Process.Start("IExplore.exe");
              break;
            case "Default Message":
              MessageBox.Show("Default Message");
              break;
            default:
              break;
          }
        }
      }
      catch (Exception)
      {
        MessageBoxService.ShowMessage($"Error while performing configured action", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        SpecialHandlers.DisplayFisrtTab = true;
      }
    }

    public static void SetResetRmmiv2Buttons(bool isSetRmmiv2Button, ushort deviceId)
    {
      IDevice device = CommonMethods.SelectedDevice;
      if (SpecialHandlers.UnwrittenRmmiv2Settings.Count > 0)
      {
        try
        {
          if (isSetRmmiv2Button)
          {
            try
            {
              foreach (var remoteMMIv2Setting in SpecialHandlers.UnwrittenRmmiv2Settings)
              {
                List<RemoteMmiInput> remoteMmiInputs = remoteMMIv2Setting.InputActionId.Select(action => (RemoteMmiInput)action).ToList();
                device.GetRemoteMmiv2Focus((RemoteMmiType)remoteMMIv2Setting.Type, remoteMmiInputs, (RemoteMmiPriority)remoteMMIv2Setting.PriorityId);

                // Clear configured RMMI Actions as it will be updated(based on IsSelected) for change in any action  
                var rmmiConfiguration = SpecialHandlers.Rmmiv2ConfiguredValues.FirstOrDefault(x => x.DeviceId == deviceId && x.Type == remoteMMIv2Setting.Type);
                if (rmmiConfiguration != null)
                  SpecialHandlers.Rmmiv2ConfiguredValues.Remove(rmmiConfiguration);

                SpecialHandlers.Rmmiv2ConfiguredValues.Add(remoteMMIv2Setting);
              }
            }
            catch (Exception ex)
            {
              MessageBoxService.ShowMessage("Failed to update Remote MMIv2.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
          else
          {
            try
            {
              foreach (var remoteMMIv2Setting in SpecialHandlers.UnwrittenRmmiv2Settings)
              {
                device.ReleaseRemoteMmiv2Focus((RemoteMmiType)remoteMMIv2Setting.Type);
                var configuredRmmi = (from e in SpecialHandlers.Rmmiv2ConfiguredValues
                                      where e.DeviceId == remoteMMIv2Setting.DeviceId
                                      where e.Type == remoteMMIv2Setting.Type
                                      select e).FirstOrDefault();

                if (configuredRmmi != null)
                  SpecialHandlers.Rmmiv2ConfiguredValues.Remove(configuredRmmi);
              }
            }
            catch (Exception ex)
            {
              MessageBoxService.ShowMessage("Failed to clear Remote MMIv2.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
        }
        catch { }
        finally
        {
          SpecialHandlers.UnwrittenRmmiv2Settings.Clear();
          SpecialHandlers.DisplayFisrtTab = true;
        }
      }
    }

    public static void PerformRMMIv2Function(RemoteMmiv2EventArgs rmmiv2EventArgs)
    {
      try
      {
        string value = (from e in SpecialHandlers.Rmmiv2ConfiguredValues
                        where e.DeviceId == rmmiv2EventArgs.DeviceId
                        where e.Type == Convert.ToInt32(rmmiv2EventArgs.Type)
                        where e.InputActionId.Contains(Convert.ToInt32(rmmiv2EventArgs.Action))
                        select e.ConfiguredFunction).FirstOrDefault();
        if (!string.IsNullOrEmpty(value))
        {
          switch (value.ToString())
          {
            case "Lock Screen":
              LockWorkStation();
              break;
            case "Open Browser":
              System.Diagnostics.Process.Start("IExplore.exe");
              break;
            case "Default Message":
              MessageBox.Show("Default Message");
              break;
            default:
              break;
          }
        }
      }
      catch (Exception)
      {
        MessageBoxService.ShowMessage($"Error while performing configured function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public static void SetRGBOutput(IDevice device, int rmmiType, int red, int green, int blue, int sequence)
    {
      try
      {
        RemoteMmiActionOutputInfo remoteMmiActionOutputInfo = new RemoteMmiActionOutputInfo()
        {
          Red = Convert.ToByte(red),
          Green = Convert.ToByte(green),
          Blue = Convert.ToByte(blue),
          Sequence = (RemoteMmiSequence)sequence
        };
        device.SetRemoteMmiv2Action((RemoteMmiType)rmmiType, remoteMmiActionOutputInfo);
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Failed to update Remote MMIv2 Output.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
