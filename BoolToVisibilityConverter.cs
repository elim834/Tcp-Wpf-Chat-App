using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LastMessenger;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isSent)
        {
            // Compare the boolean value to the parameter
            bool parameterValue = bool.Parse(parameter.ToString()!);
            return isSent == parameterValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
