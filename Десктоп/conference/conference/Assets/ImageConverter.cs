using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace conference.Assets;

internal class ImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string filePath) return null;

        if (!File.Exists(Path.GetFullPath(Environment.CurrentDirectory+@"\Image\"+filePath))||string.IsNullOrEmpty(filePath))
        {
            return new Bitmap(Path.GetFullPath(Environment.CurrentDirectory+"\\Image\\заглушка.png"));
        }
        else
        {
            return new Bitmap(Path.GetFullPath(Environment.CurrentDirectory+@"\Image\"+filePath));
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}