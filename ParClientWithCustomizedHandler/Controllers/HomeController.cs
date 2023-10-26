using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParClientWithCustomizedHandler.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        public IActionResult Index() => View();

        public IActionResult Secure() => View();

        public IActionResult Logout() => SignOut("oidc", "cookie");

        public async Task<IActionResult> CallApi()
        {
            var client = _httpClientFactory.CreateClient("client");

            var response = await client.GetStringAsync("identity");

            var doc = JsonDocument.Parse(response).RootElement;
            ViewBag.Json = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });

            return View();
        }
    }
}