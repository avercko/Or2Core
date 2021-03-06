﻿using Microsoft.Extensions.Logging;
using Orchard.Environment.Shell.State;
using System.Linq;
using System.Threading.Tasks;
using YesSql.Core.Services;

namespace Orchard.Environment.Shell
{
    /// <summary>
    /// Stores <see cref="ShellState"/> in the database. 
    /// </summary>
    public class ShellStateManager : IShellStateManager
    {
        private ShellState _shellState;
        private readonly ISession _session;

        public ShellStateManager(ISession session, ILogger<ShellStateManager> logger)
        {
            _session = session;
            Logger = logger;
        }

        ILogger Logger { get; set; }

        public async Task<ShellState> GetShellStateAsync()
        {
            if (_shellState != null)
            {
                return _shellState;
            }

            _shellState = await _session.QueryAsync<ShellState>().FirstOrDefault();

            if (_shellState == null)
            {
                _shellState = new ShellState();
                UpdateShellState();
            }

            return _shellState;
        }

        public async Task UpdateEnabledStateAsync(ShellFeatureState featureState, ShellFeatureState.State value)
        {
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug("Feature {0} EnableState changed from {1} to {2}",
                             featureState.Id, featureState.EnableState, value);
            }

            var previousFeatureState = await GetOrCreateFeatureStateAsync(featureState.Id);
            if (previousFeatureState.EnableState != featureState.EnableState)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.LogWarning("Feature {0} prior EnableState was {1} when {2} was expected",
                               featureState.Id, previousFeatureState.EnableState, featureState.EnableState);
                }
            }

            previousFeatureState.EnableState = value;
            featureState.EnableState = value;

            UpdateShellState();
        }

        public async Task UpdateInstalledStateAsync(ShellFeatureState featureState, ShellFeatureState.State value)
        {
            if (Logger.IsEnabled(LogLevel.Debug))
            {
                Logger.LogDebug("Feature {0} InstallState changed from {1} to {2}", featureState.Id, featureState.InstallState, value);
            }

            var previousFeatureState = await GetOrCreateFeatureStateAsync(featureState.Id);
            if (previousFeatureState.InstallState != featureState.InstallState)
            {
                if (Logger.IsEnabled(LogLevel.Warning))
                {
                    Logger.LogWarning("Feature {0} prior InstallState was {1} when {2} was expected",
                               featureState.Id, previousFeatureState.InstallState, featureState.InstallState);
                }
            }

            previousFeatureState.InstallState = value;
            featureState.InstallState = value;

            UpdateShellState();
        }

        private async Task<ShellFeatureState> GetOrCreateFeatureStateAsync(string id)
        {
            var shellState = await GetShellStateAsync();
            var featureState = shellState.Features.FirstOrDefault(x => x.Id == id);

            if (featureState == null)
            {
                featureState = new ShellFeatureState() { Id = id };
                _shellState.Features.Add(featureState);
            }

            return featureState;
        }

        private void UpdateShellState()
        {
            _session.Save(_shellState);
        }
    }
}
