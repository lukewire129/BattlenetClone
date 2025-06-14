﻿using FlexMVVM;
using FlexMVVM.WPF;
using FlexMVVM.WPF.Markup;
using System.Diagnostics;
using System.Windows;

namespace Battlenet;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : FlexApplication
{
    //protected override void Render() => flex.Window(()=> new MainWindow())
    //                                        .StartWithLayout<Login.Content> ();
    protected override void Render() => flex.StartWithLayout<Login.Module> ();

    protected override void ModuleContext(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<Common.Module> ();

        moduleCatalog.AddModule<Login.Module> ();
        moduleCatalog.AddModule<Main.Module> ();
        moduleCatalog.AddModule<Game.Module> ();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized ();
#if DEBUG
        HotReloadManager.Init (this);
#endif


        foreach (var name in Application.Current.Resources.MergedDictionaries)
        {
            Debug.WriteLine (name.Source);
        }
    }
}