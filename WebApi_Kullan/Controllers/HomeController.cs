using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi_Kullan.Models;

namespace WebApi_Kullan.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}


	/*
	 *Api url: https://localhost:7206/
	 Api �a��rma ad�mlar�:
   1. Yol(url) belirlenir.
   2. request nesnesi olu�turulacak.
	 3.0 response nesnesi �ncesinde client ile asenkron data taransferi i�in port a��l�r.
   3. response nesnesi olu�turulacak.
   3.1 E�er response varsa, geri cevap t�r� hazr�lan�r.(Json/Text/Xml/Model /transfer sat�rlar�)
   4. Api �a�r�l�r. (Client protokol� �zeirnden api yolu �a�r�l�r.)
   5. Response nesnei doldurulur.
   6. View e g�nderilir/farkl� i�lem s�ralamlar� varsa s�radan �a�r�l�r.
   7. api kapatma i�lemi!!! (using)
	 */


	public async Task<IActionResult> FilmList()
	{
		var request = new HttpRequestMessage
		{
			RequestUri = new Uri("https://localhost:7206/api/Film"),
			Method = HttpMethod.Get
		};

		var client = HttpClientFactory.Create();  //2
		var response = await client.SendAsync(request); //3
		var responseStream = await response.Content.ReadAsStringAsync(); //3.1 -> cursor =d�ng�

		if (responseStream != null)
		{
			var filmList = JsonConvert.DeserializeObject<List<Film>>(responseStream); //4
			return View(filmList); //5 -> e�er api den gelen veri varsa model g�nderilir.
		}

		return View(new Film()); //6 -> e�er api den gelen veri yoksa bo� bir model g�nderilir.

	}

	/// <summary>
	/// Modele gelen parametreyi kullanarak Film modelini db den dolduru ve api �zerinden g�nceller.
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	//public async Task<IActionResult> FilmEdit(Film model)
	//{

	//	Film film = new Film();
	//	film.Id = 3;
	//	film.FilmAdi = "Vurguncular";
	//	film.Yil = 2025;
	//	film.Yonetmen = "Ali";
	//	film.Tur = "Komedi";
	//	film.Silindi = false;

	//	var json = JsonConvert.SerializeObject(film); //1
	//	var content = new StringContent(json, Encoding.UTF8, "application/json"); //1.1

	//	//var request = new HttpRequestMessage
	//	//{
	//	//	RequestUri = new Uri("https://localhost:7206/api/Film"),
	//	//	Method = HttpMethod.Put
	//	//};

	//	/*var client = HttpClientFactory.Create();*/  //2 (HttpClientFactory.Create() -> HttpClient() ile ayn� i�lemi yapar. Factory daha seri �al���r.)
	//	var client = new HttpClient(); //2
	//	var response = await client.PutAsync("https://localhost:7206/api/Film", content); //3
	//	var responseStream = await response.Content.ReadAsStringAsync(); //3.1 -> cursor =d�ng�

	//	if (responseStream != null)
	//	{			
	//		return View(); //5 -> e�er api den gelen veri varsa model g�nderilir.
	//	}

	//	return View(new Film()); //6 -> e�er api den gelen veri yoksa bo� bir model g�nderilir.

	//	}a

	public async Task<IActionResult> FilmEditR2(Film model)
	{
		var gelenData = new Film();
		try
		{
			var gonderilecekData = new Film()
			{
				Id = 3,
				FilmAdi = "Vurguncular",
				Yil = 2025,
				Yonetmen = "Ali",
				Tur = "Komedi",
				Silindi = false
			};

			var request = new HttpRequestMessage
			{
				RequestUri = new Uri("https://localhost:7206/api/Film"),
				Method = HttpMethod.Put,
				Content = new StringContent(JsonConvert.SerializeObject(gonderilecekData), Encoding.UTF8, "application/json")
			};


			var client = HttpClientFactory.Create(); //2
			var response = await client.SendAsync(request); //3
			var responseStream = await response.Content.ReadAsStringAsync();//Cursor=d�ng�

			if (responseStream != null)
			{
				gelenData = JsonConvert.DeserializeObject<Film>(responseStream); //4
				return View(gelenData); //5
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}

		return View(gelenData); //6
	}

	public async Task<IActionResult> Detay(int? id)
	{
		if (id == null)
			return NotFound();

		var request = new HttpRequestMessage
		{
			RequestUri = new Uri("https://localhost:7206/api/Film/" + id),
			Method = HttpMethod.Get
		};

		var client = HttpClientFactory.Create();  //2
		var response = await client.SendAsync(request); //3
		var responseStream = await response.Content.ReadAsStringAsync(); //3.1 -> cursor =d�ng�

		if (responseStream != null)
		{
			var film = JsonConvert.DeserializeObject<Film>(responseStream); //4
			return View(film); //5 -> e�er api den gelen veri varsa model g�nderilir.
		}

		return View(new Film());
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}
