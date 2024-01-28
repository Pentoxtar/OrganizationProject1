using Microsoft.AspNetCore.Mvc;

namespace APIDataProject.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
