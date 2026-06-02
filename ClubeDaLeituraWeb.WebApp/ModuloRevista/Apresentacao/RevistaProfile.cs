using AutoMapper;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public class RevistaProfile : Profile
{
    public RevistaProfile()
    {
        CreateMap<CadastrarRevistaViewModel, CadastrarRevistaDto>();
    }
}
