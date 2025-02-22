namespace WebApi_Kullan.Models
{
	public class Film
	{
    //"id": 0,
    //"silindi": true,
    //"filmAdi": "string",
    //"yonetmen": "string",
    //"tur": "string",
    //"yil": 0

    public int Id { get; set; }
		public string FilmAdi { get; set; }
		public string Yonetmen { get; set; }
		public string Tur { get; set; }
		public int Yil { get; set; }
		public bool Silindi { get; set; }

	}
}
