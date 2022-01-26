using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Managers.VirtualDesktops
{
	/// <summary>
	/// Code taken from the Virtual Desktop project on GitHub: https://github.com/MScholtes/VirtualDesktop/blob/master/VirtualDesktop11.cs
	/// </summary>
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4CE81583-1E4C-4632-A621-07A53543148F")]
	internal interface IVirtualDesktopPinnedApps
	{
		bool IsAppIdPinned(string appId);
		void PinAppID(string appId);
		void UnpinAppID(string appId);
		bool IsViewPinned(IApplicationView applicationView);
		void PinView(IApplicationView applicationView);
		void UnpinView(IApplicationView applicationView);
	}
}
