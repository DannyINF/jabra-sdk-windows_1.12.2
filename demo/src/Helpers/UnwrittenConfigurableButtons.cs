using System.Collections.Generic;

namespace Jabra_SDK_Demo.Helpers
{
	public class UnwrittenConfigurableButtons
	{
		public int Id { get; set; }
		public string Name { get; set; }
		//todo to be handled for multiple Actions
		//Dictionary<ushort, string> Actions { get; set; }
		public ushort ButtonEventId { get; set; }
		public string ButtonEventName { get; set; }
		public string ConfigurationActionValue { get; set; }
		public int DeviceId { get; set; }
	}
}
