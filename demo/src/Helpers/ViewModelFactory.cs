using Jabra_SDK_Demo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabra_SDK_Demo.Helpers
{
    class ViewModelFactory
    {
        internal FirmwareUpdatesViewModel GetFirmwareUpdatesViewModel(int deviceId) => new FirmwareUpdatesViewModel(deviceId);

        internal AuthorizationViewModel GetAuthorizationViewModel() => new AuthorizationViewModel();
    }
}
