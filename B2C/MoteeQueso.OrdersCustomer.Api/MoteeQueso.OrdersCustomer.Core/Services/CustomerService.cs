﻿using MoteeQueso.OrdersCustomer.Core.Interfaces;
using MoteeQueso.OrdersCustomer.Infraestructure.Data;
using MoteeQueso.OrdersCustomer.Infraestructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoteeQueso.OrdersCustomer.Core.Services
{
    public class CustomerService : ICustomerService
    {
        public Customer Create(Customer customer)
        {
            using (B2CEntities entities = new B2CEntities())
            {
                entities.Customer.Add(customer);
                entities.SaveChanges();
            }

            return customer;
        }

        public Customer GetCustomerById(string CustId)
        {
            using (B2CEntities entities = new B2CEntities())
            {
                return entities.Customer.Where(x => x.CustId.Equals(CustId)).FirstOrDefault();
            }

        }

        public List<Customer> GetCustomers()
        {
            using (B2CEntities entities = new B2CEntities())
            {
                return entities.Customer.ToList();
            }
        }
    }
}
