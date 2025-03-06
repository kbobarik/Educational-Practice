using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace conference.Models;

public partial class StudingContext : DbContext
{
    public StudingContext()
    {
    }

    public StudingContext(DbContextOptions<StudingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleActivity> ScheduleActivities { get; set; }

    public virtual DbSet<SheduleActivityJure> SheduleActivityJures { get; set; }

    public virtual DbSet<TypeOfEvent> TypeOfEvents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=12345;Database=studing;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.IdActivity).HasName("activities_pk");

            entity.ToTable("activities");

            entity.Property(e => e.IdActivity).HasColumnName("id_activity");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.IdCity).HasName("cities_pk");

            entity.ToTable("cities");

            entity.Property(e => e.IdCity).HasColumnName("id_city");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("countries_pk");

            entity.ToTable("countries");

            entity.Property(e => e.IdCountry).HasColumnName("id_country");
            entity.Property(e => e.Code1)
                .HasColumnType("character varying")
                .HasColumnName("code1");
            entity.Property(e => e.Code2).HasColumnName("code2");
            entity.Property(e => e.EnglishTitle)
                .HasColumnType("character varying")
                .HasColumnName("english_title");
            entity.Property(e => e.RussianTitle)
                .HasColumnType("character varying")
                .HasColumnName("russian_title");
        });

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.IdDirection).HasName("directions_pk");

            entity.ToTable("directions");

            entity.Property(e => e.IdDirection)
                .ValueGeneratedNever()
                .HasColumnName("id_direction");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.IdEvent).HasName("events_pk");

            entity.ToTable("events");

            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdCity).HasColumnName("id_city");
            entity.Property(e => e.IdTypeOfEvent).HasColumnName("id_type_of_event");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.IdCityNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdCity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_cities_fk");

            entity.HasOne(d => d.IdTypeOfEventNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.IdTypeOfEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("events_type_of_events_fk");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.IdGender).HasName("genders_pk");

            entity.ToTable("genders");

            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("images_pk");

            entity.ToTable("images");

            entity.Property(e => e.IdImage).HasColumnName("id_image");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasMany(d => d.IdCities).WithMany(p => p.IdImages)
                .UsingEntity<Dictionary<string, object>>(
                    "CitiesImage",
                    r => r.HasOne<City>().WithMany()
                        .HasForeignKey("IdCity")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("cities_images_cities_fk"),
                    l => l.HasOne<Image>().WithMany()
                        .HasForeignKey("IdImage")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("cities_images_images_fk"),
                    j =>
                    {
                        j.HasKey("IdImage", "IdCity").HasName("cities_images_pk");
                        j.ToTable("cities_images");
                        j.IndexerProperty<int>("IdImage").HasColumnName("id_image");
                        j.IndexerProperty<int>("IdCity").HasColumnName("id_city");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("roles_pk");

            entity.ToTable("roles");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.IdSchedule).HasName("schedule_pk");

            entity.ToTable("schedule");

            entity.Property(e => e.IdSchedule).HasColumnName("id_schedule");
            entity.Property(e => e.IdEvent).HasColumnName("id_event");
            entity.Property(e => e.IdWinner).HasColumnName("id_winner");

            entity.HasOne(d => d.IdEventNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedule_events_fk");

            entity.HasOne(d => d.IdWinnerNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.IdWinner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedule_users_fk");
        });

        modelBuilder.Entity<ScheduleActivity>(entity =>
        {
            entity.HasKey(e => e.IdSheduleActivity).HasName("schedule_activity_pk");

            entity.ToTable("schedule_activity");

            entity.Property(e => e.IdSheduleActivity).HasColumnName("id_shedule_activity");
            entity.Property(e => e.IdActivity).HasColumnName("id_activity");
            entity.Property(e => e.IdModerator).HasColumnName("id_moderator");
            entity.Property(e => e.IdSchedule).HasColumnName("id_schedule");
            entity.Property(e => e.NumberDay).HasColumnName("number_day");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.IdActivityNavigation).WithMany(p => p.ScheduleActivities)
                .HasForeignKey(d => d.IdActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedule_activity_activities_fk");

            entity.HasOne(d => d.IdModeratorNavigation).WithMany(p => p.ScheduleActivities)
                .HasForeignKey(d => d.IdModerator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedule_activity_users_fk");

            entity.HasOne(d => d.IdScheduleNavigation).WithMany(p => p.ScheduleActivities)
                .HasForeignKey(d => d.IdSchedule)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("schedule_activity_schedule_fk");
        });

        modelBuilder.Entity<SheduleActivityJure>(entity =>
        {
            entity.HasKey(e => e.IdSheduleActivityJure).HasName("shedule_activity_jure_pk");

            entity.ToTable("shedule_activity_jure");

            entity.Property(e => e.IdSheduleActivityJure).HasColumnName("id_shedule_activity_jure");
            entity.Property(e => e.IdJure).HasColumnName("id_jure");
            entity.Property(e => e.IdScheduleActivity).HasColumnName("id_schedule_activity");

            entity.HasOne(d => d.IdJureNavigation).WithMany(p => p.SheduleActivityJures)
                .HasForeignKey(d => d.IdJure)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shedule_activity_jure_users_fk");

            entity.HasOne(d => d.IdScheduleActivityNavigation).WithMany(p => p.SheduleActivityJures)
                .HasForeignKey(d => d.IdScheduleActivity)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shedule_activity_jure_schedule_activity_fk");
        });

        modelBuilder.Entity<TypeOfEvent>(entity =>
        {
            entity.HasKey(e => e.IdTypeOfEvent).HasName("type_of_events_pk");

            entity.ToTable("type_of_events");

            entity.Property(e => e.IdTypeOfEvent).HasColumnName("id_type_of_event");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("jury_pk");

            entity.ToTable("users");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.DateOfBitrh).HasColumnName("date_of_bitrh");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.IdCounrty).HasColumnName("id_counrty");
            entity.Property(e => e.IdDirection).HasColumnName("id_direction");
            entity.Property(e => e.IdGender).HasColumnName("id_gender");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.IdTypeOfEvents).HasColumnName("id_type_of_events");
            entity.Property(e => e.Image)
                .HasColumnType("character varying")
                .HasColumnName("image");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasColumnType("character varying")
                .HasColumnName("patronymic");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("character varying")
                .HasColumnName("phone_number");

            entity.HasOne(d => d.IdCounrtyNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCounrty)
                .HasConstraintName("users_countries_fk");

            entity.HasOne(d => d.IdDirectionNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdDirection)
                .HasConstraintName("users_directions_fk");

            entity.HasOne(d => d.IdGenderNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdGender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_genders_fk");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_roles_fk");

            entity.HasOne(d => d.IdTypeOfEventsNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdTypeOfEvents)
                .HasConstraintName("users_type_of_events_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
