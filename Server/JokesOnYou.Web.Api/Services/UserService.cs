﻿using JokesOnYou.Web.Api.DTOs;
using JokesOnYou.Web.Api.Exceptions;
using JokesOnYou.Web.Api.Models;
using JokesOnYou.Web.Api.Repositories.Interfaces;
using JokesOnYou.Web.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JokesOnYou.Web.Api.Exceptions;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace JokesOnYou.Web.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepo, IMapper mapper, IUnitOfWork unitOfWork, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userRepository = userRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // Disable "this async method lacks an await operator" Remove this when we actually implement methods
#pragma warning disable 1998



        public async Task<IEnumerable<UserReplyDTO>> GetAll()
        {
            return await _userRepository.GetUsersReplyDtoAsync();
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userRepository.GetUserAsync(id);
            if (user == null)
            {
                throw new AppException($"Cant find user of id:{id}");
            }
            await _userRepository.DeleteUserAsync(user);
        }

        public async Task<UserReplyDTO> GetUserReplyById(string id)
        {
            return await _userRepository.GetUserReplyAsync(id);
        }

        public async Task UpdateUser(UserUpdateDTO userDTO)
        {
            var user = await _userRepository.GetUserAsync(userDTO.Id);
            _mapper.Map(userDTO, user);
            await _unitOfWork.SaveAsync();
        }


        public async Task<User> GetUserById(string id)
        {
            return await _userRepository.GetUserAsync(id);
        }



        public async Task<UserReplyDTO> LoginUser(UserLoginDTO userLogin)
        {
            var user = new EmailAddressAttribute().IsValid(userLogin.LoginName) ? await _userRepository.GetUserByEmailAsync(userLogin.LoginName) :
                await _userRepository.GetUserByUsernameAsync(userLogin.LoginName);

            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, userLogin.Password, false);

                if (!signInResult.Succeeded)
                {
                    throw new AppException("Sign in failed");
                }
                else
                {
                    var userReplyDTO = new UserReplyDTO()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.Email,
                        Token = _tokenService.GetToken(user)
                    };
                    return userReplyDTO;
                }
            }
            else
            {
                throw new AppException("User not found");
            }
        }

        public async Task RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            if (new EmailAddressAttribute().IsValid(userRegisterDTO.UserName))
            {
                throw new UserRegisterException("Cannot use an email as username");
            }
            
            var user = await _userRepository.CreateUserAsync(userRegisterDTO);

            if (user == null)
            {
                throw new AppException("Failed to find user in the Database.");
            }
        }
    }
}
