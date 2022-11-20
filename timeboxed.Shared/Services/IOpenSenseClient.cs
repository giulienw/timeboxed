namespace timeboxed.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using timeboxed.Models;

public interface IOpenSenseClient 
{
    Task<List<BoxData>> GetAllBoxes();
    Task<List<BoxData>> GetBoxesAtCoordinates(int latitude,int longitude,int maxDistance);
    Task<List<BoxSensor>> GetBoxSensorsAsync(string id);
    Task<BoxData> GetBox(string id);
}