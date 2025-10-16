using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class GenericService<TEntity, TDTO>(IRepository<TEntity> repository , IUnitOfWork unitOfWork , IMapper mapper ) : IGenericService<TEntity, TDTO> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public Task<ResponseObject<IEnumerable<TDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseObject<TDTO>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
