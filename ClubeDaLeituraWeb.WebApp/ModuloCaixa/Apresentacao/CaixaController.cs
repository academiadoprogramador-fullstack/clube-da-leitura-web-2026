using ClubeDaLeituraWeb.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;

public class CaixaController : Controller
{
    private readonly ServicoCaixa servicoCaixa;

    public CaixaController(ServicoCaixa servicoCaixa)
    {
        this.servicoCaixa = servicoCaixa;
    }

    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCaixasDto> dtos = servicoCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = dtos
            .Select(c => new ListarCaixasViewModel(c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo))
            .ToList();

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
        if (!ModelState.IsValid)
            return View(cadastrarVm);

        CadastrarCaixaDto dto = new CadastrarCaixaDto(
            cadastrarVm.Etiqueta,
            cadastrarVm.Cor,
            cadastrarVm.DiasDeEmprestimo
        );

        Result resultado = servicoCaixa.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesCaixaDto> resultado = servicoCaixa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        DetalhesCaixaDto dto = resultado.Value;

        EditarCaixaViewModel editarVm = new EditarCaixaViewModel(
            id,
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        Result resultado = servicoCaixa.Editar(new EditarCaixaDto(
            editarVm.Id,
            editarVm.Etiqueta,
            editarVm.Cor,
            editarVm.DiasDeEmprestimo
        ));

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(editarVm);
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Result<DetalhesCaixaDto> resultado = servicoCaixa.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData["MensagemErro"] = resultado.Errors.First().Message;

            return RedirectToAction(nameof(Listar));
        }

        DetalhesCaixaDto dto = resultado.Value;

        ExcluirCaixaViewModel excluirVm = new ExcluirCaixaViewModel(
            id,
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Result resultado = servicoCaixa.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData["MensagemErro"] = resultado.Errors.First().Message;

        return RedirectToAction(nameof(Listar));
    }
}
