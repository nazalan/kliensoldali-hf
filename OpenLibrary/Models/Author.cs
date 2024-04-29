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
		public int BirthDate { get; set; }

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

						string name = jsonResponse["name"]?.ToString();

						//TODO
						Name = name;

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
