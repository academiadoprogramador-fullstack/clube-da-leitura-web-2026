using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

namespace ClubeDaLeituraWeb.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoCaixa>();
        services.AddScoped<ServicoRevista>();
    }
}
