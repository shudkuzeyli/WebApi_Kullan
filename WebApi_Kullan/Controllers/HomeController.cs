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
	 Api Çaðýrma adýmlarý:
   1. Yol(url) belirlenir.
   2. request nesnesi oluþturulacak.
	 3.0 response nesnesi öncesinde client ile asenkron data taransferi için port açýlýr.
   3. response nesnesi oluþturulacak.
   3.1 Eðer response varsa, geri cevap türü hazrýlanýr.(Json/Text/Xml/Model /transfer satýrlarý)
   4. Api çaðrýlýr. (Client protokolü üzeirnden api yolu çaðrýlýr.)
   5. Response nesnei doldurulur.
   6. View e gönderilir/farklý iþlem sýralamlarý varsa sýradan çaðrýlýr.
   7. api kapatma iþlemi!!! (using)
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
		var responseStream = await response.Content.ReadAsStringAsync(); //3.1 -> cursor =döngü

		if(responseStream != null)
		{
			var filmList = JsonConvert.DeserializeObject<List<Film>>(responseStream); //4
			return View(filmList); //5 -> eðer api den gelen veri varsa model gönderilir.
		}

		return View(new Film()); //6 -> eðer api den gelen veri yoksa boþ bir model gönderilir.

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
