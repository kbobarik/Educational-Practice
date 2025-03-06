using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using conference.ViewModels;

namespace conference;

public partial class SignInView : UserControl
{
    public SignInView()
    {
        InitializeComponent();
        DataContext = new SignInViewModel();
    }
}