using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;

public class ServicoCaixa
{
    private readonly IRepositorioCaixa repositorioCaixa;
    private readonly IRepositorioRevista repositorioRevista;

    public ServicoCaixa(
        IRepositorioCaixa repositorioCaixa,
        IRepositorioRevista repositorioRevista
    )
    {
        this.repositorioCaixa = repositorioCaixa;
        this.repositorioRevista = repositorioRevista;
    }

    public Result Cadastrar(CadastrarCaixaDto dto)
    {
        if (ExisteCaixaComEtiqueta(dto.Etiqueta))
            return Falha("Etiqueta", "Já existe uma caixa com esta etiqueta.");

        Caixa novaCaixa = new Caixa(
            dto.Etiqueta,
            dto.Cor,
            dto.DiasDeEmprestimo
        );

        repositorioCaixa.Cadastrar(novaCaixa);

        return Result.Ok();
    }

    public Result Editar(EditarCaixaDto dto)
    {
        if (ExisteCaixaComEtiqueta(dto.Etiqueta, dto.Id))
            return Falha("Etiqueta", "Já existe uma caixa com esta etiqueta.");

        Caixa caixaAtualizada = new Caixa(dto.Etiqueta, dto.Cor, dto.DiasDeEmprestimo);

        bool conseguiuEditar = repositorioCaixa.Editar(dto.Id, caixaAtualizada);

        if (!conseguiuEditar)
            return Result.Fail("Caixa não encontrada.");

        return Result.Ok();
    }

    public Result Excluir(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return Result.Fail("Caixa não encontrada.");

        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        foreach (Revista r in revistas)
        {
            if (string.Equals(r.Caixa.Id, id))
                return Result.Fail("Esta caixa não pode ser excluída pois está relacionada a uma revista.");
        }

        repositorioCaixa.Excluir(id);

        return Result.Ok();
    }

    public List<ListarCaixasDto> SelecionarTodos()
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        return caixas
            .Select(c => new ListarCaixasDto(c.Id, c.Etiqueta, c.Cor, c.DiasDeEmprestimo))
            .ToList();
    }

    public Result<DetalhesCaixaDto> SelecionarPorId(string id)
    {
        Caixa? caixa = repositorioCaixa.SelecionarPorId(id);

        if (caixa == null)
            return Result.Fail("Caixa não encontrada.");

        return Result.Ok(new DetalhesCaixaDto(caixa.Id, caixa.Etiqueta, caixa.Cor, caixa.DiasDeEmprestimo));
    }

    private bool ExisteCaixaComEtiqueta(string etiqueta, string? idIgnorado = null)
    {
        List<Caixa> caixas = repositorioCaixa.SelecionarTodos();

        foreach (Caixa c in caixas)
        {
            if (c.Id != idIgnorado && string.Equals(c.Etiqueta, etiqueta, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static Result Falha(string campo, string mensagem)
    {
        IError erro = new Error(mensagem).WithMetadata("Campo", campo);

        return Result.Fail(erro);
    }
}
