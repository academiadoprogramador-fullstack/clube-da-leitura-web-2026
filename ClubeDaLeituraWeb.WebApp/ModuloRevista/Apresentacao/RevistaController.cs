using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public class RevistaController : Controller
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;

    public RevistaController(
        IRepositorioRevista repositorioRevista,
        IRepositorioCaixa repositorioCaixa
    )
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms =
            revistas.Select(revista => new ListarRevistasViewModel(
                revista.Id,
                revista.Titulo,
                revista.NumeroEdicao,
                revista.AnoPublicacao,
                revista.Caixa.Etiqueta,
                revista.Status.ToString()
            )).ToList();

        return View(listarVms);
    }
}
