using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibrary.Models
{
	public class Book
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public List<Author> Authors { get; set; }
		public string AuthorNamesAsString
		{
			get
			{
				if (Authors == null || Authors.Count == 0)
					return "";

				var authorNames = Authors.Select(author => author.Name);
				return string.Join(", ", authorNames);
			}
		}
		public string Description { get; set; }
		public int FirstPublishYear { get; set; }
		public string FirstPublishDate { get; set; }
		public List<string> Subjects { get; set; }
		public string SubjectsAsString => string.Join(", ", Subjects);
		public string CoverId {  get; set; }
		public string CoverImageUrl { get; set; }
	}
}
