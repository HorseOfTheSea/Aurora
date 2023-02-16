
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using Aurora.Utils;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;
using Aurora.Devices.AtmoOrb;
using Aurora.Devices.OpenRGB;
using Aurora.Devices.RGBNet;
using Aurora.Modules.AudioCapture;
using Aurora.Profiles;
using Aurora.Settings.Overrides.Logic;
using Define;
using static Aurora.Modules.HardwareMonitor.HardwareMonitor;

namespace Aurora.Settings
{
    /// <summary>
    /// Enum list for the percent effect type
    /// </summary>
    public enum PercentEffectType
    {
        /// <summary>
        /// All at once
        /// </summary>
        [Description("All at once")]
        AllAtOnce = 0,

        /// <summary>
        /// Progressive
        /// </summary>
        [Description("Progressive")]
        Progressive = 1,

        /// <summary>
        /// Progressive (Gradual)
        /// </summary>
        [Description("Progressive (Gradual)")]
        Progressive_Gradual = 2,

        [Description("Only highest active key (foreground color)")]
        Highest_Key = 3,

        [Description("Only highest active key (blended color)")]
        Highest_Key_Blend = 4
    }

    public enum IdleEffects
    {
        [Description("None")]
        None = 0,
        [Description("Dim")]
        Dim = 1,
        [Description("Color Breathing")]
        ColorBreathing = 2,
        [Description("Rainbow Shift (Horizontal)")]
        RainbowShift_Horizontal = 3,
        [Description("Rainbow Shift (Vertical)")]
        RainbowShift_Vertical = 4,
        [Description("Star Fall")]
        StarFall = 5,
        [Description("Rain Fall")]
        RainFall = 6,
        [Description("Blackout")]
        Blackout = 7,
        [Description("Matrix")]
        Matrix = 8,
        [Description("Rain Fall Smooth")]
        RainFallSmooth = 9
    }

    /// <summary>
    /// Enum list for the layer effects
    /// </summary>
    public enum LayerEffects
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Single Color Overlay
        /// </summary>
        [Description("Single Color Overlay")]
        ColorOverlay = 1,

        /// <summary>
        /// Color Breathing
        /// </summary>
        [Description("Color Breathing")]
        ColorBreathing = 2,

        /// <summary>
        /// Rainbow Shift (Horizontal)
        /// </summary>
        [Description("Rainbow Shift (Horizontal)")]
        RainbowShift_Horizontal = 3,

        /// <summary>
        /// Rainbow Shift (Vertical)
        /// </summary>
        [Description("Rainbow Shift (Vertical)")]
        RainbowShift_Vertical = 4,

        /// <summary>
        /// Rainbow Shift (Diagonal)
        /// </summary>
        [Description("Rainbow Shift (Diagonal)")]
        RainbowShift_Diagonal = 5,

        /// <summary>
        /// Rainbow Shift (Other Diagonal)
        /// </summary>
        [Description("Rainbow Shift (Other Diagonal)")]
        RainbowShift_Diagonal_Other = 6,

        /// <summary>
        /// Rainbow Shift (Custom Angle)
        /// </summary>
        [Description("Rainbow Shift (Custom Angle)")]
        RainbowShift_Custom_Angle = 7,

        /// <summary>
        /// Gradient Shift (Custom Angle)
        /// </summary>
        [Description("Gradient Shift (Custom Angle)")]
        GradientShift_Custom_Angle = 8,
    }

    public enum AppExitMode
    {
        [Description("Always Ask")]
        Ask = 0,
        [Description("Always Minimize")]
        Minimize = 1,
        [Description("Always Exit")]
        Exit = 2
    }

    public enum MouseOrientationType
    {
        [Description("Right Handed")]
        RightHanded = 1,
        [Description("Left Handed")]
        LeftHanded = 2
    }

    public enum PreferredKeyboard
    {
        [Description("None")]
        None = 0,

        [Description("Generic Laptop")]
        GenericLaptop = 1,

        [Description("Generic Laptop (Numpad)")]
        GenericLaptopNumpad = 2,

        [Description("Additional Lights (Debug)")]
        Debug = 3,

        [Description("Custom Json")]
        Custom = 4,

        /*
        [Description("Logitech")]
        Logitech = 1,
        [Description("Corsair")]
        Corsair = 2,
        [Description("Razer")]
        Razer = 3,

        [Description("Clevo")]
        Clevo = 4,
        [Description("Cooler Master")]
        CoolerMaster = 5,
        */

        //Logitech range is 100-199
        [Description("Logitech - G910")]
        Logitech_G910 = 100,
        [Description("Logitech - G410")]
        Logitech_G410 = 101,
        [Description("Logitech - G810")]
        Logitech_G810 = 102,
        [Description("Logitech - GPRO")]
        Logitech_GPRO = 103,
        [Description("Logitech - G213 (OpenRGB)")]
        Logitech_G213 = 104,
		[Description("Logitech - G815")]
        Logitech_G815 = 105,
        [Description("Logitech - G513")]
        Logitech_G513 = 106,
        [Description("Logitech - G915")]
        Logitech_G915 = 107,
		
        //Corsair range is 200-299
        [Description("Corsair - K95")]
        Corsair_K95 = 200,
        [Description("Corsair - K70")]
        Corsair_K70 = 201,
        [Description("Corsair - K65")]
        Corsair_K65 = 202,
        [Description("Corsair - STRAFE")]
        Corsair_STRAFE = 203,
        [Description("Corsair - K95 Platinum")]
        Corsair_K95_PL = 204,
        [Description("Corsair - K68")]
        Corsair_K68 = 205,
        [Description("Corsair - K70 MK2")]
        Corsair_K70MK2 = 206,
        [Description("Corsair - STRAFE MK2")]
        Corsair_STRAFE_MK2 = 207,
        [Description("Corsair - K100")]
        Corsair_K100 = 208,

        //Razer range is 300-399
        [Description("Razer - Blackwidow")]
        Razer_Blackwidow = 300,
        [Description("Razer - Blackwidow X")]
        Razer_Blackwidow_X = 301,
        [Description("Razer - Blackwidow Tournament Edition")]
        Razer_Blackwidow_TE = 302,
        [Description("Razer - Blade")]
        Razer_Blade = 303,

        //Clevo range is 400-499
        [Description("Clevo 3 Zone Keyboard")]
        Clevo_3_Zone = 400,

        //Cooler Master range is 500-599
        [Description("Cooler Master - Masterkeys Pro L")]
        Masterkeys_Pro_L = 500,
        [Description("Cooler Master - Masterkeys Pro S")]
        Masterkeys_Pro_S = 501,
        [Description("Cooler Master - Masterkeys MK750")]
        Masterkeys_MK750 = 503,
        [Description("Cooler Master - Masterkeys MK730")]
        Masterkeys_MK730 = 504,
        [Description("Cooler Master - SK650")]
        Cooler_Master_SK650 = 505,

        //Roccat range is 600-699
        [Description("Roccat Ryos")]
        Roccat_Ryos = 600,
        [Description("Roccat Vulcan TKL")]
        Roccat_Vualcan_TKL = 601,

        //Steelseries range is 700-799
        [Description("SteelSeries Apex M800")]
        SteelSeries_Apex_M800 = 700,
        [Description("SteelSeries Apex M750")]
        SteelSeries_Apex_M750 = 701,
        [Description("SteelSeries Apex M750 TKL")]
        SteelSeries_Apex_M750_TKL = 702,

        [Description("Wooting One")]
        Wooting_One = 800,
        [Description("Wooting Two")]
        Wooting_Two = 801,

        [Description("Asus Strix Flare")]
        Asus_Strix_Flare = 900,
        [Description("Asus Strix Scope")]
        Asus_Strix_Scope = 901,

        //Drevo range is 1000-1099
        [Description("Drevo BladeMaster")]
        Drevo_BladeMaster = 1000,

        //Creative range is 1100-1199
        [Description("SoundBlasterX VanguardK08")]
        SoundBlasterX_Vanguard_K08 = 1100,

 

        [Description("UNIWILL2ND (ANSI)")]
        Uniwill2ND_35X_1 = 2101,
        [Description("UNIWILL2ND (ISO)")]
        Uniwill2ND_35X_2 = 2102,

        [Description("UNIWILL2P1 (ISO)")]
        Uniwill2P1_550_UK = 2103,
        [Description("UNIWILL2P1 (ANSI)")]
        Uniwill2P1_550_US = 2104,
        [Description("UNIWILL2P1 (ABNT)")]
        Uniwill2P1_550_BR = 2105,
        [Description("UNIWILL2P1 (JIS)")]
        Uniwill2P1_550_JP = 2106,


        [Description("UNIWILL2P2 (ISO)")]
        Uniwill2P2_650_UK = 2107,
        [Description("UNIWILL2P2 (ANSI)")]
        Uniwill2P2_650_US = 2108,
        [Description("UNIWILL2P2 (ABNT)")]
        Uniwill2P2_650_BR = 2109,
        [Description("UNIWILL2P2 (JIS)")]
        Uniwill2P2_650_JP = 2110,

 
        //Ducky range is 1200-1299
        [Description("Ducky Shine 7/One 2 RGB")]
        Ducky_Shine_7 = 1200,
        [Description("Ducky One 2 RGB TKL")]
        Ducky_One_2_RGB_TKL = 1201,

        //OMEN range 1300-1399
        [Description("OMEN Sequencer")]
        OMEN_Sequencer = 1300,
        [Description("OMEN Four Zone")]
        OMEN_Four_Zone = 1301,

        //HyperX range is 1400-1499
        [Description("HyperX Alloy Elite RGB")]
        HyperX_Alloy_Elite_RGB = 1400,
        //Keychron q1 knob
        [Description("Keychron q1 knob ansi 75%")]
        Keychron_Q1_Knob_Ansi = 1107,
 
        //MSI range is 1500-1599
        [Description("MSI GP66 US")]
        MSI_GP66_US = 1500,

        //Bloody range is 1600-1699
        [Description("Bloody full")]
        Bloody_full = 1600,


    }

    public enum PreferredKeyboardLocalization
    {
        [Description("Automatic Detection")]
        None = 0,
        [Description("International")]
        intl = 1,
        [Description("United States")]
        us = 2,
        [Description("United Kingdom")]
        uk = 3,
        [Description("Russian")]
        ru = 4,
        [Description("French")]
        fr = 5,
        [Description("Deutsch")]
        de = 6,
        [Description("Japanese")]
        jpn = 7,
        [Description("Turkish")]
        tr = 8,
        [Description("Nordic")]
        nordic = 9,
        [Description("Swiss")]
        swiss = 10,
        [Description("Portuguese (Brazilian ABNT2)")]
        abnt2 = 11,
        [Description("DVORAK (US)")]
        dvorak = 12,
        [Description("DVORAK (INT)")]
        dvorak_int = 13,
        [Description("Hungarian")]
        hu = 14,
        [Description("Italian")]
        it = 15,
        [Description("Latin America")]
        la = 16,
        [Description("Spanish")]
        es = 17,
        [Description("ISO - Automatic (Experimental)")]
        iso = 18,
        [Description("ANSI - Automatic (Experimental)")]
        ansi = 19,
    }

    public enum PreferredMouse
    {
        [Description("None")]
        None = 0,

        [Description("Generic Peripheral")]
        Generic_Peripheral = 1,
        
        [Description("Generic Openrgb Mouse")]
        Generic_Openrgb_Mouse = 2,
        
        [Description("Custom Mouse")]
        Custom = 3,

        //Logitech range is 100-199
        [Description("Logitech - G900")]
        Logitech_G900 = 100,
        [Description("Logitech - G502")]
        Logitech_G502 = 101,
        [Description("Logitech - G102/G203")]
        Logitech_G102 = 102,

        //Corsair range is 200-299
        [Description("Corsair - Sabre")]
        Corsair_Sabre = 200,
        [Description("Corsair - M65")]
        Corsair_M65 = 201,
        [Description("Corsair - Katar")]
        Corsair_Katar = 202,

        //Razer range is 300-399
        [Description("Razer - Mamba TE")]
        Razer_Mamba_TE = 300,
        [Description("Razer - Naga Pro")]
        Razer_naga_Pro = 301,

        //Clevo range is 400-499
        [Description("Clevo - Touchpad")]
        Clevo_Touchpad = 400,

        //Cooler Master range is 500-599

        //Roccat range is 600-699
        [Description("Roccat - Kone Pure")]
        Roccat_Kone_Pure = 600,

        //Steelseries range is 700-799
        [Description("SteelSeries - Rival 300")]
        SteelSeries_Rival_300 = 700,
        [Description("SteelSeries - Rival 300 HP OMEN Edition")]
        SteelSeries_Rival_300_HP_OMEN_Edition = 701,
        
        [Description("Bloody - W60")]
        Bloody_W60 = 800,
        
        //Asus range is 900-999
        [Description("Asus - Pugio")]
        Asus_Pugio = 900,

        //OMEN range is 1000-1099
        [Description("OMEN Photon")]
        OMEN_Photon = 1000,
        [Description("OMEN Outpost + Photon")]
        OMEN_Outpost_Plus_Photon = 1001,
        [Description("OMEN Vector")]
        OMEN_Vector = 1002,
        [Description("OMEN Vector Essentials")]
        OMEN_Vector_Essentials = 1003,
    }

    public enum PreferredMousepad
    {
        [Description("None")]
        None = 0,

        [Description("Razer/Corsair Mousepad")]
        Generic_Mousepad = 1,
        
        [Description("2 Zone + Logo Mousepad")]
        Two_Zone_Plus_Logo_Mousepad = 2,
        
        [Description("Custom Mouse")]
        Custom = 3,

        //Steelseries range is 100-299
        [Description("SteelSeries - QcK Prism Mousepad")]
        SteelSeries_QcK_Prism = 100,
        [Description("SteelSeries - Two-zone QcK Mousepad")]
        SteelSeries_QcK_2_Zone = 101,
        [Description("Bloody - MP-50RS")]
        Bloody_MP50RS = 102,
    }


    public enum KeycapType
    {
        [Description("Default")]
        Default = 0,
        [Description("Default (with Backglow)")]
        Default_backglow = 1,
        [Description("Default (Backglow only)")]
        Default_backglow_only = 2,
        [Description("Colorized")]
        Colorized = 3,
        [Description("Colorized (blank)")]
        Colorized_blank = 4
    }

    public enum ApplicationDetectionMode
    {
        [Description("Windows Events (Default)")]
        WindowsEvents = 0,

        [Description("Foreground App Scan")]
        ForegroroundApp = 1
    }

    public enum BitmapAccuracy
    {
        Best = 1,
        Great = 3,
        Good = 6,
        Okay = 9,
        Fine = 12
    }

    public class Configuration : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //General Program Settings
        [JsonProperty("allow_peripheral_devices")] public bool AllowPeripheralDevices { get; set; } = true;
        [JsonProperty("close_on_exception")] public bool CloseProgramOnException { get; set; } = true;
        [JsonProperty("allow_wrappers_in_background")] public bool AllowWrappersInBackground { get; set; } = true;
        [JsonProperty("allow_all_logitech_bitmaps")] public bool AllowAllLogitechBitmaps { get; set; } = true;

        [JsonProperty("use_volume_as_brightness")] public bool UseVolumeAsBrightness { get; set; }
        [JsonProperty("global_brightness")] public float GlobalBrightness { get; set; } = 1.0f;
        [JsonProperty("keyboard_brightness_modifier")] public float KeyboardBrightness { get; set; } = 1.0f;
        [JsonProperty("peripheral_brightness_modifier")] public float PeripheralBrightness { get; set; } = 1.0f;

        public bool GetDevReleases { get; set; } = false;
        public bool GetPointerUpdates { get; set; } = true;
        public bool HighPriority { get; set; } = false;
        public BitmapAccuracy BitmapAccuracy { get; set; } = BitmapAccuracy.Okay;
        public bool EnableAudioCapture { get; set; } = false;
        public bool EnableMediaInfo { get; set; } = true;
        public bool EnableInputCapture { get; set; } = true;
        public bool EnableHttpListener { get; set; } = true;
        public bool EnableIpcListener { get; set; } = true;
        public bool EnableHardwareInfo { get; set; } = true;

        public int UpdateDelay { get; set; } = 30;

        public double Fps => 1000d / UpdateDelay;

        [JsonProperty("updates_check_on_start_up")] public bool UpdatesCheckOnStartUp { get; set; } = true;
        [JsonProperty("start_silently")] public bool StartSilently { get; set; }
        [JsonProperty("close_mode")] public AppExitMode CloseMode { get; set; } = AppExitMode.Ask;
        [JsonProperty("mouse_orientation")] public MouseOrientationType MouseOrientation { get; set; } = MouseOrientationType.RightHanded;
        [JsonProperty("keyboard_brand")] public PreferredKeyboard KeyboardBrand { get; set; } = PreferredKeyboard.GenericLaptopNumpad;
        [JsonProperty("keyboard_localization")] public PreferredKeyboardLocalization KeyboardLocalization { get; set; } = PreferredKeyboardLocalization.None;
        [JsonProperty("mouse_preference")] public PreferredMouse MousePreference { get; set; } = PreferredMouse.Generic_Peripheral;
        [JsonProperty("mousepad_preference")] public PreferredMousepad MousepadPreference { get; set; } = PreferredMousepad.None;
        [JsonProperty("virtualkeyboard_keycap_type")] public KeycapType VirtualkeyboardKeycapType { get; set; } = KeycapType.Default;
        [JsonProperty("detection_mode")] public ApplicationDetectionMode DetectionMode { get; set; } = ApplicationDetectionMode.WindowsEvents;
        [JsonProperty("devices_disable_keyboard")] public bool DevicesDisableKeyboard { get; set; }
        [JsonProperty("devices_disable_mouse")] public bool DevicesDisableMouse { get; set; }
        [JsonProperty("devices_disable_headset")] public bool DevicesDisableHeadset { get; set; }
        [JsonProperty("unified_hid_disabled")] public bool UnifiedHidDisabled { get; set; } = true;
        public bool OverlaysInPreview { get; set; } = true;

        public ObservableCollection<string> ExcludedPrograms { get; set; } = new();

        public ObservableCollection<Type> DevicesDisabled { get; set; } = new()
        {
            typeof(AsusDevice),
            typeof(AtmoOrbDevice),
            typeof(Devices.Bloody.BloodyDevice),
            typeof(Devices.Clevo.ClevoDevice),
            typeof(CorsairRgbNetDevice),
            typeof(Devices.Drevo.DrevoDevice),
            typeof(Devices.Ducky.DuckyDevice),
            typeof(RgbNetDevice),
            typeof(Devices.Razer.RazerDevice),
            typeof(Devices.Roccat.RoccatDevice),
            //typeof(Devices.RGBNet.BloodyRgbNetDevice),
            typeof(CorsairRgbNetDevice),
            typeof(LogitechRgbNetDevice),
            typeof(SteelSeriesRgbNetDevice),
            typeof(Devices.Omen.OmenDevices),
            typeof(Devices.Dualshock.DualshockDevice),
            typeof(Devices.UnifiedHID.UnifiedHIDDevice),
            typeof(Devices.Uniwill.UniwillDevice),
            typeof(Devices.Vulcan.VulcanDevice),
            typeof(Devices.Wooting.WootingDevice),
            typeof(Devices.YeeLight.YeeLightDevice),
            typeof(Devices.Creative.SoundBlasterXDevice),
        };

        public List<BitmapAccuracy> BitmapAccuracies { get; } = new List<BitmapAccuracy>()
        {
            BitmapAccuracy.Good,
            BitmapAccuracy.Okay,
            BitmapAccuracy.Fine
        };

        //Blackout and Night theme
        [JsonProperty("time_based_dimming_enabled")] public bool TimeBasedDimmingEnabled { get; set; }
        [JsonProperty("time_based_dimming_affect_games")] public bool TimeBasedDimmingAffectGames { get; set; }
        [JsonProperty("time_based_dimming_start_hour")] public int TimeBasedDimmingStartHour { get; set; } = 21;
        [JsonProperty("time_based_dimming_start_minute")] public int TimeBasedDimmingStartMinute { get; set; }
        [JsonProperty("time_based_dimming_end_hour")] public int TimeBasedDimmingEndHour { get; set; } = 8;
        [JsonProperty("time_based_dimming_end_minute")] public int TimeBasedDimmingEndMinute { get; set; }

        [JsonProperty("nighttime_enabled")] public bool NighttimeEnabled { get; set; } = false;
        [JsonProperty("nighttime_start_hour")] public int NighttimeStartHour { get; set; } = 20;
        [JsonProperty("nighttime_start_minute")] public int NighttimeStartMinute { get; set; } = 0;
        [JsonProperty("nighttime_end_hour")] public int NighttimeEndHour { get; set; } = 7;
        [JsonProperty("nighttime_end_minute")] public int NighttimeEndMinute { get; set; } = 0;

        // Idle Effects
        [JsonProperty("idle_type")] public IdleEffects IdleType { get; set; } = IdleEffects.None;
        [JsonProperty("idle_delay")] public int IdleDelay { get; set; } = 5;
        [JsonProperty("idle_speed")] public float IdleSpeed { get; set; } = 1.0f;
        [JsonProperty("idle_effect_primary_color")] public Color IdleEffectPrimaryColor { get; set; } = Color.Lime;
        [JsonProperty("idle_effect_secondary_color")] public Color IdleEffectSecondaryColor { get; set; } = Color.Black;
        [JsonProperty("idle_amount")] public int IdleAmount { get; set; } = 5;
        [JsonProperty("idle_frequency")] public float IdleFrequency { get; set; } = 2.5f;

        //Hardware Monitor
        public int HardwareMonitorUpdateRate { get; set; } = 500;
        public int HardwareMonitorMaxQueue { get; set; } = 8;
        public bool HardwareMonitorUseAverageValues { get; set; } = true;
        public int HardwareMonitorCPUTemperature { get; set; } = 0;
        public int HardwareMonitorCPULoad { get; set; } = 0;

        public VariableRegistry VarRegistry { get; set; } = new VariableRegistry();

        public double Width { get; set; } = 656;
        public double Height { get; set; } = 800;
        public double Top { get; set; } = 50;
        public double Left { get; set; } = 50;
        public WindowState WindowState { get; set; } = WindowState.Normal;

        //BitmapDebug Data
        public bool BitmapDebugTopMost { get; set; } = false;
        public WINDOWPLACEMENT BitmapPlacement { get; set; }
        public bool BitmapWindowOnStartUp { get; set; } = false;

        //httpDebug Data
        public bool HttpDebugTopMost { get; set; } = false;
        public WINDOWPLACEMENT HttpDebugPlacement { get; set; }
        public bool HttpWindowOnStartUp { get; set; } = false;

        public ObservableConcurrentDictionary<string, IEvaluatable> EvaluatableTemplates { get; set; } = new ObservableConcurrentDictionary<string, IEvaluatable>();

        public List<string> ProfileOrder { get; set; } = new List<string>();

        public Dictionary<string, Color> DeviceCalibrations { get; set; } = new();

        [JsonProperty("GSIAudioRenderDevice", NullValueHandling = NullValueHandling.Ignore)]
        public string GsiAudioRenderDevice { get; set; } = AudioDevices.DefaultDeviceId;
        [JsonProperty("GSIAudioCaptureDevice", NullValueHandling = NullValueHandling.Ignore)]
        public string GsiAudioCaptureDevice { get; set; } = AudioDevices.DefaultDeviceId;

        /// <summary>
        /// Called after the configuration file has been deserialized or created for the first time.
        /// </summary>
        public void OnPostLoad() {
            if (!UnifiedHidDisabled) {
                DevicesDisabled.Add(typeof(Devices.UnifiedHID.UnifiedHIDDevice));
                UnifiedHidDisabled = true;
            }

            // Setup events that will trigger PropertyChanged when child collections change (to trigger a save)
            ExcludedPrograms.CollectionChanged += (sender, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExcludedPrograms)));
            DevicesDisabled.CollectionChanged += (sender, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DevicesDisabled)));
            EvaluatableTemplates.CollectionChanged += (sender, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluatableTemplates)));
        }
    }

    public static class ExtensionHelpers
    {
        public static bool IsAutomaticGeneration(this PreferredKeyboardLocalization self)
        {
            return self == PreferredKeyboardLocalization.ansi || self == PreferredKeyboardLocalization.iso;
        }

        public static bool IsANSI(this PreferredKeyboardLocalization self)
        {
            return self == PreferredKeyboardLocalization.ansi || self == PreferredKeyboardLocalization.dvorak || self == PreferredKeyboardLocalization.us;
        }
    }

    public class ConfigManager
    {
        private static string ConfigPath = Path.Combine(Global.AppDataDirectory, "Config");
        private const string ConfigExtension = ".json";

        private static long _last_save_time = 0L;
        private readonly static long _save_interval = 300L;

        public static Configuration Load()
        {
            Configuration config;
            var configPath = ConfigPath + ConfigExtension;

            if (!File.Exists(configPath))
                config = CreateDefaultConfigurationFile();
            else {
                var content = File.ReadAllText(configPath, Encoding.UTF8);
                config = string.IsNullOrWhiteSpace(content)
                    ? CreateDefaultConfigurationFile()
                    : JsonConvert.DeserializeObject<Configuration>(content,
                        new JsonSerializerSettings
                        {
                            ObjectCreationHandling = ObjectCreationHandling.Replace,
                            TypeNameHandling = TypeNameHandling.All,
                            SerializationBinder = JSONUtils.SerializationBinder,
                            Error = DeserializeErrorHandler
                        })!;
            }

            config.OnPostLoad();
            return config;
        }

        private static readonly Dictionary<string,Type> Migrations = new()
        {
            {"Aurora.Devices.SteelSeriesHID.SteelSeriesHIDDevice" , typeof(Devices.UnifiedHID.UnifiedHIDDevice)},
            {"Aurora.Devices.Asus.AsusDevice" , typeof(AsusDevice)},
            {"Aurora.Devices.CoolerMaster.CoolerMasterDevice", typeof(CoolerMasterRgbNetDevice)},
            {"Aurora.Devices.AtmoOrbDevice.AtmoOrbDevice", typeof(AtmoOrbDevice)},
            {"Aurora.Devices.OpenRGB", typeof(OpenRgbAuroraDevice)},
        };

        private static readonly List<string> Discard = new()
        {
            "Aurora.Devices.Creative.SoundBlasterXDevice",
            "Aurora.Devices.NZXT.NZXTDevice",
        };
        private static void DeserializeErrorHandler(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            if (e.CurrentObject is not ICollection<Type> dd)
            {
                return;
            }

            foreach (var (key, value) in Migrations)
            {
                if (!e.ErrorContext.Error.Message.Contains(key)) continue;
                dd.Add(value);
                e.ErrorContext.Handled = true;
                return;
            }

            if (Discard.Any(s => e.ErrorContext.Error.Message.Contains(s)))
            {
                e.ErrorContext.Handled = true;
            }
        }

        public static void Save(Configuration configuration)
        {
            long current_time = Utils.Time.GetMillisecondsSinceEpoch();

            if (_last_save_time + _save_interval > current_time)
                return;
            else
                _last_save_time = current_time;

            var configPath = ConfigPath + ConfigExtension;
            string content = JsonConvert.SerializeObject(configuration, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Binder = Aurora.Utils.JSONUtils.SerializationBinder });

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, content, Encoding.UTF8);
        }

        private static Configuration CreateDefaultConfigurationFile()
        {
            Configuration config = new Configuration();
            var configData = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Binder = Aurora.Utils.JSONUtils.SerializationBinder });
            var configPath = ConfigPath + ConfigExtension;

            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(configPath));
            File.WriteAllText(configPath, configData, Encoding.UTF8);

            return config;
        }
    }
}
