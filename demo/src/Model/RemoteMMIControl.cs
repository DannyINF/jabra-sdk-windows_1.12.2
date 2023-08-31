using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Jabra_SDK_Demo.Helpers;

namespace Jabra_SDK_Demo.Model
{
	public class RemoteMMIControl : INotifyPropertyChanged
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

		private int _labelCurrentValue;
		public int LabelCurrentValue
		{
			get { return _labelCurrentValue; }
			set
			{
				_labelCurrentValue = value;
				OnPropertyChanged("LabelCurrentValue");
			}
		}

		private Dictionary<int, string> _buttonValues;
		public Dictionary<int, string> ButtonValues
		{
			get { return _buttonValues; }
			set
			{
				_buttonValues = value;
				OnPropertyChanged("ButtonValues");
			}
		}

		private Dictionary<int, string> _buttonConfig;
		public Dictionary<int, string> ButtonConfig
		{
			get { return _buttonConfig; }
			set
			{
				_buttonConfig = value;
				OnPropertyChanged("ButtonConfig");
			}
		}

		private string _buttonConfigCurrentValue;
		public string ButtonConfigCurrentValue
		{
			get { return _buttonConfigCurrentValue; }
			set
			{
				_buttonConfigCurrentValue = value;
				if (IsCheckBoxChecked && SpecialHandlers.UnwrittenConfigurableButtons != null)
				{
					var data = (from set in SpecialHandlers.UnwrittenConfigurableButtons
											where set.Id == LabelCurrentValue
											where set.ButtonEventId == CurrentValue
											select set).FirstOrDefault();
					if (data != null)
					{
						data.ConfigurationActionValue = _buttonConfigCurrentValue;
					}
				}

				OnPropertyChanged("ButtonConfigCurrentValue");
			}
		}

		private int _currentValue;
		public int CurrentValue
		{
			get { return _currentValue; }
			set
			{
				_currentValue = value;
				OnPropertyChanged("CurrentValue");
			}
		}

		private bool _isCheckBoxChecked;
		public bool IsCheckBoxChecked
		{
			get { return _isCheckBoxChecked; }
			set
			{
				_isCheckBoxChecked = value;
				OnPropertyChanged("IsCheckBoxChecked");
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
