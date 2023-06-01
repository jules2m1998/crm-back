using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Events;

public static class EditEvent
{
    public record Command(Guid Id, EventInModel Model): IRequest<EventOutModel>;
    public class Handler : IRequestHandler<Command, EventOutModel>
    {
        private readonly IEventRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IProspectionRepository _propectionRepo;
        private readonly IContactRepository _contactRepo;

        public Handler(IEventRepository repo, IUserRepository userRepository, IProspectionRepository prospectionRepository, IContactRepository contactRepository)
        {
            _repo = repo;
            _userRepo = userRepository;
            _propectionRepo = prospectionRepository;
            _contactRepo = contactRepository;
        }

        public async Task<EventOutModel> Handle(Command request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var user = await _userRepo.GetUserAndRolesAsync(model.UserName) ?? throw new UnauthorizedAccessException();
            var isAdmin = _userRepo.IsAdminUser(user);

            var e = (isAdmin ? await _repo.GetAsync(request.Id) : await _repo.GetAsync(request.Id, model.UserName)) ?? throw new NotFoundEntityException("Event not found !");

            if (!isAdmin && model.OwnerId != null)
                throw new UnauthorizedAccessException();

            Prospect? prospection = e.Prospect;
            ICollection<Contact>? contacts = e.Contact;
            User owner = e.Owner;

            if (model.AgentId != null && model.ProductId != null && model.CompanyId != null)
            {
                prospection = await _propectionRepo.GetOneAsync((Guid)model.AgentId!, (Guid)model.ProductId!, (Guid)model.CompanyId!) ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            }

            if (model.ContactIds != null && model.ContactIds.Any())
            {
                contacts = await _contactRepo.GetContactByUserAsync(model.ContactIds, model.UserName) ?? throw new NotFoundEntityException("Some of these contact don't exist !");
            }

            if (model.OwnerId is not null)
            {
                owner = await _userRepo.GetUserByIdAsync((Guid)model.OwnerId) ?? throw new NotFoundEntityException("This user doesn't exist !");
            }

            e.StartDate = model.StartDate;
            e.EndDate = model.EndDate;
            e.Description = model.Description;
            e.Name = model.Name;
            e.Prospect = prospection;
            e.Contact = contacts;
            e.Owner = owner;

            await _repo.UpdateAsync(e);

            return e.ToSimpleModel();
        }
    }
}
