// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
// 
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
// 
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source tree for the full license text.
// 
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷和责任，
//  Any legal disputes or liabilities arising from secondary development based on this project
//  本项目组织及贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
// 
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Login.Runtime;
using GameFrameX.Runtime;

namespace GameFrameX.Login.Google.Runtime
{
    /// <summary>
    /// Google 登录管理器
    /// </summary>
    /// <remarks>
    /// 渠道配置（googleProjectId，必须是 Web 端 ClientId）由渠道包经 BlankGetChannel 自行读取，核心包不建配置中心。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public sealed class GoogleLoginManager : BaseLoginManager
    {
        /// <summary>
        /// Google 渠道唯一标识
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public const string ChannelId = "Google";

        private string _projectId;

        [UnityEngine.Scripting.Preserve]
        public GoogleLoginManager()
        {
        }

        /// <summary>
        /// 初始化 Google 登录（projectId 经 BlankGetChannel 读取 googleProjectId，必须是 Web 端的才可以）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public override void Init()
        {
            var projectId = BlankGetChannel.GetChannelName("googleProjectId");
            if (string.IsNullOrEmpty(projectId))
            {
                Log.Warning("Google Login ProjectId is empty. Please configure 'googleProjectId' via BlankGetChannel.");
                return;
            }

            _projectId = projectId;
            GoogleLoginLinkBridge.Instance.Init(_projectId);
        }

        /// <summary>
        /// Google 登录是否准备好
        /// </summary>
        /// <returns>准备好返回true，否则返回false</returns>
        [UnityEngine.Scripting.Preserve]
        public override bool IsReady()
        {
            return !string.IsNullOrEmpty(_projectId);
        }

        /// <summary>
        /// 执行 Google 登录
        /// </summary>
        /// <param name="loginParams">登录参数（Google 渠道无特有入参）</param>
        [UnityEngine.Scripting.Preserve]
        public override void Login(LoginParams loginParams)
        {
            GoogleLoginLinkBridge.Instance.SignIn(
                result =>
                {
                    result.ChannelId = ChannelId;
                    result.OpenId = result.PlayerId;
                    RaiseLoginSuccess(new LoginEventArgs { ChannelId = ChannelId, Result = result });
                },
                error => { RaiseLoginFailed(new LoginEventArgs { ChannelId = ChannelId, ErrorMessage = error }); });
        }

        /// <summary>
        /// 退出 Google 账号登录
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public override void Logout()
        {
            GoogleLoginLinkBridge.Instance.SignOut();
        }
    }
}
