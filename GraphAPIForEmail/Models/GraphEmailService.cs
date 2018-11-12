using Microsoft.Graph;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GraphAPIForEmail.Models
{
  public static class GraphEmailService
  {
    public static async Task<IList<MailFolder>> GetMailFolders(GraphServiceClient graphClient)
    {
      var options = new[]
      {
        new QueryOption("$top","50"),
        new QueryOption("$expand","childFolders"),
      };

      var emailFolders = await graphClient
                                        .Me
                                        .MailFolders
                                        .Request(options)
                                        .GetAsync();

      //List<SelectListItem> folders = new List<SelectListItem>();
      //foreach(var mail in emailFolders)
      //{
      //  folders.Add(new SelectListItem() { Value = mail.Id, Text = mail.DisplayName });
      //  foreach(var subFolder in mail.ChildFolders)
      //  {
      //    folders.Add(new SelectListItem() { Value = subFolder.Id, Text = subFolder.DisplayName });
      //  }
      //}

      return emailFolders.CurrentPage;
    }

    public static async Task<IList<MailFolder>> GetMailSubFolders(GraphServiceClient graphClient, string folderID)
    {
      var options = new[]
      {
        new QueryOption("$top","50")        
      };

      var emailFolders = await graphClient
                                        .Me
                                        .MailFolders[folderID]
                                        .ChildFolders
                                        .Request(options)
                                        .GetAsync();


      return emailFolders.CurrentPage;
    }

    public static async Task<IMailFolderMessagesCollectionPage> GetAllMessagesFromFolder(GraphServiceClient graphClient, string folderID, int pageSize, int skip)
    {
      if (pageSize == 0) pageSize = 10;

      var options = new[] { new QueryOption("$top", pageSize.ToString()),
                            new QueryOption("$skip", skip.ToString()),
                            new QueryOption("$count","true")
                          };

      var messages = await graphClient
                                      .Me
                                      .MailFolders[folderID]
                                      .Messages
                                      .Request(options)
                                      .GetAsync();

      return messages;
    }

    public static async Task<MessageDetail> GetMessageDetail(GraphServiceClient graphClient, string msgId)
    {
      var message = await graphClient
                                      .Me
                                      .Messages[msgId]
                                      .Request()
                                      .GetAsync();

      var attachments = await graphClient.Me.Messages[msgId].Attachments.Request().GetAsync();


      string tempString = message.Body.Content;

      //Process inline images
      foreach (var attach in attachments)
      {
        if (attach.ODataType == "#microsoft.graph.fileAttachment" && ((FileAttachment)attach).IsInline == true)
        {
          //cid:image001.png@01D471D1.E0982D40
          tempString = tempString.Replace("cid:" + ((FileAttachment)attach).ContentId, "data:image/png;base64," + System.Convert.ToBase64String(((FileAttachment)attach).ContentBytes));
        }
      }


      message.Body.Content = tempString;

      return new MessageDetail()
      {
        Message = message,
        Attachments = attachments.CurrentPage
      };
    }

    public static async Task<Attachment> GetAttachment(GraphServiceClient graphClient, string msgId, string id)
    {
      var attachment = await graphClient.Me.Messages[msgId].Attachments[id].Request().GetAsync();

      return attachment;
    }
  }





}