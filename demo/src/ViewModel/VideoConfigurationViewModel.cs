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
 
  public class VideoConfigurationViewModel : ViewModelBase
  {
    private VideoConfiguration _videoConfiguration;
    public VideoConfiguration VideoConfiguration
    {
      get { return _videoConfiguration; }
      set
      {
        _videoConfiguration = value;
        OnPropertyChanged("VideoConfiguration");
      }
    }

    private bool _canExecute;

    private object lockObject = new object();

    public VideoConfigurationViewModel(IDevice device)
    {
      _videoConfiguration = new VideoConfiguration();
      DeviceConstants = new ObservableCollection<DevConstant>();
      _canExecute = true;
      _videoConfiguration.VideoSupported = device.IsFeatureSupported(DeviceFeature.Video);
      Zoom     = new SimpleValueViewModel("Zoom Level", MkSet(v => CommonMethods.SelectedDevice.SetZoom((ushort)v)), new IntGetter(() => CommonMethods.SelectedDevice.GetZoom()));
      var wbSetter = new IntSetter(null);
      var enumRef = new EnumRef<AutoWhiteBalance>(null);
      var wbAutoSetter = new EnumSetter(enumRef);
      var wbMultiSetter = new MultiSetter(
	      () => CommonMethods.SelectedDevice.SetWhiteBalance(new WhiteBalanceType{ WhiteBalance = wbSetter.TValue.Value, Auto = enumRef.EnumVal }),
	      new Setter[]{wbSetter, wbAutoSetter}
	  );
      var wbGetter = new IntGetter(null);
      var enumGetRef = new EnumRef<AutoWhiteBalance>(null);
      var wbAutoGetter = new TextGetter(() => enumGetRef.CurItem);
      var wbMultiGetter = new MultiGetter(() =>
      {
	      var wbt = CommonMethods.SelectedDevice.GetWhiteBalance();
	      wbGetter.TValue = wbt.WhiteBalance;
	      wbAutoGetter.TValue = enumGetRef.CurItem = wbt.Auto.ToString();
      }, new Getter[]{wbGetter, wbAutoGetter});
      ColorControls = new SimpleValueViewModel[]
      {
          // don't fall for the temptation to reduce
          //   v => CommonMethods.SelectedDevice.f(v)
          // to
          //   CommonMethods.SelectedDevice.f
          // It binds CommonMethod.SelectedDevice too early...
        new SimpleValueViewModel("Contrast Level", MkSet(v => CommonMethods.SelectedDevice.SetContrastLevel(v)), new IntGetter(() => CommonMethods.SelectedDevice.GetContrastLevel())),
        new SimpleValueViewModel("Sharpness Level", MkSet(v => CommonMethods.SelectedDevice.SetSharpnessLevel(v)),  new IntGetter(() => CommonMethods.SelectedDevice.GetSharpnessLevel())),
        new SimpleValueViewModel("Brightness Level", MkSet(v => CommonMethods.SelectedDevice.SetBrightnessLevel(v)),  new IntGetter(() => CommonMethods.SelectedDevice.GetBrightnessLevel())),
        new SimpleValueViewModel("Saturation Level", MkSet(v => CommonMethods.SelectedDevice.SetSaturationLevel(v)),  new IntGetter(() => CommonMethods.SelectedDevice.GetSaturationLevel())),
		new SimpleValueViewModel("WhiteBalance", wbMultiSetter, wbMultiGetter),
      };
      RoomCountControls = new SimpleValueViewModel[]
      {
        new SimpleValueViewModel("Room Capacity", MkSet(v => CommonMethods.SelectedDevice.SetRoomCapacity(v)), MkGet(() => CommonMethods.SelectedDevice.GetRoomCapacity())),
        new SimpleValueViewModel("  Notification", MkSet(v => CommonMethods.SelectedDevice.SetRoomCapacityNotificationEnabled(v)), MkGet(() => CommonMethods.SelectedDevice.GetRoomCapacityNotificationEnabled())),
        new SimpleValueViewModel("  Style", MkSet<NotificationStyle>(v => CommonMethods.SelectedDevice.SetNotificationStyle(v)), MkGet(() => CommonMethods.SelectedDevice.GetNotificationStyle().ToString())),
        new SimpleValueViewModel("  Usage", MkSet<NotificationUsage>(v => CommonMethods.SelectedDevice.SetNotificationUsage(v)), MkGet(() => CommonMethods.SelectedDevice.GetNotificationUsage().ToString())),
      };
      UpdConstants(device);
    }

    private void UpdConstants(IDevice device)
    {
	    DeviceConstants.Clear();
	    using (var jcc = device.GetConstants())
	    {
		    void MkConst(string s, Func<JabraConst, string> fmt)
		    {
			    var c = jcc.GetConst(s);
			    if (c != null) DeviceConstants.Add(new DevConstant(s, fmt(c)));
		    }

		    MkConst("video-resolution", c => $"{c.GetField("width").AsInt()} x {c.GetField("height").AsInt()}");
		    MkConst("slyrfing", c => $"{c.AsBool()}");
		    MkConst("slyrf", c => $"{c.AsString()}");
		    MkConst("slyrf-factor", c => $"{c.AsInt()}");
	    }

    }

    public static Setter MkSet(Action<int> action) { return new IntSetter(v => action(v.Value)); }
    public static Setter MkSet(Action<bool> action) { return new BoolSetter(v => action(v.Value)); }
    public static Setter MkSet<T>(Action<T> action) where T : struct, Enum { return new EnumSetter(new EnumRef<T>(action)); }

    public static Getter MkGet(Func<int> g) { return new IntGetter(() => g());}
    public static Getter MkGet(Func<bool> g) { return new BoolGetter(() => g());}
    public static Getter MkGet(Func<string> g) { return new TextGetter(() => g());}

    public IEnumerable<SimpleValueViewModel> ColorControls { get; }
    public IEnumerable<SimpleValueViewModel> RoomCountControls { get; }

    private IEnumerable<DevConstant> _deviceConstants;
    public ObservableCollection<DevConstant> DeviceConstants
    {
	    get;
    }

    private ICommand _clickSetZoomCommand;
    public ICommand ClickSetZoomCommand
    {
      get
      {
        return _clickSetZoomCommand ?? (_clickSetZoomCommand = new CommandHandlerArguments(param => ClickSetZoomCommandHandler(param), _canExecute));
      }
    }

    private ICommand _clickGetZoomCommand;
    public ICommand ClickGetZoomCommand
    {
      get
      {
        return _clickGetZoomCommand ?? (_clickGetZoomCommand = new CommandHandlerArguments(param => ClickGetZoomCommandHandler(param), _canExecute));
      }
    }

    private ICommand _clickGetZoomLimitsCommand;
    public ICommand ClickGetZoomLimitsCommand
    {
      get
      {
        return _clickGetZoomLimitsCommand ?? (_clickGetZoomLimitsCommand = new CommandHandlerArguments(param => ClickGetZoomLimitsCommandHandler(param), _canExecute));
      }
    }

    public void ClickSetZoomCommandHandler(object o)
    {
      CommonMethods.SelectedDevice.SetZoom(VideoConfiguration.ZoomLevel);
    }

    public void ClickGetZoomCommandHandler(object o)
    {
      VideoConfiguration.ReadZoomLevel = CommonMethods.SelectedDevice.GetZoom();
    }

    public void ClickGetZoomLimitsCommandHandler(object o)
    {
      ZoomLimits zl = CommonMethods.SelectedDevice.GetZoomLimits();
      VideoConfiguration.MinZoomLevel = zl.Min;
      VideoConfiguration.MaxZoomLevel = zl.Max;
      VideoConfiguration.ZoomStepSize = zl.StepSize;
    }

    public void UpdateWhiteboardData(DeviceLoggingEventArgs e)
    {
      try
      {
        lock (lockObject)
        {
          string message = JsonConvert.DeserializeObject(e.Data).ToString();
          _videoConfiguration.Data?.Insert(0, message);
          AllVideoConfigurationDetails.Add(new WhiteboardDetails { DeviceId = e.DeviceId, Data = message });
        }
      }
      catch { }
    }

    private ICommand _clickGetPanTiltCommand;

    public ICommand ClickGetPanTiltCommand => _clickGetPanTiltCommand ?? (_clickGetPanTiltCommand = new CommandHandlerArguments(ClickGetPanTiltCommandHandler, _canExecute));

    private void ClickGetPanTiltCommandHandler(object o)
    {
      var pt = CommonMethods.SelectedDevice.GetPanTilt();
      VideoConfiguration.ReadPan = pt.Pan;
      VideoConfiguration.ReadTilt = pt.Tilt;
    }

    private ICommand _clickSetPanTiltCommand;

    public ICommand ClickSetPanTiltCommand => _clickSetPanTiltCommand ?? (_clickSetPanTiltCommand = new CommandHandlerArguments(ClickSetPanTiltCommandHandler, _canExecute));

    private void ClickSetPanTiltCommandHandler(object o)
    {
      CommonMethods.SelectedDevice.SetPanTilt(VideoConfiguration.Pan, VideoConfiguration.Tilt);
    }

    private ICommand _clickGetPanTiltLimitsCommand;

    public ICommand ClickGetPanTiltLimitsCommand => _clickGetPanTiltLimitsCommand ?? (_clickGetPanTiltLimitsCommand = new CommandHandlerArguments(ClickGetPanTiltLimitsCommandHandler, _canExecute));
    public ICommand ClickApplyColorControlCommand => new CommandHandler(() => CommonMethods.SelectedDevice.ApplyColorControlPreset(ColorControlPresetSlot.ColorControlPreset1), _canExecute);
    public ICommand ClickStoreColorControlCommand => new CommandHandler(() => CommonMethods.SelectedDevice.StoreColorControlPreset(ColorControlPresetSlot.ColorControlPreset1), _canExecute);

    private void ClickGetPanTiltLimitsCommandHandler(object o)
    {
      var limits = CommonMethods.SelectedDevice.GetPanTiltLimits();
      VideoConfiguration.MinPan = limits.PanLimits.Min;
      VideoConfiguration.MaxPan = limits.PanLimits.Max;
      VideoConfiguration.PanStepSize = limits.PanLimits.StepSize;
      VideoConfiguration.MinTilt = limits.TiltLimits.Min;
      VideoConfiguration.MaxTilt = limits.TiltLimits.Max;
      VideoConfiguration.TiltStepSize = limits.TiltLimits.StepSize;
    }

    public void ShowVideoConfigurationDetails(int deviceId)
    {
      IDevice device = CommonMethods.SelectedDevice;
      _videoConfiguration.VideoSupported = device.IsFeatureSupported(DeviceFeature.Video);
      _videoConfiguration.ZoomLevel = 0;
      UpdConstants(device);
    }

    private ICommand _clickApplyPTZPresetCommand;
    public ICommand ClickApplyPTZPresetCommand => _clickApplyPTZPresetCommand ?? (_clickApplyPTZPresetCommand = new CommandHandlerArguments(ClickApplyPTZPresetCommandHandler, _canExecute));

    private void ClickApplyPTZPresetCommandHandler(object o)
    {
        CommonMethods.SelectedDevice.ApplyPTZPreset(VideoConfiguration.CurPTZPresetSlot);
    }

    private ICommand _clickStorePTZPresetCommand;
    public ICommand ClickStorePTZPresetCommand => _clickStorePTZPresetCommand ?? (_clickStorePTZPresetCommand = new CommandHandlerArguments(ClickStorePTZPresetCommandHandler, _canExecute));
    private void ClickStorePTZPresetCommandHandler(object o)
    {
        CommonMethods.SelectedDevice.StorePTZPreset(VideoConfiguration.CurPTZPresetSlot);
    }

    private List<WhiteboardDetails> _allVideoConfigurationDetails;

    public List<WhiteboardDetails> AllVideoConfigurationDetails
    {
      get { return _allVideoConfigurationDetails; }
      set
      {
        _allVideoConfigurationDetails = value;
        RaisePropertyChanged();
      }
    }
    // Pan, Tilt, and Zoom
    public SimpleValueViewModel Zoom { get; }

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

  public class DevConstant
  {
	  public DevConstant(string key, string value)
	  {
		  Key = key;
		  Value = value;
	  }
      public string Key { get; }
      public string Value { get; }
  }
}
