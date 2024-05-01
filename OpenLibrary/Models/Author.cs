using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace OpenLibrary.Models
{
	internal class Author
	{
		public string Key { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public int BirthDate { get; set; }
		public Author()
		{

		}
		public Author(string name)
		{
			Name = name;
		}

		public async Task SearchAuthorsAsyncByKey(string uri)
		{
			Author author = new Author();
			string apiUrl = $"https://openlibrary.org{uri}.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						//Name
						Name = jsonResponse["name"]?.ToString();
						//Link
						dynamic links = jsonResponse["links"];
						if (links != null && links.Count > 0)
						{
							Link = links[0]["url"]?.ToString();
						}
						//TODO

					}
					else
					{
						throw new Exception("A kérés nem sikerült. Hibaüzenet: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Hiba történt: " + ex.Message);
				}
			}
		}
	}
}
