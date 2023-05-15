using NLog;
using System.Linq;
using Northwind_Console_Net06.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

try
{
    var db = new NWConsoleJSGContext();
    string choice;
    string option;
    string choice2;

    do
    {
        Console.WriteLine("1) Work with products");
        Console.WriteLine("2) Work with categories");
        Console.WriteLine("q to quit");
        option = Console.ReadLine();
        Console.Clear();
        logger.Info($"Option {option} selected");

        if (option == "1")
        {
            Console.WriteLine("1) Add a new record to products table");
            Console.WriteLine("2) Edit a specific record");
            Console.WriteLine("3) Display all records in the products table");
            Console.WriteLine("4) Display a specific product");
            choice2 = Console.ReadLine();
            Console.Clear();
            logger.Info($"Option {choice2} selected");

            if (choice2 == "1")
            {
                Product product = new Product();

                Console.Write("Product Name: ");
                product.ProductName = Console.ReadLine();
                product.Discontinued = false;
                Console.Write("Supplier ID: ");
                product.SupplierId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Category ID: ");
                product.CategoryId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Quantity per unit: ");
                product.QuantityPerUnit = Console.ReadLine();
                Console.Write("Unit Price: ");
                product.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Units in Stock: ");
                product.UnitsInStock = Convert.ToInt16(Console.ReadLine());
                Console.Write("Units on Order: ");
                product.UnitsOnOrder = Convert.ToInt16(Console.ReadLine());
                Console.Write("Reorder Level: ");
                product.ReorderLevel = Convert.ToInt16(Console.ReadLine());

                db.AddProduct(product);
                Console.Clear();
                logger.Info("Product added: {product.ProductName}", product.ProductName);
            }

            if (choice2 == "2")
            {
                Console.Write("Select product to edit: ");
                var product = GetProduct(db);
                if (product != null)
                {
                    Product updatedProduct = new Product();

                    Console.Write("Product Name: ");
                    updatedProduct.ProductName = Console.ReadLine();
                    updatedProduct.Discontinued = false;
                    Console.Write("Supplier ID: ");
                    updatedProduct.SupplierId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Category ID: ");
                    updatedProduct.CategoryId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Quantity per unit: ");
                    updatedProduct.QuantityPerUnit = Console.ReadLine();
                    Console.Write("Unit Price: ");
                    updatedProduct.UnitPrice = Convert.ToDecimal(Console.ReadLine());
                    Console.Write("Units in Stock: ");
                    updatedProduct.UnitsInStock = Convert.ToInt16(Console.ReadLine());
                    Console.Write("Units on Order: ");
                    updatedProduct.UnitsOnOrder = Convert.ToInt16(Console.ReadLine());
                    Console.Write("Reorder Level: ");
                    updatedProduct.ReorderLevel = Convert.ToInt16(Console.ReadLine());
                    updatedProduct.ProductId = product.ProductId;

                    db.EditProduct(updatedProduct);
                    logger.Info($"Product {product.ProductId} updated.");
                }
            }

            if (choice2 == "3")
            {
                Console.WriteLine("1) Display Active Products");
                Console.WriteLine("2) Display Discontinued Products");
                Console.WriteLine("3) Display All Products");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    logger.Info("User choice: 1 - Display active products");
                    //display active products
                    var activeQuery = db.Products.OrderBy(p => p.ProductName).Where(p => !p.Discontinued);
                    Console.WriteLine($"Number of Active Products: {activeQuery.Count()}");

                    if (activeQuery.Count() != 0)
                    {
                        foreach (var product in activeQuery)
                        {
                            Console.WriteLine(product.ProductName);
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        logger.Info("No active products");
                        Console.WriteLine("There are no active products");
                    }
                }
                else if (input == "2")
                {
                    logger.Info("User choice: 2 - Display discontinued prodcuts");
                    //display discontinuted products
                    var discontinuedQuery = db.Products.OrderBy(p => p.ProductName).Where(p => p.Discontinued);
                    Console.WriteLine($"Number of Discontinued Products: {discontinuedQuery.Count()}");

                    if (discontinuedQuery.Count() != 0)
                    {
                        foreach (var product in discontinuedQuery)
                        {
                            Console.WriteLine(product.ProductName);
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        logger.Info("No discontinued products");
                    }
                }
                else if (input == "3")
                {
                    logger.Info("User choice: 3 - Display all products");
                    //display all products - active and discontinued
                    var activeQuery = db.Products.OrderBy(p => p.ProductName).Where(p => !p.Discontinued);
                    var discontinuedQuery = db.Products.OrderBy(p => p.ProductName).Where(p => p.Discontinued);

                    Console.WriteLine("Active Products:");
                    foreach (var product in activeQuery)
                    {
                        Console.WriteLine($"\t{product.ProductName}");
                    }
                    Console.WriteLine($"Total Active Products: {activeQuery.Count()}");

                    Console.WriteLine("Discontinued Products:");
                    foreach (var product in discontinuedQuery)
                    {
                        Console.WriteLine($"\t{product.ProductName}");
                    }
                    Console.WriteLine($"Total Discontinued Products: {discontinuedQuery.Count()}");

                }
            }

            if (choice2 == "4")
            {
                Console.WriteLine("Choose a product to display");
                var product = GetProduct(db);

                if (product != null)
                {
                    var isActive = product.Discontinued;
                    string status;
                    if (isActive)
                    {
                        status = "true";
                    }
                    else
                    {
                        status = "false";
                    }
                    Console.WriteLine($"Product Id: {product.ProductId}\nProduct name: {product.ProductName}\nSupplier Id: {product.SupplierId}\nCategory Id: {product.CategoryId}\nQuantity Per Unit: {product.QuantityPerUnit}\nUnit Price: {product.UnitPrice:C2}\nUnits in Stock: {product.UnitsInStock}\nUnits on Order: {product.UnitsOnOrder}\nReorder Level: {product.ReorderLevel}\nDiscontinued: {status}\n");
                }
                else
                {
                    logger.Error("No product to display");
                }
            }
        }

        else if (option == "2")
        {
            Console.WriteLine("1) Add category");
            Console.WriteLine("2) Display all categories");
            choice = Console.ReadLine();
            Console.Clear();
            logger.Info($"Option {choice} selected");

            if (choice == "1")
            {
                Category category = new Category();

                Console.Write("Category name: ");
                category.CategoryName = Console.ReadLine();
                Console.Write("Description: ");
                category.Description = Console.ReadLine();

                db.AddCategory(category);
                logger.Info("Category added: {name}", category.CategoryName);
            }

            if (choice == "2")
            {
                var query = db.Categories.OrderBy(p => p.CategoryName);

                Console.WriteLine($"{query.Count()} records returned");
                foreach (var item in query)
                {
                    Console.WriteLine($"{item.CategoryName} - {item.Description}");
                }
            }
        }

    } while (option.ToLower() != "q");
}
catch (Exception ex)
{
    logger.Error(ex.Message);
}

logger.Info("Program ended");

static Product GetProduct(NWConsoleJSGContext db)
{
    var products = db.Products.OrderBy(p => p.ProductId);
    foreach (Product p in products)
    {
        Console.WriteLine($"{p.ProductId}: {p.ProductName}");
    }
    if (int.TryParse(Console.ReadLine(), out int ProductId))
    {
        Product product = db.Products.FirstOrDefault(p => p.ProductId == ProductId);
        if (product != null)
        {
            return product;
        }
    }
    return null;
}
