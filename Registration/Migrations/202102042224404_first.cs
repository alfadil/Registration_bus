namespace Registration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FromCityId = c.Int(nullable: false),
                        TOCityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Cities", t => t.FromCityId)
                .ForeignKey("dbo.Cities", t => t.TOCityId)
                .Index(t => t.FromCityId)
                .Index(t => t.TOCityId);
            
            CreateTable(
                "dbo.Reserves",
                c => new
                    {
                        ReserveID = c.Int(nullable: false, identity: true),
                        TripID = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        CardNumber = c.String(),
                        Name = c.String(),
                        TaxiPaid = c.Boolean(nullable: false),
                        CustomerID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ReserveID)
                .ForeignKey("dbo.AspNetUsers", t => t.CustomerID)
                .ForeignKey("dbo.Trips", t => t.TripID, cascadeDelete: true)
                .Index(t => t.TripID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Address = c.String(),
                        Name = c.String(),
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
                "dbo.Trips",
                c => new
                    {
                        TripID = c.Int(nullable: false, identity: true),
                        RouteID = c.Int(nullable: false),
                        num_seats = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        DriverID = c.String(maxLength: 128),
                        Price = c.Single(nullable: false),
                        LuggagePrice = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.TripID)
                .ForeignKey("dbo.AspNetUsers", t => t.DriverID)
                .ForeignKey("dbo.Routes", t => t.RouteID, cascadeDelete: true)
                .Index(t => t.RouteID)
                .Index(t => t.DriverID);
            
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
            DropForeignKey("dbo.Trips", "RouteID", "dbo.Routes");
            DropForeignKey("dbo.Reserves", "TripID", "dbo.Trips");
            DropForeignKey("dbo.Trips", "DriverID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reserves", "CustomerID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Routes", "TOCityId", "dbo.Cities");
            DropForeignKey("dbo.Routes", "FromCityId", "dbo.Cities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Trips", new[] { "DriverID" });
            DropIndex("dbo.Trips", new[] { "RouteID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Reserves", new[] { "CustomerID" });
            DropIndex("dbo.Reserves", new[] { "TripID" });
            DropIndex("dbo.Routes", new[] { "TOCityId" });
            DropIndex("dbo.Routes", new[] { "FromCityId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Trips");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Reserves");
            DropTable("dbo.Routes");
            DropTable("dbo.Cities");
        }
    }
}
