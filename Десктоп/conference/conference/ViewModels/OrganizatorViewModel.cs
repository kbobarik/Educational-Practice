using System;
using conference.Models;
using conference.Views;
using ReactiveUI;

namespace conference.ViewModels;

/// <summary>
/// ViewModel для отображения информации об организаторе и навигации по приложению.
/// </summary>
public class OrganizatorViewModel : ViewModelBase
{
    // Приветствие для организатора (например, "Доброе утро!")
    public string welcome;

    // Текущий пользователь-организатор
    User organizator;

    // Пол организатора (Ms/Mrs)
    private string genderOrganizator;

    /// <summary>
    /// Свойство для хранения информации об организаторе.
    /// Использует механизм ReactiveUI для автоматического обновления привязанных представлений.
    /// </summary>
    public User Organizator
    {
        get => organizator;
        set => this.RaiseAndSetIfChanged(ref organizator, value);
    }

    /// <summary>
    /// Свойство, содержащее приветствие в зависимости от времени суток.
    /// </summary>
    public string Welcome
    {
        get => welcome;
        set => this.RaiseAndSetIfChanged(ref welcome, value);
    }

    /// <summary>
    /// Свойство, определяющее форму обращения (Ms или Mrs) в зависимости от пола организатора.
    /// </summary>
    public string GenderOrganizator
    {
        get => genderOrganizator;
        set => this.RaiseAndSetIfChanged(ref genderOrganizator, value);
    }

    /// <summary>
    /// Конструктор. Инициализирует свойства и задаёт приветствие и форму обращения.
    /// </summary>
    public OrganizatorViewModel()
    {
        // Получение информации о текущем авторизованном пользователе
        Organizator = MainWindowViewModel.Self.LoginedUser;

        // Определение приветствия в зависимости от текущего времени
        Welcome = DateTime.Now.Hour switch
        {
            >= 9 and <= 11 => "Доброе утро!",
            > 11 and <= 18 => "Добрый день!",
            > 18 and <= 24 => "Добрый вечер!",
            _ => "Доброй ночи!"
        };

        // Определение формы обращения в зависимости от пола
        GenderOrganizator = Organizator.IdGenderNavigation.Title == "Женский" ? "Mrs " : "Ms ";
    }

    /// <summary>
    /// Метод для перехода на страницу регистрации нового пользователя.
    /// </summary>
    public void NavigateToRegistration()
    {
        MainWindowViewModel.Self.UC = new RegistrationView();
    }
}
