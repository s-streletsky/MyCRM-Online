using AutoMapper;
using MyCRM_Online.Models.Entities;
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
            CreateMap<ClientCreateViewModel, ClientEntity>();
            CreateMap<ClientEntity, ClientViewModel>();
            CreateMap<ClientEntity, ClientEditViewModel>().ReverseMap();            
            CreateMap<ManufacturerCreateViewModel, ManufacturerEntity>();
            CreateMap<ManufacturerEntity, ManufacturerEditViewModel>().ReverseMap();
            CreateMap<StockItemEntity, StockItemViewModel>();
            CreateMap<StockItemCreateViewModel, StockItemEntity>();
            CreateMap<StockItemEntity, StockItemEditViewModel>().ReverseMap();
            CreateMap<StockArrivalEntity, StockArrivalViewModel>();
            CreateMap<StockArrivalCreateViewModel, StockArrivalEntity>();
            CreateMap<StockArrivalEntity, StockArrivalEditViewModel>().ReverseMap();
            CreateMap<ExchangeRateEntity, ExchangeRateViewModel>();
            CreateMap<ExchangeRateCreateViewModel, ExchangeRateEntity>();
            CreateMap<ExchangeRateEntity, ExchangeRateEditViewModel>().ReverseMap();
            CreateMap<OrderEntity, OrderViewModel>();
            CreateMap<OrderCreateViewModel, OrderEntity>();
            CreateMap<OrderEntity, OrderEditViewModel>().ReverseMap();
            CreateMap<OrderItemEntity, OrderItemViewModel>();
            CreateMap<OrderItemCreateViewModel, OrderItemEntity>();
            CreateMap<OrderItemEntity, OrderItemEditViewModel>().ReverseMap();
            CreateMap<PaymentCreateViewModel, PaymentEntity>();
            CreateMap<PaymentEntity, PaymentEditViewModel>().ReverseMap();
        }
    }
}
