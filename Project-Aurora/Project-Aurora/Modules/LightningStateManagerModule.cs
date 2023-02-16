﻿using System.Threading.Tasks;
using Aurora.Modules.GameStateListen;
using Aurora.Profiles;
using Aurora.Settings;
using Lombok.NET;

namespace Aurora.Modules;

public sealed partial class LightningStateManagerModule : IAuroraModule
{
    private readonly Task<PluginManager> _pluginManager;
    private readonly Task<IpcListener?> _ipcListener;
    private readonly Task<AuroraHttpListener?> _httpListener;

    private readonly TaskCompletionSource<LightingStateManager> _taskSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

    private LightingStateManager? _manager;

    public Task<LightingStateManager> LightningStateManager => _taskSource.Task;

    public LightningStateManagerModule(Task<PluginManager> pluginManager, Task<IpcListener?> ipcListener, Task<AuroraHttpListener?> httpListener)
    {
        _pluginManager = pluginManager;
        _ipcListener = ipcListener;
        _httpListener = httpListener;
    }

    [Async]
    public void Initialize()
    {
        Global.logger.Info("Loading Applications");
        var lightingStateManager = new LightingStateManager(_pluginManager, _ipcListener);
        _manager = lightingStateManager;
        Global.LightingStateManager = lightingStateManager;
        lightingStateManager.Initialize();
        
        _taskSource.SetResult(lightingStateManager);

        var ipcListener = _ipcListener.Result;
        if (ipcListener != null)
        {
            ipcListener.NewGameState += lightingStateManager.GameStateUpdate;
            ipcListener.WrapperConnectionClosed += lightingStateManager.ResetGameState;
        }

        var httpListener = _httpListener.Result;
        if (httpListener != null)
        {
            httpListener.NewGameState += lightingStateManager.GameStateUpdate;
        }
    }

    [Async]
    public void Dispose()
    {
        _manager?.Dispose();
        Global.LightingStateManager = null;
        _manager = null;
    }
}