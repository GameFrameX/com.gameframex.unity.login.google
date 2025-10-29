// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using GameFrameX.Runtime;
using UnityEngine;

namespace GameFrameX.Login.Google.Runtime
{
    [UnityEngine.Scripting.Preserve]
    public class GoogleLoginLinkBridge : MonoBehaviour
    {
        private static GoogleLoginLinkBridge _instance;

        [UnityEngine.Scripting.Preserve]
        public static GoogleLoginLinkBridge Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("GoogleLoginLinkBridge").AddComponent<GoogleLoginLinkBridge>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void OnMessage(string json)
        {
            Debug.Log(json);
            var split = json.Split("|", StringSplitOptions.RemoveEmptyEntries);
            if (split.Length != 2)
            {
                return;
            }

            int errorCode = Convert.ToInt32(split[0]);
            string jsonString = split[1];
            if (errorCode == 0)
            {
                // success
                GoogleLoginSuccess success = Utility.Json.ToObject<GoogleLoginSuccess>(jsonString);
                _loginSuccess?.Invoke(success);
            }
            else
            {
                // fail
                _loginFail?.Invoke(errorCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">这个ID 必须是Web 的才可以</param>
        [UnityEngine.Scripting.Preserve]
        public void Init(string projectId)
        {
            _projectId = projectId;
        }

        private string _projectId;

        private Action<GoogleLoginSuccess> _loginSuccess;
        private Action<int> _loginFail;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginSuccess">登录成功回调</param>
        /// <param name="loginFail">登录失败回调</param>
        [UnityEngine.Scripting.Preserve]
        public void SignIn(Action<GoogleLoginSuccess> loginSuccess, Action<int> loginFail)
        {
            _loginSuccess = loginSuccess;
            _loginFail = loginFail;
#if UNITY_EDITOR
            var googleLoginSuccess = new GoogleLoginSuccess();
            googleLoginSuccess.PlayerId = SystemInfo.deviceUniqueIdentifier;
            googleLoginSuccess.IdToken = "test_login@google.com";
            googleLoginSuccess.DisplayName = "Editor_Google_Test";
            googleLoginSuccess.IdToken = SystemInfo.deviceUniqueIdentifier;
            loginSuccess?.Invoke(googleLoginSuccess);
            return;
#endif

            UnityPlayerHelper.SignIn(_projectId);
        }

        /// <summary>
        /// 登出
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void SignOut()
        {
            UnityPlayerHelper.SignOut();
        }


        /// <summary>
        /// Helper methods related to the UnityPlayer Java class.
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        private static class UnityPlayerHelper
        {
            /// <summary> 
            /// 登录
            /// </summary>
            /// <param name="projectId">这个ID 必须是Web 的才可以</param>
            [UnityEngine.Scripting.Preserve]
            public static void SignIn(string projectId)
            {
                GetMainActivity().CallStatic("signIn", projectId);
            }

            /// <summary>
            /// 登出
            /// </summary>
            [UnityEngine.Scripting.Preserve]
            public static void SignOut()
            {
                GetMainActivity().CallStatic("signOut");
            }

            private static AndroidJavaClass _mainActivity;

            static AndroidJavaClass GetMainActivity()
            {
                if (_mainActivity == null)
                {
                    _mainActivity = new AndroidJavaClass("com.alianblank.google.login.GoogleLoginMainActivity");
                }

                return _mainActivity;
            }
        }
    }
}