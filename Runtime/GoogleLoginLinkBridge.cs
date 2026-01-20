// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
// 
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
// 
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
// 
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
// 
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

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
                _loginFail?.Invoke(errorCode.ToString());
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
        private Action<string> _loginFail;

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginSuccess">登录成功回调</param>
        /// <param name="loginFail">登录失败回调</param>
        [UnityEngine.Scripting.Preserve]
        public void SignIn(Action<GoogleLoginSuccess> loginSuccess, Action<string> loginFail)
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