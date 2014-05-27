using UnityEngine;
using System.Collections;

public class volumeGenerator : MonoBehaviour {


  [SerializeField]
  float _planeRatio;
  int _planeCount;

  [SerializeField]
  float _planeScale = 1.0f;

  [SerializeField]
  Transform _mainCam;

  [SerializeField]
  Material _cookieMaterial;

  GameObject []_planes;

  [SerializeField]
  float refreshTime = 0.2f;

  float accumTime= 0;
  
  [SerializeField]
  Collider []_lightVolumes;

  Vector3 _dirVector;
	// Use this for initialization
	void Start () {
    //_mainCam = Camera.main.transform;
    _planes = new GameObject[_planeCount];
    accumTime = 0;
	}

  bool updated = false;
	// Update is called once per frame
	void Update () {

   
    if (_mainCam != null && !updated)
    {

      accumTime += Time.deltaTime;

      if (accumTime > refreshTime)
        accumTime = 0;
      else
        return;


      _dirVector = _mainCam.position - transform.position;
      
      // compute distances
      float totalDistance = Mathf.Sqrt(_dirVector.x * _dirVector.x + _dirVector.y * _dirVector.y + _dirVector.z * _dirVector.z);

      // delete old planes
      for (int i = 0; i < _planeCount; i++)
      {
        deletePlane(_planes[i]);
      }

      // calculate new plane count
      _planeCount =  (int) (_planeRatio * totalDistance);
      if (_planeCount <= 0)
        return;

      _planes = new GameObject[_planeCount];


      float distanceSegment = totalDistance / _planeCount;
  
      // Step 1.


      // Step 2.
      for (int i = 0; i < _planeCount; i++)
      {
        deletePlane(_planes[i]);
        _planes[i] = generatePlane(_dirVector.normalized, distanceSegment, i,( transform.position-_mainCam.position).normalized);
      }


      // Step 3.


    }
	}


  GameObject generatePlane(Vector3 dir, float distanceSegment,int pos, Vector3 invertedDir)
  {
    Texture2D tex;
    if( pos == 0)
      tex = SamplerPlane.getStartTexture();
    else
      tex = _planes[pos-1].GetComponent<SamplerPlane>().getIntanceTex();

    Vector3 pos0 = transform.position;
    float distance = distanceSegment * pos - 1;
    GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

    plane.transform.position = new Vector3(pos0.x + dir.x * distance, pos0.y + dir.y * distance, pos0.z + dir.z * distance);
    


    plane.transform.up = dir;

    plane.transform.localScale = new Vector3(_planeScale, _planeScale, _planeScale);
    plane.renderer.material = _cookieMaterial;
    plane.layer = LayerMask.NameToLayer("Ignore Raycast");
    Destroy(plane.gameObject.GetComponent("MeshCollider"));
    plane.AddComponent<SamplerPlane>();

    plane.GetComponent<SamplerPlane>().generateTexture(invertedDir, distanceSegment, _planeScale, tex, _lightVolumes);
   
    return plane;
  }

  void deletePlane(GameObject plane)
  {
    Destroy(plane);
  }
}
