﻿using System;

namespace MoteeQueso.Providers.Lodging.Infraestructure.Entities
{
    public class room
    {
        public int id { get; set; }

        public int agreement_id { get; set; }

        public int days { get; set; }

        public Guid filed { get; set; }
    }
}