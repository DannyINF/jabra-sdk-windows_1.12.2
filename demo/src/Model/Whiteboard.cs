using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
  public class Whiteboard : INotifyPropertyChanged
  {
    private bool _whiteboardSupported;
    public bool WhiteboardSupported
    {
      get { return _whiteboardSupported; }
      set
      {
        _whiteboardSupported = value;
        OnPropertyChanged("WhiteboardSupported");
      }
    }

    private byte _whiteboardNumber = 0;
    public byte WhiteboardNumber
    {
      get { return _whiteboardNumber; }
      set
      {
        _whiteboardNumber = value;
        OnPropertyChanged("WhiteboardNumber");
      }
    }

    private ushort _ulx;
    public ushort ULX
    {
      get { return _ulx; }
      set
      {
        _ulx = value;
        OnPropertyChanged("ULX");
      }
    }

    private ushort _uly;
    public ushort ULY
    {
      get { return _uly; }
      set
      {
        _uly = value;
        OnPropertyChanged("ULY");
      }
    }

    private ushort _llx;
    public ushort LLX
    {
      get { return _llx; }
      set
      {
        _llx = value;
        OnPropertyChanged("LLX");
      }
    }

    private ushort _lly;
    public ushort LLY
    {
      get { return _lly; }
      set
      {
        _lly = value;
        OnPropertyChanged("LLY");
      }
    }

    private ushort _urx;
    public ushort URX
    {
      get { return _urx; }
      set
      {
        _urx = value;
        OnPropertyChanged("URX");
      }
    }

    private ushort _ury;
    public ushort URY
    {
      get { return _ury; }
      set
      {
        _ury = value;
        OnPropertyChanged("URY");
      }
    }

    private ushort _lrx;
    public ushort LRX
    {
      get { return _lrx; }
      set
      {
        _lrx = value;
        OnPropertyChanged("LRX");
      }
    }

    private ushort _lry;
    public ushort LRY
    {
      get { return _lry; }
      set
      {
        _lry = value;
        OnPropertyChanged("LRY");
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

    private bool _whiteboardEnabled;
    public bool WhiteboardEnabled
    {
      get { return _whiteboardEnabled; }
      set
      {
        _whiteboardEnabled = value;
        OnPropertyChanged("WhiteboardEnabled");
      }
    }

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

  public class WhiteboardDetails
  {
    public int DeviceId;
    public string Data;
  }
}
