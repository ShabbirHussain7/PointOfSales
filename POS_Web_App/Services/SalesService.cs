using Microsoft.EntityFrameworkCore;
using PosWebApp.Data;
using PosWebApp.Models;

namespace PosWebApp.Services
{
    public class SalesService
    {
        private readonly POSDbContext _dbContext;

        public SalesService(POSDbContext context)
        {
            _dbContext = context;
        }

        public void AddProductToSale(int productId, int quantityToBuy)
        {
            var product = _dbContext.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null && product.Quantity >= quantityToBuy)
            {
                var sale = new Sale();
                _dbContext.Sales.Add(sale);
                _dbContext.SaveChanges(); // Save the sale to get its Id

                var saleProductDetail = new SaleProductDetail
                {
                    SaleId = sale.Id,
                    ProductName = product.Name,
                    QuantityBought = quantityToBuy,
                    Price = product.Price
                };

                sale.ProductDetails.Add(saleProductDetail);
                sale.TotalAmount += product.Price * quantityToBuy;
                product.Quantity -= quantityToBuy;
                _dbContext.SaleProductDetails.Add(saleProductDetail);
                _dbContext.SaveChanges();
            }
        }

        public Sale GetSaleById(int id)
        {
            return _dbContext.Sales
                .Include(s => s.ProductDetails)
                .FirstOrDefault(s => s.Id == id);
        }

        public string GenerateReceipt(Sale sale)
        {
            var receipt = "Receipt:\n";
            foreach (var detail in sale.ProductDetails)
            {
                receipt += $"{detail.ProductName} - {detail.QuantityBought} @ {detail.Price:C}\n";
            }
            receipt += $"Total: {sale.TotalAmount:C}\n";
            return receipt;
        }
    }
}