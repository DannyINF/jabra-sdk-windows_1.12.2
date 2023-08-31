using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class ButtonControlViewModel : DisposeMethods
  {
    private ButtonControl _buttonControl;

    public ButtonControl ButtonControl
    {
      get { return _buttonControl; }
      set
      {
        _buttonControl = value;
        OnPropertyChanged("ButtonControl");
      }
    }

    private ICommand _clickCommand;
    public ICommand ClickCommand
    {
      get
      {
        return _clickCommand ?? (_clickCommand = new CommandHandler(() => ClickAction(), true));
      }
    }

    public void ClickAction()
    {
      var data = (from set in SpecialHandlers.UnWrittenSettings
                  where set.Guid == _buttonControl.GuidValue
                  select set).FirstOrDefault();
      if (data == null)
      {
        UnWrittenSettings settings = new UnWrittenSettings();
        settings.Guid = _buttonControl.GuidValue;
        SpecialHandlers.UnWrittenSettings.Add(settings);
        SpecialHandlers.UnWrittenSettingsDeviceId = CommonMethods.SelectedDevice != null ? CommonMethods.SelectedDevice.DeviceId : -1;
      }
      NativeCommonLibrary.UpdateSettingsToDevice(false);
    }

    public ButtonControlViewModel()
    {

    }

    public ButtonControlViewModel(ISetting setting)
    {
      bool requireSettingPadlock = setting.IsSettingProtectionEnabled ? setting.IsSettingProtected : false;
      _buttonControl = new ButtonControl
      {
        GuidValue = Guid.Parse(setting.Guid),
        Label = setting.SettingName,
        CurrentValue = setting.CurrentValue,
        HelpText = setting.HelpText,
        ControlEnabled = !requireSettingPadlock,
        Protected = requireSettingPadlock
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
