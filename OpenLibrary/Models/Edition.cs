using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibrary.Models
{
	internal class Edition
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public int Number { get; set; }
		public string Language { get; set; }
		public string EbookAccess { get; set; }
	}
}
