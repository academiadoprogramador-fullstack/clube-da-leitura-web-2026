using AutoMapper;
using ClubeDaLeituraWeb.WebApp.Compartilhado.Apresentacao.Extensions;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public class RevistaController(
    ServicoRevista servicoRevista,
    ServicoCaixa servicoCaixa,
    IMapper mapeador
) : Controller
{
    [HttpGet]
    public ActionResult Listar()
    {
        List<ListarRevistasDto> dtos = servicoRevista.SelecionarTodos();

        List<ListarRevistasViewModel> listarVms = mapeador.Map<List<ListarRevistasViewModel>>(dtos);

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
        if (!ModelState.IsValid)
            return View(cadastrarVm with { Caixas = SelecionarCaixas() });

        CadastrarRevistaDto dto = mapeador.Map<CadastrarRevistaDto>(cadastrarVm);

        Result resultado = servicoRevista.Cadastrar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(cadastrarVm with { Caixas = SelecionarCaixas() });
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Editar(string id)
    {
        Result<DetalhesRevistaDto> resultado = servicoRevista.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        EditarRevistaViewModel editarVm =
            mapeador.Map<EditarRevistaViewModel>(resultado.Value) with { Caixas = SelecionarCaixas() };

        return View(editarVm);
    }

    [HttpPost]
    public ActionResult Editar(EditarRevistaViewModel editarVm)
    {
        if (!ModelState.IsValid)
            return View(editarVm with { Caixas = SelecionarCaixas() });

        EditarRevistaDto dto = mapeador.Map<EditarRevistaDto>(editarVm);

        Result resultado = servicoRevista.Editar(dto);

        if (resultado.IsFailed)
        {
            ModelState.AddModelError(resultado);

            return View(editarVm with { Caixas = SelecionarCaixas() });
        }

        return RedirectToAction(nameof(Listar));
    }

    [HttpGet]
    public ActionResult Excluir(string id)
    {
        Result<DetalhesRevistaDto> resultado = servicoRevista.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            TempData.AddErrorMessage(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ExcluirRevistaViewModel excluirVm =
            mapeador.Map<ExcluirRevistaViewModel>(resultado.Value);

        return View(excluirVm);
    }

    [HttpPost]
    public ActionResult Excluir(ExcluirRevistaViewModel excluirVm)
    {
        Result resultado = servicoRevista.Excluir(excluirVm.Id);

        if (resultado.IsFailed)
            TempData.AddErrorMessage(resultado);

        return RedirectToAction(nameof(Listar));
    }

    private List<OpcaoCaixaViewModel> SelecionarCaixas()
    {
        List<ListarCaixasDto> dtos = servicoCaixa.SelecionarTodos();

        return mapeador.Map<List<OpcaoCaixaViewModel>>(dtos);
    }
}
