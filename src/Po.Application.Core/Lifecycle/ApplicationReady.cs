using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.Lifecycle;

/// <summary>
/// 应用程序已完成初始化信号。
/// </summary>
public sealed class ApplicationReady : AsyncSignal, IApplicationReady
{
}
