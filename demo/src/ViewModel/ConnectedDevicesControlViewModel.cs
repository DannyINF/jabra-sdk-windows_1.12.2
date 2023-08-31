using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using JabraSDK;
using System.Linq;
using ConnectedDevicesControl = Jabra_SDK_Demo.Model.ConnectedDevicesControl;

namespace Jabra_SDK_Demo.ViewModel
{
  public class ConnectedDevicesControlViewModel : ViewModelBase
  {
    private ConnectedDevicesControl _connectedDevicesControl;
    public ConnectedDevicesControl ConnectedDevicesControl
    {
      get { return _connectedDevicesControl; }
      set
      {
        _connectedDevicesControl = value;
        OnPropertyChanged("ConnectedDevicesControl");
      }
    }

    private ICommand _clickCommand;
    public ICommand ClickCommand
    {
      get
      {
        return _clickCommand ?? (_clickCommand = new CommandHandler(() => ClickAction(), _canExecute));
      }
    }
    private bool _canExecute;

    public void ClickAction()
    {
      if (SpecialHandlers.UnWrittenSettings.Count > 0)
      {
        if (SpecialHandlers.EnableDisableButton)
        {
          if (MessageBoxService.AskForConfirmation("Apply device settings before exiting?", "Apply Changes?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
          {
            NativeCommonLibrary.UpdateSettingsToDevice(true);
          }
        }
        SpecialHandlers.UnWrittenSettings.Clear();
        SpecialHandlers.UnWrittenSettingsDeviceId = -1;
      }
      MainWindowViewModel.SelectedUcConnectedDevicesControl.BackgroundColor = Brushes.White;
      ConnectedDevicesControl.BackgroundColor = Brushes.Gray;
      MainWindowViewModel.SelectedUcConnectedDevicesControl = ConnectedDevicesControl;
      IDevice device = MainWindowViewModel.AvailableDevices.FirstOrDefault(s => s.DeviceId == ConnectedDevicesControl.DeviceId);
      CommonMethods.SelectedDevice = device;
      MainWindowViewModel.ViewModelRcc.ShowRccSettings(CommonMethods.SelectedDevice.DeviceId);
      MainWindowViewModel.ViewModelDeviceInformation.ShowDeviceDetails(CommonMethods.SelectedDevice);
      MainWindowViewModel.ViewModelBtPairing.ShowPairingDetails(CommonMethods.SelectedDevice);
      SpecialHandlers.DisplayFisrtTab = true;
      MainWindowViewModel.ViewModelDeviceSettings = null;
      MainWindowViewModel.DeviceSettingsViewModel = null;
    }

    public ConnectedDevicesControlViewModel()
    {
    }
    internal ConnectedDevicesControlViewModel(IDevice device)
    {
      _canExecute = true;
      string imageSourcePath = string.Empty;
      try
      {
        imageSourcePath = device.GetDeviceImageThumbnailPath();
      }
      catch { }
      _connectedDevicesControl = new ConnectedDevicesControl() { DeviceId = device.DeviceId, DeviceName = device.Name, VersionNumber = null, ImageSource = CommonMethods.SetThumbnailImageSource(imageSourcePath) };
      Thread.Sleep(100);
    }

    #region INotifyPropertyChanged Members

#pragma warning disable CS0108 // 'ConnectedDevicesControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'ConnectedDevicesControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

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
