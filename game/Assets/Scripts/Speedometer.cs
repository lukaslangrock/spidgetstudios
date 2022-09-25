using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
          public Text SpeedText;
	    public CarController CCc;

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame 
    void Update()
    {
	if(CCc==null)
		CCc=FindObjectOfType<CarController>();
  else
  {
      if(CCc.speed>=0)
      SpeedText.text="v="+Mathf.Round(Mathf.Sqrt(CCc.speed)).ToString()+"km/h";
      else
      SpeedText.text="v="+(-Mathf.Round(Mathf.Sqrt(-CCc.speed))).ToString()+"km/h";
 
  }
    }
}
