using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XXX : MonoBehaviour
{
    public Renderer m_renderer;
    public Camera m_camera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if(!m_renderer.IsVisibleFrom(m_camera))
            {
                Debug.Log("看不见");
            }
            else
            {
                Debug.Log("看见");
            }
    }
    
    
}
