// See https://aka.ms/new-console-template for more information
using System;

class Program
{
    static void Main()
    {
        string filePath = "inventory.json";

        // First session: Seed and save data
        InventoryApp app = new InventoryApp(filePath);
        app.SeedSampleData();
        app.SaveData();

        Console.WriteLine("\n--- Simulating new session ---\n");

        // Second session: Load and print data
        InventoryApp newApp = new InventoryApp(filePath);
        newApp.LoadData();
        newApp.PrintAllItems();
    }
}

