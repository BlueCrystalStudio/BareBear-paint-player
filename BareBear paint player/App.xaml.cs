﻿using BareBear_paint_player.Logic.LocalRepozitories;
using BareBear_paint_player.Logic.Serialization;
using BareBear_paint_player.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BareBear_paint_player;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        // Dependency injection setup
        ServiceCollection services = new();
        ConfigureServices(services);

        ServiceProvider servicesProvider = services.BuildServiceProvider();

        var mainWindow = servicesProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    public void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<IStreamManager, CanvasStreamManager>();
        services.AddSingleton<RepozitoryManager>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
    }
}
