using System;
using System.Collections.Generic;
using System.Linq;
using JabraSDK;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Model;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Jabra_SDK_Demo.ViewModel
{
  public class DeviceLoggingViewModel : ViewModelBase
  {
    private DeviceLogging _deviceLogging;
    public DeviceLogging DeviceLogging
    {
      get { return _deviceLogging; }
      set
      {
        _deviceLogging = value;
        OnPropertyChanged("DeviceLogging");
      }
    }

    private object lockObject = new object();

    public DeviceLoggingViewModel(IDevice device)
    {
      _deviceLogging = new DeviceLogging();
      _deviceLogging.Data = new ObservableCollection<string>();
      AllLoggingDetails = new List<LoggingDetails>();
      ShowLoggingDetails(device.DeviceId);
    }

    private ICommand _deviceLoggingChecked;
    public ICommand DeviceLoggingChecked
    {
      get
      {
        return _deviceLoggingChecked ?? (_deviceLoggingChecked = new CommandHandlerArguments(param => EnableDeviceLoggingCommandAction(), true));
      }
    }

    private ICommand _clearDeviceLogging;
    public ICommand ClearDeviceLogging
    {
      get
      {
        return _clearDeviceLogging ?? (_clearDeviceLogging = new CommandHandlerArguments(param => ClearDeviceLoggingCommandAction(), true));
      }
    }

    public void EnableDeviceLoggingCommandAction()
    {
      IDevice device = CommonMethods.SelectedDevice;
      try
      {
        DeviceStatus status = DeviceStatus.ReturnOk;
        if (_deviceLogging.IsCheckBoxChecked)
        {
          status = device.DeviceLoggingConfiguration(true);
          if (status == DeviceStatus.ReturnOk)
          {
            _deviceLogging.DeviceLoggingEnabled = device.IsDeviceLogEnabled;
          }
        }
        else
        {
          status = device.DeviceLoggingConfiguration(false);
          if (status == DeviceStatus.ReturnOk)
          {
            _deviceLogging.DeviceLoggingEnabled = device.IsDeviceLogEnabled;
          }
          ClearDeviceLoggingCommandAction();
        }
      }
      catch (Exception ex)
      {
        _deviceLogging.IsCheckBoxChecked = false;
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void UpdateDeviceLoggingData(DeviceLoggingEventArgs e)
    {
      try
      {
        lock (lockObject)
        {
          string message = JsonConvert.DeserializeObject(e.Data).ToString();
          AllLoggingDetails.Add(new LoggingDetails { DeviceId = e.DeviceId, Data = message });
        }
      }
      catch { }
    }

    public void ClearDeviceLoggingCommandAction()
    {
      lock (lockObject)
      {
        _deviceLogging.Data.Clear();
        IDevice device = CommonMethods.SelectedDevice;
        var modelDeviceLogging = (from e in AllLoggingDetails
                                  where e.DeviceId == device.DeviceId
                                  select e).ToList();
        foreach (var deviceLogs in modelDeviceLogging)
        {
          AllLoggingDetails.Remove(deviceLogs);
        }
      }
    }

    public void ShowLoggingDetails(int deviceId)
    {
      IDevice device = CommonMethods.SelectedDevice;
      _deviceLogging.DeviceLoggingSupported = device.IsFeatureSupported(DeviceFeature.Logging);
      if (_deviceLogging.DeviceLoggingSupported)
      {
        _deviceLogging.IsCheckBoxChecked = _deviceLogging.DeviceLoggingEnabled = device.IsDeviceLogEnabled;
      }

      lock (lockObject)
      {
        if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == deviceId)
        {
          var result = (from list in AllLoggingDetails
                        where list.DeviceId == deviceId
                        select list).ToList();
          if (result != null)
          {
            _deviceLogging.Data.Clear();
            if (_deviceLogging.DeviceLoggingEnabled)
              foreach (var devLogs in result)
              {
                _deviceLogging.Data?.Insert(0, devLogs.Data);
              }
          }
        }
      }
    }

    private List<LoggingDetails> _allLoggingDetails;
    public List<LoggingDetails> AllLoggingDetails
    {
      get { return _allLoggingDetails; }
      set
      {
        _allLoggingDetails = value;
        OnPropertyChanged("AllLoggingDetails");
      }
    }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string name)
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
