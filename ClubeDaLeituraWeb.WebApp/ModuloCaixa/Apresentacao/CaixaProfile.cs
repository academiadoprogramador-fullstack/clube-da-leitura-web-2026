using AutoMapper;
using ClubeDaLeituraWeb.WebApp.ModuloCaixa.Aplicacao;

namespace ClubeDaLeituraWeb.WebApp.ModuloCaixa.Apresentacao;

public class CaixaProfile : Profile
{
    public CaixaProfile()
    {
        CreateMap<ListarCaixasDto, ListarCaixasViewModel>();
        CreateMap<CadastrarCaixaViewModel, CadastrarCaixaDto>();
        CreateMap<EditarCaixaViewModel, EditarCaixaDto>();

        CreateMap<DetalhesCaixaDto, EditarCaixaViewModel>();
        CreateMap<DetalhesCaixaDto, ExcluirCaixaViewModel>();
    }
}
