using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class GuestBehavior : MonoBehaviour
{
    public CarController cc;
    public GameObject boden;
    public int g_ID;
    private int desX;
    private int desY;
    private bool arrived = false;
    private bool graceperiod = false;
    private bool graceperiodLock = false;
    private GameObject Boden;
    
    // Start is called before the first frame update
    void Start()
    {     
        switch(g_ID) {
            case 1:
            desX = -47;
            desY = -226; 
            break;
        }

        if(cc==null)
            cc=FindObjectOfType<CarController>();
        
        Boden = Instantiate(boden, transform.position + new Vector3(0,-1,0), UnityEngine.Quaternion.identity);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !graceperiod) {
            graceperiod = true;
            graceperiodLock = true;
            Debug.Log("collision with player detected");
            if(arrived == false)
            {
                Debug.Log("guest picked up, hiding guest model at target");
                if(Mathf.Abs(cc.getSpeed()) <= 20)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    transform.position = new Vector3(desX, -3, desY); 
                    arrived = true;
                    Boden.transform.position = transform.position + new Vector3(0,5,0);
                }
            }
            else
            if(Mathf.Abs(cc.getSpeed()) <= 20)
            {
                Debug.Log("guest arrived at target, showing guest model at target");
                GetComponent<Collider>().enabled = false;
                Destroy(Boden);
                transform.position = new Vector3(desX, 3, desY); 
            }
        }
        graceperiodLock = false;
    }

    void LateUpdate() {
        // set graceperiod to false to allow pickup detection after all updates ran
        if (!graceperiodLock && graceperiod) {
            graceperiod = false;
        }
    }
}
