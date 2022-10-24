using AutoMapper;

namespace TransactionAPIApplication.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<DataContracts.CreateTransactionRequest, Models.TransactionModel>();


            CreateMap<Models.TransactionModel, DataContracts.TransactionResponse>();
        }
      

    }
}
