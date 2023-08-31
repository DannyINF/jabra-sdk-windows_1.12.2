using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;

namespace Jabra_SDK_Demo.ViewModel
{
  public abstract class ViewBase : INotifyPropertyChanged
  {
    public virtual event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public abstract class Setter : ViewBase
  {
    private bool _valid;

    public abstract void DoSet();

    public bool Valid
    {
      get => _valid;
      set { _valid = value; OnPropertyChanged(); }
    }
  }
  
  public class Setter<T> : Setter
  {
    private readonly Action<T> _setAction;
    private T _value;

    public Setter(Action<T> setAction)
    {
      _setAction = setAction;
    }

    public override void DoSet()
    {
      _setAction(TValue);
    }

    protected virtual void Validate()
    {
      Valid = TValue != null;
    }

    public T TValue
    {
      get => _value;
      set
      {
        _value = value;
        Validate();
      }
    }
  }

  public class IntSetter : Setter<int?>
  {
    public IntSetter(Action<int?> setAction) : base(setAction) { }
  }

  public class BoolSetter : Setter<bool?>
  {
    public BoolSetter(Action<bool?> setAction) : base(setAction) { }
  }

  public abstract class EnumRef
  {
    public EnumRef(IEnumerable<string> names)
    {
      Names = names;
    }
    public abstract void Cmd();
    public IEnumerable<string> Names { get; }
    public string CurItem { get; set; }
  }

  public class EnumRef<T> : EnumRef where T : struct, Enum
  {
    private readonly Action<T> _cmd;

    public EnumRef(Action<T> cmd) : base(Enum.GetNames(typeof(T)))
    {
      _cmd = cmd;
    }
    public override void Cmd()
    {
      _cmd(EnumVal);
    }

    public T EnumVal => (T)Enum.Parse(typeof(T), CurItem);
  }

  public class EnumSetter : Setter<EnumRef>
  {
    private readonly EnumRef _eref;

    public EnumSetter(EnumRef eref) : base(e => eref.Cmd())
    {
      _eref = eref;
    }

    public IEnumerable<string> Values => _eref.Names;

    public string CurItem
    {
      get => _eref.CurItem;
      set {
        _eref.CurItem = value;
        Valid = value != null;
      }
    }
  }

  public class MultiSetter : Setter
  {
	  private readonly Action doSet;
	  public IEnumerable<Setter> Setters { get; }

	  public MultiSetter(Action doSet, IEnumerable<Setter> setters)
	  {
		  this.doSet = doSet;
		  Setters = setters;
		  foreach (var setter in setters) setter.PropertyChanged += (sender, args) => { if (args.PropertyName == "Valid") Validate(); };
	  }

	  public override void DoSet()
	  {
		  doSet();
	  }

	  private void Validate()
	  {
		  Valid = Setters.FirstOrDefault(setter => !setter.Valid) == null;
	  }

  }

  public abstract class Getter : ViewBase
  {
    public abstract void DoGet();
  }

  public class Getter<T> : Getter
  {
    private readonly Func<T> _getFunc;
    private T _value;

    public Getter(Func<T> getFunc)
    {
      _getFunc = getFunc;
      TValue = default(T);
    }

    public override void DoGet()
    {
      TValue = _getFunc();
    }

    public T TValue
    {
      get => _value;
      set { _value = value; OnPropertyChanged(); OnTValueChanged(); }
    }

    protected virtual void OnTValueChanged()
    {
    }
  }

  public class IntGetter : Getter<int?> { public IntGetter(Func<int?> getFunc) : base(getFunc) { } }
  public class TextGetter : Getter<string> { public TextGetter(Func<string> getFunc) : base(getFunc) { } }

  public class BoolGetter : Getter<bool?>
  {
    public BoolGetter(Func<bool?> getFunc) : base(getFunc) { }

    public string Text
    {
      get
      {
        if (TValue.HasValue) return TValue.Value ? "Enabled" : "Disabled";
        return "";
      }
    }

    protected override void OnTValueChanged()
    {
	    OnPropertyChanged("Text");
    }
  }

  public class MultiGetter : Getter
  {
	  private readonly Action doGet;
	  public IEnumerable<Getter> Getters { get; }

	  public MultiGetter(Action doGet, IEnumerable<Getter> setters)
	  {
		  this.doGet = doGet;
		  Getters = setters;
	  }

	  public override void DoGet()
	  {
		  doGet();
	  }
  }

  public class SimpleValueViewModel : ViewBase
  {
	  private string _error;

   

    public SimpleValueViewModel(string name, Setter setAction, Getter getFunc)
    {
      Name = name;
      Set = setAction;
      Get = getFunc;
      DoGet = new CommandHandler(() => CatchError(getFunc.DoGet), true);
      DoSet = new CommandHandler(() => CatchError(setAction.DoSet), true);
    }

    private void CatchError(Action f)
    {
      try {
        f();
        Error = null;
      } catch (Exception e) {
        Error = e.Message;
        Console.WriteLine(e);
      }
    }

    public string Name { get; } // like Contrast
    public Setter Set { get; }
    public Getter Get { get; }

    public string Error
    {
      get => _error;
      private set { _error = value; OnPropertyChanged(); }
    }

    public ICommand DoGet { get; }

    public ICommand DoSet { get; }
  }
}