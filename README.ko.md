<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# Game Frame X Google 로그인

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.login.google)](https://github.com/GameFrameX/com.gameframex.unity.login.google/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

Game Frame X Google 로그인은 GameFrameX 프레임워크의 Google 로그인 컴포넌트로, 초기화, 로그인 및 로그아웃 기능을 제공합니다.

## 빠른 시작

### 설치

Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:

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
  ]
}
```

`scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

Then add the package to `dependencies`:

```json
{
  "dependencies": {
    "com.gameframex.unity.login.google": "1.1.0"
  }
}
```

## 사용 예시

1. `GameEntry` 게임 오브젝트에 `GoogleLoginComponent` 컴포넌트를 연결합니다.
2. `GoogleLoginComponent` 컴포넌트에 `ProjectId`를 설정합니다.
3. 메서드를 호출합니다:

```csharp
// Google 로그인 컴포넌트 가져오기
var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

// 초기화
googleLoginComponent.Init();

// 로그인
googleLoginComponent.Login(
    (googleLoginSuccess) =>
    {
        Debug.Log($"로그인 성공! {JsonUtility.ToJson(googleLoginSuccess)}");
    },
    (code) =>
    {
        Debug.LogError($"로그인 실패! {code}");
    });

// 로그아웃
googleLoginComponent.LogOut();
```

## 플랫폼 설정

### Android

1. `res/values/strings.xml`에 `game_services_project_id` 문자열 리소스 추가:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <resources>
       <string name="game_services_project_id">YOUR_PROJECT_ID</string>
   </resources>
   ```

2. `AndroidManifest.xml`의 `application` 노드에 `meta-data` 추가:
   ```xml
   <meta-data
       android:name="com.google.android.gms.games.APP_ID"
       android:value="@string/game_services_project_id"/>
   ```

3. `build.gradle`에 라이브러리 참조 추가:
   ```groovy
   implementation 'com.google.android.gms:play-services-games-v2:+'
   implementation 'com.google.android.gms:play-services-auth:19.0.0'
   ```

## 의존성

- `com.gameframex.unity`: GameFrameX 핵심 프레임워크
- `com.gameframex.unity.getchannel`: 채널 관리

## 문서 및 자료

- 문서: https://gameframex.doc.alianblank.com
- 저장소: https://github.com/GameFrameX/com.gameframex.unity.login.google
- Issues: https://github.com/GameFrameX/com.gameframex.unity.login.google/issues

## 라이선스

자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
