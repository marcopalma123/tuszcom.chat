using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace tuszcom.migrations.Migrations
{
    [Migration(20181230115500)]
    public class _20181230_115500_TableSettings : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Settings")
                .WithColumn("IdSetting").AsInt32().PrimaryKey().Identity()
                .WithColumn("Group").AsString().NotNullable()
                .WithColumn("Key").AsString().NotNullable()
                .WithColumn("Value").AsString().NotNullable().WithDefaultValue(string.Empty)
                .WithColumn("AllowDelete").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("Description").AsString().NotNullable().WithDefaultValue(string.Empty);

            Insert.IntoTable("Settings")
                .Row(new
                {
                    Group = "Messenger",
                    Key = "NumberOfRecentMessages",
                    Value = 15,
                    AllowDelete = false,
                    Description = "Liczba ost. wiadamości wyświetlanych przy pierwszym uruchomieniu chatu z daną osobą. "
                });
        }
    }
}
