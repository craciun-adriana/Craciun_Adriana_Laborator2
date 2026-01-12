using Craciun_Adriana_Laborator2.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess=Craciun_Adriana_Laborator2.Data;
using ModelAccess=Craciun_Adriana_Laborator2.Models;

namespace GrpcCustomerService.Services
{
    public class GrpcCRUDService : CustomerService.CustomerServiceBase
    {
        private DataAccess.Craciun_Adriana_Laborator2Context db = null;
        public GrpcCRUDService(DataAccess.Craciun_Adriana_Laborator2Context db)
        {
            this.db = db;
        }
        public override Task<CustomerList> GetAll(Empty empty, ServerCallContext context)
        {
            CustomerList pl = new CustomerList();
            var query = from cust in db.Customer
                        select new Customer()
                        {
                            CustomerId = cust.CustomerID,
                            Name = cust.Name,
                            Adress = cust.Adress,
                            Birthdate = cust.BirthDate.ToString("yyyy-MM-dd")
                        };
            pl.Item.AddRange(query.ToArray());
            return Task.FromResult(pl);
        }
        public override Task<Empty> Insert(Customer requestData, ServerCallContext context)
        {
            db.Customer.Add(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Customer> Get(CustomerId requestData, ServerCallContext context)
        {
            var data = db.Customer.Find(requestData.Id);
            Customer emp = new Customer()
            {
                CustomerId = data.CustomerID,
                Name = data.Name,
                Adress = data.Adress,
                Birthdate = data.BirthDate.ToString("yyyy-MM-dd")
            };
            return Task.FromResult(emp);
        }
        public override Task<Empty> Delete(CustomerId requestData, ServerCallContext context)
        {
            var data = db.Customer.Find(requestData.Id);
            db.Customer.Remove(data);
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Customer> Update(Customer requestData, ServerCallContext context)
        {
            db.Customer.Update(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            var c = db.Customer.Find(requestData.CustomerId);
            return Task.FromResult(new Customer()
            {
                CustomerId = c.CustomerID,
                Name = c.Name,
                Adress = c.Adress,
                Birthdate = c.BirthDate.ToString("yyyy-MM-dd")
            });
        }
    }
}