using AutoMapper;
using LementPro.Server.SvcTemplate.Repository.Entities;
using LementPro.Server.SvcTemplate.Sdk.Models.BlockWork;

namespace LementPro.Server.SvcTemplate.Service.Mapping
{
    /// <inheritdoc />
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapping profiles must be initialized in ctor
        /// </summary>
        public MappingProfile()
        {
            CreateMap<BlockWorkEntity, BlockWorkModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.DateCreated, opt => opt.MapFrom(s => s.DateCreated));

            CreateMap<BlockWorkEntity, BlockWorkModelSimple>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.DateCreated, opt => opt.MapFrom(s => s.DateCreated));
        }
    }
}
