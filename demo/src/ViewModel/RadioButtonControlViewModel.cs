using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using RadioButtonControl = Jabra_SDK_Demo.Model.RadioButtonControl;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class RadioButtonControlViewModel : DisposeMethods
  {
    private RadioButtonControl _radioButtonControl;

    public RadioButtonControl RadioButtonControl
    {
      get { return _radioButtonControl; }
      set
      {
        _radioButtonControl = value;
        OnPropertyChanged("RadioButtonControl");
      }
    }


    public RadioButtonControlViewModel()
    {
    }

    public RadioButtonControlViewModel(ISetting setting)
    {
      Dictionary<int, string> settingValue = new Dictionary<int, string>();
      int settingCurrentValue = -1;
      string radOn = string.Empty;
      string radOff = string.Empty;
      bool radOnChecked = false;
      bool radOffChecked = false;
      var chk = (from vl in setting.SettingValues
                 where vl.Key == Convert.ToInt32(setting.CurrentValue)
                 select vl).SingleOrDefault();

      if (setting.SettingValues != null)
      {
        foreach (var val in setting.SettingValues)
        {
          settingValue.Add(Convert.ToInt32(val.Key), val.Value);
          if (val.Value.Trim().ToLower(CultureInfo.InvariantCulture) == "on")
          {
            radOn = val.Value;
            if (chk?.Value == val.Value)
            {
              radOnChecked = true;
            }
            else
            {
              radOffChecked = true;
            }
          }
          else
          {
            radOff = val.Value;
          }
        }
      }

      settingCurrentValue = Convert.ToInt32(setting.CurrentValue);
      bool requireSettingPadlock = setting.IsSettingProtectionEnabled ? setting.IsSettingProtected : false;
      _radioButtonControl = new RadioButtonControl
      {
        GuidValue = Guid.Parse(setting.Guid),
        GroupName = Guid.Parse(setting.Guid) + setting.GroupName + setting.SettingName + CommonMethods.SelectedDevice.DeviceId,//Future enhancement
        Label = setting.SettingName,
        SettingValues = settingValue,
        CurrentValue = settingCurrentValue,
        HelpText = setting.HelpText,
        ControlEnabled = !requireSettingPadlock,
        ControlCreated = true,
        RadioOn = radOn,
        RadioOnIsCheck = radOnChecked,
        RadioOff = radOff,
        RadioOffIsCheck = radOffChecked,
        Protected = requireSettingPadlock,
      };
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
