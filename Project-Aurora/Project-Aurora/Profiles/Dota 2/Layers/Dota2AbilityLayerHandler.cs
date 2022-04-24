﻿using Aurora.EffectsEngine;
using Aurora.Profiles.Dota_2.GSI;
using Aurora.Profiles.Dota_2.GSI.Nodes;
using Aurora.Settings;
using Aurora.Settings.Layers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Aurora.Profiles.Dota_2.Layers
{
    public class Dota2AbilityLayerHandlerProperties : LayerHandlerProperties2Color<Dota2AbilityLayerHandlerProperties>
    {
        public Color? _CanCastAbilityColor { get; set; }

        [JsonIgnore]
        public Color CanCastAbilityColor { get { return Logic._CanCastAbilityColor ?? _CanCastAbilityColor ?? Color.Empty; } }

        public Color? _CanNotCastAbilityColor { get; set; }

        [JsonIgnore]
        public Color CanNotCastAbilityColor { get { return Logic._CanNotCastAbilityColor ?? _CanNotCastAbilityColor ?? Color.Empty; } }

        public List<Devices.DeviceKeys> _AbilityKeys { get; set; }

        [JsonIgnore]
        public List<Devices.DeviceKeys> AbilityKeys { get { return Logic._AbilityKeys ?? _AbilityKeys ?? new List<Devices.DeviceKeys>(); } }

        public Dota2AbilityLayerHandlerProperties() : base() { }

        public Dota2AbilityLayerHandlerProperties(bool assign_default = false) : base(assign_default) { }

        public override void Default()
        {
            base.Default();

            this._CanCastAbilityColor = Color.FromArgb(0, 255, 0);
            this._CanNotCastAbilityColor = Color.FromArgb(255, 0, 0);
            this._AbilityKeys = new List<Devices.DeviceKeys>() { Devices.DeviceKeys.Q, Devices.DeviceKeys.W, Devices.DeviceKeys.E, Devices.DeviceKeys.D, Devices.DeviceKeys.F, Devices.DeviceKeys.R };
        }

    }

    public class Dota2AbilityLayerHandler : LayerHandler<Dota2AbilityLayerHandlerProperties>
    {
        protected override UserControl CreateControl()
        {
            return new Control_Dota2AbilityLayer(this);
        }

        private List<string> ignoredAbilities = new() { "seasonal", "high_five" };
        private readonly EffectLayer _abilitiesLayer = new("Dota 2 - Abilities");

        public override EffectLayer Render(IGameState state)
        {
            if (state is GameState_Dota2)
            {
                GameState_Dota2 dota2state = state as GameState_Dota2;

                if (dota2state.Map.GameState == DOTA_GameState.DOTA_GAMERULES_STATE_PRE_GAME ||
                    dota2state.Map.GameState == DOTA_GameState.DOTA_GAMERULES_STATE_GAME_IN_PROGRESS)
                {
                    for (int index = 0; index < dota2state.Abilities.Count; index++)
                    {
                        Ability ability = dota2state.Abilities[index];
                        if (ignoredAbilities.Any(ignoredAbilityName => ability.Name.Contains(ignoredAbilityName)))
                            continue;

                        if(index < Properties.AbilityKeys.Count)
                        {
                            Devices.DeviceKeys key = Properties.AbilityKeys[index];

                            if (ability.CanCast && ability.Cooldown == 0 && ability.Level > 0)
                                _abilitiesLayer.Set(key, Properties.CanCastAbilityColor);
                            else if (ability.Cooldown <= 5 && ability.Level > 0)
                                _abilitiesLayer.Set(key, Utils.ColorUtils.BlendColors(Properties.CanCastAbilityColor, Properties.CanNotCastAbilityColor, (double)ability.Cooldown / 5.0));
                            else
                                _abilitiesLayer.Set(key, Properties.CanNotCastAbilityColor);
                        }
                    }
                }
                else
                {
                    _abilitiesLayer.Clear();
                }
            }

            return _abilitiesLayer;
        }

        public override void SetApplication(Application profile)
        {
            (Control as Control_Dota2AbilityLayer).SetProfile(profile);
            base.SetApplication(profile);
        }
    }
}
