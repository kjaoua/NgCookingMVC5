namespace NgCookingMVC_BackEND.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        NameToDisplay = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        UserId = c.String(),
                        Comment = c.String(),
                        Mark = c.Int(nullable: false),
                        Recettes_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recettes", t => t.Recettes_Id)
                .Index(t => t.Recettes_Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsAvailable = c.Boolean(nullable: false),
                        Picture = c.Binary(nullable: false),
                        Calories = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        RecettesViewModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.RecettesViewModel", t => t.RecettesViewModel_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.RecettesViewModel_Id);
            
            CreateTable(
                "dbo.RecettesIngredients",
                c => new
                    {
                        IngredientId = c.Int(nullable: false),
                        RecetteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IngredientId, t.RecetteId })
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Recettes", t => t.RecetteId, cascadeDelete: true)
                .Index(t => t.IngredientId)
                .Index(t => t.RecetteId);
            
            CreateTable(
                "dbo.Recettes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        NameToDisplay = c.String(),
                        CreatorId = c.String(maxLength: 128),
                        IsAvailable = c.Boolean(nullable: false),
                        Picture = c.Binary(nullable: false),
                        Preparation = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserSurName = c.String(nullable: false),
                        Picture = c.Binary(nullable: false),
                        Level = c.Int(nullable: false),
                        City = c.String(nullable: false),
                        Birth = c.DateTime(nullable: false),
                        Bio = c.String(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.IngredientViewModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        IsAvailable = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Calories = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecettesViewModel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        NameToDisplay = c.String(nullable: false, maxLength: 255),
                        IsAvailable = c.Boolean(nullable: false),
                        Preparation = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ingredients", "RecettesViewModel_Id", "dbo.RecettesViewModel");
            DropForeignKey("dbo.RecettesIngredients", "RecetteId", "dbo.Recettes");
            DropForeignKey("dbo.Recettes", "CreatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Recettes_Id", "dbo.Recettes");
            DropForeignKey("dbo.RecettesIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.Ingredients", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Recettes", new[] { "CreatorId" });
            DropIndex("dbo.RecettesIngredients", new[] { "RecetteId" });
            DropIndex("dbo.RecettesIngredients", new[] { "IngredientId" });
            DropIndex("dbo.Ingredients", new[] { "RecettesViewModel_Id" });
            DropIndex("dbo.Ingredients", new[] { "CategoryId" });
            DropIndex("dbo.Comments", new[] { "Recettes_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RecettesViewModel");
            DropTable("dbo.IngredientViewModel");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Recettes");
            DropTable("dbo.RecettesIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Comments");
            DropTable("dbo.Categories");
        }
    }
}
