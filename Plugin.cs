using BepInEx;
using GorillaTag.Rendering;
using HarmonyLib;
using System;
using UnityEngine;

namespace JustFog
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private float _groundFogDepthFadeSize = 80f;
        private float _groundFogHeightFadeSize = 100f;
        private float groundFogHeight = 80f;
        private bool hasFogged;
        void Update()
        {
            if (GorillaLocomotion.GTPlayer.Instance != null && !hasFogged)
            {
                ZoneShaderSettings Forest_ZoneShaderSettings_Prefab = GameObject.Find("Forest_ZoneShaderSettings_Prefab").GetComponent<ZoneShaderSettings>();
                Forest_ZoneShaderSettings_Prefab.isDefaultValues = true;

                Traverse ForestSettingsTraverse = Traverse.Create(Forest_ZoneShaderSettings_Prefab);
                ForestSettingsTraverse.Field("groundFogHeight").SetValue(groundFogHeight);
                ForestSettingsTraverse.Field("_groundFogDepthFadeSize").SetValue(_groundFogDepthFadeSize);
                ForestSettingsTraverse.Field("_groundFogHeightFadeSize").SetValue(_groundFogHeightFadeSize);

                typeof(ZoneShaderSettings).GetMethod("ApplyValues", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(Forest_ZoneShaderSettings_Prefab, new object[] { });

                hasFogged = true;
            }
        }

        bool isVisible = true;
        float visibleDelay = 0f;

        private float _lastgroundFogDepthFadeSize = 80f;
        private float _lastgroundFogHeightFadeSize = 100f;
        private float lastgroundFogHeight = 80f;

        void OnGUI()
        {
            if (UnityInput.Current.GetKey(KeyCode.Insert) && Time.time > visibleDelay)
            {
                isVisible = !isVisible;
                visibleDelay = Time.time + 0.2f;
            }

            if (!isVisible)
                return;

            _groundFogDepthFadeSize = GUI.HorizontalSlider(new Rect(10f, 10f, 200f, 15f), _groundFogDepthFadeSize, 0f, 150f);
            GUI.Label(new Rect(220f, 10f, 800f, 25f), "_groundFogDepthFadeSize: " + _groundFogDepthFadeSize.ToString("F0"));

            _groundFogHeightFadeSize = GUI.HorizontalSlider(new Rect(10f, 25f, 200f, 15f), _groundFogHeightFadeSize, 0f, 150f);
            GUI.Label(new Rect(220f, 25f, 800f, 25f), "_groundFogHeightFadeSize: " + _groundFogHeightFadeSize.ToString("F0"));

            groundFogHeight = GUI.HorizontalSlider(new Rect(10f, 40f, 200f, 15f), groundFogHeight, 0f, 150f);
            GUI.Label(new Rect(220f, 40f, 800f, 25f), "groundFogHeight: " + groundFogHeight.ToString("F0"));

            if (_lastgroundFogDepthFadeSize != _groundFogDepthFadeSize || _lastgroundFogHeightFadeSize != _groundFogHeightFadeSize || lastgroundFogHeight != groundFogHeight)
                hasFogged = false;

            _lastgroundFogDepthFadeSize = _groundFogDepthFadeSize;
            _lastgroundFogHeightFadeSize = _groundFogHeightFadeSize;
            lastgroundFogHeight = groundFogHeight;
        }
    }
}
