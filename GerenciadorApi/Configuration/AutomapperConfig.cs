using AutoMapper;
using DevIO.Api.ViewModel;
using DevIO.Business.Models;

namespace DevIO.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig() 
        {
            CreateMap<Transacao, TransacaoViewModel>().ReverseMap();
        }   
    }
}
