using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using conference.Models;
using conference.Views;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace conference.ViewModels
{
    /// <summary>
    /// ViewModel для обработки логики входа пользователя.
    /// </summary>
    public class SignInViewModel : ViewModelBase
    {
        private DispatcherTimer _timer;
        private int timerCounter = 10;
        string username;
        string password;
        string inputCapcha;
        string info = "";
        private int counter = 0;
        bool isEnabled = true;
        string capcha;
        Canvas canvasCapcha;

        /// <summary>
        /// Свойство для хранения имени пользователя.
        /// </summary>
        public string Username
        {
            get => username;
            set => this.RaiseAndSetIfChanged(ref username, value);
        }

        /// <summary>
        /// Свойство для ввода капчи пользователем.
        /// </summary>
        public string InputCapcha
        {
            get => inputCapcha;
            set => this.RaiseAndSetIfChanged(ref inputCapcha, value);
        }

        /// <summary>
        /// Свойство для хранения Canvas, на котором отображается капча.
        /// </summary>
        public Canvas CanvasCapcha
        {
            get => canvasCapcha;
            set => this.RaiseAndSetIfChanged(ref canvasCapcha, value);
        }

        /// <summary>
        /// Свойство для включения/выключения элементов UI (например, кнопки входа).
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set => this.RaiseAndSetIfChanged(ref isEnabled, value);
        }

        /// <summary>
        /// Свойство для хранения текущей капчи.
        /// </summary>
        public string Capcha
        {
            get => capcha;
            set => this.RaiseAndSetIfChanged(ref capcha, value);
        }

        /// <summary>
        /// Свойство для хранения пароля пользователя.
        /// </summary>
        public string Password
        {
            get => password;
            set => this.RaiseAndSetIfChanged(ref password, value);
        }

        /// <summary>
        /// Свойство для вывода информации о статусе входа.
        /// </summary>
        public string Info
        {
            get => info;
            set => this.RaiseAndSetIfChanged(ref info, value);
        }

        /// <summary>
        /// Конструктор, который создает капчу при инициализации ViewModel.
        /// </summary>
        public SignInViewModel()
        {
            CreateCapcha();
        }

        /// <summary>
        /// Навигация к публичному виду при успешной аутентификации.
        /// </summary>
        public void NavigateToPublicView()
        {
            MainWindowViewModel.Self.UC = new PublicView();
        }

        /// <summary>
        /// Обработка входа пользователя: проверка капчи, данных и аутентификация.
        /// </summary>
        public void Enter()
        {
            Info = "";
            // Проверка заполненности всех полей
            if (Password == "" || Username == "" || InputCapcha == "")
            {
                Info = "Не все поля заполнены";
                CreateCapcha();
                return;
            }

            int number;
            // Проверка корректности ввода номера пользователя
            if (!int.TryParse(Username, out number))
            {
                Info = "Введен некорректный номер";
                CreateCapcha();
                return;
            }

            // Проверка правильности введенной капчи
            if (InputCapcha != Capcha)
            {
                Info = "Неверно введена капча";
                InputCapcha = "";
                CreateCapcha();
                return;
            }

            // Проверка данных пользователя в базе данных
            User userFromBase = Db.Users.Include(user => user.IdRoleNavigation).Include(x => x.IdGenderNavigation)
                .FirstOrDefault(x => x.Password == Password && x.IdUser == Convert.ToInt32(Username));
            if (userFromBase != null)
            {
                MainWindowViewModel.Self.LoginedUser = userFromBase;
                // Навигация в зависимости от роли пользователя
                switch (userFromBase.IdRoleNavigation.Title)
                {
                    case "Организатор":
                        MainWindowViewModel.Self.UC = new OrganizatorView();
                        break;
                    case "Участник":
                        MainWindowViewModel.Self.UC = new ParticipantView();
                        break;
                    case "Жюри":
                        MainWindowViewModel.Self.UC = new JureView();
                        break;
                    case "Модератор":
                        MainWindowViewModel.Self.UC = new ModeratorView();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                counter++;
                if (counter == 3)
                {
                    Info = "Следущая попытка доступна через 10 секунд";
                    IsEnabled = false;
                    _timer.Interval = new TimeSpan(0, 0, 10);
                    _timer.Tick += Timer_Tick!;
                    _timer.Start();
                }
                else
                {
                    Info = "Неверные данные для входа";
                }
            }
            CreateCapcha();
        }

        /// <summary>
        /// Обработчик события таймера, который сбрасывает счетчик и включает элементы управления.
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            IsEnabled = true;
            counter = 0;
        }

        /// <summary>
        /// Метод для генерации новой капчи с символами и линиями.
        /// </summary>
        public void CreateCapcha()
        {
            Random rand = new Random();
            const string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int lenth = 4;
            Capcha = "";
            Canvas canvas = new Canvas()
            {
                Width = 200,
                Height = 90,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Background = new SolidColorBrush(Colors.LightGray)
            };
            double startX = rand.Next(10, 20); 
            // Генерация символов капчи
            for (int i = 0; i < lenth; i++)
            {
                TextBlock myTextBlock = new TextBlock();
                myTextBlock.FontSize = 20;
                myTextBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
                myTextBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
                myTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                myTextBlock.FontWeight = FontWeight.Bold;

                int index = rand.Next(symbols.Length);
                char result = symbols[index];
                myTextBlock.Text = result.ToString();
                Capcha += Convert.ToString(result);

                Canvas.SetLeft(myTextBlock, startX + (i * 30));
                Canvas.SetTop(myTextBlock, rand.Next(5, 40));
                canvas.Children.Add(myTextBlock);
            }

            // Генерация случайных линий для усложнения капчи
            for (int i = 0; i < rand.Next(15, 40); i++)
            {
                Line line = new Line()
                {
                    StartPoint = new Avalonia.Point(rand.Next(200), rand.Next(90)),
                    EndPoint = new Avalonia.Point(rand.Next(200), rand.Next(90)),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = rand.Next(3)
                };
                canvas.Children.Add(line);
            }

            CanvasCapcha = canvas;
        }
    }
}
