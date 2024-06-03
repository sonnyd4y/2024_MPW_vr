using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    public MeshFilter meshFilter;
    
    [ContextMenu("GetMesh")]
    public void LoadOBJModel(string path)
    {
        Debug.Log($"Loading OBJ file from: {path}");

        if (!File.Exists(path))
        {
            Debug.LogError($"File not found at path: {path}");
            return;
        }

        string objData = File.ReadAllText(path);
        Debug.Log(objData);
        Mesh newMesh = CreateMeshFromOBJ(objData);
        if (newMesh != null)
        {
            UpdateObject(newMesh);
        }
        else
        {
            Debug.LogError("Failed to create mesh from OBJ file.");
        }
    }
    

    private Mesh CreateMeshFromOBJ(string objData)
    {
        Mesh mesh = new Mesh();
        // 간단한 OBJ 데이터 파싱 (실제 사용 시 더 복잡한 파싱 필요)
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        string[] lines = objData.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("v "))
            {
                string[] parts = line.Split(' ');
                float x = float.Parse(parts[1]);
                float y = float.Parse(parts[2]);
                float z = float.Parse(parts[3]);
                vertices.Add(new Vector3(x, y, z));
            }
            else if (line.StartsWith("f "))
            {
                string[] parts = line.Split(' ');
                int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                int v3 = int.Parse(parts[3].Split('/')[0]) - 1;
                triangles.Add(v1);
                triangles.Add(v2);
                triangles.Add(v3);
            }
        }

        if (vertices.Count > 0 && triangles.Count > 0)
        {
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            Debug.Log("Mesh created successfully from OBJ file.");
            return mesh;
        }
        else
        {
            Debug.LogError("No vertices or triangles found in OBJ file.");
            return null;
        }
    }
    
    private void UpdateObject(Mesh newMesh)
    {
        if (meshFilter != null)
        {
            Debug.Log("Updating target object's mesh.");
            meshFilter.mesh = newMesh;
        }
        else
        {
            Debug.LogError("Target object does not have a MeshFilter component.");
        }
    }

}
