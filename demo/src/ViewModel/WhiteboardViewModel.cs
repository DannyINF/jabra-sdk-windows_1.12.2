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
  public class WhiteboardViewModel : ViewModelBase
  {
    private Whiteboard _whiteboard;
    public Whiteboard Whiteboard
    {
      get { return _whiteboard; }
      set
      {
        _whiteboard = value;
        OnPropertyChanged("Whiteboard");
      }
    }

    private bool _canExecute;

    private object lockObject = new object();

    public WhiteboardViewModel(IDevice device)
    {
      _whiteboard = new Whiteboard();
      _canExecute = true;
      _whiteboard.WhiteboardSupported = device.IsFeatureSupported(DeviceFeature.Whiteboard);
    }

    private ICommand _clickSetPositionCommand;
    public ICommand ClickSetPositionCommand
    {
      get
      {
        return _clickSetPositionCommand ?? (_clickSetPositionCommand = new CommandHandlerArguments(param => ClickSetPositionCommandHandler(param), _canExecute));
      }
    }

    private ICommand _clickGetPositionCommand;
    public ICommand ClickGetPositionCommand
    {
      get
      {
        return _clickGetPositionCommand ?? (_clickGetPositionCommand = new CommandHandlerArguments(param => ClickGetPositionCommandHandler(param), _canExecute));
      }
    }

    public void ClickSetPositionCommandHandler(object o)
    {
      try
      {
        WhiteboardPosition wp = new WhiteboardPosition
        {
          LowerLeftCornerX = Whiteboard.LLX,
          LowerLeftCornerY = Whiteboard.LLY,
          LowerRightCornerX = Whiteboard.LRX,
          LowerRightCornerY = Whiteboard.LRY,
          UpperRightCornerX = Whiteboard.URX,
          UpperRightCornerY = Whiteboard.URY,
          UpperLeftCornerX = Whiteboard.ULX,
          UpperLeftCornerY = Whiteboard.ULY
        };
        CommonMethods.SelectedDevice.SetWhiteboardPosition(Whiteboard.WhiteboardNumber, wp);
      } catch (Exception ex) { 
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void ClickGetPositionCommandHandler(object o)
    {
      try
      {
        WhiteboardPosition wp = CommonMethods.SelectedDevice.GetWhiteboardPosition(Whiteboard.WhiteboardNumber);
        Whiteboard.LLX = wp.LowerLeftCornerX;
        Whiteboard.LLY = wp.LowerLeftCornerY;
        Whiteboard.LRX = wp.LowerRightCornerX;
        Whiteboard.LRY = wp.LowerRightCornerY;
        Whiteboard.URX = wp.UpperRightCornerX;
        Whiteboard.URY = wp.UpperRightCornerY;
        Whiteboard.ULX = wp.UpperLeftCornerX;
        Whiteboard.ULY = wp.UpperLeftCornerY;
      }
      catch (Exception ex)
      {
        MessageBoxService.ShowMessage(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }


    public void ShowWhiteboardDetails(int deviceId)
    {
      IDevice device = CommonMethods.SelectedDevice;
      _whiteboard.WhiteboardSupported = device.IsFeatureSupported(DeviceFeature.Whiteboard);

      lock (lockObject)
      {
        if (CommonMethods.SelectedDevice != null && CommonMethods.SelectedDevice.DeviceId == deviceId)
        {

        }
      }
    }

    private List<WhiteboardDetails> _allWhiteboardDetails;
    public List<WhiteboardDetails> AllWhiteboardDetails
    {
      get { return _allWhiteboardDetails; }
      set
      {
        _allWhiteboardDetails = value;
        OnPropertyChanged("AllWhiteboardDetails");
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
