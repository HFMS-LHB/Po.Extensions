using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance
{
    /// <summary>
    /// 单实例运行检测服务接口
    /// </summary>
    public interface ISingleInstanceService : IDisposable
    {
        /// <summary>
        /// 尝试获取实例所有权。如果返回 false，表示已有实例在运行。
        /// </summary>
        /// <returns>如果成功获取所有权返回 true，否则返回 false</returns>
        bool TryAcquireOwnership();

        /// <summary>
        /// 释放实例所有权（程序退出时调用）。
        /// </summary>
        void ReleaseOwnership();

        /// <summary>
        /// 激活已有的实例窗口（当 TryAcquireOwnership 返回 false 时调用）。
        /// </summary>
        void ActivateExistingInstance();
    }
}
