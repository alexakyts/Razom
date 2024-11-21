using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rule.BL.Models;
using Rule.Common.Extensions;
using Rule.DAL.Entities;
using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.UnitOfWork.Interfaces;

namespace Rule.BL.Services
{
    public class FoundationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Foundations> _repository;
        private readonly IMapper _mapper;
        public FoundationsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Foundations>();
        }

        public async Task<FoundationsDTO> CreateAsync(FoundationsDTO newEntity)
        {
            try
            {
                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Name.ToUpper().Trim() == newEntity.Name.ToUpper().Trim());

                var existsDescription = await _repository.Get()
                   .AnyAsync(x => x.Description.ToUpper().Trim() == newEntity.Description.ToUpper().Trim());

                var existsLink = await _repository.Get()
                   .AnyAsync(x => x.Link.ToUpper().Trim() == newEntity.Link.ToUpper().Trim());

                if (existsName && existsDescription && existsLink)
                    throw new DuplicateItemException(ExceptionMessage(newEntity.Name));

                var entity = new Foundations
                {
                    Id = default,
                    Name = newEntity.Name.Trim(),
                    Description = newEntity.Description.Trim(),
                    Link = newEntity.Link.Trim(),
                    SourceLink = newEntity.SourceLink.Trim(),
                    Pictures = newEntity.Pictures.Trim()
                };

                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<FoundationsDTO>(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<ICollection<FoundationsDTO>> GetAllAsync()
        {
            try
            {
                return _mapper.Map<ICollection<FoundationsDTO>>(await _repository.GetAllAsync());
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<FoundationsDTO> GetByIdAsync(int id)
        {
            try
            {
                return _mapper.Map<FoundationsDTO>(await _repository.GetByIdAsync(id)) ??
                    throw new InvalidIdException(ExceptionMessage(id));
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<FoundationsDTO> UpdateAsync(FoundationsDTO editEntity)
        {
            try
            {
                var currentEntity = await _repository.GetByIdAsync(editEntity.Id) ??
                    throw new InvalidIdException(ExceptionMessage(editEntity.Id));

                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Name.ToUpper().Trim() == editEntity.Name.ToUpper().Trim());

                var existsDescription = await _repository.Get()
                   .AnyAsync(x => x.Description.ToUpper().Trim() == editEntity.Description.ToUpper().Trim());

                var existsLink = await _repository.Get()
                   .AnyAsync(x => x.Link.ToUpper().Trim() == editEntity.Link.ToUpper().Trim());

                if (existsName && existsDescription && existsLink)
                    throw new DuplicateItemException(ExceptionMessage(editEntity.Name));

                _mapper.Map(editEntity, currentEntity);
                await _unitOfWork.SaveChangesAsync();
                return editEntity;
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var currentEntity = await _repository.GetByIdAsync(id) ??
                    throw new InvalidIdException(ExceptionMessage(id));
                await _repository.DeleteAsync(currentEntity);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        private string ExceptionMessage(object? value = null) =>
            value switch
            {
                int idt when value is int => $"Фонд з id: {idt} ще/вже не існує!",
                string namet when value is string => $"Фонд з назваю {namet} вже існує",
                _ => "Something has gone wrong"
            };
    }
}
