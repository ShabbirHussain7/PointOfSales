using POS.Models;
using POS.Database;
using POS.Models;

namespace POS.Services
{
    public class SaleSessionService
    {
        private readonly POSDbContext _dbContext;
        private Sale _currentSale;

        public SaleSessionService(POSDbContext context)
        {
            _dbContext = context;
        }

        public Sale GetCurrentSale()
        {
            if (_currentSale == null)
            {
                _currentSale = new Sale();
                _dbContext.Sales.Add(_currentSale);
                _dbContext.SaveChanges();
            }
            return _currentSale;
        }

        public void CompleteSale()
        {
            _currentSale = null;
        }
    }
}