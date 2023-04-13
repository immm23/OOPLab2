using AutoMapper;
using LabOOP2.Models;

namespace LabOOP2
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CustomerInputModel, Customer>();
        }
    }
}
