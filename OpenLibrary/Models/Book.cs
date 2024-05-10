using System.Collections.Generic;
using System.Linq;

namespace OpenLibrary.Models
{
	/// <summary>
	/// Model class representing a book.
	/// </summary>
	public class Book
	{
		/// <summary>
		/// Unique key of the book.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Title of the book.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// List of authors of the book.
		/// </summary>
		public List<Author> Authors { get; set; }

		/// <summary>
		/// String representation of author names.
		/// </summary>
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

		/// <summary>
		/// Description of the book.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Year of the first publication of the book.
		/// </summary>
		public int FirstPublishYear { get; set; }

		/// <summary>
		/// Date of the first publication of the book.
		/// </summary>
		public string FirstPublishDate { get; set; }

		/// <summary>
		/// List of subjects related to the book.
		/// </summary>
		public List<string> Subjects { get; set; }

		/// <summary>
		/// String representation of subjects.
		/// </summary>
		public string SubjectsAsString => string.Join(", ", Subjects);

		/// <summary>
		/// ID of the book cover.
		/// </summary>
		public string CoverId { get; set; }

		/// <summary>
		/// URL of the book cover image.
		/// </summary>
		public string CoverImageUrl { get; set; }
	}
}
