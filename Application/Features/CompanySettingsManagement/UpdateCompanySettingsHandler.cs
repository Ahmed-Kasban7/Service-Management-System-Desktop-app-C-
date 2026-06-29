using Application.Common;
using Application.DTOs.CompanySettingsDTOs;
using Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CompanySettingsManagement
{
    public class UpdateCompanySettingsHandler
    {
        private readonly ICompanyRepository _companyRepository;

        public UpdateCompanySettingsHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public void Handle(CompanySettingsDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CompanyName))
            {
                throw new ArgumentNullException("اسم الشركة مطلوب");   
            }

            _companyRepository.UpdateSettings(dto);
        }
    }
}
