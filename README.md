# PrefabsMadeEasy

*Displays the name of the prefab you're currently aiming at — great for modders, devs, or builders.*

## Features

- Shows the prefab name of the object under your crosshair.
- Lightweight and works in both single-player and multiplayer.
- Includes a draggable, semi-transparent overlay window that can be toggled.
- ESC menu unlocks full drag/move functionality for clean UI alignment.

## Instructions

- PrefabsMadeEasy activates when the world is loaded or on server join.
- While playing:
  - The prefab name appears on-screen when pointing at any interactable object.
  - Press `F10` to toggle suspend/resume.
  - ESC menu opens up the draggable overlay with background for repositioning.
- Works without any dependencies, but integrates nicely with ConfigurationManager if installed.

## Installation

### Manual

1. Download and unzip `PrefabsMadeEasy.dll` into your `/Valheim/BepInEx/plugins/` directory.
2. Launch the game. The overlay should be active when pointing at objects.

### Thunderstore (manual import)

1. Go to **Settings > Import local mod** in r2modman.
2. Select `PrefabsMadeEasy_v1.X.X.zip`.
3. Click "Import local mod" when prompted.

## Configuration

- Config file is generated at:  
  `BepInEx/config/com.enjerutantei.prefabsmadeeasy.cfg`
- Settings include:
  - Toggle prefab name visibility
  - Set the suspend toggle hotkey
  - (Optional) Extend with font size, position persistence, etc.

## Notes

- [GitHub Source](https://github.com/yourusername/PrefabsMadeEasy) — view the code, contribute, or report issues.
- Originally designed as a modder’s tool to help identify prefab names while testing.
- Made with love for the Valheim modding community. ❤️
## Changelog

### 1.0.0

  * Initial release.