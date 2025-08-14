using System;
using System.Collections.Generic;

namespace WarehouseInventorySystem
{
    
    // a. Marker Interface
    
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

   
    // b. ElectronicItem class
    
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

   
    // c. GroceryItem class
       public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    
    // e. Custom Exceptions
    
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

   
    // d. Generic Inventory Repository
    
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private Dictionary<int, T> _items = new Dictionary<int, T>();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");

            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Cannot remove: Item with ID {id} not found.");
            _items.Remove(id);
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");

            if (!_items.ContainsKey(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");

            _items[id].Quantity = newQuantity;
        }
    }

    
    // f. WareHouseManager class
  
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics = new InventoryRepository<ElectronicItem>();
        private InventoryRepository<GroceryItem> _groceries = new InventoryRepository<GroceryItem>();

        // Seed data for testing
        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 10, "Samsung", 12));

            _groceries.AddItem(new GroceryItem(1, "Rice", 100, DateTime.Now.AddMonths(6)));
            _groceries.AddItem(new GroceryItem(2, "Milk", 50, DateTime.Now.AddDays(10)));
        }

        // Print all items in any repository
        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
            }
            Console.WriteLine();
        }

        // Safely increase stock with error handling
        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Stock updated for item ID {id}: New Quantity = {item.Quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
            }
        }

        // Safely remove item with error handling
        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Item with ID {id} removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing item: {ex.Message}");
            }
        }

        // Accessors for repositories
        public InventoryRepository<ElectronicItem> GetElectronicsRepo() => _electronics;
        public InventoryRepository<GroceryItem> GetGroceriesRepo() => _groceries;
    }

    
    // Main Program
    
    public class Program
    {
        public static void Main(string[] args)
        {
            WareHouseManager manager = new WareHouseManager();

            // i. Seed data
            manager.SeedData();

            // ii. Print grocery items
            Console.WriteLine("=== Grocery Items ===");
            manager.PrintAllItems(manager.GetGroceriesRepo());

            // iii. Print electronic items
            Console.WriteLine("=== Electronic Items ===");
            manager.PrintAllItems(manager.GetElectronicsRepo());

            // iv. Test exception handling
            Console.WriteLine("=== Testing Exceptions ===");

            // Add a duplicate item
            try
            {
                manager.GetGroceriesRepo().AddItem(new GroceryItem(1, "Bread", 30, DateTime.Now.AddDays(5)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding duplicate: {ex.Message}");
            }

            // Remove a non-existent item
            manager.RemoveItemById(manager.GetElectronicsRepo(), 99);

            // Update with invalid quantity
            try
            {
                manager.GetElectronicsRepo().UpdateQuantity(1, -5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating quantity: {ex.Message}");
            }
        }
    }
}
