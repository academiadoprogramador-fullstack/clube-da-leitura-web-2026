using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;

namespace ClubeDaLeituraWeb.WebApp.Compartilhado.Aplicacao;

public static class InjecaoDependencia
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ServicoCaixa>();
    }
}
