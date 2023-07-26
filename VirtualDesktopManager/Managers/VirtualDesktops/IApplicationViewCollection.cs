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
	[Guid("1841C6D7-4F9D-42C0-AF41-8747538F10E5")]
	internal interface IApplicationViewCollection
	{
		int GetViews(out IObjectArray array);
		int GetViewsByZOrder(out IObjectArray array);
		int GetViewsByAppUserModelId(string id, out IObjectArray array);
		int GetViewForHwnd(IntPtr hwnd, out IApplicationView view);
		int GetViewForApplication(object application, out IApplicationView view);
		int GetViewForAppUserModelId(string id, out IApplicationView view);
		int GetViewInFocus(out IntPtr view);
		int Unknown1(out IntPtr view);
		void RefreshCollection();
		int RegisterForApplicationViewChanges(object listener, out int cookie);
		int UnregisterForApplicationViewChanges(int cookie);
	}
}
