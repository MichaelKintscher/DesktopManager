using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Controllers
{
    /// <summary>
    /// Provides a base class for controllers that implements the
    /// singleton design pattern using an Instance property.
    /// </summary>
    /// <typeparam name="T">The type of the controller, which must implement a public parameterless constructor.</typeparam>
    internal abstract class SingletonController<T> where T : new()
    {
        /// <summary>
        /// The static reference to the only (singleton) instance of the class.
        /// </summary>
        private static readonly Lazy<T> _mySingleton = new Lazy<T>(() => new T());

        /// <summary>
        /// Returns the single instance of the class.
        /// </summary>
        public static T Instance
        {
            get => SingletonController<T>._mySingleton.Value;
        }
    }
}
