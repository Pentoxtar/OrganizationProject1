using Microsoft.AspNetCore.Mvc;
using SqliteProvider.Models;
using SqliteProvider.Repositories;

namespace YourNamespace.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationController : ControllerBase
	{
		private readonly CsvService _csvService;

		public OrganizationController(CsvService csvService)
		{
			_csvService = csvService;
		}

		[HttpGet("import")]
		public IActionResult ImportCsv()
		{
			List<Organization> records = _csvService.ReadCsv();
			_csvService.SaveToDatabase(records);

			return Ok("CSV data imported successfully.");
		}

	}
}
