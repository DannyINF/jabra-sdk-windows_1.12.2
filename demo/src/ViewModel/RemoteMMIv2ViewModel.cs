using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Helpers;
using JabraSDK;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Jabra_SDK_Demo.Model;
using System.Linq;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;

namespace Jabra_SDK_Demo.ViewModel
{
  internal class RemoteMMIv2ViewModel : ViewModelBase
  {
    private RemoteMMIv2 _remoteMMIv2;
    public RemoteMMIv2 RemoteMMIv2
    {
      get { return _remoteMMIv2; }
      set
      {
        _remoteMMIv2 = value;
        OnPropertyChanged("RemoteMMIv2");
      }
    }

    private ObservableCollection<RemoteMMIv2ControlViewModel> _remoteMmiv2ControlList = new ObservableCollection<RemoteMMIv2ControlViewModel>();
    public ObservableCollection<RemoteMMIv2ControlViewModel> RemoteMmiv2ControlList
    {
      get { return _remoteMmiv2ControlList; }
      set
      {
        _remoteMmiv2ControlList = value;
        OnPropertyChanged("RemoteMmiv2ControlList");
      }
    }

    private ICommand _clickClearCommand;
    public ICommand ClickClearCommand
    {
      get
      {
        return _clickClearCommand ?? (_clickClearCommand = new CommandHandler(() => ClickClearAction(), true));
      }
    }

    public RemoteMMIv2ViewModel()
    {
    }

    public RemoteMMIv2ViewModel(IDevice device)
    {
      _remoteMMIv2 = new RemoteMMIv2();
      _remoteMMIv2.RemoteMMIv2Supported = device.IsFeatureSupported(DeviceFeature.RemoteMMIv2);
      if (_remoteMMIv2.RemoteMMIv2Supported)
      {
        List<IRemoteMmiv2Configuration> rmmiv2Buttons = device.GetRemoteMmiv2Configurations();
        _remoteMMIv2.RMMiv2Data = new ObservableCollection<string>();
        foreach (var value in rmmiv2Buttons)
        {
          _remoteMmiv2ControlList.Add(new RemoteMMIv2ControlViewModel(value));
        }

        ShowRemoteMMIV2Settings(device.DeviceId);
      }
    }

    public void ShowRemoteMMIV2Settings(int deviceId)
    {
      if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == deviceId)
      {
        var result = (from list in MainWindowViewModel.AllRemoteMMIv2Details
                      where list.DeviceId == deviceId
                      select list);
        foreach (var rmmiEvent in result)
        {
          _remoteMMIv2.RMMiv2Data?.Insert(0, rmmiEvent.Data);
        }
      }
    }

    public void UpdateRemoteMMIv2Data(RemoteMmiv2EventArgs eventArgs)
    {
      try
      {
        if (CommonMethods.SelectedDevice == null)
        {
          return;
        }
        IDevice device = CommonMethods.SelectedDevice;
        string message = CommonMethods.ConvertToHex(eventArgs.DeviceId, "X2") + ": " + eventArgs.Type.ToString() + ", " + eventArgs.Action.ToString();
        RemoteMMIv2Details remoteMMIv2Details = new RemoteMMIv2Details()
        {
          DeviceId = eventArgs.DeviceId,
          Data = message
        };
        MainWindowViewModel.AllRemoteMMIv2Details.Add(remoteMMIv2Details);
        _remoteMMIv2.RMMiv2Data?.Insert(0, message);
      }
      catch { }
    }

    private void ClickClearAction()
    {
      _remoteMMIv2.RMMiv2Data.Clear();
      IDevice device = CommonMethods.SelectedDevice;
      var rmmiDetails = (from e in MainWindowViewModel.AllRemoteMMIv2Details
                         where e.DeviceId == device.DeviceId
                         select e).ToList();
      foreach (var rmmiData in rmmiDetails)
      {
        MainWindowViewModel.AllRemoteMMIv2Details.Remove(rmmiData);
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
