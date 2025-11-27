using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculadoraAPI.Models.WS
{
    public class Reply
    {
        public int result { get; set; }
        public object data { get; set; }
        public string message { get; set; }
    }
}