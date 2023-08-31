using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using JabraSDK;

namespace Jabra_SDK_Demo.Model
{
  public class VideoConfiguration : INotifyPropertyChanged
  {
    private bool _videoSupported;
    public bool VideoSupported
    {
      get { return _videoSupported; }
      set
      {
        _videoSupported = value;
        OnPropertyChanged("VideoSupported");
      }
    }

    public IEnumerable<PTZPresetSlot> PTZPresetSlots => (PTZPresetSlot[])Enum.GetValues(typeof(PTZPresetSlot));
    public PTZPresetSlot CurPTZPresetSlot { get; set; }

    private ushort _zoomLevel;
    public ushort ZoomLevel
    {
      get { return _zoomLevel; }
      set
      {
        _zoomLevel = value;
        OnPropertyChanged("ZoomLevel");
      }
    }

    private ushort _readZoomLevel;
    public ushort ReadZoomLevel
    {
      get { return _readZoomLevel; }
      set
      {
        _readZoomLevel = value;
        OnPropertyChanged("ReadZoomLevel");
      }
    }

    private ushort _minZoomLevel;
    public ushort MinZoomLevel
    {
      get { return _minZoomLevel; }
      set
      {
        _minZoomLevel = value;
        OnPropertyChanged("MinZoomLevel");
      }
    }

    private ushort _maxZoomLevel;
    public ushort MaxZoomLevel
    {
      get { return _maxZoomLevel; }
      set
      {
        _maxZoomLevel = value;
        OnPropertyChanged("MaxZoomLevel");
      }
    }

    private ushort _zoomStepSize;
    public ushort ZoomStepSize
    {
      get { return _zoomStepSize; }
      set
      {
        _zoomStepSize = value;
        OnPropertyChanged("ZoomStepSize");
      }
    }

    private ObservableCollection<string> _data;
    public ObservableCollection<string> Data
    {
      get { return _data; }
      set
      {
        _data = value;
        OnPropertyChanged("Data");
      }
    }

    private int _Pan;
    public int Pan { get => _Pan; set { _Pan = value; OnPropertyChanged("Pan"); } }
    private int _ReadPan;
    public int ReadPan { get => _ReadPan; set { _ReadPan = value; OnPropertyChanged("ReadPan"); } }
    private int _Tilt;
    public int Tilt { get => _Tilt; set { _Tilt = value; OnPropertyChanged("Tilt"); } }
    private int _ReadTilt;
    public int ReadTilt { get => _ReadTilt; set { _ReadTilt = value; OnPropertyChanged("ReadTilt"); } }
    private int _minPan;
    public int MinPan { get => _minPan; set { _minPan = value; OnPropertyChanged("MinPan"); } }
    private int _minTilt;
    public int MinTilt { get => _minTilt; set { _minTilt = value; OnPropertyChanged("MinTilt"); } }
    private int _maxPan;
    public int MaxPan { get => _maxPan; set { _maxPan = value; OnPropertyChanged("MaxPan"); } }
    private int _maxTilt;
    public int MaxTilt { get => _maxTilt; set { _maxTilt = value; OnPropertyChanged("MaxTilt"); } }
    private ushort _panStepSize;
    public ushort PanStepSize { get => _panStepSize; set { _panStepSize = value; OnPropertyChanged("PanStepSize"); } }
    private ushort _tiltStepSize;
    public ushort TiltStepSize { get => _tiltStepSize; set { _tiltStepSize = value; OnPropertyChanged("TiltStepSize"); } }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    // On PropertyChnaged Event
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

  public class VideoConfigurationDetails
  {
    public int DeviceId;
    public string Data;
  }
}
