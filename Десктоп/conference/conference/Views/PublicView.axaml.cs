using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using conference.ViewModels;

namespace conference.Views;

public partial class PublicView : UserControl
{
    public PublicView()
    {
        InitializeComponent();
        DataContext = new PublicViewModel();
    }
}