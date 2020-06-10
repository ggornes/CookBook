namespace CookBookData.Model.DbContext
{
    using MySql.Data.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class CookBookContext : DbContext
    {
        // Your context has been configured to use a 'CookBookContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CookBookData.Model.DbContext.CookBookContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CookBookContext' 
        // connection string in the application configuration file.
        public CookBookContext()
            : base("name=CookBookContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Recipe>()
                .HasMany(e => e.RecipeIngredients)
                .WithRequired(e => e.recipe)
                .HasForeignKey(e => e.recipeId);

            modelBuilder.Entity<Recipe>()
                .HasMany(e => e.recipeSteps)
                .WithRequired(e => e.recipe)
                .HasForeignKey(e => e.recipeId);

            modelBuilder.Entity<Ingredient>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Ingredient>()
                .HasIndex(e => e.name)
                .IsUnique();

            modelBuilder.Entity<Ingredient>()
                .HasMany(e => e.recipeIngredients)
                .WithRequired(e => e.ingredient)
                .HasForeignKey(e => e.ingredientId)
                .WillCascadeOnDelete();

            //modelBuilder.Entity<Ingredient>().HasData(new Ingredient { Id = 1, name = "salt" });
            //modelBuilder.Entity<Ingredient>().HasData(new Ingredient() { Id = 1, name = "test"});
            //modelBuilder.Entity<Ingredient>().HasData(new { Id = 1, name = "Second post"});

            modelBuilder.Entity<Measure>()
                .Property(e => e.name).HasColumnType("VARCHAR").HasMaxLength(32)
                .IsUnicode(false);

            modelBuilder.Entity<Measure>()
                .HasMany(e => e.recipeIngredients)
                .WithOptional(e => e.measure)
                .HasForeignKey(e => e.measureId)
                .WillCascadeOnDelete();

            //modelBuilder.Entity<Measure>().HasData(new Measure { Id = 1, name = "cup" });

            modelBuilder.Entity<RecipeStep>()
                .Property(e => e.stepInstructions)
                .IsUnicode(false);

        }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}