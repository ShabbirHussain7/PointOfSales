using POS.Models;
using POS.Database;

namespace POS.Services
{
    public class SalesService
    {
        private readonly POSDbContext dbContext;

        public SalesService(POSDbContext context)
        {
            dbContext = context;
        }

        public void AddProductToSale(string productName, int quantityToBuy)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Name == productName);
            if (product != null && product.Quantity >= quantityToBuy)
            {
                var sale = new Sale();
                sale.Products.Add(product);
                sale.TotalAmount += product.Price * quantityToBuy;
                product.Quantity -= quantityToBuy;
                dbContext.Sales.Add(sale);
                dbContext.SaveChanges();
            }
        }
        
        public decimal CalculateTotal(Sale sale)
        {
            return sale.Products.Sum(p => p.Price);
        }

        public void GenerateReceipt(Sale sale)
        {
            Console.WriteLine("Receipt:");
            foreach (var product in sale.Products)
            {
                Console.WriteLine($"{product.Name} - {product.Price:C}");
            }
            Console.WriteLine($"Total: {sale.TotalAmount:C}");
        }
    }
}