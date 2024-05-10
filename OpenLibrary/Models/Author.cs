namespace OpenLibrary.Models
{
	/// <summary>
	/// Model class representing an author.
	/// </summary>
	public class Author
	{
		/// <summary>
		/// Unique key of the author.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Name of the author.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Link to more information about the author.
		/// </summary>
		public string Link { get; set; }

		/// <summary>
		/// Birth date of the author.
		/// </summary>
		public int BirthDate { get; set; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Author()
		{

		}

		/// <summary>
		/// Constructor with name parameter.
		/// </summary>
		/// <param name="name">The name of the author.</param>
		public Author(string name)
		{
			Name = name;
		}
	}
}
