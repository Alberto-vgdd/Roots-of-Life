using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LavaTexture : MonoBehaviour
{


    public float duration = 0.5f;
    public Texture[] textures;
    private new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        renderer = GetComponent<Renderer>();
        StartCoroutine(DoTextureLoop());
    }

    private void Awake()
    {
    }

    public IEnumerator DoTextureLoop()
    {
        int i = 0;
        while (true)
        {
            renderer.material.mainTexture = textures[i];
            renderer.material.SetTexture("_EmissionMap", textures[i]);
            i = (i + 1) % textures.Length;
            yield return new WaitForSeconds(duration);
        }
    }
}
