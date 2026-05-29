using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;

public class CaixaController : Controller
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioRevista repositorioRevista;

    public CaixaController(
        IRepositorioCaixa repositorioCaixa,
        IRepositorioRevista repositorioRevista
    )
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = caixas.Select(c => new ListarCaixasViewModel(
            c.Id,
            c.Etiqueta,
            c.Cor,
            c.DiasDeEmprestimo
        )).ToList();

        return View(listarVms);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        CadastrarCaixaViewModel cadastrarVm = new CadastrarCaixaViewModel(
            string.Empty,
            string.Empty,
            7
        );

        return View(cadastrarVm);
    }

    [HttpPost]
    public ActionResult Cadastrar(CadastrarCaixaViewModel cadastrarVm)
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (string.Equals(c.Etiqueta, cadastrarVm.Etiqueta, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    nameof(cadastrarVm.Etiqueta),
                    "Já existe uma caixa com esta etiqueta."
                );
            }
        }

        if (!ModelState.IsValid)
            return View(cadastrarVm);

        Caixa novaCaixa = new Caixa(
            cadastrarVm.Etiqueta,
            cadastrarVm.Cor,
            cadastrarVm.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        EditarCaixaViewModel editarVm = new EditarCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (c.Id != editarVm.Id && string.Equals(c.Etiqueta, editarVm.Etiqueta, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(
                    nameof(editarVm.Etiqueta),
                    "Já existe uma caixa com esta etiqueta."
                );
            }
        }

        if (!ModelState.IsValid)
            return View(editarVm);

        Caixa caixaAtualizada = new Caixa(
            editarVm.Etiqueta,
            editarVm.Cor,
            editarVm.DiasDeEmprestimo
        );

        repositorioCaixa.Editar(editarVm.Id, caixaAtualizada);

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return RedirectToAction(nameof(Listar));

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            caixa.Etiqueta,
            caixa.Cor,
            caixa.DiasDeEmprestimo
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        foreach (Revista r in revistas)
        {
            if (string.Equals(r.Caixa.Id, excluirVm.Id))
            {
                TempData["MensagemErro"] = "Esta caixa não pode ser excluída pois está relacionada a uma revista.";

                return RedirectToAction(nameof(Listar));
            }
        }

        repositorioCaixa.Excluir(excluirVm.Id);

        return RedirectToAction(nameof(Listar));
    }
}
