using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFrame
{
    public interface IEventObserver
    {
        /// <summary>
        /// 事件观察者接口，当有事件发过来时事件管理器会调用这个方法
        /// </summary>
        /// <param name="resp">发送过来的</param>
        void HandleEvent(EventData resp);
    }
}
