using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.Compartilhado.Apresentacao;

public class HomeController : Controller
{
    [HttpGet]
    public ActionResult Index()
    {
        return View();
    }
}
