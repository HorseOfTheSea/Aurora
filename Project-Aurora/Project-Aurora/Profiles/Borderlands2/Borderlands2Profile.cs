using Aurora.EffectsEngine;
using Aurora.Settings;
using Aurora.Settings.Layers;
using Aurora.Settings.Overrides.Logic;
using Aurora.Settings.Overrides.Logic.Builder;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Aurora.Profiles.Borderlands2
{
    public class Borderlands2Profile : ApplicationProfile
    {
        public Borderlands2Profile() : base()
        {
            
        }

        public override void Reset()
        {
            base.Reset();
            Layers = new System.Collections.ObjectModel.ObservableCollection<Layer>()
            {
                new Layer("Health Indicator", new PercentLayerHandler()
                {
                    Properties = new PercentLayerHandlerProperties()
                    {
                        _PrimaryColor = Color.Red,
                        _SecondaryColor = Color.DarkRed,
                        _PercentType = PercentEffectType.Progressive_Gradual,
                        _Sequence = new KeySequence(new Devices.DeviceKeys[] {
                            Devices.DeviceKeys.ONE, Devices.DeviceKeys.TWO, Devices.DeviceKeys.THREE, Devices.DeviceKeys.FOUR, Devices.DeviceKeys.FIVE,
                            Devices.DeviceKeys.SIX, Devices.DeviceKeys.SEVEN, Devices.DeviceKeys.EIGHT, Devices.DeviceKeys.NINE, Devices.DeviceKeys.ZERO,
                            Devices.DeviceKeys.MINUS, Devices.DeviceKeys.EQUALS
                        }),
                        _BlinkThreshold = 0.0,
                        _BlinkDirection = false,
                        _VariablePath = "Player/CurrentHealth",
                        _MaxVariablePath = "Player/MaximumHealth"
                    },
                }),
                new Layer("Shield Indicator", new PercentLayerHandler()
                {
                    Properties = new PercentLayerHandlerProperties()
                    {
                        _PrimaryColor =  Color.Cyan,
                        _SecondaryColor = Color.DarkCyan,
                        _PercentType = PercentEffectType.Progressive_Gradual,
                        _Sequence = new KeySequence(new Devices.DeviceKeys[] {
                            Devices.DeviceKeys.F1, Devices.DeviceKeys.F2, Devices.DeviceKeys.F3, Devices.DeviceKeys.F4,
                            Devices.DeviceKeys.F5, Devices.DeviceKeys.F6, Devices.DeviceKeys.F7, Devices.DeviceKeys.F8,
                            Devices.DeviceKeys.F9, Devices.DeviceKeys.F10, Devices.DeviceKeys.F11, Devices.DeviceKeys.F12
                        }),
                        _BlinkThreshold = 0.0,
                        _BlinkDirection = false,
                        _VariablePath = "Player/CurrentShield",
                        _MaxVariablePath = "Player/MaximumShield"
                    },
                }),
                new Layer("Special Active", new BreathingLayerHandler(){
                    Properties = new BreathingLayerHandlerProperties()
                    {
                        _Sequence = new KeySequence(new Devices.DeviceKeys[] {
                            Devices.DeviceKeys.ESC, Devices.DeviceKeys.TILDE, Devices.DeviceKeys.TAB, Devices.DeviceKeys.CAPS_LOCK,
                            Devices.DeviceKeys.LEFT_SHIFT, Devices.DeviceKeys.LEFT_CONTROL, Devices.DeviceKeys.LEFT_WINDOWS, Devices.DeviceKeys.LEFT_ALT,
                            Devices.DeviceKeys.Z, Devices.DeviceKeys.SPACE, Devices.DeviceKeys.RIGHT_ALT, Devices.DeviceKeys.RIGHT_WINDOWS,
                            Devices.DeviceKeys.APPLICATION_SELECT, Devices.DeviceKeys.RIGHT_CONTROL, Devices.DeviceKeys.RIGHT_SHIFT, Devices.DeviceKeys.ENTER,
                            Devices.DeviceKeys.FORWARD_SLASH, Devices.DeviceKeys.BACKSLASH, Devices.DeviceKeys.BACKSPACE, Devices.DeviceKeys.ARROW_DOWN,
                            Devices.DeviceKeys.ARROW_UP, Devices.DeviceKeys.ARROW_LEFT, Devices.DeviceKeys.ARROW_RIGHT, Devices.DeviceKeys.NUM_ZERO,
                            Devices.DeviceKeys.NUM_PERIOD, Devices.DeviceKeys.NUM_THREE, Devices.DeviceKeys.NUM_ENTER, Devices.DeviceKeys.NUM_PLUS,
                            Devices.DeviceKeys.NUM_MINUS,
                        }),
                        _PrimaryColor = Color.FromArgb(255, 255, 0),
                        _SecondaryColor = Color.FromArgb(0, 0, 0),
                        _EffectSpeed = 3.0f,
                    },
                }),
                new Layer("Special Indicator", new PercentLayerHandler(){
                    Properties = new PercentLayerHandlerProperties()
                    {
                        _PercentType = PercentEffectType.Progressive_Gradual,
                        _Sequence = new KeySequence(new FreeFormObject(646.37f, 41.39f, 195f, 148.65f, -90)),
                        _PrimaryColor = Color.FromArgb(90, 255, 0),
                        _SecondaryColor = Color.FromArgb(25, 75, 0),
                        _BlinkThreshold = 0.0,
                        _BlinkDirection = false,
                        _VariablePath = "Player/CurrentSpecial",
                        _MaxVariablePath = "100",
                    },
                }),
                new Layer("Borderlands 2 Background", new SolidFillLayerHandler(){
                    Properties = new SolidFillLayerHandlerProperties()
                    {
                        _PrimaryColor = Color.LightGoldenrodYellow
                    }
                })
            };
        }
    }
}
