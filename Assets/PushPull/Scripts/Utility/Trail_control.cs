using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_control : MonoBehaviour
{
    //The speed at which the tail disappears
    private float disappear_speed;

    //SkinnedMeshRenderer passed in from outside
    public SkinnedMeshRenderer skinned_mesh_renderer;

    //Baked mesh result
    private Mesh baked_mesh_result;

    //material
    private Material material;

    //alpha
    private float alpha;

    private float _startTime;

    public void init(float disappear_speed, SkinnedMeshRenderer skinned_mesh_renderer, float alpha, float delay)
    {
        this.disappear_speed = disappear_speed;
        this.skinned_mesh_renderer = skinned_mesh_renderer;
        this.alpha = alpha;

        if (this.baked_mesh_result == null)
        {
            this.baked_mesh_result = new Mesh();
        }

        //Render mesh
        this.skinned_mesh_renderer.BakeMesh(this.baked_mesh_result);
        this.GetComponent<MeshFilter>().mesh = this.baked_mesh_result;

        //Set the material of this object
        this.material = this.GetComponent<MeshRenderer>().material;
        material.SetFloat("_alpha", this.alpha);

        _startTime = Time.time + delay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.material) return;

        if (Time.time > _startTime)
        {
            this.alpha = Mathf.Lerp(this.alpha, 0, this.disappear_speed * Time.deltaTime);

            this.material.SetFloat("_alpha", this.alpha);

            if (this.alpha < 0.01f)
            {
                this.gameObject.SetActive(false);

            }
        }
    }
}