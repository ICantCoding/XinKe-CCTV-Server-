using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStart : MonoBehaviour {

	#region 字段
	private ActorManager m_actorManager = null;
	private ServerActor m_serverActor = null;
	#endregion

	#region Unity生命周期
	void Start () {

		m_actorManager = new ActorManager(); 
		//创建看门狗Actor
		WatchDogActor watchDogActor = new WatchDogActor(this);
		m_actorManager.AddActor(watchDogActor, true);
		//创建WorldActor
		WorldActor worldActor = new WorldActor(this);
		m_actorManager.AddActor(worldActor, true);
		//创建服务器Actor
		m_serverActor = new ServerActor(this);
		ActorManager.Instance.AddActor(m_serverActor, true);
		m_serverActor.Start(3322); //启动服务器

	}
	void OnApplicationQuit()
	{
		if(m_serverActor != null)
		{
			m_serverActor.Close();
			m_serverActor = null;
		}
	}
	#endregion
}
