﻿// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.DependencyInjection;
using System;
using WidgetIutNc.Api;
using WidgetIutNc.Api.Services;
using WidgetIutNc.ViewModels;
using WidgetIutNc.ViewModels.Controls;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WidgetIutNc.Uwp;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
sealed partial class App
        : Application
{
    public new static App Current => Application.Current as App;
    public IServiceProvider Services { get; }
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        Services = ConfigureServices();

        this.InitializeComponent();
        this.Suspending += OnSuspending;
    }

    /// <summary>
    /// Inversion of Control container which's handling dependency injection through the project.
    /// </summary>
    /// <returns>Builed service provider</returns>
    public IServiceProvider ConfigureServices()
        => new ServiceCollection()
            .AddSingleton<IUpdatedCalendarFileDownloaderService, UpdatedCalendarFileDownloaderService>()
            .AddSingleton<ICalendarParserService, CalendarParserService>()

            .AddTransient<MainPageViewModel>()
            .AddTransient<ScheduleCellViewModel>()
        .BuildServiceProvider();


    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points
    /// will be used such as when the application is launched to open a specific file.
    /// </summary>
    /// <param name="e">Details about the launch request and process.</param>
    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        if (Window.Current.Content is not Frame rootFrame)
        {
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        if (!e.PrelaunchActivated)
        {
            if (rootFrame.Content is null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
        }
    }

    /// <summary>
    /// Invoked when Navigation to a certain page fails
    /// </summary>
    /// <param name="sender">The Frame which failed navigation</param>
    /// <param name="e">Details about the navigation failure</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e) => throw new Exception($"Failed to load Page {e.SourcePageType.FullName}");

    /// <summary>
    /// Invoked when application execution is being suspended.  Application state is saved
    /// without knowing whether the application will be terminated or resumed with the contents
    /// of memory still intact.
    /// </summary>
    /// <param name="sender">The source of the suspend request.</param>
    /// <param name="e">Details about the suspend request.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Save application state and stop any background activity
        deferral.Complete();
    }
}

