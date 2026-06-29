using Application.DTOs.CompanySettingsDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CompanySettingsManagement
{
    public class GetCompanySettingsHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public GetCompanySettingsHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public CompanySettingsDto Handle()
        {
            return _companyRepository.GetSettings();
        }
    }
}
