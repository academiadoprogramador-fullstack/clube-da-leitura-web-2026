using AutoMapper;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;
using ClubeDaLeituraWeb.WebApp.ModuloRevista.Aplicacao;

namespace ClubeDaLeituraWeb.WebApp.ModuloRevista.Apresentacao;

public class RevistaProfile : Profile
{
    public RevistaProfile()
    {
        CreateMap<ListarRevistasDto, ListarRevistasViewModel>();
        CreateMap<ListarCaixasDto, OpcaoCaixaViewModel>();

        CreateMap<CadastrarRevistaViewModel, CadastrarRevistaDto>();
        CreateMap<EditarRevistaViewModel, EditarRevistaDto>();

        CreateMap<DetalhesRevistaDto, EditarRevistaViewModel>()
            .ForCtorParam("Caixas", opt => opt.MapFrom(_ => new List<OpcaoCaixaViewModel>()));

        CreateMap<DetalhesRevistaDto, ExcluirRevistaViewModel>();
    }
}
