namespace Project_Sauron.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Enemies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Link = c.String(nullable: false, maxLength: 20),
                        Name = c.String(maxLength: 50),
                        Online = c.Boolean(nullable: false),
                        LastActivity = c.Long(nullable: false),
                        Platform = c.Byte(nullable: false),
                        Status = c.String(maxLength: 145),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false, maxLength: 20),
                        Password = c.Guid(nullable: false),
                        Nickname = c.String(maxLength: 20),
                        Photo = c.String(maxLength: 50),
                        RoleId = c.Int(nullable: false),
                        SiteTheme_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SiteThemes", t => t.SiteTheme_Id)
                .Index(t => t.RoleId)
                .Index(t => t.SiteTheme_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SiteThemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThemeName = c.String(nullable: false, maxLength: 20),
                        CssPath = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EnemyEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EnemyId = c.Int(nullable: false),
                        EventTypeId = c.Int(nullable: false),
                        Time = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enemies", t => t.EnemyId, cascadeDelete: true)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId, cascadeDelete: true)
                .Index(t => t.EnemyId)
                .Index(t => t.EventTypeId);
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(maxLength: 50),
                        TargetSite = c.String(maxLength: 30),
                        Time = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EnemyEvents", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.EnemyEvents", "EnemyId", "dbo.Enemies");
            DropForeignKey("dbo.Enemies", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Users", "SiteTheme_Id", "dbo.SiteThemes");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.EnemyEvents", new[] { "EventTypeId" });
            DropIndex("dbo.EnemyEvents", new[] { "EnemyId" });
            DropIndex("dbo.Users", new[] { "SiteTheme_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Enemies", new[] { "AuthorId" });
            DropTable("dbo.Errors");
            DropTable("dbo.EventTypes");
            DropTable("dbo.EnemyEvents");
            DropTable("dbo.SiteThemes");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Enemies");
        }
    }
}
