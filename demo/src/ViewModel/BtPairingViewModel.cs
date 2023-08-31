using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using JabraSDK;
using System.Threading;
using BtPairing = Jabra_SDK_Demo.Model.BtPairing;
using Jabra_SDK_Demo.Properties;

namespace Jabra_SDK_Demo.ViewModel
{
  public class BtPairingViewModel : ViewModelBase
  {
    #region Properties
    private BtPairing _btPairing;
    public BtPairing BtPairing
    {
      get { return _btPairing; }
      set
      {
        _btPairing = value;
        OnPropertyChanged("BtPairing");
      }
    }

    private List<BtPairing> _allPairingInformations;
    public List<BtPairing> AllPairingInformations
    {
      get { return _allPairingInformations; }
      set
      {
        _allPairingInformations = value;
        OnPropertyChanged("AllPairingInformations");
      }
    }

    #endregion

    #region ICommand

    private ICommand _clickStartCommand;
    public ICommand ClickStartCommand
    {
      get
      {
        return _clickStartCommand ?? (_clickStartCommand = new CommandHandlerArguments(param => ClickStartCommandCommandHandler(), true));
      }
    }

    private ICommand _clickClearCommand;
    public ICommand ClickClearCommand
    {
      get
      {
        return _clickClearCommand ?? (_clickClearCommand = new CommandHandlerArguments(param => ClickClearCommandCommandHandler(), true));
      }
    }

    private ICommand _clickResetCommand;
    public ICommand ClickResetCommand
    {
      get
      {
        return _clickResetCommand ?? (_clickResetCommand = new CommandHandlerArguments(param => ClickResetCommandCommandHandler(), true));
      }
    }

    private ICommand _checkedCommand;
    public ICommand CheckedCommand
    {
      get
      {
        return _checkedCommand ?? (_checkedCommand = new CommandHandler(() => CheckedAction(), true));
      }
    }

    private ICommand _buttonClickCommand;
    public ICommand ButtonClickCommand
    {
      get
      {
        return _buttonClickCommand ?? (_buttonClickCommand = new CommandHandlerArguments(param => ButtonClickCommandHandler(), true));
      }
    }

    private ICommand _searchClickCommand;
    public ICommand SearchClickCommand
    {
      get
      {
        return _searchClickCommand ?? (_searchClickCommand = new CommandHandlerArguments(param => SearchClickCommandHandler(param), true));
      }
    }


    private ICommand _pairingConnectClearDisconnectCommand;
    public ICommand PairingConnectClearDisconnectCommand
    {
      get
      {
        return _pairingConnectClearDisconnectCommand ?? (_pairingConnectClearDisconnectCommand = new CommandHandlerArguments(param => PairingConnectClearDisconnect(param), true));
      }
    }

    private ICommand _newConnectClickCommand;
    public ICommand NewConnectClickCommand
    {
      get
      {
        return _newConnectClickCommand ?? (_newConnectClickCommand = new CommandHandlerArguments(param => NewConnectClickCommandHandler(), true));
      }
    }

    #endregion

    #region Constructor
    public BtPairingViewModel()
    {
    }

    internal BtPairingViewModel(IDevice device)
    {
      AllPairingInformations = new List<BtPairing>();
      _btPairing = new BtPairing();
      UpdateDeviceInformations(device);
    }

    #endregion

    private BtDevice lastConnectedBtDevice = null;
    private bool enableBluetoothDeviceUpdate = false;



    private void NewConnectClickCommandHandler()
    {

      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      if (lastConnectedBtDevice == null || (_btPairing.AvailableBluetoothDevice != null && _btPairing.AvailableBluetoothDevice.BtAddress != lastConnectedBtDevice.BtAddress))
      {

        if (_btPairing.AvailableBluetoothDevice != null)
        {
          var val = (from e in _btPairing.AvailableBluetoothDevices
                     where e.BtAddress == _btPairing.AvailableBluetoothDevice.BtAddress
                     select e).FirstOrDefault();
          try
          {
            if (_btPairing.SearchPairing == "Stop")
            {
              SpecialHandlers.Show370Spinner = false;
              try
              {
                CommonMethods.SelectedDevice.StopPairing();
              }
              catch { }
              _btPairing.SearchPairing = "Search";
            }
            CommonMethods.SelectedDevice.ConnectDevice(val.BtAddress);
            lastConnectedBtDevice = val;
            UiServices.SetBusyState(5, false, false, false, true);
            _btPairing.PairedBluetoothDevice = val;
            _btPairing.AvailableBluetoothDevices = new List<Jabra_SDK_Demo.Helpers.BtDevice>();
            enableBluetoothDeviceUpdate = false;

          }
          catch (Exception ex)
          {
            MessageBoxService.ShowMessage("Connect to Device Failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }
        else
        {
          MessageBoxService.ShowMessage("No device are found", "Error", MessageBoxButton.OK, MessageBoxImage.None);
        }
      }
    }
    public void ShowPairingList(List<IBluetoothDevice> list)
    {
      List<BtDevice> pairingList = new List<BtDevice>();
      foreach (var value in list)
      {
        BtDevice device = new BtDevice();
        device.BtAddress = value.BluetoothAddress;
        device.IsConnected = value.IsConnected;
        device.Name = value.Name;
        device.IsClearPairingAllowed = !device.IsConnected && _btPairing.BtSearchAllowed;
        device.ConnectedImageSource = device.IsConnected ? Resources.BtConnected.ToWpfBitmap() : Resources.BtNotConnected.ToWpfBitmap();

        pairingList.Add(device);
      }
      _btPairing.PairedBluetoothDevices = pairingList;
      if (pairingList.Count > 0)
      {
        var connectedDevice = (from e in _btPairing.PairedBluetoothDevices
                               where e.IsConnected
                               select e).FirstOrDefault();
        if (connectedDevice != null)
        {
          _btPairing.PairedBluetoothDevice = connectedDevice;
        }
        else
        {
          _btPairing.PairedBluetoothDevice = pairingList[0];
        }
        _btPairing.PairedBluetoothDevices[0].ConnectedImageSource = pairingList[0].ConnectedImageSource;
      }
    }

    private void PairingConnectClearDisconnect(object parameter)
    {

      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;
      string message = string.Empty;
      if (_btPairing.PairedBluetoothDevice != null)
      {
        var val = (from e in _btPairing.PairedBluetoothDevices
                   where e.BtAddress == _btPairing.PairedBluetoothDevice.BtAddress
                   select e).FirstOrDefault();
        if (val?.BtAddress == null)
        {
          MessageBoxService.ShowMessage("No device presently connected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }

        try
        {
          switch (parameter.ToString())
          {
            case "Connect":
              message = "Connect operation failed.";
              device.ConnectPairedDevice(val.BtAddress);
              UiServices.SetBusyState(8, false, false, false, true);

              break;
            case "Disconnect":
              message = "Disconnect operation failed.";
              device.DisconnectPairedDevice(val.BtAddress);
              lastConnectedBtDevice = null;
              CommonMethods.SelectedDevice = null;
              Thread.Sleep(1000); // Nasty delay but dongle is not ready to connect at this stage
              UiServices.SetBusyState(3, false, false, false, true);
              break;
            case "Clear":
              message = "Clear operation failed.";
              device.ClearPairedDevice(val.BtAddress);
              List<IBluetoothDevice> pairingList = device.GetPairedDevice();
              ShowPairingList(pairingList);
              break;
          }
        }
        catch (Exception ex)
        {
          MessageBoxService.ShowMessage(message + "\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      else
      {
        MessageBoxService.ShowMessage("No devices are paired", "", MessageBoxButton.OK, MessageBoxImage.None);
      }
    }

    internal void UpdateDeviceInformations(IDevice device)
    {
      BtPairing pairingInfo = new BtPairing();
      pairingInfo.DeviceId = device.DeviceId;
      pairingInfo.DeviceName = device.Name;
      pairingInfo.IsDongleDevice = device.IsDongleDevice;
      if (pairingInfo.IsDongleDevice)
      {
        pairingInfo.DongleName = device.DongleName;
        pairingInfo.PairingSupported = true;
        pairingInfo.AutoPairingCurrentValue = Convert.ToInt32(device.IsAutoPairing);
        pairingInfo.PairingListSupported = device.IsPairingListSupported;
      }
      AllPairingInformations.Add(pairingInfo);
    }

    public void ShowPairingDetails(IDevice device)
    {
      if (device == null)
      {
        return;
      }


      var result = (from list in AllPairingInformations
                    where list.DeviceId == device.DeviceId
                    select list).FirstOrDefault();
      if (result.IsDongleDevice)
      {
          var scMode = device.GetSecureConnectionMode();
          _btPairing.BtSearchAllowed = (scMode == JabraSDK.SecureConnectionMode.SecureConnectionLegacy);
      }
      if (result != null)
      {
        _btPairing.DeviceId = result.DeviceId;
        _btPairing.DeviceName = result.DeviceName;
        _btPairing.DongleName = result.DongleName;
        _btPairing.PairingSupported = result.PairingSupported;
        _btPairing.AutoPairingCurrentValue = Convert.ToInt32(device.IsAutoPairing);
        _btPairing.PairingListSupported = result.PairingListSupported;
      }
      if (_btPairing.PairingSupported)
      {
        //todo
        BtPairingHelper btPairingHelper = null;
        try
        {
          btPairingHelper = MainWindowViewModel.BtPairingHelpers.Single(x => x.Key == device.DeviceId).Value;
        }
        catch
        {
          btPairingHelper = new BtPairingHelper();
        }
        //todo

        string strName = device.GetConnectedDeviceName;

        if (btPairingHelper != null)
        {
          if (!string.IsNullOrEmpty(strName))
          {
            btPairingHelper.ConnectedDeviceName = "Connected to " + strName;
            btPairingHelper.ConnectedDisconnected = "Disconnect";
            if (_btPairing != null)
              _btPairing.BtConnectDisconnectHelpText = _btPairing.BtDisconnectHelpText;
          }
          else
          {
            btPairingHelper.ConnectedDeviceName = "Not Connected";
            btPairingHelper.ConnectedDisconnected = "Connect";
            if (_btPairing != null)
              _btPairing.BtConnectDisconnectHelpText = _btPairing.BtReconnectHelpText;
          }
          MainWindowViewModel.BtPairingHelpers[device.DeviceId] = btPairingHelper;
          SetHelperValues(device.DeviceId);
        }
      }
      if (_btPairing.PairingListSupported)
      {
        List<IBluetoothDevice> allDevicesInPairingMode = device.GetAllDevicesInPairingMode();
        ShowallDevicesInPairingMode(allDevicesInPairingMode);
        List<IBluetoothDevice> pairingList = device.GetPairedDevice();
        ShowPairingList(pairingList);
      }
    }

    public void ShowallDevicesInPairingMode(List<IBluetoothDevice> allDevicesInPairingMode)
    {
      List<BtDevice> pairingList = new List<BtDevice>();
      foreach (var value in allDevicesInPairingMode)
      {
        if (!value.IsConnected)
        {
          BtDevice device = new BtDevice();
          device.BtAddress = value.BluetoothAddress;
          device.IsConnected = value.IsConnected;
          device.Name = value.Name;
          pairingList.Add(device);
        }
      }
      if (enableBluetoothDeviceUpdate)
      {

        _btPairing.AvailableBluetoothDevices = pairingList;
        if (pairingList.Count > 0)
          _btPairing.AvailableBluetoothDevice = pairingList[0];
      }
    }

    public void ShowDeviceList(BluetoothPairingListEventArgs bluetoothPairingListEventArgs)
    {
      if (bluetoothPairingListEventArgs.SearchStatus == DeviceSearchStatus.SearchComplete)
      {
        _btPairing.SearchPairing = "Search";
        SpecialHandlers.Show370Spinner = false;
      }
      else
      {
        ShowallDevicesInPairingMode(bluetoothPairingListEventArgs.AvailableDevices);
      }
    }

    private void ButtonClickCommandHandler()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      int selectedDeviceId = device.DeviceId;
      BtPairingHelper btPairingHelper = MainWindowViewModel.BtPairingHelpers.Single(x => x.Key == selectedDeviceId).Value;
      string message = string.Empty;
      try
      {
        switch (btPairingHelper.ConnectedDisconnected.ToLower(CultureInfo.InvariantCulture))
        {
          case "connect":
            message = "Connect operation failed.";
            device.ConnectDevice();
            //btPairingHelper.ConnectedDisconnected = "Disconnect";
            //_btPairing.BtConnectDisconnectHelpText = _btPairing.BtDisconnectHelpText;
            break;
          case "disconnect":
            message = "Disconnect operation failed.";
            device.DisconnectDevice();
            btPairingHelper.ConnectedDisconnected = "Connect";
            btPairingHelper.ConnectedDeviceName = "Not Connected";
            _btPairing.BtConnectDisconnectHelpText = _btPairing.BtReconnectHelpText;
            break;
        }
        UiServices.SetBusyState(3, false, true, false, false);
        MainWindowViewModel.BtPairingHelpers[selectedDeviceId] = btPairingHelper;
        SetHelperValues(selectedDeviceId);
        ShowPairingDetails(device);
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(message + "\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }


    public void SearchClickCommandHandler(object parameter)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      bool showSpinner = false;
      string message = string.Empty;
      try
      {
        switch (parameter.ToString())
        {
          case "Search":
            message = "Search operation failed.";
            device.SearchForDevicesInPairingMode();
            showSpinner = true;
            enableBluetoothDeviceUpdate = true;
            _btPairing.SearchPairing = "Stop";
            break;
          case "Stop":
            enableBluetoothDeviceUpdate = false;
            message = "Stop operation failed.";
            device.StopPairing();
            _btPairing.SearchPairing = "Search";
            break;
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(message + "\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      finally
      {
        SpecialHandlers.Show370Spinner = showSpinner;
      }
    }


    private void ClickStartCommandCommandHandler()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;

      try
      {
        device.StartPairing();
        UiServices.SetBusyState(60, true, false, false, false);
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Start pairing operation failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ClickClearCommandCommandHandler()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      try
      {
        device.ClearPairing();
        BtPairingHelper btPairingHelper = MainWindowViewModel.BtPairingHelpers.Single(x => x.Key == device.DeviceId).Value;
        btPairingHelper.HeadsetConnected = false;
        btPairingHelper.ConnectedDisconnected = "Connect";
        btPairingHelper.ConnectedDeviceName = "Not Connected";
        _btPairing.BtConnectDisconnectHelpText = _btPairing.BtReconnectHelpText;
        MainWindowViewModel.BtPairingHelpers[device.DeviceId] = btPairingHelper;
        SetHelperValues(device.DeviceId);
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Clear pairing operation failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void ClickResetCommandCommandHandler()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice device = CommonMethods.SelectedDevice;
      if (device.IsFactoryResetSupported)
      {
        try
        {
          device.FactoryReset();
          UiServices.SetBusyState(5, false, false, true, false);
        }
        catch (Exception ex)
        {
          MessageBoxService.ShowMessage("Factory Reset operation failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      else
      {
        MessageBoxService.ShowMessage("Reset is not supported!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void CheckedAction()
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }
      IDevice selectedDevice = CommonMethods.SelectedDevice;
      try
      {
        var ret = selectedDevice.SetAutoPairing(Convert.ToBoolean(_btPairing.AutoPairingCurrentValue));
        // update auto pairing value in the list
        AllPairingInformations.Where(device => device.DeviceId == selectedDevice.DeviceId).ToList().ForEach(device => device.AutoPairingCurrentValue = _btPairing.AutoPairingCurrentValue);
      }
      catch (Exception ex)
      {
        _btPairing.AutoPairingCurrentValue = Convert.ToInt32(selectedDevice.IsAutoPairing);
        MessageBoxService.ShowMessage("Set Auto pairing operation failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }


    private void SetHelperValues(int deviceId)
    {
      try
      {
        if (CommonMethods.SelectedDevice != null && deviceId == CommonMethods.SelectedDevice.DeviceId)
        {
          var keyValuePair = MainWindowViewModel.BtPairingHelpers.Single(x => x.Key == deviceId);
          SpecialHandlers.ConnectedDeviceName = keyValuePair.Value.ConnectedDeviceName;
          SpecialHandlers.ConnectedDisconnected = keyValuePair.Value.ConnectedDisconnected;
          SpecialHandlers.HeadsetConnected = keyValuePair.Value.HeadsetConnected;
          SpecialHandlers.IsPaired = keyValuePair.Value.IsPaired;
        }
      }
      catch { }
    }

    public void SetPairingStatus(ushort deviceId, string translatedInData, bool buttonInData)
    {
      try
      {
        switch (translatedInData.ToLower(CultureInfo.InvariantCulture))
        {
          case "headsetconnection":
            if (buttonInData)
            {
              SpecialHandlers.HeadsetConnected = buttonInData;
              if (_btPairing != null)
                _btPairing.BtConnectDisconnectHelpText = _btPairing.BtDisconnectHelpText;
            }
            else
            {
              if (_btPairing != null)
                _btPairing.BtConnectDisconnectHelpText = _btPairing.BtReconnectHelpText;
              if (MainWindowViewModel.BtPairingHelpers != null)
              {
                try
                {
                  BtPairingHelper btPairingHelper = MainWindowViewModel.BtPairingHelpers.Single(x => x.Key == deviceId).Value;
                  if (btPairingHelper != null)
                  {
                    btPairingHelper.ConnectedDisconnected = "Connect";
                    btPairingHelper.ConnectedDeviceName = "Not Connected";
                    _btPairing.SearchPairing = "Search";
                    SetHelperValues(deviceId);
                    var selectedDevice = (from det in MainWindowViewModel.AvailableDevices
                                          where det.DeviceId == deviceId
                                          select det).FirstOrDefault();

                    if (selectedDevice != null)
                    {
                      ShowPairingDetails(selectedDevice);
                    }
                  }
                }
                catch { }
              }
            }
            SpecialHandlers.ShowSpinner = false;
            SpecialHandlers.IsPaired = buttonInData;
            break;

          default:
            break;
        }
      }
      catch (Exception )
      {
        MessageBoxService.ShowMessage($"Error during set {translatedInData}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
