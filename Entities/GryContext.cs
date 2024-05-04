using Gry_Słownikowe.Entions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;


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

        public DbSet<User>          User                { get; set; }
        public DbSet<Krzyzowki>     Krzyzowki           { get; set; }
        public DbSet<Wisielec>      Wisielecs           { get; set; }
        public DbSet<Wordle>        Wordle              { get; set; }
        public DbSet<Zgadywanki>    Zgadywanki          { get; set; }
        public DbSet<Slownikowo>    Slownikowo          { get; set; }


        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            
            // W metodzie OnModelCreating:
            modelBuilder.Entity<User>(eb => {
                eb.Property(passwd => passwd.Password)
                    .IsRequired()
                    .HasConversion(
                        // Konwertuje string (hasło) na zahaszowany string przed zapisaniem do bazy danych
                        // Tutaj używamy przykładowego algorytmu haszowania, w tym przypadku SHA256
                        // Możesz wybrać odpowiedni algorytm zależnie od Twoich wymagań bezpieczeństwa
                        pass => HashPassword(pass),
                        // Konwersja zahaszowanego stringa z bazy danych na string
                        hashedPass => hashedPass
                    );
            });

            // Konfiguruje encję Users w modelu danych
            modelBuilder.Entity<User>(eb=>{
                eb.Property(e => e.Id).ValueGeneratedOnAdd();

                // Ustawia wymaganie, żeby pola w encji Users było niepuste 
                // ( nie mogą być null ), ( Są wymagane )
                eb.Property(nick => nick.Nick).IsRequired();
                eb.Property(login => login.Login)
                .IsRequired()
                .HasMaxLength(20);
                eb.Property(passwd => passwd.Password)
                .IsRequired()
                .HasMaxLength(50); 

            });

            modelBuilder.Entity<Krzyzowki>(eb => {
                eb.Property(e => e.Id).ValueGeneratedOnAdd();
                eb.Property(gd => gd.GameData).HasDefaultValueSql("GETDATE()");

                eb.Property(win => win.Win).IsRequired();
                eb.Property(loss => loss.Loss).IsRequired();
                eb.Property(gameTime => gameTime.GameTime).IsRequired();
                eb.Property(gameData => gameData.GameData).IsRequired(false); // Nie wymagane, ponieważ jest to typ nullable
            });

            modelBuilder.Entity<Wisielec>(eb => {
                eb.Property(e => e.Id).ValueGeneratedOnAdd();
                eb.Property(gd => gd.GameData).HasDefaultValueSql("GETDATE()");

                eb.Property(win => win.Win).IsRequired();
                eb.Property(loss => loss.Loss).IsRequired();
                eb.Property(gameTime => gameTime.GameTime).IsRequired();
                eb.Property(gameData => gameData.GameData).IsRequired(false); // Nie wymagane, ponieważ jest to typ nullable
            });

            modelBuilder.Entity<Wordle>(eb => {
                eb.Property(e => e.Id).ValueGeneratedOnAdd();
                eb.Property(gd => gd.GameData).HasDefaultValueSql("GETDATE()");

                eb.Property(win => win.Win).IsRequired();
                eb.Property(loss => loss.Loss).IsRequired();
                eb.Property(gameTime => gameTime.GameTime).IsRequired();
                eb.Property(gameData => gameData.GameData).IsRequired(false); // Nie wymagane, ponieważ jest to typ nullable
            });

            modelBuilder.Entity<Zgadywanki>(eb => {
                eb.Property(e => e.Id).ValueGeneratedOnAdd();
                eb.Property(gd => gd.GameData).HasDefaultValueSql("GETDATE()");

                eb.Property(win => win.Win).IsRequired();
                eb.Property(loss => loss.Loss).IsRequired();
                eb.Property(gameTime => gameTime.GameTime).IsRequired();
                eb.Property(gameData => gameData.GameData).IsRequired(false); // Nie wymagane, ponieważ jest to typ nullable
            });
            modelBuilder.Entity<Slownikowo>(eb => {
                eb.Property(e => e.Id).ValueGeneratedOnAdd();
                eb.Property(gd => gd.GameData).HasDefaultValueSql("GETDATE()");

                eb.Property(win => win.Win).IsRequired();
                eb.Property(loss => loss.Loss).IsRequired();
                eb.Property(gameTime => gameTime.GameTime).IsRequired();
                eb.Property(gameData => gameData.GameData).IsRequired(false); // Nie wymagane, ponieważ jest to typ nullable
            });



            modelBuilder.Entity<Krzyzowki>(eb => {
                eb.HasOne(u => u.User)
                    .WithMany(k => k.Krzyzowki)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Definiuje zachowanie usuwania encji zależnych (cascade delete)
            });

            modelBuilder.Entity<Wisielec>(eb => {
                eb.HasOne(u => u.User)
                    .WithMany(w => w.Wisielec)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Definiuje zachowanie usuwania encji zależnych (cascade delete)
            });

            modelBuilder.Entity<Wordle>(eb => {
                eb.HasOne(u => u.User)
                    .WithMany(w => w.Wordle)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Definiuje zachowanie usuwania encji zależnych (cascade delete)
            });

            modelBuilder.Entity<Zgadywanki>(eb => {
                eb.HasOne(u => u.User)
                    .WithMany(z => z.Zgadywanki)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Definiuje zachowanie usuwania encji zależnych (cascade delete)
            });
            modelBuilder.Entity<Slownikowo>(eb => {
                eb.HasOne(u => u.User)
                    .WithMany(z => z.Slownikowo)
                    .HasForeignKey(u => u.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Definiuje zachowanie usuwania encji zależnych (cascade delete)
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
            //modelBuilder.Entity<User>().HasData(
            //new User
            //{
            //    Id = 1,
            //    Nick = "User1",
            //    Login = "user1@example.com",
            //    Password = "hashed_password1", // Upewnij się, że używasz bezpiecznej metody haszowania hasła
               
            //},
            //new User
            //{
            //    Id = 2,
            //    Nick = "User2",
            //    Login = "user2@example.com",
            //    Password = "hashed_password2",
               
            //},
            // new User
            // {
            //     Id = 3,
            //     Nick = "User3",
            //     Login = "user3@example.com",
            //     Password = "hashed_password3",
            //     
                 
            // }
            


        // Dodaj więcej danych, jeśli to konieczne
        //);
            
        }
    }

}
