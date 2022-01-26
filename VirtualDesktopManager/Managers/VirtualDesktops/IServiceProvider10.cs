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
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	internal interface IServiceProvider10
	{
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object QueryService(ref Guid service, ref Guid riid);
	}
}
