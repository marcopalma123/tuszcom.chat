using FluentMigrator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace tuszcom.migrations.Migrations
{
    [Migration(20181229165300)]
    public class _20181229_165300_ViewMessages : ForwardOnlyMigration
    {
        public override void Up()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Migrations\Scripts\Views\ViewMessages.sql");
            Execute.Script(path);
        }
    }
}
