﻿using JokesOnYou.Web.Api.Repositories.Interfaces;
using JokesOnYou.Web.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokesOnYou.Web.Api.Services
{
    public class ReasonService : IReasonService
    {
        private readonly IReasonRepository _reasonRepo;

        public ReasonService(IReasonRepository reasonRepo)
        {
            _reasonRepo = reasonRepo;
        }
    }
}
