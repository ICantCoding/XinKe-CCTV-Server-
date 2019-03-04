﻿
namespace TDFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SingletonMgr
    {
        public static GameGlobalInfo GameGlobalInfo = GameGlobalInfo.Instance;
        public static NetworkEngine NetworkEngine = NetworkEngine.Instance;
        public static ModuleMgr ModuleMgr = ModuleMgr.Instance;
        public static SceneInfoMgr SceneInfoMgr = SceneInfoMgr.Instance;
        public static AssetBundleManager AssetBundleManager = AssetBundleManager.Instance;
        public static ResourceMgr ResourceMgr = ResourceMgr.Instance;
        public static ObjectManager ObjectManager = ObjectManager.Instance;
    }
}
