using UnityEngine;
using System.Collections;

public class FrustrumPlanes : MonoBehaviour {
  [SerializeField]
  Camera cam;
  GameObject[] planes;
  [SerializeField]
  GameObject obj;

  [SerializeField]
  Material _cookieMaterial;

  float farClip;
  float farWidth;
  float farHeight;

	// Use this for initialization
	void Start () {


    int divs = 20;
    
//	planes = GeometryUtility.CalculateFrustumPlanes(cam);

		
  /*  for( int i= 0; i < planes.Length; i++) {
		 GameObject p= GameObject.CreatePrimitive(PrimitiveType.Plane);
			p.name = "Plane " + i.ToString();
			p.transform.position = -planes[i].normal * planes[i].distance;
			p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
      
		}*/

    planes = new GameObject[20];

    float far = cam.farClipPlane;
    farClip = far;
    Vector3 farBot = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, far));
    Vector3 farTop = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, far));
    Vector3 farRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, far));

    Vector3 nearBot = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
    Vector3 nearTop = cam.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 0.0f));
    Vector3 nearRight = cam.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));


    farWidth = Vector3.Distance(farBot, farTop);
    farHeight = Vector3.Distance(farBot, farRight);

    generatePlanes(divs);

	}

  // Update is called once per frame
  void Update()
  {
    updatePlanes();
  //  updatePlanes();

    /*
    if (GeometryUtility.TestPlanesAABB(planes, obj.collider.bounds))
      Debug.Log(obj.name + " has been detected!");
    else
      Debug.Log("Nothing has been detected");*/
	
	}
  
  float getWidthAtDistance(float dist)
  {
    return farWidth / dist;
  }

  float getHeightAtDistance(float dist)
  {
    return farHeight / dist;
  }

 void generatePlanes(int divs)
{
   
    planes = new GameObject[divs];

    
    for (int i = 0; i < divs; i++)
    {
      GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
      p.name = "Plane" + i;

      p.renderer.material = _cookieMaterial;


      planes[i] = p;
    }
    updatePlanes();
  }

  void updatePlanes()
  {
    int divs = planes.Length;
    float dist = Vector3.Distance(transform.position,cam.transform.position) /divs;


    for (int i = 1; i <= divs; i++)
    {

       GameObject p = planes[i-1];

      float width = getWidthAtDistance(i * dist);
      float height = getHeightAtDistance(i * dist);

      if (width == 0 || height == 0)
      {
        continue;
      }
      p.transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)) + cam.transform.forward.normalized * i * dist;

      p.transform.up = -cam.transform.forward;
      p.transform.localScale = new Vector3(farWidth / width / 10, 1.0f, farHeight / height / 10);
    }
  }



}
