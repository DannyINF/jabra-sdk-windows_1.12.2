using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Jabra_SDK_Demo.ViewModel
{
  class RemoteMMIv2ControlViewModel
  {
    #region Properties

    private RemoteMMIv2Control _remoteMMIv2Control;
    public RemoteMMIv2Control RemoteMMIv2Control
    {
      get { return _remoteMMIv2Control; }
      set
      {
        _remoteMMIv2Control = value;
        OnPropertyChanged("RemoteMMIv2Control");
      }
    }

    #endregion

    #region ICommand   

    private ICommand _setMmiCommand;
    public ICommand SetMmiCommand
    {
      get
      {
        return _setMmiCommand ?? (_setMmiCommand = new CommandHandler(() => SetMmiCommandAction(), true));
      }
    }

    private void SetMmiCommandAction()
    {
      UpdateUnwrittenRMMIv2Configuration(true);
      if (SpecialHandlers.UnwrittenRmmiv2Settings.Count > 0)
      {
        NativeCommonLibrary.SetResetRmmiv2Buttons(true, (ushort)CommonMethods.SelectedDevice.DeviceId);
        _remoteMMIv2Control.IsInFocus = CommonMethods.SelectedDevice.IsRemoteMmiv2InFocus((RemoteMmiType)_remoteMMIv2Control.TypeValue) ? "In focus" : "Not in focus";
      }
      else
      {
        if (_remoteMMIv2Control.EnableAppActions)
          MessageBoxService.ShowMessage("Please select Action, Priority and Function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else
          MessageBoxService.ShowMessage("Please select Action and Priority", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private ICommand _resetMmiCommand;
    public ICommand ResetMmiCommand
    {
      get
      {
        return _resetMmiCommand ?? (_resetMmiCommand = new CommandHandler(() => ResetMmiCommandAction(), true));
      }
    }

    private void ResetMmiCommandAction()
    {
      UpdateUnwrittenRMMIv2Configuration(false);
      if (SpecialHandlers.UnwrittenRmmiv2Settings.Count > 0)
      {
        NativeCommonLibrary.SetResetRmmiv2Buttons(false, (ushort)CommonMethods.SelectedDevice.DeviceId);
        _remoteMMIv2Control.IsInFocus = CommonMethods.SelectedDevice.IsRemoteMmiv2InFocus((RemoteMmiType)_remoteMMIv2Control.TypeValue) ? "In focus" : "Not in focus";
      }
      else
      {
       if (_remoteMMIv2Control.EnableAppActions)
        MessageBoxService.ShowMessage("Please select Action, Priority and Function", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        else
          MessageBoxService.ShowMessage("Please select Action and Priority", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private ICommand _setOutputCommand;
    public ICommand SetOutputCommand
    {
      get
      {
        return _setOutputCommand ?? (_setOutputCommand = new CommandHandler(() => SetOutputCommandAction(), true));
      }
    }

    private void SetOutputCommandAction()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      if (_remoteMMIv2Control.SequenceCurrentValue == _remoteMMIv2Control.Sequences.ElementAt(0).Key)
      {
        MessageBoxService.ShowMessage("Please select RGB sequence", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      else
      {
        var rmmiConfiguration = SpecialHandlers.Rmmiv2ConfiguredValues.FirstOrDefault(x => x.DeviceId == CommonMethods.SelectedDevice.DeviceId && x.Type == _remoteMMIv2Control.TypeValue);
        if (rmmiConfiguration != null)
        {
          rmmiConfiguration.RedOutputValue = _remoteMMIv2Control.RedOutputValue;
          rmmiConfiguration.GreenOutputValue = _remoteMMIv2Control.GreenOutputValue;
          rmmiConfiguration.BlueOutputValue = _remoteMMIv2Control.BlueOutputValue;
          rmmiConfiguration.SequenceId = _remoteMMIv2Control.SequenceCurrentValue;
          rmmiConfiguration.IsOutputConfigured = true;
        }

        NativeCommonLibrary.SetRGBOutput(CommonMethods.SelectedDevice, _remoteMMIv2Control.TypeValue, _remoteMMIv2Control.RedOutputValue, _remoteMMIv2Control.GreenOutputValue,
        _remoteMMIv2Control.BlueOutputValue, _remoteMMIv2Control.SequenceCurrentValue);
      }
    }

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
    }

    #endregion

    public RemoteMMIv2ControlViewModel()
    {

    }

    public RemoteMMIv2ControlViewModel(IRemoteMmiv2Configuration rmmiv2Setting)
    {
      Dictionary<int, string> rmmiv2Priorities = new Dictionary<int, string>();
      rmmiv2Priorities.Add(-1, "--Select--");
      foreach (var val in rmmiv2Setting.RemoteMmiPriority)
      {
        rmmiv2Priorities.Add(Convert.ToInt32(val.Key), val.Value);
      }

      Dictionary<int, string> rmmiv2Sequences = new Dictionary<int, string>();
      rmmiv2Sequences.Add(-1, "--Select--");
      foreach (var val in rmmiv2Setting.RemoteMmiSequence)
      {
        rmmiv2Sequences.Add(Convert.ToInt32(val.Key), val.Value);
      }

      _remoteMMIv2Control = new RemoteMMIv2Control
      {
        TypeName = rmmiv2Setting.Name,
        TypeValue = rmmiv2Setting.Type,
        Priorities = rmmiv2Priorities,
        PriorityCurrentValue = rmmiv2Priorities.ElementAt(0).Key,
        ControlEnabled = true,
        ControlCreated = true
      };

      // Enable app functions if input is configurable other than NONE
      if (rmmiv2Setting.RemoteMmiInputs.Count > 0 &&
          !rmmiv2Setting.RemoteMmiInputs.ContainsKey((int)RemoteMmiInput.MMI_ACTION_NONE) &&
          !rmmiv2Setting.RemoteMmiPriority.ContainsKey((int)RemoteMmiPriority.MMI_PRIORITY_NONE))
      {
        Dictionary<int, string> configList = new Dictionary<int, string>();
        configList.Add(0, "--Select--");
        configList.Add(1, "Open Browser");
        configList.Add(2, "Lock Screen");
        configList.Add(3, "Default Message");

        _remoteMMIv2Control.EnableAppActions = true;
        _remoteMMIv2Control.AppFunctions = configList;
        _remoteMMIv2Control.AppFunctionCurrentValue = configList.ElementAt(0).Value;
      }

      _remoteMMIv2Control.InputActions = new ObservableCollection<RemoteMMIv2Control.InputAction>();
      foreach (var val in rmmiv2Setting.RemoteMmiInputs)
      {
        _remoteMMIv2Control.InputActions.Add(new RemoteMMIv2Control.InputAction(Convert.ToInt32(val.Key), val.Value, false));
      }

      try
      {
        _remoteMMIv2Control.IsInFocus = CommonMethods.SelectedDevice.IsRemoteMmiv2InFocus((RemoteMmiType)rmmiv2Setting.Type) ? "In focus" : "Not in focus";
      }
      catch { }

      if (rmmiv2Setting.RemoteMmiInputs.Count > 0)
        _remoteMMIv2Control.EnableInput = true;

      if (rmmiv2Setting.RemoteMmiOutputs.Count > 0)
      {
        _remoteMMIv2Control.EnableOutput = true;
        _remoteMMIv2Control.Sequences = rmmiv2Sequences;
        _remoteMMIv2Control.SequenceCurrentValue = rmmiv2Sequences.ElementAt(0).Key;
        _remoteMMIv2Control.EnableRedOutput = rmmiv2Setting.RemoteMmiOutputs.Any(name => name.Equals("red", StringComparison.OrdinalIgnoreCase));
        _remoteMMIv2Control.EnableGreenOutput = rmmiv2Setting.RemoteMmiOutputs.Any(name => name.Equals("green", StringComparison.OrdinalIgnoreCase));
        _remoteMMIv2Control.EnableBlueOutput = rmmiv2Setting.RemoteMmiOutputs.Any(name => name.Equals("blue", StringComparison.OrdinalIgnoreCase));
      }

      // Update view with configured RMMI values
      if (CommonMethods.SelectedDevice != null)
      {
        var value = (from val in SpecialHandlers.Rmmiv2ConfiguredValues
                     where val.Type == rmmiv2Setting.Type
                     where val.DeviceId == CommonMethods.SelectedDevice.DeviceId
                     select val).FirstOrDefault();
        if (value != null)
        {
          var rmmiSetting = (from e in SpecialHandlers.Rmmiv2ConfiguredValues
                             where e.DeviceId == CommonMethods.SelectedDevice.DeviceId
                             where e.Type == rmmiv2Setting.Type
                             select e).FirstOrDefault();

          foreach (var item in _remoteMMIv2Control.InputActions)
          {
            if (rmmiSetting.InputActionId.Contains(item.Value))
              item.IsSelected = true;
          }

          if (value.IsOutputConfigured)
          {
            _remoteMMIv2Control.RedOutputValue = value.RedOutputValue;
            _remoteMMIv2Control.GreenOutputValue = value.GreenOutputValue;
            _remoteMMIv2Control.BlueOutputValue = value.BlueOutputValue;
            _remoteMMIv2Control.SequenceCurrentValue = value.SequenceId;
          }
          _remoteMMIv2Control.PriorityCurrentValue = value.PriorityId;
          _remoteMMIv2Control.AppFunctionCurrentValue = value.ConfiguredFunction;
        }
      }
    }

    public void UpdateUnwrittenRMMIv2Configuration(bool isSetRmmi)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      if (isSetRmmi)
      {
        if (_remoteMMIv2Control.PriorityCurrentValue != _remoteMMIv2Control.Priorities.ElementAt(0).Key &&
            _remoteMMIv2Control.InputActions.Any(x => x.IsSelected))
        {
          RemoteMMIv2Setting unwrittenRemoteMMIv2Setting = new RemoteMMIv2Setting()
          {
            DeviceId = CommonMethods.SelectedDevice.DeviceId,
            Type = _remoteMMIv2Control.TypeValue,
            InputActionId = new List<int>(),
            PriorityId = _remoteMMIv2Control.PriorityCurrentValue
          };

          if (_remoteMMIv2Control.EnableAppActions)
          {
            if (!_remoteMMIv2Control.AppFunctionCurrentValue.Equals(_remoteMMIv2Control.AppFunctions.ElementAt(0).Value))
            {
              unwrittenRemoteMMIv2Setting.ConfiguredFunction = _remoteMMIv2Control.AppFunctionCurrentValue;
              foreach (var inputAction in _remoteMMIv2Control.InputActions)
              {
                if (inputAction.IsSelected)
                  unwrittenRemoteMMIv2Setting.InputActionId.Add(inputAction.Value);
              }
              SpecialHandlers.UnwrittenRmmiv2Settings.Add(unwrittenRemoteMMIv2Setting);
            }
          }
          else
          {
            foreach (var inputAction in _remoteMMIv2Control.InputActions)
            {
              if (inputAction.IsSelected)
                unwrittenRemoteMMIv2Setting.InputActionId.Add(inputAction.Value);
            }
            SpecialHandlers.UnwrittenRmmiv2Settings.Add(unwrittenRemoteMMIv2Setting);
          }
        }
      }
      else
      {
        RemoteMMIv2Setting unwrittenRemoteMMIv2Setting = new RemoteMMIv2Setting()
        {
          DeviceId = CommonMethods.SelectedDevice.DeviceId,
          Type = _remoteMMIv2Control.TypeValue,
        };
        SpecialHandlers.UnwrittenRmmiv2Settings.Add(unwrittenRemoteMMIv2Setting);
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
    protected void OnPropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion
  }
}
