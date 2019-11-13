﻿namespace MoteeQueso.B2C.Customer.Api.ViewModels
{
    public class CustomerViewModel
    {
        public int id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string phone_number { get; set; }

        public string email { get; set; }

        public int credit_card_type_id { get; set; }

        public string credit_card_number { get; set; }

        public int status_id { get; set; }
    }
}