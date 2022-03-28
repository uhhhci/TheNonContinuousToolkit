using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SlicingManager : MonoBehaviour
{
    public static SlicingManager _instance;
    private bool _combineCutMeshes = false;
    public Transform _cuttingPlane;
    public GameObject _target;
    public Material _crossSectionMaterial;
    public float _seperation = 0.05f;

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Slice(GameObject target, Transform cuttingPlane, Vector3 positionalOffset, Vector3 rotationalOffset)
    {
        _cuttingPlane.transform.position = cuttingPlane.transform.position + positionalOffset;
        _cuttingPlane.transform.rotation = cuttingPlane.transform.rotation * Quaternion.Euler(rotationalOffset);
        _target = target;
        Slice();
    }

    public void Slice(GameObject target, Transform cuttingPlane)
    {
        _cuttingPlane.transform.position = cuttingPlane.transform.position;
        _cuttingPlane.transform.rotation = cuttingPlane.transform.rotation;
        _target = target;
        Slice();
    }

    public void Slice()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(_target.GetComponentsInChildren<MeshFilter>());

        if (meshFilters.Count > 32)
        {
            _combineCutMeshes = true;
        }
        else
        {
            _combineCutMeshes = false;
        }

        foreach (MeshFilter meshFilter in meshFilters)
        {
            SlicedHull hull = meshFilter.gameObject.Slice(_cuttingPlane.position, _cuttingPlane.up, _crossSectionMaterial);

            if (hull != null)
            {
                GameObject upperHull = hull.CreateUpperHull(meshFilter.gameObject, _crossSectionMaterial);
                GameObject lowerHull = hull.CreateLowerHull(meshFilter.gameObject, _crossSectionMaterial);


                upperHull.transform.SetParent(meshFilter.transform);
                lowerHull.transform.SetParent(meshFilter.transform);

                upperHull.transform.localPosition = Vector3.zero;
                lowerHull.transform.localPosition = Vector3.zero;

                upperHull.transform.localRotation = Quaternion.identity;
                lowerHull.transform.localRotation = Quaternion.identity;

                upperHull.transform.Translate(_cuttingPlane.up * (_seperation * 0.5f), Space.World);
                lowerHull.transform.Translate(_cuttingPlane.up * -(_seperation * 0.5f), Space.World);

                upperHull.transform.SetParent(null);
                lowerHull.transform.SetParent(null);

                //upperHull.AddComponent<Sliceable>();
                //lowerHull.AddComponent<Sliceable>();
                upperHull.AddComponent<Dissolve>();
                lowerHull.AddComponent<Dissolve>();
                upperHull.AddComponent<MeshCollider>().convex = true;
                lowerHull.AddComponent<MeshCollider>().convex = true;
                upperHull.AddComponent<Rigidbody>().AddForce((upperHull.transform.position - lowerHull.transform.position).normalized * 2f, ForceMode.Impulse);
                lowerHull.AddComponent<Rigidbody>().AddForce((upperHull.transform.position - lowerHull.transform.position).normalized * -2f, ForceMode.Impulse);

                Destroy(upperHull, 3f);
                Destroy(lowerHull, 3f);

                if (_combineCutMeshes)
                {
                    CombineInstance[] combine = new CombineInstance[2];

                    combine[0].mesh = upperHull.GetComponent<MeshFilter>().sharedMesh;
                    if (!meshFilter.gameObject.name.Contains("Outline"))
                    {
                        combine[0].mesh.vertices = upperHull.GetComponent<MeshFilter>().sharedMesh.vertices;
                        combine[0].mesh.triangles = upperHull.GetComponent<MeshFilter>().sharedMesh.triangles;
                    }

                    combine[1].mesh = lowerHull.GetComponent<MeshFilter>().sharedMesh;
                    if (!meshFilter.gameObject.name.Contains("Outline"))
                    {
                        combine[1].mesh.vertices = lowerHull.GetComponent<MeshFilter>().sharedMesh.vertices;
                        combine[1].mesh.triangles = lowerHull.GetComponent<MeshFilter>().sharedMesh.triangles;
                    }

                    Matrix4x4 upperHullTransform = meshFilter.transform.worldToLocalMatrix;
                    Matrix4x4 lowerHullTransform = meshFilter.transform.worldToLocalMatrix;

                    combine[0].transform = upperHullTransform * upperHull.transform.localToWorldMatrix;
                    combine[1].transform = lowerHullTransform * lowerHull.transform.localToWorldMatrix;

                    meshFilter.mesh = new Mesh();
                    meshFilter.mesh.CombineMeshes(combine, false);

                    Destroy(upperHull);
                    Destroy(lowerHull);
                }
                else
                {
                    //upperHull.layer = LayerMask.NameToLayer("Item");
                    //lowerHull.layer = LayerMask.NameToLayer("Item");

                    //Destroy(meshFilter.GetComponent<MeshRenderer>());
                    //Destroy(meshFilter.GetComponent<MeshFilter>());

                    Destroy(meshFilter.gameObject);
                }
            }

        }
    }
}
