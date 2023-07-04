using CarPartsShop.Data.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace CarPartsShop.Models.FormModels
{
    public class OrderInformationForm
    {
        public CartOrderModel? Cart { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public bool PayNow { get; set; } = false;

        public bool IsPaid { get; set; } = false;
    }


    public class CartOrderModel
    {
        public string Id { get; set; }

        public List<PickedUpPart> Parts { get; set; }
    }
}
