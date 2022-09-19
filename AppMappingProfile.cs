using AutoMapper;
using MyCRM_Online.Models;
using MyCRM_Online.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM_Online
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<ClientCreateViewModel, Client>();
            CreateMap<Client, ClientsViewModel>();
            CreateMap<Client, ClientEditViewModel>().ReverseMap();
        }
    }
}
