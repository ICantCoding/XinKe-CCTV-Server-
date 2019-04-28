using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RendererExtensions
{
    //渲染器扩展方法
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
