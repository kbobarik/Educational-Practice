using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using conference.ViewModels;

namespace conference.Views;

public partial class OrganizatorView : UserControl
{
    public OrganizatorView()
    {
        InitializeComponent();
        DataContext = new OrganizatorViewModel();
    }
}