﻿using System;

namespace MoteeQueso.Providers.Lodging.Api.ViewModels
{
    public class ReserveViewModel
    {
        public int id { get; set; }

        public int provider_id { get; set; }

        public int integration_type_id { get; set; }

        public int agreement_id { get; set; }

        public int days { get; set; }

        public Guid filed { get; set; }
    }
}