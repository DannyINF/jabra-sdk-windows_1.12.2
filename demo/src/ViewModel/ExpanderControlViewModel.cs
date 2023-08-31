using System;
using System.ComponentModel;
using System.Windows.Data;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
	internal class ExpanderControlViewModel : DisposeMethods
	{
		private ExpanderControl _expanderControl;
		public ExpanderControl ExpanderControl
		{
			get { return _expanderControl; }
			set
			{
				_expanderControl = value;
				OnPropertyChanged("ExpanderControl");
			}
		}

		public ExpanderControlViewModel()
		{
		}
		public ExpanderControlViewModel(ISetting setting)
		{
			_expanderControl = new ExpanderControl
			{
				Guid = Guid.Parse(setting.Guid),
				Label = setting.GroupName,
				HelpText = setting.GroupHelpText,
				ControlEnabled = true,
			};
		}

		public CompositeCollection UserControlList { get; set; }


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
