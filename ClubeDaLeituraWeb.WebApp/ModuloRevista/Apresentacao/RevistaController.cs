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

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarRevistaViewModel cadastrarVm = new CadastrarRevistaViewModel(
            string.Empty,
            0,
            DateTime.Now.Year,
            string.Empty,
            SelecionarCaixas()
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarRevistaViewModel cadastrarVm)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(cadastrarVm.CaixaId);

        if (caixaSelecionada == null)
            ModelState.AddModelError(nameof(cadastrarVm.CaixaId), "Selecione uma caixa válida.");

        if (cadastrarVm.AnoPublicacao > DateTime.Now.Year)
            ModelState.AddModelError(nameof(cadastrarVm.AnoPublicacao), "O campo \"Ano de Publicação\" deve conter um ano válido.");

        if (!ModelState.IsValid)
            return View(cadastrarVm with { Caixas = SelecionarCaixas() });

        Revista novaRevista = new Revista(
            cadastrarVm.Titulo,
            cadastrarVm.NumeroEdicao,
            cadastrarVm.AnoPublicacao,
            caixaSelecionada!
        );

        repositorioRevista.Cadastrar(novaRevista);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        EditarRevistaViewModel editarVm = new EditarRevistaViewModel(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Id,
            SelecionarCaixas()
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistaViewModel editarVm)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(editarVm.Id);
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(editarVm.CaixaId);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        if (caixaSelecionada == null)
            ModelState.AddModelError(nameof(editarVm.CaixaId), "Selecione uma caixa válida.");

        if (editarVm.AnoPublicacao > DateTime.Now.Year)
            ModelState.AddModelError(nameof(editarVm.AnoPublicacao), "O campo \"Ano de Publicação\" deve conter um ano válido.");

        if (!ModelState.IsValid)
            return View(editarVm with { Caixas = SelecionarCaixas() });

        Revista revistaAtualizada = new Revista(
            editarVm.Titulo,
            editarVm.NumeroEdicao,
            editarVm.AnoPublicacao,
            caixaSelecionada!,
            revista.Status
        );

        repositorioRevista.Editar(editarVm.Id, revistaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return RedirectToAction(nameof(Listar));

        ExcluirRevistaViewModel excluirVm = new ExcluirRevistaViewModel(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Etiqueta
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirRevistaViewModel excluirVm)
    {
        repositorioRevista.Excluir(excluirVm.Id);

        return RedirectToAction(nameof(Listar));
    }

    private List<OpcaoCaixaViewModel> SelecionarCaixas()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        return caixas
            .Select(caixa => new OpcaoCaixaViewModel(caixa.Id, caixa.Etiqueta))
            .ToList();
    }
}
