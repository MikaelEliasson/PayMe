namespace PayMe.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transfers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromUserId = c.Guid(nullable: false),
                        ToUserId = c.Guid(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                        InstanceId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUserId, cascadeDelete: false)
                .ForeignKey("dbo.Instances", t => t.InstanceId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUserId, cascadeDelete: false)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId)
                .Index(t => t.InstanceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "ToUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transfers", "InstanceId", "dbo.Instances");
            DropForeignKey("dbo.Transfers", "FromUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Transfers", new[] { "InstanceId" });
            DropIndex("dbo.Transfers", new[] { "ToUserId" });
            DropIndex("dbo.Transfers", new[] { "FromUserId" });
            DropTable("dbo.Transfers");
        }
    }
}
