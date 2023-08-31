namespace Jabra_SDK_Demo.Model
{
	public class StaticTextControl : CommonControls
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
	}
}