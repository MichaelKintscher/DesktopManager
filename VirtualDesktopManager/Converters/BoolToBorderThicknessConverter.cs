using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Converters
{
    /// <summary>
    /// Converts a bool to a border thickness. The convert back behavior treats any border thickness aside from the "true" thickness as false.
    /// </summary>
    internal class BoolToBorderThicknessConverter : IValueConverter
    {
        /// <summary>
        /// The thickness to return if the input is true.
        /// </summary>
        public Thickness TrueThickness { get; set; }

        /// <summary>
        /// The thickness to return if the input is false.
        /// </summary>
        public Thickness FalseThickness { get; set; }

        public BoolToBorderThicknessConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;

            return boolValue ? this.TrueThickness : this.FalseThickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool convertedValue;

            convertedValue = (Thickness)value == this.TrueThickness;

            return convertedValue;
        }
    }
}
