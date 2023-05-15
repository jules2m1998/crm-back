using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.UserUcs.GetAllUsers;

public record GetAllUsersQuery(): IRequest<ICollection<UserModel>>;
