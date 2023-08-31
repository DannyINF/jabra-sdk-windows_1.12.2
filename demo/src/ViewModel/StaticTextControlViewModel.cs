using System;
using System.ComponentModel;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.Model;
using JabraSDK;

namespace Jabra_SDK_Demo.ViewModel
{
	internal class StaticTextControlViewModel : DisposeMethods
	{
		private StaticTextControl _staticTextControl;

		public StaticTextControl StaticTextControl
		{
			get { return _staticTextControl; }
			set
			{
				_staticTextControl = value;
				OnPropertyChanged("StaticTextControl");
			}
		}

		public StaticTextControlViewModel()
		{
		}

		public StaticTextControlViewModel(ISetting setting)
		{
			_staticTextControl = new StaticTextControl
			{
				GuidValue = Guid.Parse(setting.Guid),
				Label = setting.SettingName,
				CurrentValue = string.IsNullOrEmpty(setting.CurrentValue) ? "" : setting.CurrentValue,
				HelpText = setting.HelpText,
				ControlEnabled = true,
			};
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

