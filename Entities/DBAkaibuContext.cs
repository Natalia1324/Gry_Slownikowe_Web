using Gry_Słownikowe.Entions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
namespace Gry_Słownikowe.Entities
{
    public class GryContext : DbContext
    {
       
        public GryContext(DbContextOptions<GryContext> options) : base(options)
        {

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;

            // Dodaj tę linijkę, aby włączyć bardziej szczegółowe logowanie, w tym informacje o konfliktach kluczy
            // Może pomóc w zidentyfikowaniu, które encje powodują konflikty
            // Uwaga: Nie używaj tego w środowisku produkcyjnym z uwagi na bezpieczeństwo danych
            // DbContextOptionsBuilder.EnableSensitiveDataLogging();

        }
       
        public DbSet<Users> Users { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder){

            // Konfiguruje encję Users w modelu danych
            modelBuilder.Entity<Users>(eb=>{
                eb.Property(e => e.Id)
                 .ValueGeneratedOnAdd();

                // Ustawia wymaganie, żeby pola w encji Users było niepuste 
                // ( nie mogą być null ), ( Są wymagane )
                eb.Property(nick => nick.Nick).IsRequired();
                eb.Property(login => login.Login).IsRequired();
                //eb.Property(nick => nick.Ranks).IsRequired();
                eb.Property(passwd => passwd.Password).IsRequired();


                // Ustawia domyślną wartość 0 dla pola Ranks w encji Users
                //eb.Property(ranks => ranks.Ranks).HasDefaultValue(0);
            });

            SeedData(modelBuilder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Gry;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Przykładowe dane do tabeli Users
            modelBuilder.Entity<Users>().HasData(
            new Users
            {
                Id = 1,
                Nick = "User1",
                Login = "user1@example.com",
                Password = "hashed_password1", // Upewnij się, że używasz bezpiecznej metody haszowania hasła
               
            },
            new Users
            {
                Id = 2,
                Nick = "User2",
                Login = "user2@example.com",
                Password = "hashed_password2",
               
            },
             new Users
             {
                 Id = 3,
                 Nick = "User3",
                 Login = "user3@example.com",
                 Password = "hashed_password3",
                 Ranks = 69,
                 
             }
            


        // Dodaj więcej danych, jeśli to konieczne
        );
            
        }
    }

}
