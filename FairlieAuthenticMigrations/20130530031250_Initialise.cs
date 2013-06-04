namespace FairlieAuthenticMigrations
{
  using System;
  using System.ComponentModel;
  using Mindscape.LightSpeed.Migrations;
  
  
  [Migration("20130530031250")]
  public class Initialise : Migration
  {
    
    public override void Up()
    {
      this.AddKeyTable("KeyTable", null, ModelDataType.Int32, 1);
      this.AddTable("Customer", null, new Field("Id", ModelDataType.Int32, false), new Field[] {
            new Field("Username", ModelDataType.String, false),
            new Field("Email", ModelDataType.String, false)});
      this.AddTable("Identity", null, new Field("Id", ModelDataType.Int32, false), new Field[] {
            new Field("EntityName", ModelDataType.String, false),
            new ForeignKeyField("CustomerId", ModelDataType.Int32, false, "Customer", null, "Id")});
    }
    
    public override void Down()
    {
      this.DropColumn("Identity", null, "CustomerId", true);
      this.DropTable("Identity", null);
      this.DropTable("Customer", null);
    }
  }
}
