using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using FluentResults;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

public class ServicoRevista
{
    private readonly IRepositorioRevista repositorioRevista;
    private readonly IRepositorioCaixa repositorioCaixa;

    public ServicoRevista(
        IRepositorioRevista repositorioRevista,
        IRepositorioCaixa repositorioCaixa
    )
    {
        this.repositorioRevista = repositorioRevista;
        this.repositorioCaixa = repositorioCaixa;
    }

    public Result Cadastrar(CadastrarRevistaDto dto)
    {
        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(dto.CaixaId);

        if (caixaSelecionada == null)
        {
            return Result.Fail(
                new Error("Selecione uma caixa válida.")
                    .WithMetadata("Campo", nameof(dto.CaixaId))
            );
        }

        if (ExisteRevistaComMesmoTituloEEdicao(dto.Titulo, dto.NumeroEdicao))
        {
            return Result.Fail(
                new Error("Já existe uma revista com este título e edição.")
                    .WithMetadata("Campo", nameof(dto.NumeroEdicao))
            );
        }

        Revista novaRevista = new Revista(
            dto.Titulo,
            dto.NumeroEdicao,
            dto.AnoPublicacao,
            caixaSelecionada!
        );

        List<string> erros = novaRevista.Validar();

        if (erros.Count > 0)
        {
            return Result.Fail(
                new Error(erros.First())
                    .WithMetadata("Campo", nameof(dto.AnoPublicacao))
            );
        }

        repositorioRevista.Cadastrar(novaRevista);

        return Result.Ok();
    }

    public Result Editar(EditarRevistaDto dto)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(dto.Id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        Caixa? caixaSelecionada = repositorioCaixa.SelecionarPorId(dto.CaixaId);

        if (caixaSelecionada == null)
        {
            return Result.Fail(
                new Error("Selecione uma caixa válida.")
                    .WithMetadata("Campo", nameof(dto.CaixaId))
            );
        }

        if (ExisteRevistaComMesmoTituloEEdicao(dto.Titulo, dto.NumeroEdicao, dto.Id))
        {
            return Result.Fail(
                new Error("Já existe uma revista com este título e edição.")
                    .WithMetadata("Campo", nameof(dto.NumeroEdicao))
            );
        }

        Revista revistaAtualizada = new Revista(
            dto.Titulo,
            dto.NumeroEdicao,
            dto.AnoPublicacao,
            caixaSelecionada!,
            revista.Status
        );

        List<string> erros = revistaAtualizada.Validar();

        if (erros.Count > 0)
        {
            return Result.Fail(
                new Error(erros.First())
                    .WithMetadata("Campo", nameof(dto.AnoPublicacao))
            );
        }

        repositorioRevista.Editar(dto.Id, revistaAtualizada);

        return Result.Ok();
    }

    public Result Excluir(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        repositorioRevista.Excluir(id);

        return Result.Ok();
    }

    public Result<DetalhesRevistaDto> SelecionarPorId(string id)
    {
        Revista? revista = repositorioRevista.SelecionarPorId(id);

        if (revista == null)
            return Result.Fail("Revista não encontrada.");

        return Result.Ok(new DetalhesRevistaDto(
            id,
            revista.Titulo,
            revista.NumeroEdicao,
            revista.AnoPublicacao,
            revista.Caixa.Id,
            revista.Caixa.Etiqueta
        ));
    }

    private bool ExisteRevistaComMesmoTituloEEdicao(
        string titulo,
        int numeroEdicao,
        string? idIgnorado = null
    )
    {
        List<Revista> revistas = repositorioRevista.SelecionarTodos();

        return revistas.Any(r =>
            r.Id != idIgnorado &&
            string.Equals(r.Titulo, titulo, StringComparison.OrdinalIgnoreCase) &&
            r.NumeroEdicao == numeroEdicao
        );
    }
}
