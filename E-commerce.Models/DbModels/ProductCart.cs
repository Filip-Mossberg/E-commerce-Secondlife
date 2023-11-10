﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class ProductCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
        public int CartId { get; set; }
        public Cart cart { get; set; }
    }
}
