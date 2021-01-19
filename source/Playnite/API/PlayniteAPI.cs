﻿using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using Playnite.Settings;
using Playnite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Playnite.API
{
    public class PlayniteAPI : IPlayniteAPI
    {
        private readonly GamesEditor gameEditor;

        public PlayniteAPI(
            IGameDatabaseAPI databaseApi,
            IDialogsFactory dialogs,
            IMainViewAPI mainViewApi,
            IPlayniteInfoAPI infoApi,
            IPlaynitePathsAPI pathsApi,
            IWebViewFactory webViewFactory,
            IResourceProvider resources,
            INotificationsAPI notifications,
            GamesEditor gameEditor,
            IUriHandlerAPI uriHandler,
            IPlayniteSettingsAPI settingsApi,
            IAddons addonsApi)
        {
            WebViews = webViewFactory;
            Paths = pathsApi;
            ApplicationInfo = infoApi;
            MainView = mainViewApi;
            Dialogs = dialogs;
            Database = databaseApi;
            Resources = resources;
            Notifications = notifications;
            this.gameEditor = gameEditor;
            UriHandler = uriHandler;
            ApplicationSettings = settingsApi;
            Addons = addonsApi;
        }

        public IDialogsFactory Dialogs { get; }

        public IGameDatabaseAPI Database { get; }

        public IMainViewAPI MainView { get; set; }

        public IPlaynitePathsAPI Paths { get; }

        public IPlayniteInfoAPI ApplicationInfo { get; }

        public IWebViewFactory WebViews { get; }

        public IResourceProvider Resources { get; }

        public INotificationsAPI Notifications { get; }

        public IUriHandlerAPI UriHandler { get; }

        public IPlayniteSettingsAPI ApplicationSettings { get; }

        public IAddons Addons { get; }

        public string ExpandGameVariables(Game game, string inputString)
        {
            return game?.ExpandVariables(inputString);
        }

        public GameAction ExpandGameVariables(Game game, GameAction action)
        {
            return action?.ExpandVariables(game);
        }

        [Obsolete("Use LogManager class instead.")]
        public ILogger CreateLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        [Obsolete("Use LogManager class instead.")]
        public ILogger CreateLogger()
        {
            var className = (new StackFrame(1)).GetMethod().DeclaringType.Name;
            return CreateLogger(className);
        }

        public void StartGame(Guid gameId)
        {
            var game = Database.Games.Get(gameId);
            if (game == null)
            {
                throw new Exception($"Can't start game, game ID {gameId} not found.");
            }
            else
            {
                gameEditor.PlayGame(game);
            }
        }
    }
}