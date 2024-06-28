using Microsoft.EntityFrameworkCore;
using POS.Services;
using POS.Models;
using POS.Database;

namespace POS
{
    class Program
    {
        static void Main(string[] args)
        {
            //new instance of DbContextOptionsBuilder for POSDbContext
            var optionsBuilder = new DbContextOptionsBuilder<POSDbContext>();
            optionsBuilder.UseInMemoryDatabase("POSDatabase");  // Use an in-memory database
            var options = optionsBuilder.Options;
            var dbContext = new POSDbContext(options);

            var userService = new UserService(dbContext);
            var productService = new ProductService(dbContext);
            var salesService = new SalesService(dbContext);

            userService.AddUser("Admin", "admin@mail.com", "admin123", UserRole.Admin);
            

            User currentUser = null;

            while (true)
            {
                #region Menu
                Console.WriteLine("\n" + new string('#', 50));
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Add Product");
                Console.WriteLine("4. View Products");
                Console.WriteLine("5. Update Product");
                Console.WriteLine("6. Remove Product");
                Console.WriteLine("7. Add Product to Sale");
                Console.WriteLine("8. Calculate Total and Generate Receipt");
                Console.WriteLine("9. Exit");
                Console.WriteLine(new string('#', 50) + "\n");
                #endregion 

                var choice = Console.ReadLine();

                switch (choice)
                {
                    #region SignUp/Login
                    case "1":
                        if (currentUser == null || currentUser.Role != UserRole.Admin)
                        {
                            Console.WriteLine("Access denied. Only admins can add users.");
                            break;
                        }
                        Console.WriteLine("Enter name:");
                        var name = Console.ReadLine();
                        Console.WriteLine("Enter email:");
                        var email = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        var password = Console.ReadLine();
                        Console.WriteLine("Enter role (1 for Admin, 2 for Cashier):");
                        var roleInput = Console.ReadLine();
                        UserRole role;

                        while (roleInput != "1" && roleInput != "2")
                        {
                            Console.WriteLine("Invalid role input. Please enter 1 for Admin or 2 for Cashier:");
                            roleInput = Console.ReadLine();
                        }

                        if (roleInput == "1")
                        {
                            role = UserRole.Admin;
                        }
                        else
                        {
                            role = UserRole.Cashier;
                        }

                        userService.AddUser(name, email, password, role);
                        Console.WriteLine("User registered successfully!");
                        break;
                    
                    case "2":
                        Console.WriteLine("Enter email:");
                        email = Console.ReadLine();
                        Console.WriteLine("Enter password:");
                        password = Console.ReadLine();
                        currentUser = userService.Authenticate(email, password);
                        if (currentUser != null)
                        {
                            Console.WriteLine($"Welcome {currentUser.Name}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials");
                        }
                        break;
                    #endregion

                    #region Admin Options
                    case "3":
                        if (currentUser == null || currentUser.Role != UserRole.Admin)
                        {
                            Console.WriteLine("Access denied. Only admins can add products.");
                            break;
                        }
                        Console.WriteLine("Enter product name:");
                        var productName = Console.ReadLine();
                        Console.WriteLine("Enter price:");
                        var price = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter quantity:");
                        var quantity = int.Parse(Console.ReadLine());
                        productService.AddProduct(productName, price, quantity);
                        break;

                    case "4":
                        if (currentUser == null)
                        {
                            Console.WriteLine("Access denied. You need to login to view products.");
                            break;
                        }
                        var products = productService.ViewProducts();
                        for (int i = 0; i < products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price:C} - {products[i].Quantity} units");
                        }
                        break;

                    case "5":
                        if (currentUser == null || currentUser.Role != UserRole.Admin)
                        {
                            Console.WriteLine("Access denied. Only admins can update products.");
                            break;
                        }
                        Console.WriteLine("Enter the index of the product to update:");

                        var allProducts = productService.ViewProducts();
                        for (int i = 0; i < allProducts.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {allProducts[i].Name} - {allProducts[i].Price:C} - {allProducts[i].Quantity} units");
                        }
                        int productIndexToUpdate = int.Parse(Console.ReadLine()) - 1;
                        Console.WriteLine("Enter new price:");
                        decimal newPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter new quantity:");
                        int newQuantity = int.Parse(Console.ReadLine());
                        productService.UpdateProduct(allProducts[productIndexToUpdate].Name, newPrice, newQuantity);
                        break;

                    case "6":
                        if (currentUser == null || currentUser.Role != UserRole.Admin)
                        {
                            Console.WriteLine("Access denied. Only admins can remove products.");
                            break;
                        }
                        var removeProducts = productService.ViewProducts();
                        for (int i = 0; i < removeProducts.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {removeProducts[i].Name} - {removeProducts[i].Price:C} - {removeProducts[i].Quantity} units");
                        }
                        int productIndexToRemove = int.Parse(Console.ReadLine()) - 1;
                        
                        productService.RemoveProduct(removeProducts[productIndexToRemove].Name);

                        break;
                    #endregion

                    #region Cashier Options
                    case "7":
                        // Add Product to Sale
                        if (currentUser == null || currentUser.Role != UserRole.Cashier)
                        {
                            Console.WriteLine("Access denied. Only cashiers can add products to sale.");
                            break;
                        }

                        Console.WriteLine("Enter the index of the product to add to sale:");
                        var availableProducts = productService.GetAvailableProducts();
                        for (int i = 0; i < availableProducts.Count; i++)
                        {
                            Console.WriteLine($"{i+1}. {availableProducts[i].Name} - {availableProducts[i].Quantity} available");
                        }
                        int productIndex = int.Parse(Console.ReadLine()) - 1;
                        Console.WriteLine("Enter the quantity to buy:");
                        int quantityToBuy = int.Parse(Console.ReadLine());
                        salesService.AddProductToSale(availableProducts[productIndex].Name, quantityToBuy);
                        break;

                    case "8":
                        if (currentUser == null || currentUser.Role != UserRole.Cashier)
                        {
                            Console.WriteLine("Access denied. Only cashiers can calculate total.");
                            break;
                        }
                        var sale = dbContext.Sales.Include(s => s.Products).FirstOrDefault();
                        if (sale != null)
                        {
                            var total = salesService.CalculateTotal(sale);
                            salesService.GenerateReceipt(sale);
                        }
                        else
                        {
                            Console.WriteLine("No sale found");
                        }
                        break;
                    #endregion

                    #region Others
                    case "9":
                        return;

                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                    #endregion
                }
            }
        }
    }
}