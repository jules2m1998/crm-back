﻿using CRM.Core.Business.Models;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> AddAsync(User user, string pwd, string role);
        Task<Tuple<User, List<Role>>?> GetByUserAndRoleAsync(string username, string password);
        Task<Tuple<User, List<Role>>?> GetUserAndRole(string username);
        UserModel UserToUserModel(User user, List<Role> roles);
        Task<List<UserCsvModel>> AddFromListAsync(List<UserCsvModel> users, string role);
    }
}
