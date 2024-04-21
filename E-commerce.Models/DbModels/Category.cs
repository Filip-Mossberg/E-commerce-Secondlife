using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category()
        {
            
        }

        public Category(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
