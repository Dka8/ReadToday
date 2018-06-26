namespace ReadToday.DataAccess.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ReadToday.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<ReadToday.DataAccess.CReadTodayDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ReadToday.DataAccess.CReadTodayDbContext context)
        {
            context.Books.AddOrUpdate(
                b => b.Title,
                new CBook("Туманность Андромеды", "\"Туманность Андромеды\" - самый известный роман основоположника современной российской фантастики, философа и ученого-палеонтолога Ивана Ефремова. В книге описан мир будущего, где живут красивые умные люди, не ведающие страха, всегда готовые к любви, дружбе, подвигу. На Земле давно стерты границы, народы живут единой семьей, пустыни превращены в оазисы, повсюду царит идеальный климат, а человек устремился к звездам, навстречу удивительным открытиям и неведомым цивилизациям.", "Иван Ефремов"),
                new CBook("Час Быка", "Социально-философский роман \"Час Быка\" - одно из главных произведений основоположника современной российской фантастики, философа и ученого-палеонтолога Ивана Ефремова. Действие романа разворачивается на далекой планете Торманс, много тысячелетий назад приютившей землян, покинувших Землю на грани катастрофы. Цивилизация Торманса замерла на стадии тоталитарного \"государственного капитализма\", безысходного и безнадежного. Волею случая там оказывается космический корабль с новой Земли. Через столкновение двух мировоззрений автор высвечивает многие сложные проблемы и противоречия развития человеческого общества. Так рождается своеобразная антиутопия, предупреждающая мир об опасностях, таящихся в стремительном прогрессе безнравственной цивилизации.", "Иван Ефремов"),
                new CBook("Вино из одуванчиков", "Войдите в светлый мир двенадцатилетнего мальчика и проживите с ним одно лето, наполненное событиями радостными и печальными, загадочными и тревожными; лето, когда каждый день совершаются удивительные открытия, главное из которых - ты живой, ты дышишь, ты чувствуешь! \"Вино из одуванчиков\" Рэя Брэдбери - классическое произведение, вошедшее в золотой фонд мировой литературы.", "Рэй Брэдбери")
            );

            context.LiteraryGenres.AddOrUpdate(
                g => g.Name,
                new CLiteraryGenre("Science fiction"),
                new CLiteraryGenre("Fantasy"),
                new CLiteraryGenre("Adventure"),
                new CLiteraryGenre("Romance")
            );

            context.SaveChanges();

            context.Characters.AddOrUpdate(
                ch => ch.Name,
                new CCharacter("Дар Ветер")
                {
                    BookId = context.Books.First().Id
                },
                new CCharacter("Низа Крит")
                {
                    BookId = context.Books.First().Id
                }
            );

            context.Shelfs.AddOrUpdate(sh => sh.Name,
                new CShelf("Ефремов")
                {
                    Books = new List<CBook>
                    {
                        context.Books.Single(b=>b.Title == "Час быка"),
                        context.Books.Single(b=>b.Title == "Туманность Андромеды")
                    }
                });
        }
    }
}
