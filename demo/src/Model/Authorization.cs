using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Jabra_SDK_Demo.Helpers;
namespace Jabra_SDK_Demo.Model
{
  public class Authorization : INotifyPropertyChanged
  {

    private string _password;
    public string Password
    {
      get { return _password; }
      set
      {
        _password = value;
        if (!string.IsNullOrEmpty(_password))
        {
          CommonMethods.Password = _password;
          SpecialHandlers.IsClearEnableDisable = true;
        }
        else
        {
          SpecialHandlers.IsClearEnableDisable = false;
        }
        OnPropertyChanged("Password");
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
