using System.Linq;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Model;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ButtonTakeOver = Jabra_SDK_Demo.Model.ButtonTakeOver;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class ButtonTakeOverViewModel : ViewModelBase
  {

    #region Properties

    private ButtonTakeOver _buttonTakeOver;
    public ButtonTakeOver ButtonTakeOver
    {
      get { return _buttonTakeOver; }
      set
      {
        _buttonTakeOver = value;
        OnPropertyChanged("ButtonTakeOver");
      }
    }

    private ObservableCollection<RemoteMMIControlViewModel> _remoteMmiControlControlList = new ObservableCollection<RemoteMMIControlViewModel>();
    public ObservableCollection<RemoteMMIControlViewModel> RemoteMmiControlControlList
    {
      get { return _remoteMmiControlControlList; }
      set
      {
        _remoteMmiControlControlList = value;
        OnPropertyChanged("RemoteMmiControlControlList");
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
      if (CommonMethods.SelectedDevice != null && SpecialHandlers.UnwrittenConfigurableButtons != null)
      {
        NativeCommonLibrary.SetResetConfigurableButtons(true, (ushort)CommonMethods.SelectedDevice.DeviceId, false);
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

    private ICommand _windowLoaded;
    public ICommand WindowLoaded
    {
      get
      {
        return _windowLoaded ?? (_windowLoaded = new CommandHandler(() => WindowLoadedAction(), true));
      }
    }

    #endregion

    #region Constructor

    public ButtonTakeOverViewModel()
    {
    }

    public ButtonTakeOverViewModel(IDevice device)
    {
      _buttonTakeOver = new ButtonTakeOver();
      _buttonTakeOver.ButtonConfigurationSupported = device.IsButtonConfigurationSupported;
      if (_buttonTakeOver.ButtonConfigurationSupported)
      {
        List<IConfigurableButton> configurableButtons = device.GetConfigurableButtons();
        SpecialHandlers.ConfigurableButton.Add(configurableButtons);
        ShowButtonSettings(configurableButtons);
      }
    }

    #endregion

    private void WindowLoadedAction()
    {
    }

    private void ResetMmiCommandAction()
    {
      if (SpecialHandlers.UnwrittenConfigurableButtons != null && CommonMethods.SelectedDevice != null)
      {
        NativeCommonLibrary.SetResetConfigurableButtons(false, (ushort)CommonMethods.SelectedDevice.DeviceId, false);
      }
    }

    private void ShowButtonSettings(List<IConfigurableButton> configurableButtons)
    {
      foreach (var value in configurableButtons)
      {
        _remoteMmiControlControlList.Add(new RemoteMMIControlViewModel(value));
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
