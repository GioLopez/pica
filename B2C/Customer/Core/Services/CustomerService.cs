﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoteeQueso.B2C.Customer.Core.Interfaces;
using MoteeQueso.B2C.Customer.Infraestructure.Data;
using MoteeQueso.B2C.Customer.Infraestructure.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MoteeQueso.B2C.Customer.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<int> CreateCustomer(customer customer, string password)
        {
            customer.status_id = 1;

            using (TransactionScope transaction = new TransactionScope(
                TransactionScopeOption.Required, 
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                using (B2CCustomerEntities entities = new B2CCustomerEntities())
                {
                    entities.customer.Add(customer);
                    await entities.SaveChangesAsync();
                }

                await UpdatePassword(customer.email, password);

                transaction.Complete();
            }

            return customer.id;
        }

        public async Task UpdateCustomer(customer customer, string password)
        {
            using (TransactionScope transaction = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                using (B2CCustomerEntities entities = new B2CCustomerEntities())
                {
                    entities.Entry(customer).State = EntityState.Modified;
                    await entities.SaveChangesAsync();

                    foreach (address address in customer.addresses)
                    {
                        address.customer_id = customer.id;

                        if (address.id == 0)
                        {
                            entities.address.Add(address);
                        }
                        else
                        {
                            entities.Entry(address).State = EntityState.Modified;
                        }

                        await entities.SaveChangesAsync();
                    }
                }

                await UpdatePassword(customer.email, password);

                transaction.Complete();
            }
        }

        public async Task UpdatePassword(string email, string password)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string JWTService = configuration.GetValue<string>("JWTService");

            user user = new user
            {
                username = email,
                email = email,
                password = password,
                is_staff = false
            };

            string body = JsonConvert.SerializeObject(user);
            StringContent stringContent = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(JWTService, stringContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception("Change Password Fail");
            }
        }
    }
}