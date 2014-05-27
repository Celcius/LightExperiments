using UnityEngine;
using System.Collections;

public class SamplerPlane : MonoBehaviour {

  const int TEX_DIM = 16;
  const int planeSize = 5;

  Texture2D _planeTex;

  Color _col = new Color(0.388f, 0.386f, 0.157f);

	// Use this for initialization
	void Start () {

    

	}

  public Texture2D getIntanceTex()
  {
    return _planeTex;
  }

  public static Texture2D getStartTexture()
  {
    Texture2D tex = new Texture2D(TEX_DIM, TEX_DIM);
    for(int x = 0; x <= TEX_DIM; x++)
      for (int y = 0; y <= TEX_DIM; y++)
      {
        tex.SetPixel(x, y, new Color(0.388f, 0.386f, 0.157f));
      }
    return tex;
  }

  public Texture2D generateTexture(Vector3 rayDir, float rayDistance, float scale, Texture2D oriTex, Collider []lightVolumes)
  {


    Texture2D tex = new Texture2D(TEX_DIM, TEX_DIM);

    Vector3 oripos = transform.position - planeSize * transform.right * scale - planeSize * transform.forward * scale;
    float segment = (planeSize * 2*scale)/ TEX_DIM;
    Vector3 pos = oripos;
    //print(transform.right + " " + transform.forward+ " "+ segment);
    
    
    
    for (int x = 0; x <= TEX_DIM; x++)
    {
      for (int y = 0; y <= TEX_DIM; y++)
      {
        pos = oripos + x * transform.forward * segment + y * transform.right * segment;
        Color oriColor = oriTex.GetPixel(TEX_DIM - x, TEX_DIM - y);


        if (Physics.Raycast(pos, rayDistance * rayDir))
        {
          tex.SetPixel(TEX_DIM - x, TEX_DIM - y, Color.black);
          Debug.DrawRay(pos, rayDistance * rayDir, Color.red);

        }
        else
        {

         // if (Physics.Raycast(pos, rayDistance * rayDir, LayerMask.NameToLayer("Light Volumes")))
          tex.SetPixel(TEX_DIM - x, TEX_DIM - y, oriColor);
        //  else
          //  tex.SetPixel(TEX_DIM - x, TEX_DIM - y, Color.black);
          
           
          //else
            // Debug.DrawRay(pos, rayDistance * rayDir, Color.white);

        }
       
        

      }

    }

    tex.Apply();
    renderer.material.SetTexture("_node_4", tex);
    _planeTex = tex;


    return tex;
  }


}
