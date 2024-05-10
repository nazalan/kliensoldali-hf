namespace OpenLibrary.Models
{
	// Model class representing an author
	public class Author
	{
		// Unique key of the author
		public string Key { get; set; }
		// Name of the author
		public string Name { get; set; }
		// Link to more information about the author
		public string Link { get; set; }
		// Birth date of the author
		public int BirthDate { get; set; }
		// Default constructor
		public Author()
		{

		}
		// Constructor with name parameter
		public Author(string name)
		{
			Name = name;
		}
	}
}
