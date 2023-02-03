using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;

namespace CRM.Core.Business.UseCases.AddUsersByCSV
{
    public class AddUsersByCSVHandler : IRequestHandler<AddUsersByCSVCommand, List<UserCsvModel>>
    {
        private readonly IFileHelper _fileHelper;
        private readonly IUserRepository _userRepository;

        public AddUsersByCSVHandler(IFileHelper fileHelper, IUserRepository userRepository)
        {
            _fileHelper = fileHelper;
            _userRepository = userRepository;
        }

        public async Task<List<UserCsvModel>> Handle(AddUsersByCSVCommand request, CancellationToken cancellationToken)
        {
            var dataExtract = _fileHelper.ReadCsvFile<UserCsvModel, UserCsvModelMapper>(request.File);
            return await _userRepository.AddFromListAsync(dataExtract, request.Role);
        }
    }
}
