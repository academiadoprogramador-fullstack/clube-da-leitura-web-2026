using ClubeDaLeituraWeb.WebApp.Compartilhado.Infra.Arquivos;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloAmigo.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloEmprestimo.Infra;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Dominio;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Infra;

namespace ClubeDaLeituraWeb.WebApp.Compartilhado.Infra;

public static class InjecaoDependencia
{
    public static void AddInfraRepositories(this IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            ContextoJson contextoJson = new ContextoJson();

            contextoJson.Carregar();

            return contextoJson;
        });

        services.AddScoped<IRepositorioCaixa, RepositorioCaixaEmArquivo>();
        services.AddScoped<IRepositorioRevista, RepositorioRevistaEmArquivo>();
        services.AddScoped<IRepositorioAmigo, RepositorioAmigoEmArquivo>();
        services.AddScoped<IRepositorioEmprestimo, RepositorioEmprestimoEmArquivo>();
    }
}
