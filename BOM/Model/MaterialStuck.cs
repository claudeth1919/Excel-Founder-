using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM.Model
{
    public class MaterialStuck : Material
    {
        private string provider;
        private string location;
        private string ktl;
        private double total;
        private bool isActive;

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public string Ktl
        {
            get { return ktl; }
            set { ktl = value; }
        }
       
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }
        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        
    }
}
