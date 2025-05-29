namespace PrefabsMadeEasy
{
    using BepInEx;
    using BepInEx.Configuration;
    using System;
    using System.Linq;
    using UnityEngine;
    using Color = UnityEngine.Color;
    using FontStyle = UnityEngine.FontStyle;

    

    [BepInPlugin("com.enjerutantei.prefabsmadeeasy", "Prefabs Made Easy", "1.0.0")]
    public class PrefabsMadeEasyMod : BaseUnityPlugin
    {
        private float checkInterval = 0.5f;
        private float timeSinceLastCheck = 0f;
        private string _currentPrefabName = "";
        private Rect _windowRect = new Rect(20, 20, 300, 60);
        private static bool _suspendModLogic = false;
        private ConfigEntry<KeyCode> SuspendToggleKey;

        public static ConfigEntry<bool> ShowPrefabDisplay;

        private void Awake()
        {
            ShowPrefabDisplay = Config.Bind(
                "Display Options",
                "Show Prefab Name",
                true,
                "Show the draggable prefab name window when aiming at objects."
            );
            SuspendToggleKey = Config.Bind(
                "Hotkeys",
                "Suspend Toggle Key",
                KeyCode.F10,
                "Toggles whether prefab UI logic is suspended"
            );
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(SuspendToggleKey.Value))
            {
                _suspendModLogic = !_suspendModLogic;
                Debug.Log($"[PrefabsMadeEasy] Suspended: {_suspendModLogic}");
            }

            if (_suspendModLogic)
                return;

            timeSinceLastCheck += Time.deltaTime;

            if (timeSinceLastCheck >= checkInterval)
            {
                timeSinceLastCheck = 0f;
                CheckCrosshairTarget();
            }
        }
        private string GetPrefabName(ZNetView znv)
        {
            if (znv == null || znv.GetZDO() == null)
                return null;

            int prefabHash = znv.GetZDO().GetPrefab();
            GameObject prefab = ZNetScene.instance.GetPrefab(prefabHash);
            return prefab != null ? prefab.name : null;
        }
        private void OnGUI()
        {
            if (_suspendModLogic || !ShowPrefabDisplay.Value || string.IsNullOrEmpty(_currentPrefabName))
                return;

            bool showBackground = Game.IsPaused();

            // Common display rectangle
            Rect displayRect = new Rect(_windowRect.x, _windowRect.y, _windowRect.width, _windowRect.height);

            if (showBackground)
            {
                GUIStyle windowStyle = new GUIStyle(GUI.skin.window)
                {
                    padding = new RectOffset(4, 4, 4, 4),
                    margin = new RectOffset(0, 0, 0, 0),
                };
                windowStyle.normal.background = MakeTex(1, 1, new Color(0, 0, 0, 0.6f));

                _windowRect = GUI.Window(123456, _windowRect, DrawPrefabWindow, "", windowStyle);
            }
            else
            {
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleLeft,
                    wordWrap = true
                };

                GUI.Label(displayRect, $"Looking at: {_currentPrefabName}", labelStyle);
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        private void DrawPrefabWindow(int windowID)
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true
            };

            GUI.Label(new Rect(4, 4, _windowRect.width - 8, _windowRect.height - 8), $"Looking at: {_currentPrefabName}", labelStyle);

            if (Game.IsPaused())
            {
                GUI.DragWindow(new Rect(0, 0, _windowRect.width, 20));
            }
        }

        private void CheckCrosshairTarget()
        {
            Debug.Log($"CheckCrosshairTarget()");
            var player = Player.m_localPlayer;
            if (player == null || !player.IsOwner())
                return;

            var camera = Camera.main;
            if (camera == null)
                return;

            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                var go = hit.collider?.gameObject;
                if (go != null)
                {
                    var zNetView = go.GetComponentInParent<ZNetView>();
                    if (zNetView != null && this.GetPrefabName(zNetView) != null)
                    {
                        string prefabName = this.GetPrefabName(zNetView);
                        //Debug.Log($"Crosshair Target: {prefabName}");
                        _currentPrefabName = prefabName;
                    }
                    else
                    {
                        //Debug.Log($"Crosshair Target: {go.name} (no ZNetView)");
                        _currentPrefabName = go.name + " (no ZNetView)";
                    }
                }
            }
        }

        



    }
}
