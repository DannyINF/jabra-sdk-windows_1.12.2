using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Jabra_SDK_Demo.Model
{
  class RemoteMMIv2Control : INotifyPropertyChanged
  {
    private bool _controlCreated;
    public bool ControlCreated
    {
      get { return _controlCreated; }
      set
      {
        _controlCreated = value;
        OnPropertyChanged("ControlCreated");
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

    private string _typeName;
    public string TypeName
    {
      get { return _typeName; }
      set
      {
        _typeName = value;
        OnPropertyChanged("TypeName");
      }
    }

    private int _typeValue;
    public int TypeValue
    {
      get { return _typeValue; }
      set
      {
        _typeValue = value;
        OnPropertyChanged("TypeValue");
      }
    }

    private string _isInFocus;
    public string IsInFocus
    {
      get { return _isInFocus; }
      set
      {
        _isInFocus = value;
        OnPropertyChanged("IsInFocus");
      }
    }

    private ObservableCollection<InputAction> _inputActions;
    public ObservableCollection<InputAction> InputActions
    {
      get { return _inputActions; }
      set
      {
        _inputActions = value;
        OnPropertyChanged("InputActions");
      }
    }

    private Dictionary<int, string> _priorities;
    public Dictionary<int, string> Priorities
    {
      get { return _priorities; }
      set
      {
        _priorities = value;
        OnPropertyChanged("Priorities");
      }
    }

    private Dictionary<int, string> _sequences;
    public Dictionary<int, string> Sequences
    {
      get { return _sequences; }
      set
      {
        _sequences = value;
        OnPropertyChanged("Sequences");
      }
    }

    private Dictionary<int, string> _appFunctions;
    public Dictionary<int, string> AppFunctions
    {
      get { return _appFunctions; }
      set
      {
        _appFunctions = value;
        OnPropertyChanged("AppFunctions");
      }
    }

    private string _appFunctionCurrentValue;
    public string AppFunctionCurrentValue
    {
      get { return _appFunctionCurrentValue; }
      set
      {
        _appFunctionCurrentValue = value;
        OnPropertyChanged("AppActionCurrentValue");
      }
    }

    private int _priorityCurrentValue;
    public int PriorityCurrentValue
    {
      get { return _priorityCurrentValue; }
      set
      {
        _priorityCurrentValue = value;
        OnPropertyChanged("PriorityCurrentValue");
      }
    }

    private int _sequenceCurrentValue;
    public int SequenceCurrentValue
    {
      get { return _sequenceCurrentValue; }
      set
      {
        _sequenceCurrentValue = value;
        OnPropertyChanged("SequenceCurrentValue");
      }
    }

    private bool _enableRedOutput;
    public bool EnableRedOutput
    {
      get { return _enableRedOutput; }
      set
      {
        _enableRedOutput = value;
        OnPropertyChanged("EnableRedOutput");
      }
    }

    private int _redOutputValue;
    public int RedOutputValue
    {
      get { return _redOutputValue; }
      set
      {
        _redOutputValue = value;
        OnPropertyChanged("RedOutputValue");
      }
    }

    private bool _enableGreenOutput;
    public bool EnableGreenOutput
    {
      get { return _enableGreenOutput; }
      set
      {
        _enableGreenOutput = value;
        OnPropertyChanged("EnableGreenOutput");
      }
    }

    private int _greenOutputValue;
    public int GreenOutputValue
    {
      get { return _greenOutputValue; }
      set
      {
        _greenOutputValue = value;
        OnPropertyChanged("GreenOutputValue");
      }
    }

    private bool _enableBlueOutput;
    public bool EnableBlueOutput
    {
      get { return _enableBlueOutput; }
      set
      {
        _enableBlueOutput = value;
        OnPropertyChanged("EnableBlueOutput");
      }
    }

    private int _blueOutputValue;
    public int BlueOutputValue
    {
      get { return _blueOutputValue; }
      set
      {
        _blueOutputValue = value;
        OnPropertyChanged("BlueOutputValue");
      }
    }

    private bool _enableOutput;
    public bool EnableOutput
    {
      get { return _enableOutput; }
      set
      {
        _enableOutput = value;
        OnPropertyChanged("EnableOutput");
      }
    }

    private bool _enableInput;
    public bool EnableInput
    {
      get { return _enableInput; }
      set
      {
        _enableInput = value;
        OnPropertyChanged("EnableInput");
      }
    }

    private bool _enableAppActions;
    public bool EnableAppActions
    {
      get { return _enableAppActions; }
      set
      {
        _enableAppActions = value;
        OnPropertyChanged("EnableAppActions");
      }
    }

    /// <summary>
    /// Used for storing input action
    /// </summary>
    public class InputAction
    {
      public bool IsSelected { get; set; }
      public int Value { get; set; }
      public string Type { get; set; }

      public InputAction(int type, string name, bool selected)
      {
        Value = type;
        Type = name;
        IsSelected = selected;
      }
    }

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    // Create the OnPropertyChanged method to raise the event
    protected void OnPropertyChanged(string name)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion
  }
}
