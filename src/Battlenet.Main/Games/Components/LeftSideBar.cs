﻿using Battlenet.Main.Games.Models;
using Battlenet.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using FlexMVVM.WPF;
using FlexMVVM.WPF.Markup;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Battlenet.Main.Games.Components
{
    public partial class LeftSideBar : Component
    {
        private readonly ILayoutNavigator _layoutNavigator;
        private readonly BattlenetGameLoad _battlenetGameLoad;
        public Action<String> Title { get; set; }
        [ObservableProperty] private ObservableCollection<GameDataModel> gameDataModels;
        [ObservableProperty] ObservableCollection<GameDataModel> mobileGame;

        public LeftSideBar(ILayoutNavigator layoutNavigator,
                           BattlenetGameLoad battlenetGameLoad)
        {
            this._layoutNavigator = layoutNavigator;
            this._battlenetGameLoad = battlenetGameLoad;
            GameDataModels = new ();
            MobileGame = new ();
        }

        public void LoadGameData()
        {
            GameDataModels.Clear ();
            Task.Run (async () =>
            {
                var datas = await this._battlenetGameLoad.Load ();
                foreach (var data in datas)
                {
                    GameDataModels.Add (new GameDataModel (data));
                }
                MobileGame = new (GameDataModels.Where (x => x.IsMobile));
            });
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded (sender, e);

            this.LoadGameData ();
        }

        public LeftSideBar UpdateTile(Action<string> actions)
        {
            Title = actions;
            return this;
        }

        protected override Visual Build()
            => new Grid ()
                    .Columns ("*")
                    .Rows ("auto, auto, auto, auto, auto, auto, auto, auto, auto, auto")
                    .AddChild (new GameSearchBox (), row: 0)
                    .AddChild (new FlexDivider ()
                                  .Margin(topbottom:10)
                                  .Background ("#31363f"), row: 1)
                    .AddChild (TabItemTemplate ("My Games")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                    .OnCheckedAsync(async()=>
                                    {
                                        Title?.Invoke ("My Games");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/MyGames", GameDataModels);
                                    })
                                    .IsChecked (true), row: 2)
                    .AddChild (TabItemTemplate ("Installed")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                    .OnCheckedAsync (async () =>
                                    {
                                        Title?.Invoke ("Installed");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/Installed", GameDataModels);
                                    }), row: 3)
                    .AddChild (TabItemTemplate ("Favorites")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                     .OnCheckedAsync (async () =>
                                     {
                                         Title?.Invoke ("Favorites");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/Favorites", GameDataModels);
                                    }), row: 4)
                    .AddChild (new FlexDivider ()
                                  .Margin (topbottom: 10)
                                  .Background ("#31363f"), row: 5)
                    .AddChild (TabItemTemplate ("All Games")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                     .OnCheckedAsync (async () =>
                                     {
                                        Title?.Invoke ("All Games");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/AllGames", GameDataModels);

                                    }), row: 6)
                    .AddChild (TabItemTemplate ("Start For Free")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                     .OnCheckedAsync (async () =>
                                     {
                                         Title?.Invoke ("Start For Free");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/StartForFree", GameDataModels);
                                    }), row: 7)
                    .AddChild (TabItemTemplate ("Mobile")
                                    .Link (TabItem.CountProperty, "MobileGame.Count")
                                    .OnCheckedAsync (async () =>
                                    {
                                        Title?.Invoke ("Mobile");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/Mobile", MobileGame);
                                    }), row: 8)
                    .AddChild (TabItemTemplate ("MacOS")
                                    .Link (TabItem.CountProperty, "GameDataModels.Count")
                                     .OnCheckedAsync (async () =>
                                     {
                                         Title?.Invoke ("MacOS");
                                        await this._layoutNavigator.NavigateToAsync ($"{RouteNames.Games}/MacOS", GameDataModels);
                                    }), row: 9);

        private TabItem TabItemTemplate(string name)
            => new TabItem ()
                    .Content (
                        new Label()
                            .Foreground(Colors.White)
                            .FontSize(13)
                            .Content (name)
                    );
    }
}
