﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using JokesOnYou.Web.Api.Data;
using JokesOnYou.Web.Api.DTOs;
using JokesOnYou.Web.Api.Models;
using JokesOnYou.Web.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesOnYou.Web.Api.Repositories
{
    public class JokesRepository : IJokesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public JokesRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> DoesJokeExist(JokeCreateDto jokeCreateDto) =>
            _context.Jokes.AnyAsync(joke => joke.NormalizedPremise == jokeCreateDto.NormalizedPremise &&
                                            joke.NormalizedPunchLine == jokeCreateDto.NormalizedPunchline);

        public Task CreateJokeAsync(Joke joke) => _context.Jokes.AddAsync(joke).AsTask();
        public async Task<IEnumerable<Joke>> GetAllJokesAsync() => await _context.Jokes.ToListAsync();
        public async Task<IEnumerable<JokeReplyDto>> GetAllJokeDtosAsync() => await _context.Jokes.ProjectTo<JokeReplyDto>(_mapper.ConfigurationProvider).ToListAsync();

        public Task<JokeReplyDto> GetJokeDtoAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
