using System.Diagnostics;
using System.Net.Http;
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

		var client =  HttpClientFactory.Create();	//2
		var response = await client.SendAsync(request); //3
		var responseStream = await response.Content.ReadAsStringAsync(); //3.1 -> cursor =d�ng�

		if(responseStream != null)
		{
			var filmList = JsonConvert.DeserializeObject<List<Film>>(responseStream); //4
			return View(filmList); //5 -> e�er api den gelen veri varsa model g�nderilir.
		}

		return View(new Film()); //6 -> e�er api den gelen veri yoksa bo� bir model g�nderilir.

	}

	public IActionResult Index()
	{
		return View();
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
