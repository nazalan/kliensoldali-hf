using Newtonsoft.Json.Linq;
using OpenLibrary.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenLibrary.Services
{
	// Service class responsible for interacting with the Open Library API
	class LibraryService
	{
		public LibraryService() { }

		// Method to search for a book by its unique key asynchronously
		public async Task<Book> SearchWorkAsyncByKey(string uri)
		{
			Book results;
			// Construct the API URL using the provided URI
			string apiUrl = $"https://openlibrary.org{uri}.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Check if the HTTP request was successful
					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						// Extract book details from the JSON response
						string key = jsonResponse["key"]?.ToString();
						string title = jsonResponse["title"]?.ToString();
						string coverId = jsonResponse["covers"]?[0].ToString();
						string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg" : null;
						JArray authorsArray = (JArray)jsonResponse["authors"];
						List<string> authorKeys = new List<string>();

						// Extract author keys from the JSON response
						foreach (JObject authorObject in authorsArray)
						{
							string authorkey = (string)authorObject["author"]["key"];
							authorKeys.Add(authorkey);
						}

						// Retrieve author details asynchronously
						List<Author> authorNames = new List<Author>();
						foreach (string authorkey in authorKeys)
						{
							Author author = new Author();
							author = await SearchAuthorsAsyncByKey(authorkey);
							authorNames.Add(author);
						}

						string description = "";
						// Extract description from the JSON response
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

						string firstPublishDate = null;
						// Extract first publish date from the JSON response
						if (jsonResponse["first_publish_date"] != null)
						{
							firstPublishDate = jsonResponse["first_publish_date"];
						}

						List<string> subjects = new List<string>();
						// Extract subjects from the JSON response
						if (jsonResponse["subjects"] != null)
						{
							foreach (var subject in jsonResponse["subjects"])
							{
								subjects.Add(subject?.ToString());
							}
						}

						// Create a new Book object with the extracted details
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
						throw new Exception("The request failed. Status code: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("An error occurred: " + ex.Message);
				}
			}

			return results;
		}

		// Method to search for books based on a search query asynchronously
		public async Task<List<Book>> SearchBooksAsync(string searchQuery)
		{
			List<Book> results = new List<Book>();

			// Construct the API URL for book search
			string apiUrl = $"https://openlibrary.org/search.json?limit=40&q={searchQuery}&fields=key,title,cover_i,author_name,editions,editions.key,editions.title,editions.ebook_access,editions.language,first_publish_year&.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Check if the HTTP request was successful
					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						// Extract book details from the JSON response
						foreach (var doc in jsonResponse["docs"])
						{
							string key = doc["key"].ToString().Trim().Replace(" ", "");
							string title = doc["title"]?.ToString();

							// Extract language from the API URL
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
							// Extract author details from the JSON response
							if (doc["author_name"] != null)
							{
								foreach (var authorName in doc["author_name"])
								{
									authors.Add(new Author(authorName?.ToString()));
								}
							}

							int firstPublishYear = 0;
							// Extract first publish year from the JSON response
							if (doc["first_publish_year"] != null)
							{
								firstPublishYear = (int)doc["first_publish_year"];
							}
							string coverId = doc["cover_i"]?.ToString();
							string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-M.jpg" : null;

							// Create a new Book object with the extracted details
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
						throw new Exception("The request failed. Status code: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("An error occurred: " + ex.Message);
				}
			}

			return results;
		}

		// Method to search for authors asynchronously by key
		public async Task<Author> SearchAuthorsAsyncByKey(string uri)
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

						// Retrieve author name from response
						author.Name = jsonResponse["name"]?.ToString();
						// Retrieve author link from response
						dynamic links = jsonResponse["links"];
						if (links != null && links.Count > 0)
						{
							author.Link = links[0]["url"]?.ToString();
						}

					}
					else
					{
						throw new Exception("The request failed. Status code: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("An error occurred: " + ex.Message);
				}
			}
			return author;
		}
	}
}
