using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Jabra_SDK_Demo.ViewModel;

namespace Jabra_SDK_Demo.Helpers
{
	public static class UserControlDetails
	{
		internal static ComboBoxControlViewModel GetComboBoxControlInformations(Guid guid, ObservableCollection<ExpanderControlViewModel> expanderControlList)
		{
			return (from e in expanderControlList
							from r in e.UserControlList.OfType<ComboBoxControlViewModel>()
							where r.ComboBoxControl.GuidValue.ToString().ToLower(CultureInfo.InvariantCulture) == guid.ToString().ToLower(CultureInfo.InvariantCulture)
							select r).FirstOrDefault();
		}

		internal static RadioButtonControlViewModel GetRadioButtonControlInformations(Guid guid, ObservableCollection<ExpanderControlViewModel> expanderControlList)
		{
			return (from e in expanderControlList
							from r in e.UserControlList.OfType<RadioButtonControlViewModel>()
							where r.RadioButtonControl.GuidValue.ToString().ToLower(CultureInfo.InvariantCulture) == guid.ToString().ToLower(CultureInfo.InvariantCulture)
							select r).FirstOrDefault();
		}

		internal static TextControlViewModel GetTextControlInformations(Guid guid, ObservableCollection<ExpanderControlViewModel> expanderControlList)
		{
			return (from e in expanderControlList
							from r in e.UserControlList.OfType<TextControlViewModel>()
							where r.TextControl.GuidValue.ToString().ToLower(CultureInfo.InvariantCulture) == guid.ToString().ToLower(CultureInfo.InvariantCulture)
							select r).FirstOrDefault();
		}

		internal static ButtonControlViewModel GetButtonControlInformations(Guid guid, ObservableCollection<ExpanderControlViewModel> expanderControlList)
		{
			return (from e in expanderControlList
							from r in e.UserControlList.OfType<ButtonControlViewModel>()
							where r.ButtonControl.GuidValue.ToString().ToLower(CultureInfo.InvariantCulture) == guid.ToString().ToLower(CultureInfo.InvariantCulture)
							select r).FirstOrDefault();
		}
	}
}
