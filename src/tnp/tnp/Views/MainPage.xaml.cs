using System.Collections.ObjectModel;
using tnp.Models;
using tnp.ViewModels;
using TNPSupport;

namespace tnp.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
