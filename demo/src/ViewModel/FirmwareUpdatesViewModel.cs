using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using JabraSDK;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using System.Windows.Forms;

namespace Jabra_SDK_Demo.ViewModel
{
  public class FirmwareUpdatesViewModel : ViewModelBase
  {

    #region Properties

    private FirmwareUpdate _firmwareUpdate;
    public FirmwareUpdate FirmwareUpdate
    {
      get { return _firmwareUpdate; }
      set
      {
        _firmwareUpdate = value;
        OnPropertyChanged("FirmwareUpdate");
      }
    }

    private List<FirmwareUpdates> _allFirmwareUpdatesDetails;
    public List<FirmwareUpdates> AllFirmwareUpdatesDetails
    {
      get { return _allFirmwareUpdatesDetails; }
      set
      {
        _allFirmwareUpdatesDetails = value;
        OnPropertyChanged("AllFirmwareUpdatesDetails");
      }
    }

    #endregion

    #region ICommand   

    private ICommand _clickUpdateFirmwareCommand = null;
    public ICommand ClickUpdateFirmwareCommand
    {
      get { return _clickUpdateFirmwareCommand; }
      set { _clickUpdateFirmwareCommand = value; }
    }

    private ICommand _clickDownloadCancelCommand = null;
    public ICommand ClickDownloadCancelCommand
    {
      get { return this._clickDownloadCancelCommand; }
    }

    private ICommand _clickCheckForFirmwareUpdateCommand = null;
    public ICommand ClickCheckForFirmwareUpdateCommand
    {
      get { return this._clickCheckForFirmwareUpdateCommand; }
    }
    #endregion

    #region Constructor

    public FirmwareUpdatesViewModel()
    {
    }

    public FirmwareUpdatesViewModel(int deviceId)
    {
      this._clickUpdateFirmwareCommand = new RelayCommand(ClickUpdateFirmwareAction);
      this._clickDownloadCancelCommand = new RelayCommand(ClickDownloadCancelAction);
      this._clickCheckForFirmwareUpdateCommand = new RelayCommand(ClickCheckForFirmwareUpdateAction);
      AllFirmwareUpdatesDetails = new List<FirmwareUpdates>();
      _firmwareUpdate = new FirmwareUpdate();
      UpdateFirmwareUpdatesDetails(deviceId);
      ShowFirmwareUpdatesDetails(deviceId);
    }

    #endregion

    internal void UpdateFirmwareUpdatesDetails(int deviceId)
    {
      FirmwareUpdates firmwareUpdates = new FirmwareUpdates();
      firmwareUpdates.DeviceId = deviceId;
      firmwareUpdates.DownloadCancel = "Download";
      firmwareUpdates.SelectedIndex = 0;
      AllFirmwareUpdatesDetails.Add(firmwareUpdates);
    }

    public void ShowFirmwareUpdatesDetails(int deviceId)
    {
      if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == deviceId)
      {
        var result = (from list in AllFirmwareUpdatesDetails
                      where list.DeviceId == deviceId
                      select list).SingleOrDefault();
        if (result != null)
        {
          _firmwareUpdate.DeviceId = result.DeviceId;
          _firmwareUpdate.FirmwareInformation = result.FirmwareInformation;
          _firmwareUpdate.ProgressVisibility = result.ProgressVisibility;
          _firmwareUpdate.FileDownloaded = result.FileDownloaded;
          _firmwareUpdate.DownloadCancel = result.DownloadCancel;
          _firmwareUpdate.Version = result.Version;
          _firmwareUpdate.Progress = result.Progress;
          _firmwareUpdate.Message = result.Message;
          _firmwareUpdate.FileDownloadInProgress = result.FileDownloadInProgress;
          _firmwareUpdate.SelectedItem = result.SelectedItem;
          _firmwareUpdate.SelectedIndex = result.SelectedIndex;
          _firmwareUpdate.IsFwLockEnabled = CommonMethods.SelectedDevice.IsFirmwareLockEnabled ? "Enabled" : "Disabled";
        }
      }
    }

    public void ShowProgress(FirmwareUpdateProgressEventArgs args)
    {
      var result = (from list in AllFirmwareUpdatesDetails
                    where list.DeviceId == args.DeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        result.Progress = args.Percentage;
        result.Message = args.Message;
        if (args.Status != FirmwareEventStatus.InProgress && args.Status != FirmwareEventStatus.Initiating)
        {
          result.FileDownloaded = true;
          result.FileDownloadInProgress = false;
          result.DownloadCancel = "Download";
        }
        else if(args.Status == FirmwareEventStatus.NotAllowed || args.Status == FirmwareEventStatus.FileDownloadInProgress) { 
          MessageBoxService.ShowMessage("Firmware download for the same file is in progress.\nPlease try after some time.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
      ShowFirmwareUpdatesDetails(args.DeviceId);
    }
        
    private void ClickCheckForFirmwareUpdateAction(object parameter)
    {
      if (CommonMethods.SelectedDevice == null)
      {
        return;
      }

      IDevice device = CommonMethods.SelectedDevice;
      try
      {
        var result = (from list in AllFirmwareUpdatesDetails
                      where list.DeviceId == device.DeviceId
                      select list).FirstOrDefault();
        if (result != null)
        {
          if (result.FileDownloadInProgress)
          {
            if (MessageBoxService.AskForConfirmation("Firmware download in progress.\n Do you want to cancel the download and continue?", "Check for Update", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
              device.CancelUpdate();
              result.FileDownloadInProgress = false;
              result.FirmwareInformation = null;
              result.ProgressVisibility = false;
              ShowFirmwareUpdatesDetails(device.DeviceId);
            }
            else
            {
              return;
            }
          }
          else
          {
            result.ProgressVisibility = false;
          }
        }
        bool isUpdateAvailable = device.IsFirmwareAvailableForUpdate(SpecialHandlers.AuthorizationToken);

        if (isUpdateAvailable)
        {
          ShowAvailableFilesinCloud();
        }
        else
        {
          if (MessageBoxService.AskForConfirmation("No Update available.\n Do you want to Update?", "Check for Update", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
          {
            ShowAvailableFilesinCloud();
          }
          else
          {
            var modelFirmwareUpdateInformation = (from e in AllFirmwareUpdatesDetails
                                                  where e.DeviceId == device.DeviceId
                                                  select e).FirstOrDefault();
            if (modelFirmwareUpdateInformation != null)
            {
              AllFirmwareUpdatesDetails.Remove(modelFirmwareUpdateInformation);
            }
            UpdateFirmwareUpdatesDetails(device.DeviceId);
            ShowFirmwareUpdatesDetails(device.DeviceId);
            return;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage("Failed to Check For Firmware Update\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void ShowAvailableFilesinCloud()
    {
      if (CommonMethods.SelectedDevice == null)
        return;
      IDevice device = CommonMethods.SelectedDevice;
      var result = (from list in AllFirmwareUpdatesDetails
                    where list.DeviceId == device.DeviceId
                    select list).FirstOrDefault();
      if (result != null)
      {
        List<IFirmware> firmwareInfos = new List<IFirmware>();
        IFirmware firmwareInfo = device.GetLatestFirmware(SpecialHandlers.AuthorizationToken);
        firmwareInfos.Add(firmwareInfo);
        if (firmwareInfos.Count > 0)
        {
          result.FirmwareInformation = firmwareInfos;
          ShowFirmwareUpdatesDetails(device.DeviceId);
        }
        else
        {
          MessageBoxService.ShowMessage("Failed to Check For Firmware Update\n", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    protected DeviceStatus UpdateFirmware(IDevice device, string filePath) {
      DeviceStatus status = DeviceStatus.ReturnOk;
      try
      {
        status = device.UpdateDevice(filePath);
      }catch(Exception ex)
      {
        MessageBoxService.ShowMessage("Failed to Update Firmware\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
      return status;
    }

    public void ClickUpdateFirmwareAction(object parameter)
    {
      if (CommonMethods.SelectedDevice == null)
        return;
      IDevice device = CommonMethods.SelectedDevice;

      if (_firmwareUpdate.FirmwareInformation == null)
      {
        MessageBoxService.ShowMessage("No Firmware Version available for update", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
      }
      else
      {
        if (_firmwareUpdate.SelectedItem != null)
        {
          var result = (from list in AllFirmwareUpdatesDetails
                        where list.DeviceId == device.DeviceId
                        select list).FirstOrDefault();
          if (result != null)
          {
            result.Version = _firmwareUpdate.SelectedItem.Version;
            if (result.FileDownloadInProgress)
            {
              if (
                MessageBoxService.AskForConfirmation(
                  "Firmware download in progress.\nDo you want to cancel the download and Update Firmware?",
                  "Update Firmware", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
              {
                device.CancelUpdate();
                result.FileDownloadInProgress = false;
                result.ProgressVisibility = false;
                ShowFirmwareUpdatesDetails(device.DeviceId);
                string filePath = device.GetFirmwareFilePath(_firmwareUpdate.Version.ToString());
                UpdateFirmware(device, filePath);
              }
              else
              {
                return;
              }
            }
            else
            {
              result.ProgressVisibility = false;
              ShowFirmwareUpdatesDetails(device.DeviceId);
              string filePath = device.GetFirmwareFilePath(_firmwareUpdate.Version.ToString());
              DeviceStatus status = UpdateFirmware(device, filePath);
              if (status == DeviceStatus.FirmwareUpdaterApplicationNotAvailable)
              {
                MessageBoxService.ShowMessage("Firmware Updater Application Not Available.", "Information",
                  MessageBoxButton.OK, MessageBoxImage.Information);
              }
              else if (status == DeviceStatus.FileNotAccessible)
              {
                MessageBoxService.ShowMessage("Firmware file is not available in the downloaded path", "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
              }
            }
          }
        }
        else
        {
          MessageBoxService.ShowMessage("Please select a Version to update", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
      }
    }

    public void ClickDownloadCancelAction(object parameter)
    {
      if (CommonMethods.SelectedDevice == null)
        return;
      IDevice device = CommonMethods.SelectedDevice;
      DeviceStatus status = DeviceStatus.ReturnOk;
      string message = string.Empty;
      try
      {
        if (_firmwareUpdate.FirmwareInformation == null)
        {
          MessageBoxService.ShowMessage("No Firmware Version available for download", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
          var result = (from list in AllFirmwareUpdatesDetails
                        where list.DeviceId == device.DeviceId
                        select list).FirstOrDefault();
          if (result != null)
          {
            switch (result.DownloadCancel.ToLower(CultureInfo.InvariantCulture))
            {
              case "download":
                message = "Firmware Download failed";
                if (_firmwareUpdate.SelectedItem != null)
                {
                  result.SelectedItem = _firmwareUpdate.SelectedItem;
                  result.Progress = 0;
                  result.Message = String.Empty;
                  result.Version = _firmwareUpdate.SelectedItem.Version;
                  result.FileDownloadInProgress = true;
                  status = device.DownloadFirmware(result.Version.ToString(), SpecialHandlers.AuthorizationToken);

                  _firmwareUpdate.DownloadCancel = result.DownloadCancel = "Cancel Download";
                  _firmwareUpdate.ProgressVisibility = result.ProgressVisibility = true;
                  ShowFirmwareUpdatesDetails(device.DeviceId);
                }
                else
                {
                  MessageBoxService.ShowMessage("Please select a Version to download", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                break;
              case "cancel download":
                message = "Firmware Download cancel failed";
                status = device.CancelUpdate();
                result.DownloadCancel = "Download";
                result.FileDownloadInProgress = false;
                ShowFirmwareUpdatesDetails(device.DeviceId);
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(message + "\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
