<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>
## Project Overview

Game Frame X Google Login is a Google login component for the GameFrameX framework, providing initialization, login, and logout capabilities.

## Quick Start

### Installation

Choose one of the following methods:

1. Add the following to the `dependencies` section in your project's `manifest.json`:
   ```json
   {"com.gameframex.unity.login.google": "https://github.com/AlianBlank/com.gameframex.unity.login.google.git"}
   ```

2. Use `Git URL` in Unity's Package Manager:
   ```
   https://github.com/AlianBlank/com.gameframex.unity.login.google.git
   ```

3. Download the repository and place it in your Unity project's `Packages` directory. It will be loaded automatically.

## Usage Examples

1. Attach the `GoogleLoginComponent` component to the `GameEntry` game object.
2. Set the `ProjectId` on the `GoogleLoginComponent` component.
3. Call the methods:

```csharp
// Get Google login component
var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

// Initialize
googleLoginComponent.Init();

// Login
googleLoginComponent.Login(
    (googleLoginSuccess) =>
    {
        Debug.Log($"Login successful! {JsonUtility.ToJson(googleLoginSuccess)}");
    },
    (code) =>
    {
        Debug.LogError($"Login failed! {code}");
    });

// Logout
googleLoginComponent.LogOut();
```

## Platform Configuration

### Android

1. Add the `game_services_project_id` string resource in `res/values/strings.xml`:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <resources>
       <string name="game_services_project_id">YOUR_PROJECT_ID</string>
   </resources>
   ```

2. Add `meta-data` in the `application` node of `AndroidManifest.xml`:
   ```xml
   <meta-data
       android:name="com.google.android.gms.games.APP_ID"
       android:value="@string/game_services_project_id"/>
   ```

3. Add library references in `build.gradle`:
   ```groovy
   implementation 'com.google.android.gms:play-services-games-v2:+'
   implementation 'com.google.android.gms:play-services-auth:19.0.0'
   ```

## Dependencies

- `com.gameframex.unity`: GameFrameX core framework
- `com.gameframex.unity.getchannel`: Channel management

## Documentation & Resources

- Documentation: https://gameframex.doc.alianblank.com
- Repository: https://github.com/GameFrameX/com.gameframex.unity.login.google
- Issues: https://github.com/GameFrameX/com.gameframex.unity.login.google/issues

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE.md) for details.
