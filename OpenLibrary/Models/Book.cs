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
		public string Description { get; set; }
		public int? FirstPublishYear { get; set; }
		public List<string> Subjects { get; set; }
		public string SubjectsAsString => string.Join(", ", Subjects);
		public List<Edition> Editions { get; set; }
		public string CoverImageUrl { get; set; }
	}
}
