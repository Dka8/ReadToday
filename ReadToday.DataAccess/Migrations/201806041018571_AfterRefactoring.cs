namespace ReadToday.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterRefactoring : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Character", newName: "CCharacter");
            RenameTable(name: "dbo.LiteraryGenre", newName: "CLiteraryGenre");
            RenameTable(name: "dbo.Shelf", newName: "CShelf");
            RenameTable(name: "dbo.ShelfCBook", newName: "CShelfCBook");
            RenameColumn(table: "dbo.CShelfCBook", name: "Shelf_Id", newName: "CShelf_Id");
            RenameIndex(table: "dbo.CShelfCBook", name: "IX_Shelf_Id", newName: "IX_CShelf_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.CShelfCBook", name: "IX_CShelf_Id", newName: "IX_Shelf_Id");
            RenameColumn(table: "dbo.CShelfCBook", name: "CShelf_Id", newName: "Shelf_Id");
            RenameTable(name: "dbo.CShelfCBook", newName: "ShelfCBook");
            RenameTable(name: "dbo.CShelf", newName: "Shelf");
            RenameTable(name: "dbo.CLiteraryGenre", newName: "LiteraryGenre");
            RenameTable(name: "dbo.CCharacter", newName: "Character");
        }
    }
}
