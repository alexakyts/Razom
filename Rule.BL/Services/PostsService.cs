using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rule.BL.Models;
using Rule.Common.Extensions;
using Rule.DAL.Entities;
using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.UnitOfWork.Interfaces;

namespace Rule.BL.Services
{
    public class PostsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Posts> _repository;
        private readonly IRepository<StatusPost> _statusPostRepository;
        private readonly IRepository<TypePost> _typePostRepository;
        private readonly IMapper _mapper;
        public PostsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Posts>();
            _statusPostRepository = _unitOfWork.GetRepository<StatusPost>();
            _typePostRepository = _unitOfWork.GetRepository<TypePost>();
        }

        public async Task<PostsDTO> CreateAsync(PostsDTO newEntity)
        {
            try
            {
                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Name.ToUpper().Trim() == newEntity.Name.ToUpper().Trim());

                var existsDescription = await _repository.Get()
                   .AnyAsync(x => x.Description.ToUpper().Trim() == newEntity.Description.ToUpper().Trim());

                if (existsName && existsDescription)
                    throw new DuplicateItemException(ExceptionMessage(newEntity.Name));

                var entity = new Posts
                {
                    Id = default,
                    Name = newEntity.Name.Trim(),
                    Description = newEntity.Description.Trim(),
                    FinishSum = newEntity.FinishSum,
                    CreationTime = newEntity.CreationTime,
                    UsersId = newEntity.UsersId,
                    StatusPostId = newEntity.StatusPostId,
                    TypePostId = newEntity.TypePostId,
                    Link = newEntity.Link.Trim()
                };

                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<PostsDTO>(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<int> GetStatusPostIdAsync(string status)
        {
            var statusPost = await _statusPostRepository.Get().FirstOrDefaultAsync(x => x.Status.ToUpper().Trim() == status.ToUpper().Trim());
            return statusPost?.Id ?? 0; 
        }

        public async Task<int> GetTypePostIdAsync(string type)
        {
            var typePost = await _typePostRepository.Get().FirstOrDefaultAsync(x => x.Type.ToUpper().Trim() == type.ToUpper().Trim());
            return typePost?.Id ?? 0; 
        }

        public async Task<ICollection<PostsDTO>> GetAllAsync()
        {
            try
            {
                return _mapper.Map<ICollection<PostsDTO>>(await _repository.GetAllAsync());
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<PostsDTO> GetByIdAsync(int id)
        {
            try
            {
                return _mapper.Map<PostsDTO>(await _repository.GetByIdAsync(id)) ??
                    throw new InvalidIdException(ExceptionMessage(id));
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<PostsDTO> UpdateAsync(PostsDTO editEntity)
        {
            try
            {
                var currentEntity = await _repository.GetByIdAsync(editEntity.Id) ??
                    throw new InvalidIdException(ExceptionMessage(editEntity.Id));

                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Name.ToUpper().Trim() == editEntity.Name.ToUpper().Trim());

                var existsDescription = await _repository.Get()
                   .AnyAsync(x => x.Description.ToUpper().Trim() == editEntity.Description.ToUpper().Trim());

                if (existsName && existsDescription)
                    throw new DuplicateItemException(ExceptionMessage(editEntity.Name));
                // забрав    
                var existsLink = await _repository.Get()
                    .AnyAsync(x => x.Link.ToUpper().Trim() == editEntity.Link.ToUpper().Trim());

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
                int idt when value is int => $"Пост з id: {idt} ще/вже не існує!",
                string namet when value is string => $"Пост з назваю {namet} вже існує",
                _ => "Something has gone wrong"
            };
    }
}
