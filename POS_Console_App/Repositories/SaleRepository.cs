using POS.Database;
using POS.Models;
using System.Collections.Generic;
using System.Linq;
using POS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace POS.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly POSDbContext _dbContext;

        public SaleRepository(POSDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public ReceiptDto CompleteSale(int saleId)
        {
            var sale = GetSaleById(saleId);
            if (sale == null)
            {
                return null;
            }
            var receipt = GenerateReceipt(sale);
            return receipt;
        }
    }
}