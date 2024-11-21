using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Rule.BL.Models;
using Rule.Common.Extensions;
using Rule.DAL.Entities;
using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rule.BL.Services
{
    public class UsersService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Users> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UsersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<Users>();
        }
        private string ExceptionMessage(object? value = null) =>
               value switch
               {
                   int idt when value is int => $"Юзер з id: {idt} ще/вже не існує!",
                   string namet when value is string => $"Юзер з назваю {namet} вже існує",
                   _ => "Something has gone wrong"
               };

        public async Task<Users> GetUserByUsername(string username)
        {
            try
            {
                var user = await _repository.Get()
                    .FirstOrDefaultAsync(x => x.Username.ToUpper().Trim() == username.ToUpper().Trim());

                if (user is null)
                {
                    throw new NullReferenceException("User not found");
                }

                return user;
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<UsersDTO> CreateAsync(UsersDTO newCheck)
        {
            try
            {
                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Username.ToUpper().Trim() == newCheck.Username.ToUpper().Trim());

                if (existsName)
                    throw new DuplicateItemException(ExceptionMessage(newCheck.Username));

                var existsEmail = await _repository.Get()
                   .AnyAsync(x => x.Email.ToUpper().Trim() == newCheck.Email.ToUpper().Trim());

                if (existsEmail)
                    throw new DuplicateItemException(ExceptionMessage(newCheck.Email));

                var entity = new Users
                {
                    Id = default,
                    Name = newCheck.Name.Trim(),
                    LastName = newCheck.LastName.Trim(),
                    Username = newCheck.Username.Trim(),
                    Phone = newCheck.Phone,
                    Email = newCheck.Email.Trim(),
                    Password = newCheck.Password.Trim()
                };

                await _repository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<UsersDTO>(entity);
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }
        public async Task<ICollection<UsersDTO>> GetAllAsync()
        {
            try
            {
                return _mapper.Map<ICollection<UsersDTO>>(await _repository.GetAllAsync());
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<UsersDTO> GetByIdAsync(int id)
        {
            try
            {
                return _mapper.Map<UsersDTO>(await _repository.GetByIdAsync(id)) ??
                    throw new InvalidIdException(ExceptionMessage(id));
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

        public async Task<UsersDTO> UpdateAsync(UsersDTO updateUsers)
        {
            try
            {
                var currentEntity = await _repository.GetByIdAsync(updateUsers.Id) ??
                     throw new InvalidIdException(ExceptionMessage(updateUsers.Id));

                var existsName = await _repository.Get()
                   .AnyAsync(x => x.Username.ToUpper().Trim() == updateUsers.Username.ToUpper().Trim());

                if (existsName)
                    throw new DuplicateItemException(ExceptionMessage(updateUsers.Username));

                _mapper.Map(updateUsers, currentEntity);
                await _unitOfWork.SaveChangesAsync();
                return updateUsers;
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
                var deleteUsers = await _repository.GetByIdAsync(id) ??
                    throw new InvalidIdException(ExceptionMessage(id));
                await _repository.DeleteAsync(deleteUsers);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ServerErrorException(ex.Message, ex);
            }
        }

    }
}
