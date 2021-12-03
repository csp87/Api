using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api_Rest.Models
{
    public class CreateUser
    {
        public string email { get; set; }
        public string password { get; set; }
		public string accion { get; set; }
	}
}