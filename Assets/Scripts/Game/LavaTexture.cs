using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LavaTexture : MonoBehaviour
{


    public float duration = 0.5f;
    public Texture[] textures;
    public new Renderer renderer;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DoTextureLoop());
        //GetComponent<Renderer>();
    }

    public IEnumerator DoTextureLoop()
    {
        int i = 0;
        while (true)
        {
            renderer.material.mainTexture = textures[i];
            i = (i + 1) % textures.Length;
            yield return new WaitForSeconds(duration);
        }
    }
}
