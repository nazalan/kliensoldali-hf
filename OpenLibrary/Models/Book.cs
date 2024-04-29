using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibrary.Models
{
	internal class Book
	{
		public string Key { get; set; }
		public string ISBN { get; set; }
		public string Title { get; set; }
		public List<string> AuthorNames { get; set; } = new List<string>();
		public string AuthorNamesAsString => string.Join(", ", AuthorNames);
		public int? FirstPublishYear { get; set; }
		public List<string> Subject { get; set; }
		public List<Edition> Editions { get; set; }
		public string CoverImageUrl { get; set; }
	}
}
