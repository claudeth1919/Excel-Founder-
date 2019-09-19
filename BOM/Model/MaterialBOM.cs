using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM.Model
{
    public class MaterialBOM : Material
    {
        private double amount;
        public int Row { get; set; }
        public string Sheet { get; set; }
        
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }
}
