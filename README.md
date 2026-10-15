<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Project Overview

**Google Login Channel** - The Google channel package of the GameFrameX login channel registry. It implements `ILoginManager` as `GoogleLoginManager`, self-registers into `LoginChannelRegistry` at startup via `GoogleLoginBootstrap` (installing the package is all it takes), and returns `GoogleLoginResult` (subclass of `LoginResult`; the IdToken is carried by the unified `Token` field).

### Features

- Implements the `ILoginManager` Google channel; registers itself into `LoginChannelRegistry` at `BeforeSceneLoad` via `GoogleLoginBootstrap` (channel id `"Google"`) - installing the package is all it takes, no scene or entry package change required
- `GoogleLoginResult : LoginResult` keeps `FamilyName` / `GivenName` / `PlayerId`; the IdToken is carried by the unified `Token` field and `PlayerId` is also written into `OpenId`
- Channel config is read by the channel package itself: `googleProjectId` via BlankGetChannel (must be the Web client id)
- Google registers as the default channel on Android / iOS via the Bootstrap macro wall (including the Editor with an Android/iOS build target); Editor runs a stub sign-in for flow verification

## Quick Start

### Installation

Choose one of the following methods:

1. Edit your Unity project's `Packages/manifest.json` and add the `scopedRegistries` section:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.login.google": "3.0.0"
     }
   }
   ```

   `scopes` controls which packages are resolved through this registry. Only packages whose names start with `com.gameframex` will be fetched from it.

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.

## Usage Examples

```csharp
using GameFrameX.Login.Runtime;
using UnityEngine;

public class GoogleLoginDemo : MonoBehaviour
{
    void Start()
    {
        // Google registers itself into LoginChannelRegistry at BeforeSceneLoad when this
        // package is installed; use the single LoginComponent facade (no per-channel component)
        var login = GameEntry.GetComponent<LoginComponent>(); // or GameApp.Login

        login.OnLoginSuccess += (sender, e) =>
        {
            var google = e.Result as GoogleLoginResult; // channel-specific fields when needed
            Debug.Log($"token={e.Result.Token} openId={e.Result.OpenId} playerId={google?.PlayerId}");
        };

        login.Init();                          // reads googleProjectId via BlankGetChannel
        login.Login(new LoginParams("Google")); // explicit routing (or empty for platform default)
    }
}
```

**Channel config**: the `googleProjectId` (must be the Web client id) is read by the channel package itself via BlankGetChannel - configure it the same way as other channel values (e.g. `BlankGetChannel.GetChannelName("googleProjectId")`).

## Architecture

### Self-Registration Semantics

`GoogleLoginBootstrap` registers the `"Google"` channel (factory `() => new GoogleLoginManager()`) into `LoginChannelRegistry` at `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)` - earlier than `LoginComponent.Awake`, on Android / iOS only, enforced by the macro wall. When this package is absent (or the platform is outside the macro wall) the code does not compile / is stripped and the registration simply does not happen; projects with / without this package both compile and run.

### Relationship with the core package

- Depends only on `com.gameframex.unity.login` (plus `com.gameframex.unity` and BlankGetChannel).
- Google self-registers as the default channel on Android / iOS (incl. Editor with matching build target) via the Bootstrap macro wall; on other platforms the bootstrap does not compile, no registration happens, and the core-package DefaultLoginManager forwarder warns and no-ops.
- `LoginParams.ChannelId = "Google"` routes explicitly; empty resolves via registry mutual exclusion (first registered channel; the module slot is occupied by the core-package `DefaultLoginManager` forwarder).

### Migration (2.x -> 3.0.0)

| Old | New |
|-----|-----|
| `GoogleLoginComponent` | `LoginComponent` (core package; delete the scene component) |
| `GameApp.GoogleLogin` | `GameApp.Login` (define -> `ENABLE_GAME_FRAME_X_LOGIN`) |
| `GoogleLoginSuccess.IdToken` | `GoogleLoginResult.Token` (server verification reads `Token`) |
| `googleLoginComponent.Login(onSuccess, onFail)` | `login.Login(onSuccess, onFailed)` one-shot overload or aggregated events |
| Component `Init()` | `GameApp.Login.Init()` (reads `googleProjectId` the same way) |

## Platform Support

| Platform | Supported |
|----------|-----------|
| Android | Yes (native bridge) |
| iOS | Yes (native bridge) |
| Editor | Yes (stub sign-in) |

## Documentation & Resources

- [Documentation](https://gameframex.doc.alianblank.com)
- [GitHub Repository](https://github.com/GameFrameX/com.gameframex.unity.login.google)

## Community & Support

- QQ Group: Join via [QR Code](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)

## Dependencies

| Package | Version |
|---------|---------|
| `com.gameframex.unity` | 2.5.4 |
| `com.gameframex.unity.getchannel` | 1.3.2 |
| `com.gameframex.unity.login` | 1.0.0 |

## Changelog

See [Releases](https://github.com/gameframex/com.gameframex.unity.login.google/releases) for changelog.

## License

See [LICENSE.md](LICENSE.md) for license information.
