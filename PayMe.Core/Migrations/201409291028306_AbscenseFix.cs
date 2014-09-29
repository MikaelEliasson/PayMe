namespace PayMe.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbscenseFix : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Abscenses");
            AddColumn("dbo.Abscenses", "Id", c => c.Guid(nullable: false));
            AddColumn("dbo.Abscenses", "Description", c => c.String());
            AddPrimaryKey("dbo.Abscenses", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Abscenses");
            DropColumn("dbo.Abscenses", "Description");
            DropColumn("dbo.Abscenses", "Id");
            AddPrimaryKey("dbo.Abscenses", new[] { "UserId", "InstanceId", "From" });
        }
    }
}
