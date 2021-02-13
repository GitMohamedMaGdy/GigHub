namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class populateGenersTable : DbMigration
    {
        public override void Up()
        {

            Sql("INSERT INTO GENRES (ID,NAME) VALUES (1, 'JAZZ')");
            Sql("INSERT INTO GENRES (ID,NAME) VALUES (2, 'BULUES')");
            Sql("INSERT INTO GENRES (ID,NAME) VALUES (3, 'ROCK')");
            Sql("INSERT INTO GENRES (ID,NAME) VALUES (4, 'COUNTRY')");
        }

        public override void Down()
        {
            Sql("DELETE FROM GENRES WHERE ID IN(1,2,3,4)");

        }
    }
}
