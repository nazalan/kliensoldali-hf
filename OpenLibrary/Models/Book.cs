using System.Collections.Generic;
using System.Linq;

namespace OpenLibrary.Models
{
	// Model class representing a book
	public class Book
	{
		// Unique key of the book
		public string Key { get; set; }
		// Title of the book
		public string Title { get; set; }
		// List of authors of the book
		public List<Author> Authors { get; set; }
		// String representation of author names
		public string AuthorNamesAsString
		{
			get
			{
				// If no authors or authors count is zero, return an empty string
				if (Authors == null || Authors.Count == 0)
					return "";

				// Extract author names and join them with a comma
				var authorNames = Authors.Select(author => author.Name);
				return string.Join(", ", authorNames);
			}
		}
		// Description of the book
		public string Description { get; set; }
		// Year of the first publication of the book
		public int FirstPublishYear { get; set; }
		// Date of the first publication of the book
		public string FirstPublishDate { get; set; }
		// List of subjects related to the book
		public List<string> Subjects { get; set; }
		// String representation of subjects
		public string SubjectsAsString => string.Join(", ", Subjects);
		// ID of the book cover
		public string CoverId { get; set; }
		// URL of the book cover image
		public string CoverImageUrl { get; set; }
	}
}
