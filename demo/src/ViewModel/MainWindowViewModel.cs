using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Jabra_SDK_Demo.ViewModel
{
  public class MainWindowViewModel : ViewModelBase
  {
    #region Properties

    private ViewModelBase _headerViewModel;
    public ViewModelBase HeaderViewModel
    {
      get
      {
        return _headerViewModel;
      }
      set
      {
        if (_headerViewModel == value)
          return;
        _headerViewModel = value;
        RaisePropertyChanged("HeaderViewModel");
      }
    }

    private ViewModelBase _deviceInformationViewModel;
    public ViewModelBase DeviceInformationViewModel
    {
      get
      {
        return _deviceInformationViewModel;
      }
      set
      {
        if (_deviceInformationViewModel == value)
          return;
        _deviceInformationViewModel = value;
        RaisePropertyChanged("DeviceInformationViewModel");
      }
    }

    private static ViewModelBase _deviceSettingsViewModel;
    public static ViewModelBase DeviceSettingsViewModel
    {
      get
      {
        return _deviceSettingsViewModel;
      }
      set
      {
        if (_deviceSettingsViewModel == value)
          return;
        _deviceSettingsViewModel = value;
        RaiseStaticPropertyChanged("DeviceSettingsViewModel");
      }
    }

    public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
    public static void RaiseStaticPropertyChanged(string propName)
    {
      StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propName));
    }

    private ViewModelBase _callControlViewModel;
    public ViewModelBase CallControlViewModel
    {
      get
      {
        return _callControlViewModel;
      }
      set
      {
        if (_callControlViewModel == value)
          return;
        _callControlViewModel = value;
        RaisePropertyChanged("CallControlViewModel");
      }
    }

    private ViewModelBase _btPairingViewModel;
    public ViewModelBase BtPairingViewModel
    {
      get
      {
        return _btPairingViewModel;
      }
      set
      {
        if (_btPairingViewModel == value)
          return;
        _btPairingViewModel = value;
        RaisePropertyChanged("BtPairingViewModel");
      }
    }


    private ViewModelBase _defaultDeviceInformationViewModel;
    public ViewModelBase DefaultDeviceInformationViewModel
    {
      get
      {
        return _defaultDeviceInformationViewModel;
      }
      set
      {
        if (_defaultDeviceInformationViewModel == value)
          return;
        _defaultDeviceInformationViewModel = value;
        RaisePropertyChanged("DefaultDeviceInformationViewModel");
      }
    }


    private ViewModelBase _buttonTakeOverViewModel;
    public ViewModelBase ButtonTakeOverViewModel
    {
      get
      {
        return _buttonTakeOverViewModel;
      }
      set
      {
        if (_buttonTakeOverViewModel == value)
          return;
        _buttonTakeOverViewModel = value;
        RaisePropertyChanged("ButtonTakeOverViewModel");
      }
    }

    private ViewModelBase _remoteMMIv2ViewModel;
    public ViewModelBase RemoteMMIv2ViewModel
    {
      get
      {
        return _remoteMMIv2ViewModel;
      }
      set
      {
        if (_remoteMMIv2ViewModel == value)
          return;
        _remoteMMIv2ViewModel = value;
        RaisePropertyChanged("RemoteMMIv2ViewModel");
      }
    }

    private ViewModelBase _authorizationViewModel;
    public ViewModelBase AuthorizationViewModel
    {
      get
      {
        return _authorizationViewModel;
      }
      set
      {
        if (_authorizationViewModel == value)
          return;
        _authorizationViewModel = value;
        RaisePropertyChanged("AuthorizationViewModel");
      }
    }


    private Model.MainWindow _mainWindow;
    public Model.MainWindow MainWindow
    {
      get { return _mainWindow; }
      set
      {
        _mainWindow = value;
        RaisePropertyChanged("MainWindow");
      }
    }

    private static ViewModelBase _firmwareUpdatesViewModel;
    public static ViewModelBase FirmwareUpdatesViewModel
    {
      get
      {
        return _firmwareUpdatesViewModel;
      }
      set
      {
        if (_firmwareUpdatesViewModel == value)
          return;
        _firmwareUpdatesViewModel = value;
        RaiseStaticPropertyChanged("FirmwareUpdatesViewModel");
      }
    }

    private static ViewModelBase _deviceLoggingViewModel;
    public static ViewModelBase DeviceLoggingViewModel
    {
      get
      {
        return _deviceLoggingViewModel;
      }
      set
      {
        if (_deviceLoggingViewModel == value)
          return;
        _deviceLoggingViewModel = value;
        RaiseStaticPropertyChanged("DeviceLoggingViewModel");
      }
    }

    private static ViewModelBase _whiteboardViewModel;
    public static ViewModelBase WhiteboardViewModel
    {
      get
      {
        return _whiteboardViewModel;
      }
      set
      {
        if (_whiteboardViewModel == value)
          return;
        _whiteboardViewModel = value;
        RaiseStaticPropertyChanged("WhiteboardViewModel");
      }
    }

    private static ViewModelBase _videoConfigurationViewModel;
    public static ViewModelBase VideoConfigurationViewModel
    {
      get
      {
        return _videoConfigurationViewModel;
      }
      set
      {
        if (_videoConfigurationViewModel == value)
          return;
        _videoConfigurationViewModel = value;
        RaiseStaticPropertyChanged("VideoConfigurationViewModel");
      }
    }

    public static ObservableCollection<FileUploadProgress> AllFileUploadDetails = new ObservableCollection<FileUploadProgress>();
    public static ObservableCollection<RemoteMMIv2Details> AllRemoteMMIv2Details = new ObservableCollection<RemoteMMIv2Details>();
    public static HashSet<IDevice> AvailableDevices = new HashSet<IDevice>();

    public static ConnectedDevicesControl SelectedUcConnectedDevicesControl { get; set; }

    private ObservableCollection<ConnectedDevicesControlViewModel> _connectedDeviceList = new ObservableCollection<ConnectedDevicesControlViewModel>();
    public ObservableCollection<ConnectedDevicesControlViewModel> ConnectedDeviceList
    {
      get { return _connectedDeviceList; }
      set
      {
        _connectedDeviceList = value;
        RaisePropertyChanged("ConnectedDeviceList");
      }
    }


    private int _selectedTabIndex;
    public int SelectedTabIndex
    {
      get
      {
        return _selectedTabIndex;
      }
      set
      {
        _selectedTabIndex = value;
        LoadSelectedTabItemContent();
      }
    }

    private bool _isFirstScanDone;
    public bool IsFirstScanDone
    {
      get { return _isFirstScanDone; }
      private set
      {
        _isFirstScanDone = value;
        RaisePropertyChanged("IsFirstScanDone");
      }
    }

    public static CallControlViewModel ViewModelRcc = new CallControlViewModel();
    public static DeviceInformationViewModel ViewModelDeviceInformation = new DeviceInformationViewModel();
    internal static DeviceSettingsViewModel ViewModelDeviceSettings;
    public static BtPairingViewModel ViewModelBtPairing = new BtPairingViewModel();
    internal static ButtonTakeOverViewModel ViewModelButtonTakeOver;
    internal static RemoteMMIv2ViewModel ViewModelRemoteMMIv2;
    public static AuthorizationViewModel ViewModelAuthorization;
    internal static FirmwareUpdatesViewModel ViewModelFirmwareUpdates;
    public static Dictionary<int, BtPairingHelper> BtPairingHelpers = new Dictionary<int, BtPairingHelper>();
    internal static DeviceLoggingViewModel ViewModelDeviceLogging;
    internal static WhiteboardViewModel ViewModelWhiteboard;
    internal static VideoConfigurationViewModel ViewModelVideoConfiguration;

    private IServiceFactory serviceFactory;
    public static IDeviceService DeviceService;
    public static IIntegrationService IntegrationServices;

    readonly string _logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JabraSDK", "Logging");
    int dataSize = 0;
    const int MAX_FILE_SIZE = 10000000; // 10 MB
    private string fileName = $"Log_{DateTime.Now}.txt";

    readonly string _dectInfoFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JabraSDK", "DectInfo");
    int dectDataSize = 0;
    private string dectFileName = $"DectInfo_{DateTime.Now}.txt";

        object lockObject = new object();
    private static ViewModelFactory _viewModelFactory;

    public static OpenFileDialog openFileDialog;

    #endregion

    #region ICommands

    private ICommand _windowLoaded;

    public ICommand WindowLoaded
    {
      get
      {
        return _windowLoaded ?? (_windowLoaded = new CommandHandler(() => WindowLoadedAction(), true));
      }
    }

    public ICommand WindowClosing
    {
      get
      {
        return new RelayCommand<CancelEventArgs>(
            (args) =>
            {
              if (SpecialHandlers.UnWrittenSettings.Count > 0)
              {
                if (SpecialHandlers.EnableDisableButton)
                {
                  var result = MessageBoxService.AskForConfirmation("Apply device settings before exiting?", "Apply Changes?", MessageBoxButton.YesNo, MessageBoxImage.Information);
                  if (result == MessageBoxResult.Yes)
                  {
                    BtPairingHelpers.Clear();
                    NativeCommonLibrary.UpdateSettingsToDevice(true);
                  }
                }
              }
              if (SpecialHandlers.ButtonConfigValues.Count > 0)
              {
                var deviceIdvalues = (from e in SpecialHandlers.ButtonConfigValues
                                      select e.DeviceId).Distinct().ToList();
                for (int j = 0; j < deviceIdvalues.Count; j++)
                {
                  NativeCommonLibrary.SetResetConfigurableButtons(false, (ushort)deviceIdvalues[j], true);
                }
              }
              if (DeviceService != null)
              {
                DeviceService.Dispose();
              }

            });
      }
    }

    private ICommand _loggingCommand;

    public ICommand LoggingCommand
    {
      get
      {
        return _loggingCommand ?? (_loggingCommand = new CommandHandlerArguments(param => LoggingCommandAction(param), true));
      }
    }

    private ICommand openFirmwareUpdateDialogCommand = null;
    public ICommand OpenFirmwareUpdateDialogCommand
    {
      get { return this.openFirmwareUpdateDialogCommand; }
      set { this.openFirmwareUpdateDialogCommand = value; }
    }

    private ICommand _enableStdHidEventsCommand;

    public ICommand EnableStdHIDEventsCommand
    {
      get
      {
        return _enableStdHidEventsCommand ?? (_enableStdHidEventsCommand = new CommandHandlerArguments(param => EnableStdHIDEventsCommandAction(param), true));
      }
    }

    #endregion

    static MainWindowViewModel()
    {
      _viewModelFactory = new ViewModelFactory();
      ViewModelAuthorization = _viewModelFactory.GetAuthorizationViewModel();
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MainWindowViewModel()
    {
      Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 1 });
      CommonMethods.ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;
      SetMandatoryInputs();
      CommonMethods.SetEnvironmentVariables();
      _mainWindow = new Model.MainWindow();
      DefaultDeviceInformationViewModel = new DefaultDeviceInformationViewModel();
    }

    private void WindowLoadedAction()
    {
      try
      {
        serviceFactory = new ServiceFactory();
        serviceFactory.SetClientId(CommonMethods.DemoAppId);
        serviceFactory.DetectNonJabraDevice = true;
        DeviceService = serviceFactory.CreateDeviceService();
        IntegrationServices = serviceFactory.CreateIntegrationService();
        DeviceService.FirstScanDone += OnFirstScanDone;
        DeviceService.DeviceRemoved += DeviceDetached;
        DeviceService.DeviceAdded += DeviceAttached;
        DeviceService.RawButtonInput += ButtonInDataRawHid;
        DeviceService.TranslatedButtonInput += ButtonInDataTranslated;
        DeviceService.ButtonConfigurationInput += ButtonGNPEvent;
        DeviceService.BluetoothPairingList += PairingListEvent;
        DeviceService.LoggingInput += LoggingData;
        DeviceService.FirmwareUpdateProgressInput += FirmwareUpdateProgressData;
        DeviceService.FileUploadProgressInput += FileUploadProgressData;
        DeviceService.DeviceLoggingInput += DeviceLoggingData;
        DeviceService.RemoteMmiv2Input += RemoteMmiv2Data;
        DeviceService.BatteryStatusUpdateInput += BatteryStatusData;
        DeviceService.DectInformationInput += DectInformationEvent;
        HeaderViewModel = new HeaderControlViewModel(DeviceService.SdkVersion);
        AuthorizationViewModel = ViewModelAuthorization;
        DeviceService.LoggingConfiguration(Logging.Local, true);

        if (!Directory.Exists(_logFilePath))
        {
          Directory.CreateDirectory(_logFilePath);
        }
        WriteLog(string.Empty, Path.Combine(_logFilePath, fileName.Replace(' ', '_').Replace(':', '_').Replace('/', '_').Replace('-', '_')));
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage($"MainWindow execption: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        Application.Current.MainWindow.Close();
      }
    }

    private void DectInformationEvent(object sender, DectInformationEventArgs e)
    {
      try
      {
        Application.Current.Dispatcher.BeginInvoke((Action)delegate
        {
          string msg = "DeviceId:" + e.DeviceId.ToString() + Environment.NewLine;
          string rawData = string.Join(" ", Array.ConvertAll(e.DectInformation.RawData, Convert.ToUInt32));
          if (e.DectInformation.DectInfoType == DectInfoType.DectDensity)
          {
            msg += "Dect density:" + rawData + Environment.NewLine;
            msg += "SumMeasuredRSSI:" + e.DectInformation.DectInfoDensity.SumMeasuredRSSI + Environment.NewLine;
            msg += "MaximumReferenceRSSI:" + e.DectInformation.DectInfoDensity.MaximumReferenceRSSI + Environment.NewLine;
            msg += "NumberMeasuredSlots:" + e.DectInformation.DectInfoDensity.NumberMeasuredSlots + Environment.NewLine;
            msg += "DataAgeSeconds:" + e.DectInformation.DectInfoDensity.DataAgeSeconds + Environment.NewLine;
          }
          else
          {
            msg += "Dect error count:" + rawData + Environment.NewLine;
            msg += "SyncError:" + e.DectInformation.DectInfoErrorCount.SyncError + Environment.NewLine;
            msg += "AErrors:" + e.DectInformation.DectInfoErrorCount.AErrors + Environment.NewLine;
            msg += "XErrors:" + e.DectInformation.DectInfoErrorCount.XErrors + Environment.NewLine;
            msg += "ZErrors:" + e.DectInformation.DectInfoErrorCount.ZErrors + Environment.NewLine;
            msg += "HUBSyncErrors:" + e.DectInformation.DectInfoErrorCount.HUBSyncErrors + Environment.NewLine;
            msg += "HUBAFieldErrors:" + e.DectInformation.DectInfoErrorCount.HUBAFieldErrors + Environment.NewLine;
            msg += "HandoverCount:" + e.DectInformation.DectInfoErrorCount.HandoverCount + Environment.NewLine;
          }
          LogDectInfo(msg);
        });
      }
      catch { }
    }

    private void BatteryStatusData(object sender, BatteryStatusUpdateEventArgs data)
    {
      try
      {
        Application.Current.Dispatcher.BeginInvoke((Action)delegate
        {
          ViewModelDeviceInformation.UpdateBatteryStatus(data);
        });
      }
      catch { }
    }

    private void RemoteMmiv2Data(object sender, RemoteMmiv2EventArgs eventArgs)
    {
	  Application.Current.Dispatcher.BeginInvoke((Action)delegate
	  {
		try
	    {
		  ViewModelRemoteMMIv2.UpdateRemoteMMIv2Data(eventArgs);
		  NativeCommonLibrary.PerformRMMIv2Function(eventArgs);
        }
		catch { }
	  });
    }

    private void DeviceLoggingData(object sender, DeviceLoggingEventArgs data)
    {
      try
      {
        Application.Current.Dispatcher.BeginInvoke((Action)delegate
        {
          ViewModelDeviceLogging?.UpdateDeviceLoggingData(data);
        });
      }
      catch { }
    }

    private void FirmwareUpdateProgressData(object sender, FirmwareUpdateProgressEventArgs data)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          ViewModelFirmwareUpdates.ShowProgress(data);

        }));
      }
      catch { }
    }

    private void FileUploadProgressData(object sender, FileUploadProgressEventArgs data)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          ViewModelDeviceSettings.ShowProgress(data);
        }));
      }
      catch { }
    }

    private void LoggingData(object sender, LoggingEventArgs e)
    {
      var msg = e.Data;
      Application.Current.Dispatcher.BeginInvoke((Action)delegate
      {
        WriteToFile(msg.ToString() + "\n");
      });
    }

    private void ButtonGNPEvent(object sender, ButtonConfigurationEventArgs buttonConfigurationEventArgs)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          NativeCommonLibrary.DisplayButtonConfig(buttonConfigurationEventArgs);
        }));
      }
      catch { }

    }

    private void PairingListEvent(object sender, BluetoothPairingListEventArgs bluetoothPairingListEventArgs)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          ViewModelBtPairing.ShowDeviceList(bluetoothPairingListEventArgs);
        }));
      }
      catch { }
    }

    private void ButtonInDataTranslated(object sender, TranslatedButtonInputEventArgs translatedButtonInputEventArgs)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          string message = CommonMethods.ConvertToHex(translatedButtonInputEventArgs.DeviceId, "X2") + ": " + translatedButtonInputEventArgs.ButtonId + ": " + Convert.ToInt32(translatedButtonInputEventArgs.Value);
          ViewModelRcc.UpdateTranslatedData(message);
          var value = translatedButtonInputEventArgs.Value.HasValue && translatedButtonInputEventArgs.Value.GetValueOrDefault(true);
          ViewModelRcc.SetRccButtonStatus((ushort)translatedButtonInputEventArgs.DeviceId, translatedButtonInputEventArgs.ButtonId.ToString(), value);
          ViewModelBtPairing.SetPairingStatus((ushort)translatedButtonInputEventArgs.DeviceId, translatedButtonInputEventArgs.ButtonId.ToString(), value);
        }));
      }
      catch { }
    }

    private void ButtonInDataRawHid(object sender, RawButtonInputEventArgs rawButtonInputEventArgs)
    {
      try
      {
        Application.Current.Dispatcher.Invoke((Action)(() =>
        {
          string message = CommonMethods.ConvertToHex(rawButtonInputEventArgs.DeviceId, "X2") + ": " + CommonMethods.ConvertToHex(rawButtonInputEventArgs.UsagePage, "X4") + ", " + CommonMethods.ConvertToHex(rawButtonInputEventArgs.Usage, "X4") + ", " + CommonMethods.ConvertToHex(Convert.ToInt32(rawButtonInputEventArgs.Value), "X4");
          ViewModelRcc.UpdateRawHidData(message);
        }));
      }
      catch { }
    }

    private void DeviceDetached(object sender, DeviceRemovedEventArgs deviceRemovedEventArgs)
    {
      IDevice device = deviceRemovedEventArgs.Device;
      try
      {
        AvailableDevices.Remove(device);
      }
      catch { }

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == device.DeviceId)
          {
            ViewModelDeviceSettings = null;
            DeviceSettingsViewModel = null;
            SpecialHandlers.UnWrittenSettings.Clear();
            SpecialHandlers.UnWrittenSettingsDeviceId = -1;
            CommonMethods.SelectedDevice = null;
            DialogService.CloseDialogs();
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var modelDeviceInformation = (from e in ViewModelDeviceInformation.AllDeviceInformations
                                        where e.DeviceId == device.DeviceId
                                        select e).FirstOrDefault();
          if (modelDeviceInformation != null)
          {
            ViewModelDeviceInformation.AllDeviceInformations.Remove(modelDeviceInformation);
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);


      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var modelRccSetting = (from e in ViewModelRcc.AllCallControls
                                 where e.DeviceId == device.DeviceId
                                 select e).FirstOrDefault();
          if (modelRccSetting != null)
          {
            ViewModelRcc.AllCallControls.Remove(modelRccSetting);
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var modelpairingInformation = (from e in ViewModelBtPairing.AllPairingInformations
                                         where e.DeviceId == device.DeviceId
                                         select e).FirstOrDefault();
          if (modelpairingInformation != null)
          {
            ViewModelBtPairing.AllPairingInformations.Remove(modelpairingInformation);
            BtPairingHelpers.Remove(device.DeviceId);
            SpecialHandlers.Show370Spinner = false;
            SpecialHandlers.ShowSpinner = false;
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var modelFirmwareUpdateInformation = (from e in ViewModelFirmwareUpdates.AllFirmwareUpdatesDetails
                                                where e.DeviceId == device.DeviceId
                                                select e).FirstOrDefault();
          if (modelFirmwareUpdateInformation != null)
          {
            ViewModelFirmwareUpdates.AllFirmwareUpdatesDetails.Remove(modelFirmwareUpdateInformation);
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {

        try
        {
          var remoteMmiInformation = (from e in SpecialHandlers.ButtonConfigValues
                                      where e.DeviceId == device.DeviceId
                                      select e).ToList();
          {
            if (remoteMmiInformation.Count > 0)
            {
              foreach (var value in remoteMmiInformation)
              {
                SpecialHandlers.ButtonConfigValues.Remove(value);
              }
            }
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var fileUploadInfoToBeRemoved = (from e in AllFileUploadDetails
                                           where e.DeviceId == device.DeviceId
                                           select e).FirstOrDefault();
          if (fileUploadInfoToBeRemoved != null)
          {
            AllFileUploadDetails.Remove(fileUploadInfoToBeRemoved);
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        try
        {
          var rmmiv2InfoToBeRemoved = (from e in AllRemoteMMIv2Details
                                       where e.DeviceId == device.DeviceId
                                       select e).FirstOrDefault();
          if (rmmiv2InfoToBeRemoved != null)
          {
            AllRemoteMMIv2Details.Remove(rmmiv2InfoToBeRemoved);
          }
        }
        catch { }
      }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
        {
          try
          {
            var modelDisConnectedDevice = (from e in ConnectedDeviceList
                                           where e.ConnectedDevicesControl.DeviceId == device.DeviceId
                                           select e).FirstOrDefault();
            if (modelDisConnectedDevice != null)
            {
              ConnectedDeviceList.Remove(modelDisConnectedDevice);
            }
          }
          catch { }
        }), DispatcherPriority.Send, null);

      Application.Current.Dispatcher.Invoke((Action)(() =>
      {
        bool isReadyForChange = true;
        try
        {
          var selectedDeviceDetails = (from det in ConnectedDeviceList
                                       where det.ConnectedDevicesControl.DeviceId == device.DeviceId
                                       select det.ConnectedDevicesControl).FirstOrDefault();

          if (selectedDeviceDetails == null)
          {

            if (CommonMethods.SelectedDevice != null)
            {
              var selectedDevice = (from e in ConnectedDeviceList
                                    where e.ConnectedDevicesControl.DeviceId == CommonMethods.SelectedDevice.DeviceId
                                    select e).FirstOrDefault();
              if (selectedDevice != null)
              {
                isReadyForChange = false;
              }
            }

            if (isReadyForChange)
            {
              var newDeviceDetails = (from det in ConnectedDeviceList
                                      select det).FirstOrDefault();

              if (newDeviceDetails != null)
              {
                CommonMethods.SelectedDevice = AvailableDevices.FirstOrDefault();
                SelectedUcConnectedDevicesControl = newDeviceDetails.ConnectedDevicesControl;
                SelectedUcConnectedDevicesControl.BackgroundColor = Brushes.Gray;
                if (CommonMethods.SelectedDevice != null)
                  ViewModelDeviceInformation.ShowDeviceDetails(CommonMethods.SelectedDevice);
              }
              else
              {
                SpecialHandlers.DeviceConnected = false;
              }
            }
          }
          //SpecialHandlers.DisplayFisrtTab = true;
        }
        catch { }
        if (isReadyForChange)
        {
          SpecialHandlers.DisplayFisrtTab = true;
        }
        GC.Collect();
      }), DispatcherPriority.Send);

    }

    private void AddDevice(IDevice device)
    {
      lock (device.Name)
      {
        try
        {
          if (!BtPairingHelpers.ContainsKey(device.DeviceId))
          {
            BtPairingHelpers.Add(device.DeviceId, new BtPairingHelper());
          }
          ConnectedDeviceList.Add(new ConnectedDevicesControlViewModel(device));

          if (device.IsUploadRingtoneSupported || device.IsUploadImageSupported)
          {
            FileUploadProgress progressUpdates = new FileUploadProgress();
            progressUpdates.DeviceId = device.DeviceId;
            AllFileUploadDetails.Add(progressUpdates);
          }

          if (_connectedDeviceList.Count == 1)
          {
            SelectedTabIndex = 0;
            SpecialHandlers.DisplayFisrtTab = true;
            CommonMethods.SelectedDevice = device;
            SelectedUcConnectedDevicesControl = _connectedDeviceList[0].ConnectedDevicesControl;
            SelectedUcConnectedDevicesControl.BackgroundColor = Brushes.Gray;
            ViewModelDeviceInformation = new DeviceInformationViewModel(device);
            DeviceInformationViewModel = ViewModelDeviceInformation;
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
              ViewModelRcc = new CallControlViewModel(device);
              CallControlViewModel = ViewModelRcc;
              ViewModelBtPairing = new BtPairingViewModel(device);
              BtPairingViewModel = ViewModelBtPairing;
              ViewModelFirmwareUpdates = _viewModelFactory.GetFirmwareUpdatesViewModel(device.DeviceId);
              FirmwareUpdatesViewModel = ViewModelFirmwareUpdates;
              ViewModelDeviceLogging = new DeviceLoggingViewModel(device);
              DeviceLoggingViewModel = ViewModelDeviceLogging;
              ViewModelWhiteboard = new WhiteboardViewModel(device);
              WhiteboardViewModel = ViewModelWhiteboard;
              ViewModelVideoConfiguration = new VideoConfigurationViewModel(device);
              VideoConfigurationViewModel = ViewModelVideoConfiguration;
              SpecialHandlers.DeviceConnected = true;
              AuthorizationViewModel = ViewModelAuthorization;
            }), DispatcherPriority.Background, null);
          }
          else if (_connectedDeviceList.Count > 1)
          {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
              ViewModelDeviceInformation.UpdateDeviceInformations(device);
              ViewModelRcc.UpdateRccSettings(device.DeviceId);
              ViewModelBtPairing.ShowPairingDetails(CommonMethods.SelectedDevice);
              ViewModelBtPairing.UpdateDeviceInformations(device);
              ViewModelFirmwareUpdates.UpdateFirmwareUpdatesDetails(device.DeviceId);
            }), DispatcherPriority.Background, null);
          }
        }
        catch { }
      }
    }

    private void DeviceAttached(object sender, DeviceAddedEventArgs deviceAddedEventArgs)
    {
      IDevice device = deviceAddedEventArgs.Device;
      lock (lockObject)
      {
        if (string.IsNullOrEmpty(device.Name))
        {
          return;
        }
        Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        if (device.ErrorStatus != ErrorStatus.NoError && !device.IsInFirmwareUpdateMode)
        {
          MessageBoxService.ShowMessage(device.GetErrorMessage(), device.Name + " Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }), DispatcherPriority.Background, null);

        if (!AvailableDevices.Contains(device))
        {
          AvailableDevices.Add(device);
          Application.Current.Dispatcher.Invoke((Action)(() =>
         {
           AddDevice(device);
         }), DispatcherPriority.Background, null);
        }
        GC.Collect();
      }
    }

    private void OnFirstScanDone(object sender, EventArgs e)
    {
      IsFirstScanDone = true;
    }


    public static void UpdateApplySettingsControl(bool isError)
    {
      if (SpecialHandlers.UnWrittenSettings.Count > 0 && !isError && SpecialHandlers.ValidationErrors.Count == 0)
      {
        SpecialHandlers.EnableDisableButton = true;
      }
      else
      {
        SpecialHandlers.EnableDisableButton = false;
      }
    }


    public void LoadSelectedTabItemContent()
    {
      Application.Current.Dispatcher.BeginInvoke((Action)(() =>
      {
        if (CommonMethods.SelectedDevice == null)
          return;
        IDevice selectedDevice = CommonMethods.SelectedDevice;
        if (SpecialHandlers.UnWrittenSettings.Count > 0 && (_selectedTabIndex != 1))
        {
          if (SpecialHandlers.EnableDisableButton)
          {
            var result = MessageBoxService.AskForConfirmation("Apply device settings before exiting?", "Apply Changes?", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
              NativeCommonLibrary.UpdateSettingsToDevice(false);
            }
            else if (result == MessageBoxResult.No)
            {
              SpecialHandlers.UnWrittenSettings.Clear();
              SpecialHandlers.UnWrittenSettingsDeviceId = -1;
              SpecialHandlers.EnableDisableButton = false;
            }
          }
        }
        if (_connectedDeviceList.Count == 0)
        {
          _selectedTabIndex = 0;
        }
        else
        {
          if (_selectedTabIndex == 0 && selectedDevice != null)
          {
            ViewModelDeviceInformation.ShowDeviceDetails(selectedDevice);
          }
          else if (_selectedTabIndex == 1 && selectedDevice != null)
          {
            SpecialHandlers.SettingsLoaded = false;
            DeviceSettingsViewModel = ViewModelDeviceSettings = null;
            ViewModelDeviceSettings = new DeviceSettingsViewModel(selectedDevice, null);
            DeviceSettingsViewModel = ViewModelDeviceSettings;
            ViewModelDeviceSettings.ShowFileUploadDetails(selectedDevice.DeviceId);
          }
          else if (_selectedTabIndex == 2 && selectedDevice != null)
          {
            // Refresh integration state services in call control tab
            IntegrationService.ShowIntegrationServices();
            ViewModelRcc.ShowRccSettings(selectedDevice.DeviceId);
          }
          else if (_selectedTabIndex == 3 && selectedDevice != null)
          {
            // Refresh pairing information in BT pairing tab
            ViewModelBtPairing.ShowPairingDetails(selectedDevice);
          }
          else if (_selectedTabIndex == 4 && selectedDevice != null)
          {
            if (selectedDevice.IsButtonConfigurationSupported)
            {
              ViewModelButtonTakeOver = new ButtonTakeOverViewModel(selectedDevice);
              ButtonTakeOverViewModel = ViewModelButtonTakeOver;
              SpecialHandlers.IsRMMISupported = true;
            }
            else if (selectedDevice.IsFeatureSupported(DeviceFeature.RemoteMMIv2))
            {
              ViewModelRemoteMMIv2 = new RemoteMMIv2ViewModel(selectedDevice);
              RemoteMMIv2ViewModel = ViewModelRemoteMMIv2;
              SpecialHandlers.IsRMMISupported = true;
            }
            else
            {
              SpecialHandlers.IsRMMISupported = false;
            }
          }
          else if (_selectedTabIndex == 5 && selectedDevice != null)
          {
            ViewModelFirmwareUpdates.ShowFirmwareUpdatesDetails(selectedDevice.DeviceId);
          }
          else if (_selectedTabIndex == 7 && selectedDevice != null)
          {
            ViewModelDeviceLogging.ShowLoggingDetails(selectedDevice.DeviceId);
          }
          else if(_selectedTabIndex == 8 && selectedDevice != null)
          {
            ViewModelWhiteboard.ShowWhiteboardDetails(selectedDevice.DeviceId);
          }
          else if(_selectedTabIndex == 9 && selectedDevice != null)
          {
            ViewModelVideoConfiguration.ShowVideoConfigurationDetails(selectedDevice.DeviceId);
          }
        }

        if (_selectedTabIndex == 6)
        {
          ViewModelAuthorization.LoadAuthorizationContent();
        }
      }), DispatcherPriority.Send, null);
    }

    private void LoggingCommandAction(object param)
    {
      Logging flag = Logging.Local;
      string[] arr = ((IEnumerable)param).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
      switch (arr[0].ToString())
      {
        case "File":
          flag = Logging.Local;
          break;
        case "Cloud":
          flag = Logging.Cloud;
          break;
        case "All":
          flag = Logging.All;
          break;
      }
      DeviceService.LoggingConfiguration(flag, Convert.ToBoolean(arr[1]));
    }

    private void EnableStdHIDEventsCommandAction(object param)
    {
      try
      {
        string[] arr = ((IEnumerable)param).Cast<object>()
                                   .Select(x => x.ToString())
                                   .ToArray();
        switch (arr[0].ToString())
        {
          case "Jabra":
            DeviceService.SetStdHIDEventsFromJabraDevices(Convert.ToBoolean(arr[1]));
            break;
          case "NonJabra":
            DeviceService.SetHIDEventsFromNonJabraDevices(Convert.ToBoolean(arr[1]));
            break;
          default:
            break;
        }
      }
      catch (Exception e)
      {
        MessageBoxService.ShowMessage("Exception occurred\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void LogDectInfo(string data)
    {
        try
        {
            dectDataSize += data.Length;
            if (dectDataSize > MAX_FILE_SIZE)
            {
                dectDataSize = data.Length;
                dectFileName = $"DectInfo_{DateTime.Now}.txt";
            }
            WriteLog(data, Path.Combine(_dectInfoFilePath, dectFileName.Replace(' ', '_').Replace(':', '_').Replace('/', '_').Replace('-', '_')));
        }
        catch (Exception ex)
        {
            MessageBoxService.ShowMessage($"Error when writing to file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void WriteToFile(string data)
    {
      try
      {
        dataSize += data.Length;
        if (dataSize > MAX_FILE_SIZE)
        {
          dataSize = data.Length;
          fileName = $"Log_{DateTime.Now}.txt";
        }
        WriteLog(data, Path.Combine(_logFilePath, fileName.Replace(' ', '_').Replace(':', '_').Replace('/', '_').Replace('-', '_')));
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage($"Error when writing to file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public static void WriteLog(string strLog, string logFilePath)
    {
      StreamWriter log;
      FileStream fileStream = null;
      DirectoryInfo logDirInfo = null;
      FileInfo logFileInfo;

      logFileInfo = new FileInfo(logFilePath);
      logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
      if (!logDirInfo.Exists) logDirInfo.Create();
      if (!logFileInfo.Exists)
      {
        fileStream = logFileInfo.Create();
      }
      else
      {
        fileStream = new FileStream(logFilePath, FileMode.Append);
      }
      log = new StreamWriter(fileStream);
      log.WriteLine(strLog);
      log.Close();
    }

    /// <summary>
    /// Inputs from Jabra needs to be set here to continue using the application
    /// </summary>
    private void SetMandatoryInputs()
    {
      string appId = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String("WWh1ZnZONENWR2hyczJBOE5iZ1hHeURRMWJqQ2tYc3FyY1ZOdVhpbW0yYz0="));
      if (appId == "" || appId == null)
      {
        MessageBoxService.ShowMessage("You need to provide a key from the Jabra Developer Portal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }
      CommonMethods.DemoAppId = appId;
    }
  }
}
