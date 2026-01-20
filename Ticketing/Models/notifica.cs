using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Ticketing.Models
{
    public class notifica
    {
        public int Ticket {  get; set; }
        public string nomeMittente { get; set; }
        public string messaggioNotifica { get; set; }
        public bool letturaNotifica { get; set; }
    }
}