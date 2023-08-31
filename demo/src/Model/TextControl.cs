using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.ViewModel;

namespace Jabra_SDK_Demo.Model
{
	public class TextControl : CommonControls, IDataErrorInfo
	{
		private string _currentValue;
		public string CurrentValue
		{
			get { return _currentValue; }
			set
			{
				_currentValue = value;
				OnPropertyChanged("CurrentValue");
			}
		}

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

		public int MinLength { get; set; }
		public int MaxLength { get; set; }
		public string RegexPattern { get; set; }
		public string RegexPatternErrorMessage { get; set; }


		#region IDataErrorInfo Members
		string IDataErrorInfo.Error
		{
			get { return null; }
		}

		string IDataErrorInfo.this[string propertyName]
		{
			get { return GetValidationError(); }
		}
		#endregion

		#region Validation


		public bool IsValid
		{
			get
			{
				if (GetValidationError() != null)
					return false;

				return true;
			}
		}

		string GetValidationError()
		{
			string error = ValidateCurrentvalue();
			if (!string.IsNullOrEmpty(error))
			{
				MainWindowViewModel.UpdateApplySettingsControl(true);
				var result = (from val in SpecialHandlers.ValidationErrors
											where val.ControlName == Label
											select val).FirstOrDefault();
				if (!SpecialHandlers.ValidationErrors.Contains(result))
				{
					ValidationErrors errorVal = new ValidationErrors();
					errorVal.ControlName = Label;
					SpecialHandlers.ValidationErrors.Add(errorVal);
					var item = SpecialHandlers.UnWrittenSettings.FirstOrDefault(x => x.Guid == GuidValue);
					if (item != null)
						SpecialHandlers.UnWrittenSettings.Remove(item);
				}
				return error;
			}
			else
			{
				var result = (from val in SpecialHandlers.ValidationErrors
											where val.ControlName == Label
											select val).FirstOrDefault();
				if (result != null)
				{
					SpecialHandlers.ValidationErrors.Remove(result);
				}
			}
			if (SpecialHandlers.SettingsLoaded)
			{
				var data = (from set in SpecialHandlers.UnWrittenSettings
										where set.Guid == GuidValue
										select set).FirstOrDefault();
				if (data == null)
				{
					SpecialHandlers.UnWrittenSettings.Add(new UnWrittenSettings() {Guid = GuidValue });
                    if (CommonMethods.SelectedDevice != null)
                    {
                        SpecialHandlers.UnWrittenSettingsDeviceId = CommonMethods.SelectedDevice.DeviceId;
                    }
                    else
                    {
                      SpecialHandlers.UnWrittenSettingsDeviceId = -1;
                    }
                }
				MainWindowViewModel.UpdateApplySettingsControl(false);
			}
			return error;
		}

		private string ValidateCurrentvalue()
		{
			string result = null;
			if (CurrentValue != null)
			{
				if (Encoding.UTF8.GetBytes(CurrentValue).Length < MinLength)
					result = "The string is too short";
				if (Encoding.UTF8.GetBytes(CurrentValue).Length > MaxLength)
					result = "The string is too long";

				if (!string.IsNullOrEmpty(RegexPattern))
				{
					var match = Regex.Match(CurrentValue, RegexPattern);
					if (!match.Success)
					{
						if (!string.IsNullOrEmpty(result))
							result += ". ";
						result += RegexPatternErrorMessage;
					}
				}
			}
			return result;
		}

		#endregion
	}
}