using System;
using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
	public class ExpanderControl : INotifyPropertyChanged
	{
		public Guid _guid;
		public Guid Guid
		{
			get { return _guid; }
			set
			{
				_guid = value;
				OnPropertyChanged("Guid");
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
