using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Text;

namespace tuszcom.migrations.Migrations
{
    [Migration(20181227221600)]
    public class _20181227_221500_InitDatabase : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("ChatMessages")
                .WithColumn("IdChatMessage").AsInt32().PrimaryKey()
                .WithColumn("SenderUserId").AsString(450).ForeignKey("FK_ChatMessage_AspNetUsers_SenderUserId", "AspNetUsers", "Id").NotNullable()
                .WithColumn("CustomerUserId").AsString(450).ForeignKey("FK_ChatMessage_AspNetUsers_CustomerUserId", "AspNetUsers", "Id").NotNullable()
                .WithColumn("Message").AsString(int.MaxValue).NotNullable().WithDefaultValue(string.Empty)
                .WithColumn("IsRead").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("Date").AsDateTime().NotNullable()
                .WithColumn("ConnectionId").AsString(200).NotNullable();

            Create.Table("ChatMessageFiles")
                .WithColumn("IdChatMessageFile").AsInt32().PrimaryKey()
                .WithColumn("Path").AsString(int.MaxValue).NotNullable()
                .WithColumn("Name").AsString(150).NotNullable()
                .WithColumn("ChatMessageId").AsInt32().NotNullable().ForeignKey("FK_ChatMessageFile_ChatMessages_Id", "ChatMessages", "IdChatMessage");

            Create.Table("ChatUserDetails")
                .WithColumn("IdChatUserDetail").AsInt32().PrimaryKey()
                .WithColumn("ConnectionId").AsString(200).NotNullable()
                .WithColumn("UserId").AsString(450).ForeignKey("FK_ChatUserDetails_AspNetUsers_UserId", "AspNetUsers", "Id").NotNullable();
        }
    }
}
