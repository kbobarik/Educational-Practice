using System;
using System.Collections.Generic;
using System.Linq;
using conference.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace conference.ViewModels;

/// <summary>
/// ViewModel для публичного просмотра мероприятий и фильтрации по типу и дате.
/// </summary>
public class PublicViewModel : ViewModelBase
{
    // Список всех мероприятий
    private List<Event> events;
    public List<Event> Events
    {
        get => events;
        set => this.RaiseAndSetIfChanged(ref events, value);
    }

    // Исходный (нефильтрованный) список мероприятий
    private List<Event> events0;
    public List<Event> Events0
    {
        get => events0;
        set => this.RaiseAndSetIfChanged(ref events0, value);
    }

    // Список типов мероприятий
    private List<TypeOfEvent> typeOfEvents;
    public List<TypeOfEvent> TypeOfEvents
    {
        get => typeOfEvents;
        set => this.RaiseAndSetIfChanged(ref typeOfEvents, value);
    }

    // Выбранный тип мероприятия для фильтрации
    private TypeOfEvent selectedEvent;
    public TypeOfEvent SelectedEvent
    {
        get => selectedEvent;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedEvent, value);
            Search(); // Вызов метода фильтрации при изменении значения
        }
    }

    // Видимость сообщения о пустом результате поиска
    private bool isVisible;
    public bool IsVisible
    {
        get => isVisible;
        set => this.RaiseAndSetIfChanged(ref isVisible, value);
    }

    // Список доступных дат мероприятий
    private List<string> dateEvents;
    public List<string> DateEvents
    {
        get => dateEvents;
        set => this.RaiseAndSetIfChanged(ref dateEvents, value);
    }

    // Выбранная дата для фильтрации
    private string selectedDate;
    public string SelectedDate
    {
        get => selectedDate;
        set
        {
            this.RaiseAndSetIfChanged(ref selectedDate, value);
            Search(); // Вызов метода фильтрации при изменении значения
        }
    }

    /// <summary>
    /// Конструктор. Загружает данные и инициализирует свойства.
    /// </summary>
    public PublicViewModel()
    {
        // Загрузка всех мероприятий с привязкой к типам из базы данных
        Events0 = Db.Events.Include(x => x.IdTypeOfEventNavigation).ToList();
        Events = Events0;

        // Загрузка всех типов мероприятий и добавление опции "Выберите тип мероприятия"
        TypeOfEvents = Db.TypeOfEvents.ToList();
        TypeOfEvents.Add(new TypeOfEvent() { IdTypeOfEvent = 0, Title = "Выберите тип мероприятия" });
        TypeOfEvents = TypeOfEvents.OrderBy(x => x.IdTypeOfEvent).ToList();
        
        // Установка типа мероприятия по умолчанию
        SelectedEvent = TypeOfEvents.FirstOrDefault(x => x.IdTypeOfEvent == 0);

        // Скрытие сообщения о пустом результате поиска
        IsVisible = false;

        // Формирование списка уникальных дат из мероприятий и добавление опции "Выберите дату"
        DateEvents = Events0.Select(x => x.Date.ToString()).Distinct().Order().ToList();
        DateEvents.Insert(0, "Выберите дату");

        // Установка даты по умолчанию
        SelectedDate = DateEvents[0];
    }

    /// <summary>
    /// Метод для фильтрации мероприятий по выбранной дате и типу.
    /// </summary>
    private void Search()
    {
        // Сброс списка к полному исходному списку
        Events = Events0;

        // Проверка на выбор и типа мероприятия, и даты
        if (SelectedEvent.IdTypeOfEvent != 0 && SelectedDate != "Выберите дату")
        {
            Events = Events0
                .Where(x => x.IdTypeOfEvent == SelectedEvent.IdTypeOfEvent && x.Date.ToString() == SelectedDate)
                .ToList();
        }
        else
        {
            // Фильтрация только по типу мероприятия, если он выбран
            if (SelectedEvent.IdTypeOfEvent != 0)
            {
                Events = Events0
                    .Where(x => x.IdTypeOfEvent == SelectedEvent.IdTypeOfEvent)
                    .ToList();
            }

            // Фильтрация только по дате, если она выбрана
            if (SelectedDate != "Выберите дату")
            {
                Events = Events0
                    .Where(x => x.Date.ToString() == SelectedDate)
                    .ToList();
            }
        }

        // Установка видимости сообщения, если мероприятий нет
        IsVisible = Events.Count == 0;
    }

    /// <summary>
    /// Метод для навигации на страницу авторизации.
    /// </summary>
    public void NavigationToSignIn()
    {
        MainWindowViewModel.Self.UC = new SignInView();
    }
}
