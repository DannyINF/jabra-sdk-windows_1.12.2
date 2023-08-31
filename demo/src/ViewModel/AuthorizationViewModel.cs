using GalaSoft.MvvmLight;
using System.Windows.Input;
using Jabra_SDK_Demo.Commands;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Jabra_SDK_Demo.ViewModel
{
  public class AuthorizationViewModel : ViewModelBase
  {
    protected Authorization _authorization;
    public Authorization Authorization
    {
      get { return _authorization; }
      set
      {
        _authorization = value;
        OnPropertyChanged("Authorization");
      }
    }

    private ICommand _clickClearCommand;
    public ICommand ClickClearCommand
    {
      get
      {
        return _clickClearCommand ?? (_clickClearCommand = new CommandHandlerArguments(param => ClickClearAction(param), true));
      }
    }

    public void ClickClearAction(object o)
    {
      SpecialHandlers.AuthorizationUserName = CommonMethods.Password = _authorization.Password = SpecialHandlers.AuthorizationToken = CommonMethods.AuthorizationTokenError = string.Empty;
      SpecialHandlers.IsClearEnableDisable = false;
    }

    public AuthorizationViewModel()
    {
      _authorization = new Authorization();
      CommonMethods.Password = _authorization.Password;
    }

    public void LoadAuthorizationContent()
    {
      CommonMethods.Password = _authorization.Password;
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
