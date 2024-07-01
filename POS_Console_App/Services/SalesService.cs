using POS.Models;
using POS.Repositories;
using System.Collections.Generic;
using POS.DTOs;

namespace POS.Services
{
    public class SalesService
    {
        private readonly ISaleRepository _saleRepository;

        public SalesService(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        public Sale GetSaleById(int id)
        {
            return _saleRepository.GetSaleById(id);
        }

        // New method to add products to a sale
        public void AddProductsToSale(AddProductsToSaleDto addProductsToSaleDto)
        {
            _saleRepository.AddProductsToSale(addProductsToSaleDto);
        }

        public ReceiptDto GenerateReceipt(Sale sale)
        {
            return _saleRepository.GenerateReceipt(sale);
        }

        public ReceiptDto CompleteSale(int saleId)
        {
           return _saleRepository.CompleteSale(saleId);
        }
    }
}