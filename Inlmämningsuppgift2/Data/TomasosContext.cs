using Inlmämningsuppgift2.Models;
using Inlmämningsuppgift2.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inlmämningsuppgift2.Data
{
    public partial class TomasosContext : DbContext
    {
        public TomasosContext(DbContextOptions<TomasosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderFoodItem> OrderFoodItems { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FoodItem> FoodItems { get; set; }
        public virtual DbSet<FoodItemProduct> FoodItemProducts { get; set; }
        public virtual DbSet<FoodItemType> FoodItemTypes { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Bestallning");
                entity.Property(e => e.OrderId).HasColumnName("BestallningID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("BestallningDatum");

                entity.Property(e => e.TotalAmount).HasColumnName("Totalbelopp");

                entity.Property(e => e.Delivered).HasColumnName("Levererad");

                entity.Property(e => e.CustomerId).HasColumnName("KundID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Bestallning_Kund");
            });

            modelBuilder.Entity<OrderFoodItem>(entity =>
            {
                entity.ToTable("BestallningMatratt");
                entity.HasKey(e => new { MatrattId = e.FoodItemId, BestallningId = e.OrderId });

                entity.Property(e => e.FoodItemId).HasColumnName("MatrattID");

                entity.Property(e => e.Antal).HasColumnName("Antal");

                entity.Property(e => e.OrderId).HasColumnName("BestallningID");

                entity.Property(e => e.Antal).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderFoodItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BestallningMatratt_Bestallning");

                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.OrderFoodItems)
                    .HasForeignKey(d => d.FoodItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BestallningMatratt_Matratt");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Kund");
                entity.Property(e => e.CustomerId).HasColumnName("KundID");

                /*entity.Property(e => e.Username)
                    .HasColumnName("AnvandarNamn")
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);*/

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Adress)
                    .HasColumnName("Gatuadress")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                /*entity.Property(e => e.Password)
                    .HasColumnName("Losenord")
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);*/

                entity.Property(e => e.Name)
                    .HasColumnName("Namn")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasColumnName("Postnr")
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("Postort")
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("Telefon")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.ToTable("Matratt");
                entity.Property(e => e.FoodItemId).HasColumnName("MatrattID");

                entity.Property(e => e.Price).HasColumnName("Pris");

                entity.Property(e => e.FoodItemTypeId).HasColumnName("MatrattTyp");

                entity.Property(e => e.Description)
                    .HasColumnName("Beskrivning")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasColumnName("MatrattNamn")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FoodItemType)
                    .WithMany(p => p.FoodItems)
                    .HasForeignKey(d => d.FoodItemTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Matratt_MatrattTyp");
            });

            modelBuilder.Entity<FoodItemProduct>(entity =>
            {
                entity.ToTable("MatrattProdukt");
                entity.HasKey(e => new { MatrattId = e.FoodItemId, ProduktId = e.ProductId });

                entity.Property(e => e.FoodItemId).HasColumnName("MatrattID");

                entity.Property(e => e.ProductId).HasColumnName("ProduktID");

                entity.HasOne(d => d.FoodItem)
                    .WithMany(p => p.FoodItemProducts)
                    .HasForeignKey(d => d.FoodItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MatrattProdukt_Matratt");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.FoodItemProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MatrattProdukt_Produkt");
            });

            modelBuilder.Entity<FoodItemType>(entity =>
            {
                entity.ToTable("MatrattTyp");
                entity.HasKey(e => e.FoodItemTypeId);

                entity.Property(e => e.FoodItemTypeId).HasColumnName("MatrattTyp");

                entity.Property(e => e.Description)
                    .HasColumnName("Beskrivning")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Produkt");
                entity.Property(e => e.ProductId).HasColumnName("ProduktID");

                entity.Property(e => e.ProductNamn)
                    .HasColumnName("ProduktNamn")
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
