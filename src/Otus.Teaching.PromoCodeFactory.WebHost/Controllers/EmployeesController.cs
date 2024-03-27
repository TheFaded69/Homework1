using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x => 
                new EmployeeShortResponse()
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FullName = x.FullName,
                    }).ToList();

            return employeesModelList;
        }
        
        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();
            
            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Create employee (async)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task AddEmployeeAsync(EmployeeRequest employeeRequest)
        {
            await  _employeeRepository.AddAsync(new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Roles = new List<Role>(employeeRequest.Roles.Select(role => new Role
                {
                    Id = Guid.NewGuid(),
                    Name = role.Name,
                    Description = role.Description
                })),
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
            });
        }
        
        [HttpPut("{id}")]
        public async Task UpdateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            await _employeeRepository.UpdateAsync(new Employee
            {
                Id = employeeRequest.Id,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Roles = new List<Role>(employeeRequest.Roles.Select(role => new Role
                {
                    Id = Guid.NewGuid(),
                    Name = role.Name,
                    Description = role.Description
                })),
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
            });
        }
        
        [HttpDelete("{id:guid}")]
        public async Task DeleteEmployeeAsync(Guid id)
        {
            await _employeeRepository.DeleteAsync(id);
        }
    }
}