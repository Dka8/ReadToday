namespace ReadToday.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedShelfs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shelf",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ShelfCBook",
                c => new
                    {
                        Shelf_Id = c.Guid(nullable: false),
                        CBook_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shelf_Id, t.CBook_Id })
                .ForeignKey("dbo.Shelf", t => t.Shelf_Id, cascadeDelete: true)
                .ForeignKey("dbo.CBook", t => t.CBook_Id, cascadeDelete: true)
                .Index(t => t.Shelf_Id)
                .Index(t => t.CBook_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShelfCBook", "CBook_Id", "dbo.CBook");
            DropForeignKey("dbo.ShelfCBook", "Shelf_Id", "dbo.Shelf");
            DropIndex("dbo.ShelfCBook", new[] { "CBook_Id" });
            DropIndex("dbo.ShelfCBook", new[] { "Shelf_Id" });
            DropTable("dbo.ShelfCBook");
            DropTable("dbo.Shelf");
        }
    }
}
