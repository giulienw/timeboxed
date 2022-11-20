// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using System.Collections.ObjectModel;
using timeboxed.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Media.Animation;
using timeboxed.Models;

namespace timeboxed.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ListPageViewModel ViewModel => (DataContext as ListPageViewModel);
        private Frame _frame;
        
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = (Application.Current as App).Host.Services.GetRequiredService<ListPageViewModel>();
            DetailColumn.Width = new GridLength(0);
        }

        private async void SearchResults_ItemClick(object sender, ItemClickEventArgs e)
        {
            var box = await ViewModel.GetBoxById((e.ClickedItem as BoxData)._id);
            #if HAS_UNO_WASM
                DetailContentPresenter.Content = box;
                DetailColumn.Width = GridLength.FromString("*");
                DetailContentPresenter.ContentTransitions.Clear();
                DetailContentPresenter.ContentTransitions.Add(new EntranceThemeTransition());
            #else
                await this.Navigator().NavigateRouteAsync(this, "../Frame/info", Qualifiers.None, box);
                //_frame.Navigate(typeof(BoxInfoPage), box, new EntranceNavigationTransitionInfo());
            #endif
            //await this.Navigator().NavigateViewAsync<BoxInfoPage>(this, Qualifiers.None, e.ClickedItem as BoxData);
        }
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _frame = (Frame) e.Parameter;
            
            base.OnNavigatedTo(e);
        }
    }
}
