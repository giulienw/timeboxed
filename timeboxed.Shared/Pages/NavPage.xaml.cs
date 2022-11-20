
using System.Collections.Generic;
using Microsoft.UI.Xaml.Media.Animation;
using timeboxed.Helper;

namespace timeboxed.Pages;
public sealed partial class NavPage : Page
{
    public IList<NavigationViewItem> Pages { get; set; }
    private NavigationSyncHelper _navigationSyncHelper;

    public NavPage()
    {
        this.InitializeComponent();
        this.Loaded += NavPage_Loaded;
    }
    
    private async void NavPage_Loaded(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(MainPage), ContentFrame, new EntranceNavigationTransitionInfo());
    }
}
