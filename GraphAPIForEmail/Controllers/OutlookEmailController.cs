using GraphAPIForEmail.Helpers;
using GraphAPIForEmail.Models;
using Microsoft.Graph;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GraphAPIForEmail.Controllers
{
  public class OutlookEmailController : Controller
  {
    private GraphServiceClient graphClient;
    public OutlookEmailController()
    {
      graphClient = SDKHelper.GetAuthenticatedClient();
    }


    [Authorize]
    public async Task<ActionResult> Index()
    {
      try
      {
        var folders = await GraphEmailService.GetMailFolders(graphClient);

        EmailView emailView = new EmailView();
        emailView.EmailFolders = folders;
        //emailView.Messages = new List<Message>();
        return View(emailView);
      }
      catch (ServiceException se)
      {

        return RedirectToAction("Index", "Error", new { message = Resource.Error_Message + Request.RawUrl + ": " + se.Error.Message });
      }
    }


    [Authorize]
    [HttpPost]
    public async Task<JsonResult> GetSubFolder(string folderId)
    {
      List<SelectListItem> subFolders = new List<SelectListItem>();

      var folders = await GraphEmailService.GetMailSubFolders(graphClient, folderId);

      for (int i = 0; i < folders.Count; i++)
      {
        subFolders.Add(new SelectListItem
        {
          Value = folders[i].Id,
          Text = folders[i].DisplayName
        });
      }
      return Json(subFolders);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> GetMessages(string id, int pgSize, int skip)
    {
      int pageSize = pgSize;
      var folderId = id;

     var folderMessages = await GraphEmailService.GetAllMessagesFromFolder(graphClient, folderId, pageSize, skip);

      MessagePaging messages = new MessagePaging();
      messages.Messages = folderMessages.CurrentPage;
      messages.Top = pageSize;
      messages.Older = skip == 0 ? pageSize : skip + pageSize;
      messages.Newer = skip == 0 ? 0 : skip - pageSize;
      messages.PageSizeList = new List<int>() { 10, 25, 50, 100, 500 };
      messages.Count = Convert.ToInt32(folderMessages.AdditionalData["@odata.count"]);
      return PartialView("MessageList", messages);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> MessageDetail(string msgId)
    {
      MessageDetail message = await GraphEmailService.GetMessageDetail(graphClient, msgId);

      var rx = new Regex("(linkedin|facebook)", RegexOptions.IgnoreCase);

      //filter attachment for unwanted attachment based on Name
      message.Attachments = (from a in message.Attachments.ToList()
                     where !rx.IsMatch(a.Name)
                     select a).ToList();

      /*
      message.Attachments = (from a in message.Attachments.ToList()
                            where a.Name != "LinkedIn.png"
                            select a).ToList();
      */
     
      return PartialView("MessageDetail", message);
    }

    [Authorize]
    public async Task<FileResult> ViewAttachment(string msgId, string fileName, string id)
    {
       var attachment = await GraphEmailService.GetAttachment(graphClient, msgId, id);

      if (attachment.ODataType == "#microsoft.graph.itemAttachment")
      {
        //return File((attachment.con, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
      }
      else if (attachment.ODataType == "#microsoft.graph.fileAttachment")
      {
        return File(((FileAttachment)attachment).ContentBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
      }

      return null;
    }
  }



}