using System.Collections.Generic;
using System.Linq;
using Jabra_SDK_Demo.Helpers;
using Jabra_SDK_Demo.ViewModel;

namespace Jabra_SDK_Demo.Model
{
	public class ComboBoxControl : CommonControls
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

		private Dictionary<int, string> _settingValues;
		public Dictionary<int, string> SettingValues
		{
			get { return _settingValues; }
			set
			{
				_settingValues = value;
				OnPropertyChanged("SettingValues");
			}
		}

		private int _currentValue;
		public int CurrentValue
		{
			get { return _currentValue; }
			set
			{
				_currentValue = value;

				if (SpecialHandlers.SettingsLoaded)
				{
					var data = (from set in SpecialHandlers.UnWrittenSettings
											where set.Guid == GuidValue
											select set).FirstOrDefault();
					if (data == null)
					{
						UnWrittenSettings settings = new UnWrittenSettings();
						settings.Guid = GuidValue;
						SpecialHandlers.UnWrittenSettings.Add(settings);
                        if (CommonMethods.SelectedDevice != null)
                        {
                            SpecialHandlers.UnWrittenSettingsDeviceId = CommonMethods.SelectedDevice.DeviceId;
                        }
                        else
                        {
                          SpecialHandlers.UnWrittenSettingsDeviceId = -1;
                        }
                    }
                    if (CommonMethods.SelectedDevice != null)
                    {
                        DependencyFeatures.UpdateDependentControls(CommonMethods.SelectedDevice.DeviceId, GuidValue, CurrentValue, MainWindowViewModel.ViewModelDeviceSettings.ExpanderControlList);
                    }
                    MainWindowViewModel.UpdateApplySettingsControl(false);
				}

				OnPropertyChanged("CurrentValue");
			}
		}
	}
}
