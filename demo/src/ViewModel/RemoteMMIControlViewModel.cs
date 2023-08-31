using System;
using System.ComponentModel;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using System.Collections.Generic;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using System.Windows;
using System.Linq;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class RemoteMMIControlViewModel : DisposeMethods
  {
    #region Properties

    private RemoteMMIControl _remoteMMIControl;
    public RemoteMMIControl RemoteMMIControl
    {
      get { return _remoteMMIControl; }
      set
      {
        _remoteMMIControl = value;
        OnPropertyChanged("RemoteMMIControl");
      }
    }

    #endregion

    #region ICommand   

    private ICommand _clickMMICheckBox;
    public ICommand ClickMMICheckBox
    {
      get
      {
        return _clickMMICheckBox ?? (_clickMMICheckBox = new CommandHandlerArguments(param => ClickMMICheckBoxCommandHandler(), true));
      }
    }

    #endregion

    #region Constructor

    public RemoteMMIControlViewModel()
    {

    }

    public RemoteMMIControlViewModel(IConfigurableButton configurableButton)
    {
      Dictionary<int, string> settingValue = new Dictionary<int, string>();

      if (configurableButton.Actions != null)
      {
        settingValue.Add(-1, "--Select--");
        foreach (var val in configurableButton.Actions)
        {
          settingValue.Add(Convert.ToInt32(val.Key), val.Value);
        }
      }

      Dictionary<int, string> ConfigList = new Dictionary<int, string>();
      ConfigList.Add(0, "--Select--");
      ConfigList.Add(1, "Open Browser");
      ConfigList.Add(2, "Lock Screen");
      ConfigList.Add(3, "Default Message");
      _remoteMMIControl = new RemoteMMIControl
      {
        Label = configurableButton.Name,
        LabelCurrentValue = configurableButton.Id,
        ButtonValues = settingValue,
        CurrentValue = settingValue.ElementAt(0).Key,
        ButtonConfig = ConfigList,
        ButtonConfigCurrentValue = ConfigList[0],
        ControlEnabled = true,
        ControlCreated = true

      };
      if (CommonMethods.SelectedDevice != null)
      {
        var value = (from val in SpecialHandlers.ButtonConfigValues
                     where val.Id == configurableButton.Id
                     where val.DeviceId == CommonMethods.SelectedDevice.DeviceId
                     select val).FirstOrDefault();
        if (value != null)
        {
          _remoteMMIControl.ButtonConfigCurrentValue = value.ConfigurationActionValue;
          _remoteMMIControl.CurrentValue = value.ButtonEventId;
        }
      }
    }

    #endregion

    public void ClickMMICheckBoxCommandHandler()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      UnwrittenConfigurableButtons focus = new UnwrittenConfigurableButtons();

      if (_remoteMMIControl.IsCheckBoxChecked)
      {
        if (_remoteMMIControl.ButtonConfigCurrentValue != _remoteMMIControl.ButtonConfig[0] && _remoteMMIControl.CurrentValue != _remoteMMIControl.ButtonValues.ElementAt(0).Key)
        {
          focus.Id = _remoteMMIControl.LabelCurrentValue;
          focus.Name = _remoteMMIControl.Label;
          focus.ButtonEventId = Convert.ToUInt16(_remoteMMIControl.CurrentValue);
          focus.ConfigurationActionValue = _remoteMMIControl.ButtonConfigCurrentValue;
          focus.DeviceId = CommonMethods.SelectedDevice.DeviceId;
          if (focus.ConfigurationActionValue != _remoteMMIControl.ButtonConfig[0])
          {
            var values = (from e in SpecialHandlers.UnwrittenConfigurableButtons
                          where e.DeviceId == focus.DeviceId
                          where e.Id == focus.Id
                          where e.ButtonEventId == focus.ButtonEventId
                          select e).FirstOrDefault();
            if (values == null)
            {
              SpecialHandlers.UnwrittenConfigurableButtons.Add(focus);
            }
          }
        }
        else
        {
          MessageBoxService.ShowMessage("Please select Event,Action", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          _remoteMMIControl.IsCheckBoxChecked = false;
        }
      }
      else
      {
        var values = (from e in SpecialHandlers.UnwrittenConfigurableButtons
                      where e.DeviceId == CommonMethods.SelectedDevice.DeviceId
                      where e.Id == _remoteMMIControl.LabelCurrentValue
                      where e.ButtonEventId == _remoteMMIControl.CurrentValue
                      select e).FirstOrDefault();
        SpecialHandlers.UnwrittenConfigurableButtons.Remove(values);
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
