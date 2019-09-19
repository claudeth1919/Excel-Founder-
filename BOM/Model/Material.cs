using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOM.Model
{
    public class Material
    {
        private Guid id;
        private string name;
        private string code;
        private string description;
        private string originalCode;
        private string providerName;
        private string unit;

        public string OriginalCode
        {
            get { return originalCode; }
            set { originalCode = value; }
        }
        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
