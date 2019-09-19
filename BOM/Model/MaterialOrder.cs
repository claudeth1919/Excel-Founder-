using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOM.Model;

namespace BOM.Model
{
    public class MaterialOrder : Material
    {
        private double wantedAmount;
        private double availableAmount;
        private double chosenAmount;
        private double stockTotal;
        private bool isSelected;
        private string location;
        private string ktl;
        private string bomUnit;
        private string originName;
        private string warehouseUnit;
        private Guid warehouseId;

        public MaterialOrder()
        {
            isSelected = true;
        }
        public string Ktl
        {
            get { return ktl; }
            set { ktl = value; }
        }
        public string BOMUnit
        {
            get { return bomUnit; }
            set { bomUnit = value; }
        }
        public string OriginName
        {
            get { return originName; }
            set { originName = value; }
        }
        public string WarehouseUnit
        {
            get { return warehouseUnit; }
            set { warehouseUnit = value; }
        }
        public Guid WarehouseId
        {
            get { return warehouseId; }
            set { warehouseId = value; }
        }
        public double WantedAmount
        {
            get { return wantedAmount; }
            set { wantedAmount = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public double AvailableAmount
        {
            get { return availableAmount; }
            set { availableAmount = value; }
        }
        public double ChosenAmount
        {
            get { return chosenAmount; }
            set { chosenAmount = value; }
        }
        public double StockTotal
        {
            get { return stockTotal; }
            set { stockTotal = value; }
        }
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
    }
}
