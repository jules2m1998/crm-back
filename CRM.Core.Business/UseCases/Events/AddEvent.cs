using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.Services;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.Events;

public static class AddEvent
{
    public record Command(EventInModel Model) : IRequest<EventOutModel>;

    public class Handler : IRequestHandler<Command, EventOutModel>
    {
        private readonly IEventRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IProspectionRepository _propectionRepo;
        private readonly IContactRepository _contactRepo;
        private readonly IEmailService _emailService;
        private readonly IEmailRepository _emailRepository;

        public Handler(
            IEventRepository repo,
            IUserRepository userRepository,
            IProspectionRepository prospectionRepository,
            IContactRepository contactRepository,
            IEmailService emailService,
            IEmailRepository emailRepository
            )
        {
            _repo = repo;
            _userRepo = userRepository;
            _propectionRepo = prospectionRepository;
            _contactRepo = contactRepository;
            _emailService = emailService;
            _emailRepository = emailRepository;
        }

        public async Task<EventOutModel> Handle(Command request, CancellationToken cancellationToken)
        {
            var model = request.Model;
            var user = await _userRepo.GetUserAndRolesAsync(model.UserName) ?? throw new UnauthorizedAccessException();
            var isAdmin = _userRepo.IsAdminUser(user);

            if (!isAdmin && model.OwnerId != null) 
                throw new UnauthorizedAccessException();

            Prospect? prospection = null;
            ICollection<Contact>? contacts = null;
            User? owner = null;

            if(model.AgentId != null && model.ProductId != null && model.CompanyId != null)
            {
                Guid agenId = model.AgentId ?? Guid.Empty;
                Guid productId = model.ProductId ?? Guid.Empty;
                Guid companyId = model.CompanyId ?? Guid.Empty;
                prospection = 
                    await _propectionRepo.GetOneAsync(agenId, productId, companyId) 
                    ?? throw new NotFoundEntityException("This prospection doesn't exist !");
            }

            if(model.ContactIds != null && model.ContactIds.Any())
            {
                contacts = await _contactRepo.GetContactByUserAsync(model.ContactIds, model.UserName) ?? throw new NotFoundEntityException("Some of these contact don't exist !");
            }

            if(model.OwnerId is not null)
            {
                owner = await _userRepo.GetUserByIdAsync((Guid)model.OwnerId) ?? throw new NotFoundEntityException("This user doesn't exist !");
            }

            Event e = new()
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                Name = model.Name,
                Topic = model.Topic,

                Prospect = prospection,
                Owner = owner ?? user,
                Creator = user,
                Contact = new List<Contact>()
            };

            await _repo.AddAsync(e);

            e.Contact = contacts;
            await _repo.UpdateAsync(e);

            await _emailRepository.AddAsync(e);

            return e.ToSimpleModel();
        }
    }
}
