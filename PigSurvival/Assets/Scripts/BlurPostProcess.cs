using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurPostProcess : MonoBehaviour
{

    public RenderTexture blurRT;
    public Material blurMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        blurMaterial.SetTexture("_BlurTex", blurRT);
        Graphics.Blit(source, destination, blurMaterial);
    }
}
