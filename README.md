# GameFrameX.Login.Google 谷歌登录

> GameFrameX.Login.Google 是 GameFrameX 框架的谷歌登录组件。

## 功能

- `初始化`
- `登录`
- `登出`

## 使用方法

1.  **挂载组件**
    在 `GameEntry` 游戏入口对象上挂载 `GoogleLoginComponent` 组件。

2.  **设置参数**
    在 `GoogleLoginComponent` 组件上设置 `ProjectId`。

3.  **调用方法**
    ```csharp
    // 获取谷歌登录组件
    var googleLoginComponent = GameEntry.GetComponent<GoogleLoginComponent>();

    // 初始化
    googleLoginComponent.Init();

    // 登录
    googleLoginComponent.Login(
        (googleLoginSuccess) =>
        {
            Debug.Log($"登录成功! {JsonUtility.ToJson(googleLoginSuccess)}");
        },
        (code) =>
        {
            Debug.LogError($"登录失败! {code}");
        });

    // 登出
    googleLoginComponent.LogOut();
    ```

## Android 配置

### 1. 添加字符串资源

在项目 `res/values/strings.xml` 文件中添加 `game_services_project_id` 字符串，值为谷歌后台生成的 `ProjectId`。

```xml
<?xml version="1.0" encoding="utf-8"?>
<resources>
    <!--  这里填写后台生成的ID-->
    <string name="game_services_project_id">924091193176</string>
</resources>
```

### 2. 配置 AndroidManifest.xml

在 `AndroidManifest.xml` 文件的 `application` 节点下添加 `meta-data`。

```xml
<meta-data
    android:name="com.google.android.gms.games.APP_ID"
    android:value="@string/game_services_project_id"/>
```

### 3. 添加库引用

在 `build.gradle` 文件中添加以下库引用：

```groovy
implementation 'com.google.android.gms:play-services-games-v2:+'
implementation 'com.google.android.gms:play-services-auth:19.0.0'
```
