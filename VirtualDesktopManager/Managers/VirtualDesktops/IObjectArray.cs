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
	[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
	internal interface IObjectArray
	{
		void GetCount(out int count);
		void GetAt(int index, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object obj);
	}
}
