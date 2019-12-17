using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour {

    public GameObject parentObject;

    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> normals = new List<Vector3>();
    List<List<int>> triangles = new List<List<int>>();

    List<Material> materials = new List<Material>();

    int vertCount = 0;

    MeshRenderer mR;
    MeshFilter mF;

    int meshCount = 0;
    int subMeshCount = 0;

    private void Start() {

        mR = gameObject.AddComponent<MeshRenderer>();
        mF = gameObject.AddComponent<MeshFilter>();

        CombineMeshes();

    }

    public void CombineMeshes () {

        Debug.Log("Starting Mesh Combiner...");

        // Get an array of all the child objects of our parentObject.
        Transform[] children = parentObject.GetComponentsInChildren<Transform>();

        // Loop through each of those children.
        foreach (Transform child in children) {

            // Get MeshRenderer and MeshFilter.
            MeshRenderer meshR = child.GetComponent<MeshRenderer>();
            MeshFilter meshF = child.GetComponent<MeshFilter>();

            // We're only interested in children that have a Renderer and Filter so check for that.
            if (meshR != null && meshF != null) {

                // Loop through each of the materials in in the current MeshRenderer.
                foreach (Material mat in meshR.sharedMaterials) {

                    // If our materials list doesn't contain the current material, add it and create a new triangles list for the submesh.
                    if (!materials.Contains(mat)) {

                        materials.Add(mat);
                        triangles.Add(new List<int>());

                        // The materials and submesh index are the same, so from herein, we use the same index for both.

                    }

                }

                // Add verts and triangles. Can't use AddRange because we need to modify each entry.
                foreach (Vector3 vert in meshF.mesh.vertices)
                    vertices.Add(child.TransformPoint(vert));

                // Loop through each submesh in the current mesh.
                for (int i = 0; i < meshF.mesh.subMeshCount; i++) {

                    // Check the current material against our materials list to get the right triangle index.
                    int triIndex = GetTriangleIndex(meshR.sharedMaterials[i]);

                    // For each submesh, get the triangles and loop through, modifying each one to account for existing verts.
                    int[] tris = meshF.mesh.GetTriangles(i);
                    for (int t = 0; t < tris.Length; t++) {

                        triangles[triIndex].Add(vertCount + tris[t]);

                    }

                    subMeshCount++;

                }

                // Add UV and normals data. This can be copied directly across.
                uvs.AddRange(meshF.mesh.uv);
                normals.AddRange(meshF.mesh.normals);

                vertCount = vertices.Count;

                meshCount++;

            }

        }

        // Create a new mesh to put our data into.
        Mesh mesh = new Mesh();

        // If vertcount is greater than 65535 we need to change the IndexFormat to 32bit.
        if (vertices.Count > 65535)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.subMeshCount = triangles.Count;
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normals.ToArray();

        // Loop through each triangle list and set them.
        for (int i = 0; i < triangles.Count; i++) {
            mesh.SetTriangles(triangles[i].ToArray(), i);
        }

        // Apply our mesh and materials to the gameobject.
        mR.sharedMaterials = materials.ToArray();
        mF.mesh = mesh;

        Debug.Log("Mesh Combining Complete.");
        Debug.Log("Mesh Index Format: " + ((mesh.indexFormat == UnityEngine.Rendering.IndexFormat.UInt16) ? "UInt16" : "UInt32"));
        Debug.Log("Total Meshes Combined: " + meshCount);
        Debug.Log("Total Submeshes: " + subMeshCount);
        Debug.Log("Total Materials: " + materials.Count);

    }

    int GetTriangleIndex (Material material) {

        int index = 0;

        for (int i = 0; i < materials.Count; i++) {

            if (material == materials[i]) {
                index = i;
                break;
            }

        }

        return index;

    }

}