namespace FairlieAuthenticMigrations
{
  using System;
  using System.ComponentModel;
  using Mindscape.LightSpeed.Migrations;
  
  
  [Migration("20130606222405")]
  public class Customer : Migration
  {
    
    public override void Up()
    {
      this.AddTable("Role", null, new Field("Id", ModelDataType.Int32, false), new Field[] {
            new Field("RoleName", ModelDataType.String, false)});
      this.DropColumn("Customer", null, "Username", false);
      this.AddColumn("Customer", null, "Name", ModelDataType.String, false);
      this.AddColumn("Customer", null, "CreatedOn", ModelDataType.DateTime, false);
      this.AddColumn("Customer", null, "UpdatedOn", ModelDataType.DateTime, false);
      this.DropColumn("Identity", null, "EntityName", false);
      this.AddColumn("Identity", null, "IdentityId", ModelDataType.String, false);
      this.AddTable("CustomerRole", null, new Field("Id", ModelDataType.Int32, false), new Field[] {
            new ForeignKeyField("RoleId", ModelDataType.Int32, false, "Role", null, "Id"),
            new ForeignKeyField("CustomerId", ModelDataType.Int32, false, "Customer", null, "Id")});
    }
    
    public override void Down()
    {
      this.DropColumn("Customer", null, "Name", false);
      this.AddColumn("Customer", null, "Username", ModelDataType.String, false);
      this.DropColumn("Customer", null, "CreatedOn", false);
      this.DropColumn("Customer", null, "UpdatedOn", false);
      this.DropColumn("Identity", null, "IdentityId", false);
      this.AddColumn("Identity", null, "EntityName", ModelDataType.String, false);
      this.DropTable("Role", null);
    }
  }
}
