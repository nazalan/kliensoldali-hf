using OpenLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace OpenLibrary.Services
{
    class LibraryService
    {
		public LibraryService() { }

		public async Task<Book> SearchWorkAsyncByKey(string uri)
		{
			Book results;
			string apiUrl = $"https://openlibrary.org{uri}.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						//Key
						string key = jsonResponse["key"]?.ToString();
						//Title
						string title = jsonResponse["title"]?.ToString();
						//Cover
						string coverId = jsonResponse["covers"]?[0].ToString();
						string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg" : null;
						//Authors
						JArray authorsArray = (JArray)jsonResponse["authors"];
						List<string> authorKeys = new List<string>();
						foreach (JObject authorObject in authorsArray)
						{
							string authorkey = (string)authorObject["author"]["key"];
							authorKeys.Add(authorkey);
						}
						List<Author> authorNames = new List<Author>();
						foreach (string authorkey in authorKeys)
						{
							Author author = new Author();
							await author.SearchAuthorsAsyncByKey(authorkey);
							authorNames.Add(author);
						}
						//Description
						string description = "";
						if (jsonResponse["description"] != null)
						{
							JToken descriptionToken = jsonResponse["description"];
							if (descriptionToken.Type == JTokenType.Object)
							{
								description = descriptionToken["value"]?.ToString();
							}
							else if (descriptionToken.Type == JTokenType.String)
							{
								description = descriptionToken.ToString();
							}
						}
						//PublishDate
						string firstPublishDate = null;
						if (jsonResponse["first_publish_date"] != null)
						{
							firstPublishDate = jsonResponse["first_publish_date"];
						}
						//Subjects
						List<string> subjects = new List<string>();
						if (jsonResponse["subjects"] != null)
						{
							foreach (var subject in jsonResponse["subjects"])
							{
								subjects.Add(subject?.ToString());
							}
						}
						//new Book
						results = (new Book
						{
							Key = key,
							Title = title,
							CoverId = coverId,
							CoverImageUrl = coverImageUrl,
							Authors = authorNames,
							Description = description,
							FirstPublishDate = firstPublishDate,
							Subjects = subjects,
						});

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

			return results;
		}

		public async Task<List<Book>> SearchBooksAsync(string searchQuery)
		{
			List<Book> results = new List<Book>();

			string apiUrl = $"https://openlibrary.org/search.json?limit=20&q={searchQuery}&fields=key,title,cover_i,author_name,editions,editions.key,editions.title,editions.ebook_access,editions.language,first_publish_year&.json";
			System.Diagnostics.Debug.WriteLine(apiUrl);
			//string apiUrl = $"https://openlibrary.org/search.json?q=";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						foreach (var doc in jsonResponse["docs"])
						{
							string key = doc["key"].ToString().Trim().Replace(" ", "");
							string title = doc["title"]?.ToString();

							int langIndex = apiUrl.IndexOf("lang=");
							string language = "";
							if (langIndex != -1 && langIndex + 5 < apiUrl.Length)
							{
								language = apiUrl.Substring(langIndex + 5, 2);
							}
							string foreignTitle = null;
							if (doc["editions"] != null)
							{
								foreach (var edition in doc["editions"]["docs"])
								{
									if (edition["language"] != null)
									{
										foreach (var lang in edition["language"])
										{
											if (lang.ToString().Contains(language))
											{
												foreignTitle = edition["title"]?.ToString();
												break;
											}
										}
									}
									if (!string.IsNullOrEmpty(foreignTitle))
									{
										break;
									}
								}
							}
							string finalTitle = !string.IsNullOrEmpty(foreignTitle) ? foreignTitle : title;

							List<Author> authors = new List<Author>();
							if (doc["author_name"] != null)
							{
								foreach (var authorName in doc["author_name"])
								{
									authors.Add(new Author(authorName?.ToString()));
								}
							}

							int firstPublishYear = 0;
							if (doc["first_publish_year"] != null)
							{
								firstPublishYear = (int)doc["first_publish_year"];
							}
							string coverId = doc["cover_i"]?.ToString();
							string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg" : null;

							results.Add(new Book
							{
								Key = key,
								Title = finalTitle,
								Authors = authors,
								FirstPublishYear = firstPublishYear,
								CoverImageUrl = coverImageUrl
							});
						}

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

			return results;
		}
	}
}
