namespace PayMe.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abscenses",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        InstanceId = c.Guid(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.InstanceId, t.From })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Instances", t => t.InstanceId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.InstanceId);
            
            CreateTable(
                "dbo.Instances",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        JoinCode = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserToInstanceMappings",
                c => new
                    {
                        InstanceId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        JoinDateUtc = c.DateTime(nullable: false),
                        Creator = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.InstanceId, t.UserId })
                .ForeignKey("dbo.Instances", t => t.InstanceId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.InstanceId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
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
                        UserId = c.Guid(nullable: false),
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
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        DefaultUsers = c.String(),
                        InstanceId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Instances", t => t.InstanceId, cascadeDelete: true)
                .Index(t => t.InstanceId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Shop = c.String(),
                        OwnerId = c.Guid(nullable: false),
                        AffectedUsers = c.String(),
                        InstanceId = c.Guid(nullable: false),
                        Created = c.DateTime(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Instances", t => t.InstanceId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: false)
                .Index(t => t.OwnerId)
                .Index(t => t.InstanceId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.StoredEvents",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InstanceId = c.Guid(),
                        TimeUtc = c.DateTime(nullable: false),
                        EventType = c.String(),
                        UserId = c.Guid(nullable: false),
                        Ip = c.String(),
                        UserAgent = c.String(),
                        EventAsJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Expenses", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenses", "InstanceId", "dbo.Instances");
            DropForeignKey("dbo.Expenses", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Categories", "InstanceId", "dbo.Instances");
            DropForeignKey("dbo.Abscenses", "InstanceId", "dbo.Instances");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserToInstanceMappings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Abscenses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserToInstanceMappings", "InstanceId", "dbo.Instances");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Expenses", new[] { "CategoryId" });
            DropIndex("dbo.Expenses", new[] { "InstanceId" });
            DropIndex("dbo.Expenses", new[] { "OwnerId" });
            DropIndex("dbo.Categories", new[] { "InstanceId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserToInstanceMappings", new[] { "UserId" });
            DropIndex("dbo.UserToInstanceMappings", new[] { "InstanceId" });
            DropIndex("dbo.Abscenses", new[] { "InstanceId" });
            DropIndex("dbo.Abscenses", new[] { "UserId" });
            DropTable("dbo.StoredEvents");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Expenses");
            DropTable("dbo.Categories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserToInstanceMappings");
            DropTable("dbo.Instances");
            DropTable("dbo.Abscenses");
        }
    }
}
