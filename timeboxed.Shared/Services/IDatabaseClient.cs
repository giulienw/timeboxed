using System.Collections.Generic;
using System.Threading.Tasks;
using timeboxed.Models;

namespace timeboxed.Services;

public interface IDatabaseClient
{
    BoxData GetItem(string id);
    List<BoxData> GetItems();
    BoxData UpdateItem(BoxData item);
    BoxData SaveItem(BoxData item);
    void SaveItems(List<BoxData> items);
}