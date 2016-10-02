namespace COS.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnNameForMobilePhoneAndAddLengthToHomePhone : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ContactDetails", name: "MobilePhone", newName: "CellPhone");
            AlterColumn("dbo.ContactDetails", "HomePhone", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContactDetails", "HomePhone", c => c.String());
            RenameColumn(table: "dbo.ContactDetails", name: "CellPhone", newName: "MobilePhone");
        }
    }
}
