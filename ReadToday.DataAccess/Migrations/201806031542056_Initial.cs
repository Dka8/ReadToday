namespace ReadToday.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CBook",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        Author = c.String(nullable: false, maxLength: 50),
                        LiteraryGenreId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LiteraryGenre", t => t.LiteraryGenreId)
                .Index(t => t.LiteraryGenreId);
            
            CreateTable(
                "dbo.Character",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        BookId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CBook", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.LiteraryGenre",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CBook", "LiteraryGenreId", "dbo.LiteraryGenre");
            DropForeignKey("dbo.Character", "BookId", "dbo.CBook");
            DropIndex("dbo.Character", new[] { "BookId" });
            DropIndex("dbo.CBook", new[] { "LiteraryGenreId" });
            DropTable("dbo.LiteraryGenre");
            DropTable("dbo.Character");
            DropTable("dbo.CBook");
        }
    }
}
