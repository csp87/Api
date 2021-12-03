using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Api_Rest.Models
{
	public class ModifyMovies
	{
		[Required]
		public string name { get; set; }
		[Required]
		public string id { get; set; }
		public string id_user { get; set; }
		public string accion { get; set; }


	}
}