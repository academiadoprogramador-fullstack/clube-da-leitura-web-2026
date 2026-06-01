using ClubeDaLeituraWeb.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;
using FluentResults;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;

public class CaixaController(ServicoCaixa servicoCaixa, IMapper mapeador) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarCaixasDto> dtos = servicoCaixa.SelecionarTodos();

        List<ListarCaixasViewModel> listarVms = mapeador.Map<List<ListarCaixasViewModel>>(dtos);

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

        CadastrarCaixaDto dto = mapeador.Map<CadastrarCaixaDto>(cadastrarVm);

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
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        DetalhesCaixaDto dto = resultado.Value;

        EditarCaixaViewModel editarVm = mapeador.Map<EditarCaixaViewModel>(dto);

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarCaixaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm);

        EditarCaixaDto dto = mapeador.Map<EditarCaixaDto>(editarVm);

        Result resultado = servicoCaixa.Editar(dto);

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
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        DetalhesCaixaDto dto = resultado.Value;

        ExcluirCaixaViewModel excluirVm = mapeador.Map<ExcluirCaixaViewModel>(dto);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirCaixaViewModel excluirVm)
    {
        Result resultado = servicoCaixa.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }
}
