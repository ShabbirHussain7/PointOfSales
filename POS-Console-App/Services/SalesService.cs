using Microsoft.EntityFrameworkCore;
using POS.Database;
using POS.Models;
using POS.DTOs;

namespace POS.Services
{
    public class SalesService
    {
        private readonly POSDbContext _dbContext;

        public SalesService(POSDbContext context)
        {
            _dbContext = context;
        }

        public void AddProductsToSale(AddProductsToSaleDto addProductsToSaleDto)
        {
            var sale = new Sale();

            foreach (var productQuantity in addProductsToSaleDto.Products)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == productQuantity.ProductId);
                if (product != null && product.Quantity >= productQuantity.Quantity)
                {
                    var saleProductDetail = new SaleProductDetail
                    {
                        SaleId = sale.Id,
                        ProductName = product.Name,
                        QuantityBought = productQuantity.Quantity,
                        Price = product.Price
                    };

                    sale.ProductDetails.Add(saleProductDetail);
                    sale.TotalAmount += product.Price * productQuantity.Quantity;
                    product.Quantity -= productQuantity.Quantity;
                    _dbContext.SaleProductDetails.Add(saleProductDetail);
                }
            }

            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();
        }

        public Sale GetSaleById(int id)
        {
            return _dbContext.Sales
                .Include(s => s.ProductDetails)
                .FirstOrDefault(s => s.Id == id);
        }

        public ReceiptDto GenerateReceipt(Sale sale)
        {
            var receipt = new ReceiptDto
            {
                TotalAmount = sale.TotalAmount
            };

            foreach (var detail in sale.ProductDetails)
            {
                receipt.ProductDetails.Add(new ReceiptProductDetailDto
                {
                    ProductName = detail.ProductName,
                    QuantityBought = detail.QuantityBought,
                    Price = detail.Price
                });
            }

            return receipt;
        }
    }
}