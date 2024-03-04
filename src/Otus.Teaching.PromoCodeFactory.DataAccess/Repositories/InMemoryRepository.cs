using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public async Task<ActionResult> AddEmployeeAsync(BaseEntity employeeRequest)
        {
            if (Data is List<BaseEntity> listData)
            {
                listData.Add(employeeRequest);
                return await Task.FromResult();
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public async Task<ActionResult> UpdateEmployeeAsync(BaseEntity employeeRequest)
        {
            if (Data is List<BaseEntity> listData)
            {
                listData
                    .Where(data => data.Id == employeeRequest.Id)
                    .Select(data =>
                    {
                        data = employeeRequest;
                        return data;
                    })
                    .ToList();
                return  Task.FromResult(200);
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        public async Task<ActionResult> DeleteEmployeeAsync(Guid id)
        {
            if (Data is List<BaseEntity> listData)
            {
                listData.Remove(listData.FirstOrDefault(data => data.Id == id));
                return await Task.FromResult();
            }
            else
                throw new ArgumentOutOfRangeException();
        }
    }
}