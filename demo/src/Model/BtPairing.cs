using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using JabraSDK;
using Jabra_SDK_Demo.Helpers;

namespace Jabra_SDK_Demo.Model
{
  public class BtPairing : INotifyPropertyChanged
  {
    public BtPairing()
    {

    }


    private int _deviceId = -1;
    public int DeviceId
    {
      get { return _deviceId; }
      set
      {
        _deviceId = value;
        OnPropertyChanged("DeviceId");
      }
    }

    private string _deviceName;
    public string DeviceName
    {
      get { return _deviceName; }
      set
      {
        _deviceName = value;
        OnPropertyChanged("DeviceName");
      }
    }

    private string _dongleName;
    public string DongleName
    {
      get { return _dongleName; }
      set
      {
        _dongleName = value;
        OnPropertyChanged("DongleName");
      }
    }

    private int _autoPairingCurrentValue;
    public int AutoPairingCurrentValue
    {
      get { return _autoPairingCurrentValue; }
      set
      {
        _autoPairingCurrentValue = value;
        OnPropertyChanged("AutoPairingCurrentValue");
      }
    }

    private bool _isDongleDevice;
    public bool IsDongleDevice
    {
      get { return _isDongleDevice; }
      set
      {
        _isDongleDevice = value;
        OnPropertyChanged("IsDongleDevice");
      }
    }

    private bool _pairingSupported;
    public bool PairingSupported
    {
      get { return _pairingSupported; }
      set
      {
        _pairingSupported = value;
        OnPropertyChanged("PairingSupported");
      }
    }

    private bool _pairingListSupported;
    public bool PairingListSupported
    {
      get { return _pairingListSupported; }
      set
      {
        _pairingListSupported = value;
        OnPropertyChanged("PairingListSupported");
      }
    }

    private List<BtDevice> _pairedBluetoothDevices;
    public List<BtDevice> PairedBluetoothDevices
    {
      get { return _pairedBluetoothDevices; }
      set
      {
        _pairedBluetoothDevices = value;
        OnPropertyChanged("PairedBluetoothDevices");
      }
    }

    private BtDevice _pairedBluetoothDevice;
    public BtDevice PairedBluetoothDevice
    {
      get { return _pairedBluetoothDevice; }
      set
      {
        _pairedBluetoothDevice = value;
        OnPropertyChanged("PairedBluetoothDevice");
      }
    }

    private string _searchPairing = "Search";
    public string SearchPairing
    {
      get { return _searchPairing; }
      set
      {
        _searchPairing = value;
        OnPropertyChanged("SearchPairing");
      }
    }
    
    private List<BtDevice> _availableBluetoothDevices;
    public List<BtDevice> AvailableBluetoothDevices
    {
      get { return _availableBluetoothDevices; }
      set
      {
        _availableBluetoothDevices = value;
        OnPropertyChanged("AvailableBluetoothDevices");
      }
    }

    private BtDevice _availableBluetoothDevice;
    public BtDevice AvailableBluetoothDevice
    {
      get { return _availableBluetoothDevice; }
      set
      {
        _availableBluetoothDevice = value;
        OnPropertyChanged("AvailableBluetoothDevice");
      }
    }

    private bool _btSearchAllowed = true;
    public bool BtSearchAllowed
    {
        get { return _btSearchAllowed; }
        set
        {
            _btSearchAllowed = value;
            OnPropertyChanged("BtSearchAllowed");
        }
    }

    private ImageSource _imageSource;
    public ImageSource ImageSource
    {
      get { return _imageSource; }
      set
      {
        _imageSource = value;
        OnPropertyChanged("ImageSource");
      }
    }


    private string _btDeviceHelpText = "Search now for any available Bluetooth devices. Ensure the Bluetooth device is switched on, within range and ready to connect. Refer to the device manual if needed.";
    public string BtDeviceHelpText
    {
      get { return _btDeviceHelpText; }
      set
      {
        _btDeviceHelpText = value;
        OnPropertyChanged("BtDeviceHelpText");
      }
    }

    private string _btAutomaticSearchHelpText = "When the Jabra Bluetooth adapter is plugged into the PC it will attempt to connect with the last connected Bluetooth device. If it cannot connect, it will automatically search for new Bluetooth devices to connect to.";
    public string BtAutomaticSearchHelpText
    {
      get { return _btAutomaticSearchHelpText; }
      set
      {
        _btAutomaticSearchHelpText = value;
        OnPropertyChanged("BtAutomaticSearchHelpText");
      }
    }


    private string _btFactoryResetHelpText = "Reset all settings to factory-default.";
    public string BtFactoryResetHelpText
    {
      get { return _btFactoryResetHelpText; }
      set
      {
        _btFactoryResetHelpText = value;
        OnPropertyChanged("BtFactoryResetHelpText");
      }
    }

    private string _btClearPairingHelpText = "Clear the list of paired devices.";
    public string BtClearPairingHelpText
    {
      get { return _btClearPairingHelpText; }
      set
      {
        _btClearPairingHelpText = value;
        OnPropertyChanged("BtClearPairingHelpText");
      }
    }


    private string _btConnectDisconnectHelpText = "Connect/Disconnect your Bluetooth device from Jabra Bluetooth adapter.";
    public string BtConnectDisconnectHelpText
    {
      get { return _btConnectDisconnectHelpText; }
      set
      {
        _btConnectDisconnectHelpText = value;
        OnPropertyChanged("BtConnectDisconnectHelpText");
      }
    }
    private string _btPairedDeviceHelpText = "In the list of paired devices, you can select a device and either connect or disconnect it. Click clear to delete a device from the list if it is not currently connected via Bluetooth.";
    public string BtPairedDeviceHelpText
    {
      get { return _btPairedDeviceHelpText; }
      set
      {
        _btPairedDeviceHelpText = value;
        OnPropertyChanged("BtPairedDeviceHelpText");
      }
    }

    private string _btAvailableDeviceHelpText = "In the list of available devices, you can select a device and connect to your Jabra Bluetooth adapter.";
    public string BtAvailableDeviceHelpText
    {
      get { return _btAvailableDeviceHelpText; }
      set
      {
        _btAvailableDeviceHelpText = value;
        OnPropertyChanged("BtAvailableDeviceHelpText");
      }
    }

    public string BtDisconnectHelpText => "Disconnect your Bluetooth device from your Jabra Bluetooth adapter.";
    public string BtReconnectHelpText => "Connect your Bluetooth device to the Jabra Bluetooth adapter. Ensure the Bluetooth device is switched on and within range.";


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
