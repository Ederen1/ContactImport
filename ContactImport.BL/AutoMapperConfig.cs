using AutoMapper;
using ContactImport.BL.Models;
using ContactImport.DAL.Entities;

namespace ContactImport.BL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<ContactEntity, ContactModel>().ReverseMap();
    }
}