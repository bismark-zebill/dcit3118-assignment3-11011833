using System;

public class InventoryApp
{
    private readonly InventoryLogger<InventoryItem> _logger;

    public InventoryApp(string filePath)
    {
        _logger = new InventoryLogger<InventoryItem>(filePath);
    }

    public void SeedSampleData()
    {
        _logger.Add(new InventoryItem(1, "Laptop", 10, DateTime.Now));
        _logger.Add(new InventoryItem(2, "Mouse", 50, DateTime.Now));
        _logger.Add(new InventoryItem(3, "Keyboard", 30, DateTime.Now));
        _logger.Add(new InventoryItem(4, "Monitor", 15, DateTime.Now));
        _logger.Add(new InventoryItem(5, "Printer", 5, DateTime.Now));
        Console.WriteLine("Sample data seeded.");
    }

    public void SaveData()
    {
        _logger.SaveToFile();
    }

    public void LoadData()
    {
        _logger.LoadFromFile();
    }

    public void PrintAllItems()
    {
        var items = _logger.GetAll();
        if (items.Count == 0)
        {
            Console.WriteLine("No inventory data to display.");
            return;
        }

        Console.WriteLine("\nInventory Items:");
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, DateAdded: {item.DateAdded}");
        }
    }
}
