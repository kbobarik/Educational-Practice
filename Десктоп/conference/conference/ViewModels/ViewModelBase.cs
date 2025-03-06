using conference.Models;
using ReactiveUI;

namespace conference.ViewModels;

public class ViewModelBase : ReactiveObject
{
    public StudingContext Db = new();
}