using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using JabraSDK;

namespace Jabra_SDK_Demo.Helpers
{
  public class SpecialHandlers
  {
    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    public static void RaiseStaticPropertyChanged(string propName)
    {
      EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
      if (handler != null)
        handler(null, new PropertyChangedEventArgs(propName));
    }

    private static bool _deviceConnected;
    public static bool DeviceConnected
    {
      get { return _deviceConnected; }
      set
      {
        _deviceConnected = value;
        RaiseStaticPropertyChanged("DeviceConnected");
      }
    }

    private static bool _displayFisrtTab;
    public static bool DisplayFisrtTab
    {
      get { return _displayFisrtTab; }
      set
      {
        _displayFisrtTab = value;
        RaiseStaticPropertyChanged("DisplayFisrtTab");
      }
    }

    private static bool _enableDisableButton;
    public static bool EnableDisableButton
    {
      get { return _enableDisableButton; }
      set
      {
        _enableDisableButton = value;
        RaiseStaticPropertyChanged("EnableDisableButton");
      }
    }

    private static bool _settingsLoaded;
    public static bool SettingsLoaded
    {
      get { return _settingsLoaded; }
      set
      {
        _settingsLoaded = value;
        RaiseStaticPropertyChanged("SettingsLoaded");
      }
    }

    private static bool _showSpinner;
    public static bool ShowSpinner
    {
      get { return _showSpinner; }
      set
      {
        _showSpinner = value;
        RaiseStaticPropertyChanged("ShowSpinner");
      }
    }

    private static bool _show370Spinner;
    public static bool Show370Spinner
    {
      get { return _show370Spinner; }
      set
      {
        _show370Spinner = value;
        RaiseStaticPropertyChanged("Show370Spinner");
      }
    }

    private static bool _showUiSpinner;
    public static bool ShowUiSpinner
    {
      get { return _showUiSpinner; }
      set
      {
        _showUiSpinner = value;
        RaiseStaticPropertyChanged("ShowUiSpinner");
      }
    }

    private static bool _showUiConnectSpinner;
    public static bool ShowUiConnectSpinner
    {
      get { return _showUiConnectSpinner; }
      set
      {
        _showUiConnectSpinner = value;
        RaiseStaticPropertyChanged("ShowUiConnectSpinner");
      }
    }

    private static bool _showUiResetSpinner;
    public static bool ShowUiResetSpinner
    {
      get { return _showUiResetSpinner; }
      set
      {
        _showUiResetSpinner = value;
        RaiseStaticPropertyChanged("ShowUiResetSpinner");
      }
    }

    private static bool _ispaired;
    public static bool IsPaired
    {
      get { return _ispaired; }
      set
      {
        _ispaired = value;
        RaiseStaticPropertyChanged("IsPaired");
      }
    }

    private static string _connectedDeviceName;
    public static string ConnectedDeviceName
    {
      get { return _connectedDeviceName; }
      set
      {
        _connectedDeviceName = value;
        RaiseStaticPropertyChanged("ConnectedDeviceName");
      }
    }

    private static string _connectedDisconnected = "Disconnect";
    public static string ConnectedDisconnected
    {
      get { return _connectedDisconnected; }
      set
      {
        _connectedDisconnected = value;
        RaiseStaticPropertyChanged("ConnectedDisconnected");
      }
    }

    private static bool _headsetConnected;
    public static bool HeadsetConnected
    {
      get { return _headsetConnected; }
      set
      {
        _headsetConnected = value;
        RaiseStaticPropertyChanged("HeadsetConnected");
      }
    }

    private static ObservableCollection<UnWrittenSettings> _unWrittenSettings = new ObservableCollection<UnWrittenSettings>();
    public static ObservableCollection<UnWrittenSettings> UnWrittenSettings
    {
      get { return _unWrittenSettings; }
      set
      {
        _unWrittenSettings = value;
        RaiseStaticPropertyChanged("UnWrittenSettings");
      }
    }

    private static ObservableCollection<ValidationErrors> _validationErrors = new ObservableCollection<ValidationErrors>();
    public static ObservableCollection<ValidationErrors> ValidationErrors
    {
      get { return _validationErrors; }
      set
      {
        _validationErrors = value;
        RaiseStaticPropertyChanged("ValidationErrors");
      }
    }

    private static ObservableCollection<SettingsInformation> _settingsInformation = new ObservableCollection<SettingsInformation>();
    public static ObservableCollection<SettingsInformation> SettingsInformation
    {
      get { return _settingsInformation; }
      set
      {
        _settingsInformation = value;
        RaiseStaticPropertyChanged("SettingsInformation");
      }
    }

    private static ObservableCollection<List<IConfigurableButton>> _configurableButton = new ObservableCollection<List<IConfigurableButton>>();
    public static ObservableCollection<List<IConfigurableButton>> ConfigurableButton
    {
      get { return _configurableButton; }
      set
      {
        _configurableButton = value;
        RaiseStaticPropertyChanged("ConfigurableButton");
      }
    }

    private static ObservableCollection<UnwrittenConfigurableButtons> _unwrittenConfigurableButtons = new ObservableCollection<UnwrittenConfigurableButtons>();
    public static ObservableCollection<UnwrittenConfigurableButtons> UnwrittenConfigurableButtons
    {
      get { return _unwrittenConfigurableButtons; }
      set
      {
        _unwrittenConfigurableButtons = value;
        RaiseStaticPropertyChanged("UnwrittenConfigurableButtons");
      }
    }

    private static ObservableCollection<UnwrittenConfigurableButtons> _buttonConfigValues = new ObservableCollection<UnwrittenConfigurableButtons>();
    public static ObservableCollection<UnwrittenConfigurableButtons> ButtonConfigValues
    {
      get { return _buttonConfigValues; }
      set
      {
        _buttonConfigValues = value;
        RaiseStaticPropertyChanged("ButtonConfigValues");
      }
    }

    private static ObservableCollection<RemoteMMIv2Setting> _unwrittenRmmiV2Settings = new ObservableCollection<RemoteMMIv2Setting>();
    public static ObservableCollection<RemoteMMIv2Setting> UnwrittenRmmiv2Settings
    {
      get { return _unwrittenRmmiV2Settings; }
      set
      {
        _unwrittenRmmiV2Settings = value;
        RaiseStaticPropertyChanged("UnwrittenRmmiv2Settings");
      }
    }

    private static ObservableCollection<RemoteMMIv2Setting> _rmmiv2ConfiguredValues = new ObservableCollection<RemoteMMIv2Setting>();
    public static ObservableCollection<RemoteMMIv2Setting> Rmmiv2ConfiguredValues
    {
      get { return _rmmiv2ConfiguredValues; }
      set
      {
        _rmmiv2ConfiguredValues = value;
        RaiseStaticPropertyChanged("Rmmiv2ConfiguredValues");
      }
    }

    private static bool _displayAuthorizationTab;
    public static bool DisplayAuthorizationTab
    {
      get { return _displayAuthorizationTab; }
      set
      {
        _displayAuthorizationTab = value;
        RaiseStaticPropertyChanged("DisplayAuthorizationTab");
      }
    }

    private static string _authorizationToken = string.Empty;
    public static string AuthorizationToken
    {
      get { return _authorizationToken; }
      set
      {
        _authorizationToken = value;
        if (!string.IsNullOrEmpty(_authorizationToken) || !string.IsNullOrEmpty(_authorizationUserName) || !string.IsNullOrEmpty(CommonMethods.Password))
        {
          IsClearEnableDisable = true;
        }
        else
        {
          IsClearEnableDisable = false;
        }
        RaiseStaticPropertyChanged("AuthorizationToken");
      }
    }

    private static string _authorizationUserName;
    public static string AuthorizationUserName
    {
      get { return _authorizationUserName; }
      set
      {
        _authorizationUserName = value;
        if (!string.IsNullOrEmpty(_authorizationUserName) || !string.IsNullOrEmpty(_authorizationToken) || !string.IsNullOrEmpty(CommonMethods.Password))
        {
          IsClearEnableDisable = true;
        }
        else
        {
          IsClearEnableDisable = false;
        }
        RaiseStaticPropertyChanged("AuthorizationUserName");
      }
    }

    private static bool _isClearEnableDisable = false;
    public static bool IsClearEnableDisable
    {
      get { return _isClearEnableDisable; }
      set
      {
        _isClearEnableDisable = value;
        RaiseStaticPropertyChanged("IsClearEnableDisable");
      }
    }

    private static int _unWrittenSettingsDeviceId = -1;
    public static int UnWrittenSettingsDeviceId
    {
      get { return _unWrittenSettingsDeviceId; }
      set
      {
        _unWrittenSettingsDeviceId = value;
        RaiseStaticPropertyChanged("UnWrittenSettingsDeviceId");
      }
    }

    private static bool _isRMMISupported;
    public static bool IsRMMISupported
    {
      get { return _isRMMISupported; }
      set
      {
        _isRMMISupported = value;
        RaiseStaticPropertyChanged("IsRMMISupported");
      }
    }
  }
}


