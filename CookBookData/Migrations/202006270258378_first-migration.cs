namespace CookBookData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.name, unique: true);
            
            CreateTable(
                "dbo.recipe_ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        recipeId = c.Int(nullable: false),
                        ingredientId = c.Int(nullable: false),
                        measureId = c.Int(),
                        amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Measures", t => t.measureId, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.recipeId, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.ingredientId, cascadeDelete: true)
                .Index(t => t.recipeId)
                .Index(t => t.ingredientId)
                .Index(t => t.measureId);
            
            CreateTable(
                "dbo.Measures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.name, unique: true);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 32, unicode: false),
                        prepTime = c.Int(nullable: false),
                        favorite = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.recipe_steps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        recipeId = c.Int(nullable: false),
                        stepNumber = c.Int(nullable: false),
                        stepInstructions = c.String(nullable: false, maxLength: 512, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.recipeId, cascadeDelete: true)
                .Index(t => t.recipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.recipe_ingredients", "ingredientId", "dbo.Ingredients");
            DropForeignKey("dbo.recipe_steps", "recipeId", "dbo.Recipes");
            DropForeignKey("dbo.recipe_ingredients", "recipeId", "dbo.Recipes");
            DropForeignKey("dbo.recipe_ingredients", "measureId", "dbo.Measures");
            DropIndex("dbo.recipe_steps", new[] { "recipeId" });
            DropIndex("dbo.Measures", new[] { "name" });
            DropIndex("dbo.recipe_ingredients", new[] { "measureId" });
            DropIndex("dbo.recipe_ingredients", new[] { "ingredientId" });
            DropIndex("dbo.recipe_ingredients", new[] { "recipeId" });
            DropIndex("dbo.Ingredients", new[] { "name" });
            DropTable("dbo.recipe_steps");
            DropTable("dbo.Recipes");
            DropTable("dbo.Measures");
            DropTable("dbo.recipe_ingredients");
            DropTable("dbo.Ingredients");
        }
    }
}
