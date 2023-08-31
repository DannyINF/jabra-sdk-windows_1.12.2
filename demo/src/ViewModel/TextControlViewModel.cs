using System;
using System.ComponentModel;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class TextControlViewModel : DisposeMethods
  {
    private TextControl _textControl;

    public TextControl TextControl
    {
      get { return _textControl; }
      set
      {
        _textControl = value;
        OnPropertyChanged("TextControl");
      }
    }

    public TextControlViewModel()
    {
    }

    public TextControlViewModel(ISetting setting)
    {
      bool requireSettingPadlock = setting.IsSettingProtectionEnabled ? setting.IsSettingProtected : false;
      _textControl = new TextControl
      {
        GuidValue = Guid.Parse(setting.Guid),
        Label = setting.SettingName,
        CurrentValue = setting.CurrentValue,
        HelpText = setting.HelpText,
        MinLength = setting.ValidationRules.MinLength,
        MaxLength = setting.ValidationRules.MaxLength,
        RegexPattern = @setting.ValidationRules.RegexExp,
        RegexPatternErrorMessage = @setting.ValidationRules.ErrorMessage,
        ControlEnabled = !requireSettingPadlock,
        ControlCreated = true,
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
