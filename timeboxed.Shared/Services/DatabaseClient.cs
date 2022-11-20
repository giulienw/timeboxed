using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using timeboxed.Services;
using timeboxed.Models;
using SQLite;

namespace timeboxed.Services;

public class DatabaseClient : IDatabaseClient
{
    private Lazy<SQLiteConnection> _databaseConnectionHolder;
    private IOpenSenseClient _apiClient;

    private SQLiteConnection Database
    {
        get
        {
            var database = _databaseConnectionHolder.Value;
            return database;
        }
    }

    public DatabaseClient()
    {
        _apiClient = new OpenSenseClient();
        _ = InitializeDatabase();
    }

    private async Task InitializeDatabase()
    {
        await Windows.Storage.StorageFolder.GetFolderFromPathAsync(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
        var dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "boxes.db");
        var exists = File.Exists(dbPath);
        Console.WriteLine(exists);
        _databaseConnectionHolder = new Lazy<SQLiteConnection>(() => new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
        if(!exists)
        {
            Database.CreateTable<BoxDataDatabase>();
            SaveItems(await _apiClient.GetAllBoxes().ConfigureAwait(false));
        }
    }

    public List<BoxData> GetItems()
    {
        var boxes = Database.Table<BoxDataDatabase>().ToList().SelectToList(b => b.ToNormal());
        return boxes;
    }

    public BoxData GetItem(string id)
    {
        BoxData box = Database.Get<BoxDataDatabase>(id).ToNormal();
        return box;
    }

    public BoxData UpdateItem(BoxData item)
    {
        Console.WriteLine("Updated Box: " + item.name);
        Database.Update(item.ToDatabase());
        return item;
    }

    public BoxData SaveItem(BoxData item)
    {
        Console.WriteLine("Saved Box: " + item.name);
        Database.InsertOrReplace(item.ToDatabase());
        return item;
    }

    public void SaveItems(List<BoxData> items)
    {
        var dbItems = items.Select(b => b.ToDatabase());

        Database.InsertAll(dbItems);
    }

    public async Task<List<BoxData>> GetBoxesAtCoordinates(int latitude, int longitude, int radius)
    {
        List<BoxData> boxes = await _apiClient.GetBoxesAtCoordinates(latitude, longitude, radius).ConfigureAwait(false);

        foreach (var box in boxes)
        {
            if (GetItems().IndexOf(box) == -1)
            {
                SaveItem(box);
            }
            else
            {
                UpdateItem(box);
            }
        }

        return boxes;
    }

    public async Task<List<BoxData>> UpdateItems()
    {
        List<BoxData> boxes = await _apiClient.GetAllBoxes();
        var dbBoxes = GetItems();

        if (!boxes.Any())
            return dbBoxes;

        foreach (var box in boxes)
        {
            if (!dbBoxes.Exists(b => b._id.Equals(box._id)))
            {
                SaveItem(box);
            }
            else
            {
                UpdateItem(box);
            }
        }

        return boxes;
    }
}