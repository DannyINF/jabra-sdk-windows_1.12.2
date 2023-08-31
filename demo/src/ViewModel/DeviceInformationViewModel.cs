using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using DeviceInformation = Jabra_SDK_Demo.Model.DeviceInformation;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
  public class DeviceInformationViewModel : ViewModelBase
  {
    private DeviceInformation _deviceInformation;
    public DeviceInformation DeviceInformation
    {
      get { return _deviceInformation; }
      set
      {
        _deviceInformation = value;
        OnPropertyChanged("DeviceInformation");
      }
    }

    private List<DeviceInformation> _allDeviceInformations;
    public List<DeviceInformation> AllDeviceInformations
    {
      get { return _allDeviceInformations; }
      set
      {
        _allDeviceInformations = value;
        OnPropertyChanged("AllDeviceInformations");
      }
    }

    private void DisplayBatteryStatus(List<string> batteryDetails)
    {
      if (batteryDetails.Count > 0)
      {
        _deviceInformation.BatteryStatus = batteryDetails[0];

        if (batteryDetails.Count > 1)
        {
          _deviceInformation.HasExtraBatteryUnits = true;
          _deviceInformation.BatteryStatusUnits = batteryDetails[1];
        }
      }
    }

    public DeviceInformationViewModel()
    {
    }
    internal DeviceInformationViewModel(IDevice devInfo)
    {
      _canExecute = true;
      AllDeviceInformations = new List<DeviceInformation>();
      _deviceInformation = new DeviceInformation();
      UpdateDeviceInformations(devInfo);
      ShowDeviceDetails(devInfo);
    }


    public void ShowDeviceDetails(IDevice device)
    {
      var result = (from list in AllDeviceInformations
                    where list.DeviceId == device.DeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        _deviceInformation.DeviceId = result.DeviceId;
        _deviceInformation.DeviceName = result.DeviceName;
        _deviceInformation.SerialNumber = result.SerialNumber;
        try
        {
          List<string> batteryDetails = CommonMethods.GetBatteryStatus(device);
          DisplayBatteryStatus(batteryDetails);
        }
        catch { }
        string imageSourcePath = string.Empty;
        try
        {
          imageSourcePath = device.GetDeviceImagePath();
        }
        catch { }
        _deviceInformation.ImageSource = CommonMethods.SetImageSource(imageSourcePath);
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
      try
      {
        if (CommonMethods.SelectedDevice != null)
        {
          List<string> batteryDetails = CommonMethods.GetBatteryStatus(CommonMethods.SelectedDevice);
          DisplayBatteryStatus(batteryDetails);
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Unable to reterive battery Information.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    internal void UpdateBatteryStatus(BatteryStatusUpdateEventArgs eventArgs)
    {
      string batteryDetails = eventArgs.LevelInPercent.ToString();
      if (eventArgs.Charging)
        batteryDetails += " charging";
      if (eventArgs.BatteryLow)
        batteryDetails += " low battery";
      if (AllDeviceInformations != null)
      {
        var result = (from list in AllDeviceInformations
                      where list.DeviceId == eventArgs.DeviceId
                      select list).FirstOrDefault();
        if (result != null)
        {
          result.BatteryStatus = batteryDetails;
          if (eventArgs.ExtraUnits != null && eventArgs.ExtraUnits.Count > 0)
          {
            result.HasExtraBatteryUnits = true;
            string extraUnits = string.Empty;
            foreach (var unit in eventArgs.ExtraUnits)
            {
              extraUnits += unit.LevelInPercent.ToString() + " (" + unit.Component.ToString() + ") ";
            }
            result.BatteryStatusUnits = extraUnits;
          }
        }
      }
    }

    internal void UpdateDeviceInformations(IDevice device)
    {
      string imageSourcePath = string.Empty;
      try
      {
        imageSourcePath = device.GetDeviceImagePath();
      }
      catch { }
      DeviceInformation deviceInfo = new DeviceInformation
      {
        DeviceId = device.DeviceId,
        DeviceName = device.Name,
        ImageSource = CommonMethods.SetImageSource(imageSourcePath),
        SerialNumber = device.ElectronicSerialNumber
      };
      AllDeviceInformations.Add(deviceInfo);
    }


    #region INotifyPropertyChanged Members

#pragma warning disable CS0108 // 'DeviceInformationViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'DeviceInformationViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

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

