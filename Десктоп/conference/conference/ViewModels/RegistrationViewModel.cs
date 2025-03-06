using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using conference.Models;
using conference.Views;
using ReactiveUI;

namespace conference.ViewModels
{
    /// <summary>
    /// ViewModel для регистрации пользователя.
    /// Управляет процессом регистрации, включая ввод данных и валидацию.
    /// </summary>
    public class RegistrationViewModel : ViewModelBase
    {
        // Переменные для хранения данных пользователя
        private int numberID;
        private string fio;
        private string email;
        private string phoneNumber;
        private string password;
        private string image;
        private string info;
        private string confirmPassword;
        private List<Gender> genders;
        private Gender selectedGender;
        private List<Role> roles;
        private Role selectedRole;
        private List<TypeOfEvent> typeOfEvents;
        private TypeOfEvent selectedTypeOfEvent;
        private List<Event> events;
        private Event selectedEvent;
        private bool isVisible;
        private bool isAttach;
        private string _passwordChar;
        
        private readonly Regex EmailMask = new Regex(@"/^\S+@\S+\.\S+$/");
        /// <summary>
        /// Получает или задает идентификатор пользователя.
        /// </summary>
        public int NumberId
        {
            get => numberID;
            set => this.RaiseAndSetIfChanged(ref numberID, value);
        }

        /// <summary>
        /// Получает или задает флаг видимости пароля. При изменении флага, меняется символ отображения пароля.
        /// </summary>
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref isVisible, value);
                PasswordChar = value ? "" : "*";
            } 
        }

        /// <summary>
        /// Получает или задает флаг прикрепления изображения.
        /// </summary>
        public bool IsAttach
        {
            get => isAttach;
            set => this.RaiseAndSetIfChanged(ref isAttach, value);
        }

        /// <summary>
        /// Получает или задает полное имя пользователя.
        /// </summary>
        public string FIO { get => fio; set => this.RaiseAndSetIfChanged(ref fio, value); }

        /// <summary>
        /// Получает или задает путь к изображению пользователя.
        /// </summary>
        public string Image { get => image; set => this.RaiseAndSetIfChanged(ref image, value); }

        /// <summary>
        /// Получает или задает дополнительную информацию о пользователе.
        /// </summary>
        public string Info { get => info; set => this.RaiseAndSetIfChanged(ref info, value); }

        /// <summary>
        /// Получает или задает электронную почту пользователя.
        /// </summary>
        public string Email { get => email; set => this.RaiseAndSetIfChanged(ref email, value); }

        /// <summary>
        /// Получает или задает номер телефона пользователя.
        /// </summary>
        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (value == null) return;
                this.RaiseAndSetIfChanged(ref phoneNumber, value);
            }
        }

        /// <summary>
        /// Получает или задает пароль пользователя.
        /// </summary>
        public string Password { get => password; set => this.RaiseAndSetIfChanged(ref password, value); }

        /// <summary>
        /// Получает или задает подтверждение пароля.
        /// </summary>
        public string ConfirmPassword { get => confirmPassword; set => this.RaiseAndSetIfChanged(ref confirmPassword, value); }

        /// <summary>
        /// Получает или задает список ролей пользователя.
        /// </summary>
        public List<Role> Roles { get => roles; set => this.RaiseAndSetIfChanged(ref roles, value); }

        /// <summary>
        /// Получает или задает выбранную роль пользователя.
        /// </summary>
        public Role SelectedRole { get => selectedRole; set => this.RaiseAndSetIfChanged(ref selectedRole, value); }

        /// <summary>
        /// Получает или задает список типов мероприятий.
        /// </summary>
        public List<TypeOfEvent> TypeOfEvents { get => typeOfEvents; set => this.RaiseAndSetIfChanged(ref typeOfEvents, value); }

        /// <summary>
        /// Получает или задает выбранный тип мероприятия. При изменении, фильтрует список событий.
        /// </summary>
        public TypeOfEvent SelectedTypeOfEvent
        {
            get => selectedTypeOfEvent;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedTypeOfEvent, value);
                FiltredEvents(); // Фильтрация событий по выбранному типу
            } 
        }

        /// <summary>
        /// Получает или задает список событий.
        /// </summary>
        public List<Event> Events { get => events; set => this.RaiseAndSetIfChanged(ref events, value); }

        /// <summary>
        /// Получает или задает выбранное событие.
        /// </summary>
        public Event SelectedEvent { get => selectedEvent; set => this.RaiseAndSetIfChanged(ref selectedEvent, value); }

        /// <summary>
        /// Получает или задает список доступных полов.
        /// </summary>
        public List<Gender> Genders { get => genders; set => this.RaiseAndSetIfChanged(ref genders, value); }

        /// <summary>
        /// Получает или задает выбранный пол.
        /// </summary>
        public Gender SelectedGender { get => selectedGender; set => this.RaiseAndSetIfChanged(ref selectedGender, value); }

        /// <summary>
        /// Получает или задает символ, используемый для отображения скрытого пароля.
        /// </summary>
        public string PasswordChar { get => _passwordChar; set => this.RaiseAndSetIfChanged(ref _passwordChar, value); }

        /// <summary>
        /// Конструктор, инициализирующий данные для регистрации пользователя, включая генерирование уникального ID.
        /// </summary>
        public RegistrationViewModel()
        {
            Random rnd = new Random();
            NumberId = rnd.Next(10000, 99999); // Генерация случайного ID
            
            // Инициализация списков с выбором по умолчанию
            Genders = Db.Genders.ToList();
            Genders.Add(new Gender() { Title = "Выберите пол", IdGender = 0 });
            Genders = Genders.OrderBy(x => x.IdGender).ToList();
            SelectedGender = Genders.First(x => x.IdGender == 0);
            
            Roles = Db.Roles.ToList();
            Roles.RemoveAll(x => x.Title == "Организатор" || x.Title == "Участник");
            Roles.Add(new Role() { IdRole = 0, Title = "Выберите роль" });
            SelectedRole = Roles.FirstOrDefault(x => x.IdRole == 0);
            
            TypeOfEvents = Db.TypeOfEvents.ToList();
            Events = Db.Events.ToList();
            
            IsVisible = false;
            IsAttach = false;
            Image = "";
        }

        /// <summary>
        /// Метод для выбора и сохранения изображения для пользователя.
        /// </summary>
        public async Task SelectAndSaveImage()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop || desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Отсутствует экземпляр StorageProvider.");

            var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Выберите аватарку",
                AllowMultiple = false
            });

            if (files.Count > 0)
            {
                await using var readStream = await files[0].OpenReadAsync();
                var savePath = Path.Combine(Environment.CurrentDirectory, "Image", NumberId + ".png");
                await using var writeStream = new FileStream(savePath, FileMode.Create);
                await readStream.CopyToAsync(writeStream);
            }
            Image = NumberId + ".png";
        }

        /// <summary>
        /// Метод для фильтрации списка событий по выбранному типу мероприятия.
        /// </summary>
        private void FiltredEvents()
        {
            Events = Db.Events.Where(x => x.IdTypeOfEventNavigation == SelectedTypeOfEvent).ToList();
        }

        /// <summary>
        /// Метод для отмены регистрации и возврата к экрану организатора.
        /// </summary>
        public void Cancel()
        {
            MainWindowViewModel.Self.UC = new OrganizatorView();
        }

        /// <summary>
        /// Метод для сохранения данных пользователя, с валидацией обязательных полей и пароля.
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(FIO) || 
                SelectedGender.IdGender == 0 || string.IsNullOrWhiteSpace(PhoneNumber) || SelectedRole.IdRole == 0 || SelectedTypeOfEvent.IdTypeOfEvent == 0)
            {
                Info = "Заполните все необходимые поля";
                return;
            }

            if (!EmailMask.IsMatch(Email))
            {
                Info = "Некорректная почта";
                return;
            }

            // Проверка пароля на сложность
            if (Password.Length < 6 || !Password.Any(char.IsUpper) || !Password.Any(char.IsLower) || !Password.Any(char.IsDigit) || Password.All(char.IsLetterOrDigit))
            {
                Info = "Пароль должен содержать минимум 6 символов, одну заглавную и одну строчную букву, цифру и спецсимвол.";
                return;
            }

            if (Password != ConfirmPassword)
            {
                Info = "Пароли не совпадают";
                return;
            }
            
            string[] words = FIO.Split(" ");
            
            User newUser = new User()
            {
                IdUser = NumberId,
                LastName = words[0],
                FirstName = words[1],
                Patronymic = words[2],
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = Password,
                IdRole = SelectedRole.IdRole,
                IdTypeOfEvents = SelectedTypeOfEvent.IdTypeOfEvent,
            };
            if (Image != "") newUser.Image = NumberId + ".png";

            try
            {
                Db.Users.Add(newUser);
                Db.SaveChanges();
                MainWindowViewModel.Self.UC = new OrganizatorView();
                
            }
            catch (Exception ex)
            {
                Info = ex.Message;
            }
        }
    }
}
