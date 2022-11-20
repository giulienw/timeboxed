using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web;
using timeboxed.Models;
using timeboxed.Services;

namespace timeboxed.ViewModels;

public class BoxInfoPageViewModel : BaseViewModel
{
    private BoxData _box;
    private ObservableCollection<BoxSensor> _sensors;
    private Uri _image;

    public ObservableCollection<BoxSensor> Sensors {
        get => _sensors;
        set => SetProperty(ref _sensors,value);
    }

    public BoxData Box {
        get => _box;
        set => SetProperty(ref _box,value);
    }

    public Uri Image {
        get => _image;
        set => SetProperty(ref _image,value);
    }
    private IOpenSenseClient _apiClient;
    private IDatabaseClient _database;

    public BoxInfoPageViewModel(IOpenSenseClient apiClient, BoxData box)
    {
        _apiClient = apiClient;
        Box = box;
    }

    public async void SetBox(BoxData box)
    {
        Box = box;
        
        var sensors = Box.sensors;
        Sensors = sensors.ToObservableCollection();
        Image = new Uri(string.Format("https://opensensemap.org/userimages/{0}",_box.Image));
        //Console.WriteLine(Box.sensors[0].lastMeasurment.value);
    }


}
