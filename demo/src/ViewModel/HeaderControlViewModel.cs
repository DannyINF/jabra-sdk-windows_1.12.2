using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;

namespace Jabra_SDK_Demo.ViewModel
{
	internal class HeaderControlViewModel : ViewModelBase
  {
		private HeaderControl _headerControl;
		public HeaderControl HeaderControl
		{
			get { return _headerControl; }
			set
			{
				_headerControl = value;
				OnPropertyChanged("HeaderControl");
			}
		}
		public HeaderControlViewModel(string sdkVersion)
		{
			_headerControl = new HeaderControl(sdkVersion);
		}


		#region INotifyPropertyChanged Members

#pragma warning disable CS0108 // 'HeaderControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'HeaderControlViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

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
