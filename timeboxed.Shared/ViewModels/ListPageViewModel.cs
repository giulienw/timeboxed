using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using timeboxed.Models;
using timeboxed.Services;

namespace timeboxed.ViewModels;

public class ListPageViewModel : BaseViewModel
{
    private IDatabaseClient _database;
    private ObservableCollection<BoxData> _boxes;
    private bool _isLoading;
    private string _searchTerm;
    private IOpenSenseClient _apiClient;
    private ILogger<ListPageViewModel> _logger;
    public string SearchTerm
    {
        get => _searchTerm;
        set => SetProperty(ref _searchTerm, value);
    }
    public ICommand RefreshCommand { private set; get; }
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }
    public ObservableCollection<BoxData> Boxes
    {
        get => _boxes;
        set => SetProperty(ref _boxes, value);
    }

    public string Title => "Test123";

    public ListPageViewModel(ILogger<ListPageViewModel> logger, DatabaseClient database, IOpenSenseClient apiClient)
    {
        _database = database;
        _apiClient = apiClient;
        _logger = logger;
        _ = Load();
    }

    public void PerformSearch()
    {
        var normalizedQuery = SearchTerm?.ToLower() ?? "";
        Boxes = new ObservableCollection<BoxData>(_database.GetItems().Where(b => b.name.ToLowerInvariant().Contains(normalizedQuery)).ToList());
    }

    public async Task Load() 
    {
        var data = await _apiClient.GetAllBoxes();
        _logger.Log(LogLevel.Debug,data.Count.ToString());
        Boxes = new ObservableCollection<BoxData>(data.Take(10));
        //_database.SaveItems(data);
    }

    public async Task<BoxData> GetBoxById(string id)
    {
        return await _apiClient.GetBox(id).ConfigureAwait(false);
    }
}