using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<T> GetByIdAsync(Guid id);
        Task<ActionResult> AddEmployeeAsync(BaseEntity employeeRequest);
        Task<ActionResult> UpdateEmployeeAsync(BaseEntity employeeRequest);
        Task<ActionResult> DeleteEmployeeAsync(Guid id);
    }
}