using Application.Common;
using Application.DTOs;
using Application.DTOs.EmployeeDTOs; // تأكد من اسم الـ DTO الخاص بملخص الموظف
using Application.Features.AttachmentManagement.Commands;
using Application.Features.AttachmentManagement.Queries;
using Application.Features.DepartmentManagement;
using Application.Features.EmployeeManagement.Commands;
using Application.Features.EmployeeManagement.Queries; // تأكد من وجود الـ Queries هنا
using Application.Features.PhoneManagement.Commands;
using Application.Features.PhoneManagement.Queries;
using Presentation.View.CustomerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentation.View.EmployeeView
{
    /// <summary>
    /// Interaction logic for EmployeeUC.xaml
    /// </summary>
    public partial class EmployeeUC : UserControl
    {
        //  pagination & Search variables بنفس المسطرة
        private int CurrentPage = 1;
        private const int ROWPERPAGE = 8;
        private bool IsSearching = false;
        private string CurrentSearchText = "";

        private CreateEmployeeHandler _createEmployeeHandler;
        private GetDepartmentsLookupHandler _getDepartmentsHandler;
        private GetRolesByDepartmentHandler _getRolesHandler;

        private GetPagedEmployeeSummariesHandler _getPagedEmployeeSummariesHandler;
        private SearchEmployeeHandler _searchEmployeePageHandler;
        private GetEmployeeProfileHandler _getEmployeeProfileHandler;
        private GetEmployeePhonesHandler _getEmployeePhonesHandler;
        private AddPhoneToEmployee _addPhoneToEmployee;
        private UpdatePhoneHandler _updatePhoneHandler;
        private DeleteEmployeePhoneHandler _deleteEmployeePhoneHandler;
        private GetEmployeeAttachmentsHandler _getEmployeeAttachmentsHandler;
        private AddAttachmentHandler _addAttachmentHandler;
        private DeleteAttachmentHandler _deleteAttachmentHandler;

        public EmployeeUC()
        {
            InitializeComponent();
        }

        public void InitializeServices(
            CreateEmployeeHandler createEmployee,
            GetDepartmentsLookupHandler getDepartments,
            GetRolesByDepartmentHandler getRoles,
            GetPagedEmployeeSummariesHandler getPagedEmployees , 
            SearchEmployeeHandler searchEmployee , GetEmployeeProfileHandler getEmployeeProfile ,
            GetEmployeePhonesHandler getEmployeePhones , AddPhoneToEmployee addPhoneToEmployee ,
            UpdatePhoneHandler updatePhoneHandler , DeleteEmployeePhoneHandler deleteEmployeePhone , 
            GetEmployeeAttachmentsHandler getEmployeeAttachments , AddAttachmentHandler addAttachmentHandler , DeleteAttachmentHandler deleteAttachment
           )        
        {
            _createEmployeeHandler = createEmployee;
            _getDepartmentsHandler = getDepartments;
            _getRolesHandler = getRoles;
            _getPagedEmployeeSummariesHandler = getPagedEmployees;
            _searchEmployeePageHandler = searchEmployee;
            _getEmployeeProfileHandler = getEmployeeProfile;
            _getEmployeePhonesHandler = getEmployeePhones;
            _addPhoneToEmployee = addPhoneToEmployee;
            _updatePhoneHandler = updatePhoneHandler;
            _deleteEmployeePhoneHandler = deleteEmployeePhone;
            _getEmployeeAttachmentsHandler = getEmployeeAttachments;
            _addAttachmentHandler = addAttachmentHandler;
            _deleteAttachmentHandler = deleteAttachment;
            _createEmployeeHandler.EmployeeCreated += LoadAndBindEmployees;

            LoadAndBindEmployees();
        }

        private void BtnCreateEmployee_Click(object sender, RoutedEventArgs e)
        {
            var createEmployeeWin = new CreateEmployee(_createEmployeeHandler, _getDepartmentsHandler, _getRolesHandler)
            {
                Owner = Window.GetWindow(this)
            };

            createEmployeeWin.ShowDialog();
        }

        private PagedResult<EmployeeSummaryDto> LoadEmployees()
        {
            try
            {
                return _getPagedEmployeeSummariesHandler.Handle(CurrentPage, ROWPERPAGE);
            }
            catch
            {
                MessageBox.Show("خطأ في تحميل بيانات الموظفين");
                return new PagedResult<EmployeeSummaryDto>(
                    new List<EmployeeSummaryDto>(), 0, 1, ROWPERPAGE);
            }
        }

        public void LoadAndBindEmployees()
        {
            PagedResult<EmployeeSummaryDto> result;

            if (IsSearching && !string.IsNullOrWhiteSpace(CurrentSearchText))
            {
                result = _searchEmployeePageHandler.Handle(CurrentPage,ROWPERPAGE , CurrentSearchText);
            }
            else
            {
                result = LoadEmployees();
            }

            Bind(result);
        }

        private void Bind(PagedResult<EmployeeSummaryDto> result)
        {
            if (result == null) return;

            DgEmployees.ItemsSource = result.Items;
            TxtPageInfo.Text = CurrentPage.ToString();
            TxtEmployeeCountNumber.Text = result.TotalCount.ToString(); 

            BtnNextPage.IsEnabled = result.HasNextPage;
            BtnPrevPage.IsEnabled = result.HasPreviousPage;
        }

        private void BtnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if (!BtnNextPage.IsEnabled) return;

            CurrentPage++;
            LoadAndBindEmployees();
        }

        private void BtnPrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <= 1) return;

            CurrentPage--;
            LoadAndBindEmployees();
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentPage = 1;
                IsSearching = true;
                CurrentSearchText = SearchBox.Text.Trim();

                LoadAndBindEmployees();
            }
        }

        private void ResetSearch()
        {
            IsSearching = false;
            CurrentSearchText = "";
            CurrentPage = 1;
        }

        private void DgEmployees_SelectionChanged(object sender, MouseButtonEventArgs e)
        {
            if (DgEmployees.SelectedItem is EmployeeSummaryDto selectedEmployee)
            {
             
                var employeeProfileUC = new EmployeeProfileUC(
                    selectedEmployee.EmployeeId , _getEmployeeProfileHandler , 
                    _getEmployeePhonesHandler , _addPhoneToEmployee , _updatePhoneHandler ,
                    _deleteEmployeePhoneHandler , _getEmployeeAttachmentsHandler , _addAttachmentHandler , _deleteAttachmentHandler
                );

                //employeeProfileUC.BackRequested += (s, args) =>
                //{
                //    MainContentGrid.Children.Clear();
                //    MainContentGrid.Children.Add(this); 
                //};


                EmployeeProfileHolder.Content = employeeProfileUC;
                EmployeesContainer.Visibility = Visibility.Collapsed;
                EmployeeProfileHolder.Visibility = Visibility.Visible;
            }
        }
    }
}