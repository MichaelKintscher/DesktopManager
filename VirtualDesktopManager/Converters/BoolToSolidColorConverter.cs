using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace VirtualDesktopManager.Converters
{
    /// <summary>
    /// Converts a bool to one of the given colors (as a brush). The convert back behavior treats any color aside from the "true" color as false.
    /// </summary>
    public class BoolToSolidColorBrushConverter : IValueConverter
    {
        /// <summary>
        /// The color to return if the input is true.
        /// </summary>
        public Color TrueColor { get; set; }

        /// <summary>
        /// The color to return if the input is false.
        /// </summary>
        public Color FalseColor { get; set; }

        public BoolToSolidColorBrushConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool boolValue = (bool)value;

            return boolValue ? new SolidColorBrush(this.TrueColor) : new SolidColorBrush(this.FalseColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool convertedValue;

            convertedValue = (Color)value == this.TrueColor;

            return convertedValue;
        }
    }
}
