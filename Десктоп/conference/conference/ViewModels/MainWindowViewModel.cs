using Avalonia.Controls;
using conference.Models;
using conference.Views;
using ReactiveUI;

namespace conference.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    UserControl uc;
    private User loginedUser;

    public User LoginedUser
    {
        get => loginedUser;
        set => this.RaiseAndSetIfChanged(ref loginedUser, value);
    }

    public static MainWindowViewModel Self;
    public UserControl UC
    {
        get => uc;
        set => this.RaiseAndSetIfChanged(ref uc, value);
    }

    public MainWindowViewModel()
    {
        UC = new PublicView();
        Self = this;
    }

}