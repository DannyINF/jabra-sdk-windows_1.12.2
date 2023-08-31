using System;
using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
  public class CommonControls : INotifyPropertyChanged
  {
    public Guid _guidValue;
    public Guid GuidValue
    {
      get { return _guidValue; }
      set
      {
        _guidValue = value;
        OnPropertyChanged("GuidValue");
      }
    }

    private string _helpText;
    public string HelpText
    {
      get { return _helpText; }
      set
      {
        _helpText = value;
        OnPropertyChanged("HelpText");
      }
    }

    private bool _controlEnabled;
    public bool ControlEnabled
    {
      get { return _controlEnabled; }
      set
      {
        _controlEnabled = value;
        OnPropertyChanged("ControlEnabled");
      }
    }

    private string _label;
    public string Label
    {
      get { return _label; }
      set
      {
        _label = value;
        OnPropertyChanged("Label");
      }
    }

    private bool _canInvoke;
    public bool CanInvoke
    {
      get { return _canInvoke; }
      set
      {
        _canInvoke = value;
        OnPropertyChanged("CanInvoke");
      }
    }

    private bool _protected;
    public bool Protected
    {
      get { return _protected; }
      set
      {
        _protected = value;
        OnPropertyChanged("Protected");
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
