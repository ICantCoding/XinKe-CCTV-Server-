using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TDFramework
{
    public class StationModule : IModule
    {
        #region 字段
        private StationEngine m_stationEngine;
        #endregion

        #region IModule抽象函数实现
        public override void Init()
        {
            m_stationEngine = StationEngine.Instance;
            m_stationEngine.Init();
        }
        public override void Release()
        {

        }
        #endregion
    }
}


