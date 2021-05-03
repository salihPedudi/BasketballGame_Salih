using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer) )]
public class MaterialController : MonoBehaviour
{
   
    [SerializeField]private Renderer materialRenderer;
    private MaterialPropertyBlock materialPropertyBlock;
    
    private void Awake()
    {       
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    public virtual void SetMaterialColor(Color _color)
    {
        if(materialPropertyBlock == null)
        {
            materialPropertyBlock = new MaterialPropertyBlock();
        }        

        materialPropertyBlock.SetColor("_Color",_color);       
        materialRenderer.SetPropertyBlock(materialPropertyBlock,0);       

    }
}
