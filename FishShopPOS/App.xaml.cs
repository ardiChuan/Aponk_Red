using Microsoft.Extensions.DependencyInjection;
using FishShopPOS.Services;

namespace FishShopPOS;

public partial class App : Application
{
	public App(DatabaseService databaseService)
	{
		InitializeComponent();
		databaseService.InitializeAsync().Wait();
	}
}