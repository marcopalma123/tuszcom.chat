GO
/****** Object:  View [dbo].[ViewMessages]    Script Date: 29.12.2018 16:52:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ViewMessages]
AS
SELECT        chatmessage.IdChatMessage, chatmessage.SenderUserId, chatmessage.CustomerUserId, chatmessage.Message, chatmessage.IsRead, chatmessage.Date, chatmessage.ConnectionId, 
                         senderUser.UserName AS senderUsername, senderUser.Email AS senderEmail, senderUser.FirstName AS senderFirstname, senderUser.SurName AS senderSurname, customerUser.UserName AS customerUsername, 
                         customerUser.Email AS customerEmail, customerUser.FirstName AS customerFirstname, customerUser.SurName AS customerSurname, customerUser.RegistrationDate AS customerRegistrationDate, 
                         senderUser.RegistrationDate AS senderRegistrationDate
FROM            dbo.ChatMessages AS chatmessage INNER JOIN
                         dbo.AspNetUsers AS senderUser ON senderUser.Id = chatmessage.SenderUserId INNER JOIN
                         dbo.AspNetUsers AS customerUser ON customerUser.Id = chatmessage.CustomerUserId
GO