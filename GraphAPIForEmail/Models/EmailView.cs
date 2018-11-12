using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPIForEmail.Models
{
  public class EmailView
  {
    public string User { get; set; }
    public IList<MailFolder> EmailFolders { get; set; }
    //public int Count { get; set; }
    public int SelectedFolderId { get; set; }
    public int SelectedSubFolderId { get; set; }
    public IList<Message> Messages { get; set; }
  }


  public class MessagePaging
  {
    public IList<Message> Messages { get; set; }
    public int Count { get; set; }
    public int Top { get; set; }
    public int Older { get; set; }
    public int Newer { get; set; }
    public IList<int> PageSizeList { get; set; }
  }
}