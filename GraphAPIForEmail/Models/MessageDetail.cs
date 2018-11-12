using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphAPIForEmail.Models
{
  public class MessageDetail
  {
    public Message Message { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
  }
}