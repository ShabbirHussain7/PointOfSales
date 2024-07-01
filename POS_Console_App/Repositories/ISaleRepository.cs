using POS.Models;
using POS.DTOs;
using System.Collections.Generic;

namespace POS.Repositories
{
    public interface ISaleRepository
    {

        Sale GetSaleById(int id);
        ReceiptDto GenerateReceipt(Sale sale);
        ReceiptDto CompleteSale(int saleId);


        void AddProductsToSale(AddProductsToSaleDto addProductsToSaleDto);
    }
}