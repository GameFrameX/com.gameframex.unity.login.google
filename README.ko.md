<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google Login

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](../../README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

**Google 로그인 채널** - GameFrameX 로그인 채널 레지스트리의 Google 채널 패키지. `GoogleLoginManager`로 `ILoginManager`를 구현하고, 시작 시 `GoogleLoginBootstrap`을 통해 `LoginChannelRegistry`에 자동 등록합니다(패키지 설치만으로 활성화). `GoogleLoginResult`(`LoginResult` 하위 클래스, IdToken은 통합 `Token` 필드로 전달)를 반환합니다.

### 기능

- `GoogleLoginManager`로 `ILoginManager` Google 채널 구현. `GoogleLoginBootstrap`이 `BeforeSceneLoad`에서 `LoginChannelRegistry`에 자동 등록(채널 ID `"Google"`). 패키지 설치만으로 활성화되며 씬과 entry 패키지 변경 불필요
- `GoogleLoginResult : LoginResult`는 `FamilyName` / `GivenName` / `PlayerId` 유지. IdToken은 통합 `Token` 필드로 전달되고 `PlayerId`는 `OpenId`에도 기록됩니다
- 채널 설정은 채널 패키지가 직접 읽습니다: BlankGetChannel을 통해 `googleProjectId` 읽기(Web 클라이언트 ID여야 함)
- Google은 Bootstrap 매크로 월을 통해 Android / iOS에서 기본 채널로 등록됩니다(Android/iOS 빌드 타깃인 Editor 포함). Editor에서는 스텁 데이터를 반환해 흐름 검증 가능

## 빠른 시작

### 설치

아래 방법 중 하나를 선택하세요:

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

   `scopes`는 이 레지스트리를 통해 해석될 패키지를 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.login.google": "https://github.com/gameframex/com.gameframex.unity.login.google.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.login.google.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.

## 사용 예시

```csharp
using GameFrameX.Login.Runtime;
using UnityEngine;

public class GoogleLoginDemo : MonoBehaviour
{
    void Start()
    {
        // 이 패키지를 설치하면 Google이 BeforeSceneLoad에서 LoginChannelRegistry에 자동 등록됩니다.
        // LoginComponent 파사드로 통일된 접근(per-channel 컴포넌트 없음)
        var login = GameEntry.GetComponent<LoginComponent>(); // 또는 GameApp.Login

        login.OnLoginSuccess += (sender, e) =>
        {
            var google = e.Result as GoogleLoginResult; // 채널 고유 필드가 필요하면 다운캐스트
            Debug.Log($"token={e.Result.Token} openId={e.Result.OpenId} playerId={google?.PlayerId}");
        };

        login.Init();                          // 내부에서 BlankGetChannel로 googleProjectId 읽기
        login.Login(new LoginParams("Google")); // 명시적 라우팅(비어 있으면 플랫폼 기본 채널)
    }
}
```

**채널 설정**: `googleProjectId`(Web 클라이언트 ID여야 함)는 채널 패키지가 BlankGetChannel을 통해 직접 읽습니다. 다른 채널 값과 동일한 방식으로 설정합니다(예: `BlankGetChannel.GetChannelName("googleProjectId")`).

## 아키텍처

### 자동 등록 시맨틱

`GoogleLoginBootstrap`는 `RuntimeInitializeOnLoadMethod(BeforeSceneLoad)`(`LoginComponent.Awake`보다 이른 시점)에서 `"Google"` 채널(팩토리 `() => new GoogleLoginManager()`)을 `LoginChannelRegistry`에 등록하며, 매크로 월에 의해 Android / iOS에서만 발생합니다. 이 패키지가 없거나 플랫폼이 매크로 월 밖이면 코드가 컴파일되지 않거나 스트리핑되어 등록이 발생하지 않습니다. 설치 / 미설치 두 가지 프로젝트 형태 모두 컴파일 및 실행 가능합니다.

### 핵심 패키지와의 관계

- `com.gameframex.unity.login`만 의존(`com.gameframex.unity` 및 BlankGetChannel 포함).
- Google은 Bootstrap 매크로 월을 통해 Android / iOS(빌드 타깃이 일치하는 Editor 포함)에서 기본 채널로 자동 등록됩니다. 다른 플랫폼에서는 bootstrap이 컴파일되지 않고 등록도 발생하지 않으며, 핵심 패키지의 DefaultLoginManager 포워더가 경고를 내고 아무 동작도 하지 않습니다.
- `LoginParams.ChannelId = "Google"` 명시적 라우팅. 비어 있으면 레지스트리 상호 배제로 해결됩니다(처음 등록된 채널. 모듈 슬롯은 핵심 패키지의 `DefaultLoginManager` 포워더가 차지).

### 마이그레이션 안내(2.x -> 3.0.0)

| 이전 | 새 |
|------|-----|
| `GoogleLoginComponent` | `LoginComponent`(핵심 패키지. 씬의 기존 컴포넌트 삭제) |
| `GameApp.GoogleLogin` | `GameApp.Login`(정의 -> `ENABLE_GAME_FRAME_X_LOGIN`) |
| `GoogleLoginSuccess.IdToken` | `GoogleLoginResult.Token`(서버 검증은 `Token` 읽기) |
| `googleLoginComponent.Login(onSuccess, onFail)` | `login.Login(onSuccess, onFailed)` 일회성 오버로드 또는 통합 이벤트 |
| 컴포넌트 `Init()` | `GameApp.Login.Init()`(`googleProjectId` 읽기 방식은 동일) |

## 플랫폼 지원

| 플랫폼 | 지원 |
|----------|-----------|
| Android | 지원(네이티브 브리지) |
| iOS | 지원(네이티브 브리지) |
| Editor | 지원(스텁) |

## 문서 및 자료

- [문서](https://gameframex.doc.alianblank.com)
- [GitHub 저장소](https://github.com/GameFrameX/com.gameframex.unity.login.google)

## 커뮤니티 및 지원

- QQ 그룹: [QR 코드](https://qm.qq.com/cgi-bin/qm/qr?k=ikT9gA5m2sKwOyNOfYmQvSAPK_c3GmD6)로 참여

## 의존성

| 패키지 | 버전 |
|---------|---------|
| `com.gameframex.unity` | 2.5.4 |
| `com.gameframex.unity.getchannel` | 1.3.2 |
| `com.gameframex.unity.login` | 1.0.0 |

## 변경 로그

변경 로그는 [Releases](https://github.com/gameframex/com.gameframex.unity.login.google/releases)를 참조하세요.

## 라이선스

자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
