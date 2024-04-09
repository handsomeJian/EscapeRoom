/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A smaller helper class for Custom Scene Manager samples.
/// </summary>
public class SceneManagerHelper
{
    public GameObject AnchorGameObject { get; }

    public SceneManagerHelper(GameObject gameObject)
    {
        AnchorGameObject = gameObject;
    }

    public void SetLocation(OVRLocatable locatable, Camera camera = null)
    {
        if (!locatable.TryGetSceneAnchorPose(out var pose))
            return;

        var projectionCamera = camera == null ? Camera.main : camera;
        var position = pose.ComputeWorldPosition(projectionCamera);
        var rotation = pose.ComputeWorldRotation(projectionCamera);

        if (position != null && rotation != null)
            AnchorGameObject.transform.SetPositionAndRotation(
                position.Value, rotation.Value);
    }

    public void CreatePlane(OVRBounded2D bounds)
    {
        var planeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        planeGO.name = "Plane";
        planeGO.transform.SetParent(AnchorGameObject.transform, false);
        planeGO.transform.localScale = new Vector3(
            bounds.BoundingBox.size.x,
            bounds.BoundingBox.size.y,
            0.01f);
        planeGO.GetComponent<MeshRenderer>().material.SetColor(
            "_Color", UnityEngine.Random.ColorHSV());
    }

    public void UpdatePlane(OVRBounded2D bounds)
    {
        var planeGO = AnchorGameObject.transform.Find("Plane");
        if (planeGO == null)
            CreatePlane(bounds);
        else
        {
            planeGO.transform.localScale = new Vector3(
                bounds.BoundingBox.size.x,
                bounds.BoundingBox.size.y,
                0.01f);
        }
    }

    public void CreateVolume(OVRBounded3D bounds)
    {
        var volumeGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        volumeGO.name = "Volume";
        volumeGO.transform.SetParent(AnchorGameObject.transform, false);
        volumeGO.transform.localPosition = new Vector3(
            0, 0, -bounds.BoundingBox.size.z / 2);
        volumeGO.transform.localScale = bounds.BoundingBox.size;
        volumeGO.GetComponent<MeshRenderer>().material.SetColor(
            "_Color", UnityEngine.Random.ColorHSV());
    }

    public void UpdateVolume(OVRBounded3D bounds)
    {
        var volumeGO = AnchorGameObject.transform.Find("Volume");
        if (volumeGO == null)
            CreateVolume(bounds);
        else
        {
            volumeGO.transform.localPosition = new Vector3(
                0, 0, -bounds.BoundingBox.size.z / 2);
            volumeGO.transform.localScale = bounds.BoundingBox.size;
        }
    }

    private static Color[] _COLORS = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
        };

    public Color[] CreateColors(NativeArray<Vector3> vs, NativeArray<int> ts)
    {
        int vertexNum = vs.Length;
        int[] vertexLables = new int[vertexNum];

        List<int[]> triangleList = new List<int[]>();
        for (int i = 0; i < ts.Length; i += 3)
        {
            List<int> tmpList = new List<int> { ts[i], ts[i + 1], ts[i + 2] };
            tmpList.Sort();
            triangleList.Add(tmpList.ToArray());
        }
        triangleList.Sort((int[] t1, int[] t2) =>
        {
            for (int i = 0; i < 3; i++)
            {
                if (t1[i] < t2[i])
                    return -1;
                else if (t1[i] > t2[i])
                    return 1;
            }
            return 0;
        });

        foreach (var triangle in triangleList)
        {
            List<int> lable = new List<int> { 1, 2, 3 };
            foreach (var index in triangle)
            {
                if (lable.Contains(vertexLables[index]))
                    lable.Remove(vertexLables[index]);
            }
            foreach (var index in triangle)
            {
                if (vertexLables[index] != 0)
                    continue;

                if (lable.Count == 0)
                {
                    Debug.Log("Color Mesh Error!");
                    continue;
                }

                vertexLables[index] = lable[0];
                lable.RemoveAt(0);
            }
        }

        Color[] vertexColor = new Color[vertexNum];
        for (int i = 0; i < vertexNum; i++)
        {
            vertexColor[i] = vertexLables[i] > 0 ? _COLORS[vertexLables[i] - 1] : _COLORS[0];
        }
        return vertexColor;
    }

    private Color[] _SortedColoring(Mesh mesh)
    {
        int n = mesh.vertexCount;
        int[] labels = new int[n];

        List<int[]> triangles = _GetSortedTriangles(mesh.triangles);
        triangles.Sort((int[] t1, int[] t2) =>
        {
            int i = 0;
            while (i < t1.Length && i < t2.Length)
            {
                if (t1[i] < t2[i]) return -1;
                if (t1[i] > t2[i]) return 1;
                i += 1;
            }
            if (t1.Length < t2.Length) return -1;
            if (t1.Length > t2.Length) return 1;
            return 0;
        });

        foreach (int[] triangle in triangles)
        {
            List<int> availableLabels = new List<int>() { 1, 2, 3 };
            foreach (int vertexIndex in triangle)
            {
                if (availableLabels.Contains(labels[vertexIndex]))
                    availableLabels.Remove(labels[vertexIndex]);
            }
            foreach (int vertexIndex in triangle)
            {
                if (labels[vertexIndex] == 0)
                {
                    if (availableLabels.Count == 0)
                    {
                        Debug.LogError("Could not find color");
                        return null;
                    }
                    labels[vertexIndex] = availableLabels[0];
                    availableLabels.RemoveAt(0);
                }
            }
        }

        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
            colors[i] = labels[i] > 0 ? _COLORS[labels[i] - 1] : _COLORS[0];

        return colors;
    }

    private List<int[]> _GetSortedTriangles(int[] triangles)
    {
        List<int[]> result = new List<int[]>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            List<int> t = new List<int> { triangles[i], triangles[i + 1], triangles[i + 2] };
            t.Sort();
            result.Add(t.ToArray());
        }
        return result;
    }

    public void CreateMesh(OVRTriangleMesh mesh)
    {
        if (!mesh.TryGetCounts(out var vcount, out var tcount)) return;
        using var vs = new NativeArray<Vector3>(vcount, Allocator.Temp);
        using var ts = new NativeArray<int>(tcount * 3, Allocator.Temp);
        if (!mesh.TryGetMesh(vs, ts)) return;

        var trimesh = new Mesh();
        
        /*trimesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        trimesh.SetVertices(vs);
        trimesh.SetTriangles(ts.ToArray(), 0);*/
        

        
        var c = new Color[ts.Length];
        var v = new Vector3[ts.Length];
        var idx = new int[ts.Length];
        for (var i = 0; i < ts.Length; i++)
        {
            c[i] = new Color(
                i % 3 == 0 ? 1.0f : 0.0f,
                i % 3 == 1 ? 1.0f : 0.0f,
                i % 3 == 2 ? 1.0f : 0.0f);
            v[i] = vs[ts[i]];
            idx[i] = i;
        }

        trimesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        trimesh.SetVertices(v);
        trimesh.SetColors(c);
        trimesh.SetIndices(idx, MeshTopology.Triangles, 0, true, 0);
        trimesh.RecalculateNormals();

        var meshGO = GameObject.CreatePrimitive(PrimitiveType.Quad);
        meshGO.name = "Mesh";
        meshGO.transform.SetParent(AnchorGameObject.transform, false);
        meshGO.GetComponent<MeshFilter>().sharedMesh = trimesh;
        meshGO.GetComponent<MeshCollider>().enabled = false;
        //meshGO.GetComponent<MeshCollider>().sharedMesh = trimesh;
        //meshGO.GetComponent<MeshRenderer>().material.SetColor(
            //"_Color", UnityEngine.Random.ColorHSV());
    }

    public void UpdateMesh(OVRTriangleMesh mesh)
    {
        var meshGO = AnchorGameObject.transform.Find("Mesh");
        if (meshGO != null) UnityEngine.Object.Destroy(meshGO);
        CreateMesh(mesh);
    }

    /// <summary>
    /// A wrapper function for requesting Scene Capture.
    /// </summary>
    public static async Task<bool> RequestSceneCapture()
    {
        if (SceneCaptureRunning) return false;
        SceneCaptureRunning = true;

        var waiting = true;
        Action<ulong, bool> onCaptured = (id, success) => { waiting = false; };

        // subscribe, make non-blocking call, yield and wait
        return await Task.Run(() =>
        {
            OVRManager.SceneCaptureComplete += onCaptured;
            if (!OVRPlugin.RequestSceneCapture("", out var _))
            {
                OVRManager.SceneCaptureComplete -= onCaptured;
                SceneCaptureRunning = false;
                return false;
            }
            while (waiting) Task.Delay(200);
            OVRManager.SceneCaptureComplete -= onCaptured;
            SceneCaptureRunning = false;
            return true;
        });
    }
    private static bool SceneCaptureRunning = false; // single instance

    /// <summary>
    /// A wrapper function for requesting the Android
    /// permission for scene data.
    /// </summary>
    public static void RequestScenePermission()
    {
        const string permission = "com.oculus.permission.USE_SCENE";
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
            UnityEngine.Android.Permission.RequestUserPermission(permission);
    }
}
