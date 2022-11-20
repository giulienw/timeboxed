
using timeboxed.Pages;

namespace timeboxed.ViewModels;

public class ShellViewModel : BaseViewModel
{
	private INavigator Navigator { get; }


	public ShellViewModel(
		INavigator navigator)
	{

		Navigator = navigator;

		_ = Start();
	}

	public async Task Start()
	{
		await Navigator.NavigateViewAsync<NavPage>(this);
	}
}
