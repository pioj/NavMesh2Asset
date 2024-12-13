using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class NavMesh2Asset : MonoBehaviour
{
    [MenuItem("GamaScorpio/Create Mesh from NavMesh")]
    static void ExportNavMesh()
    {
        NavMeshTriangulation triangulatedNavMesh = NavMesh.CalculateTriangulation();
 
        var clone = new GameObject("BakedNav01", typeof(MeshFilter));
        var clonecomp = clone.GetComponent<MeshFilter>();
        
        clonecomp.sharedMesh = new Mesh();
        clonecomp.sharedMesh.name = "exported";
        clonecomp.sharedMesh.vertices = triangulatedNavMesh.vertices;
        clonecomp.sharedMesh.triangles = triangulatedNavMesh.indices;
        
        clonecomp.sharedMesh.RecalculateBounds();
        clonecomp.sharedMesh.RecalculateNormals();
        clonecomp.sharedMesh.RecalculateTangents();
        MeshUtility.Optimize(clonecomp.sharedMesh);

        clone.AddComponent<MeshRenderer>();
        var mat = new Material(Shader.Find("Standard"));
        clone.GetComponent<MeshRenderer>().sharedMaterial = mat;
        
        SaveMesh(clonecomp.sharedMesh, clonecomp.sharedMesh.name, true);
        
    }

    public static void SaveMesh (Mesh mesh, string name, bool makeNewInstance) {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;
        
        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;
		
        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
