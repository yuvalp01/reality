using AutoMapper;
using Nadlan.Models;



namespace Nadlan.ViewModels
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();
            CreateMap<PersonalTransaction, PersonalTransactionDto>();

        }
    }
}
