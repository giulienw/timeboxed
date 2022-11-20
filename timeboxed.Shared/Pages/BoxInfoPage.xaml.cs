using Newtonsoft.Json;
using timeboxed.Models;
using timeboxed.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace timeboxed.Pages;


public sealed partial class BoxInfoPage : Page
{
    public BoxInfoPageViewModel? ViewModel => (DataContext as BoxInfoPageViewModel);

    public BoxInfoPage()
    {
        this.InitializeComponent();

        DataContext = (Application.Current as App).Host.Services.GetRequiredService<BoxInfoPageViewModel>();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        var box = e.Parameter as BoxData;
        ViewModel.SetBox(box);
        base.OnNavigatedTo(e);
    }
}

