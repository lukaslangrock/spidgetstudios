using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class GastBehavior : MonoBehaviour
{
    public int g_ID;
    public bool uberThere = false;
    private bool arrived = false;
    private int desX;
    private int desY;
    public CarController cc;
    public Transform bodenPos;
    public GameObject boden;
    private GameObject Boden;
    


    // Start is called before the first frame update
    void Start()
    {
        main();
        Boden = Instantiate(boden, transform.position + new Vector3(0,-1,0), UnityEngine.Quaternion.identity);
    }

    void main()
    {   
         switch(g_ID)
        {
            case 1:
            desX = -47;
            desY = -226; 
            break; }
           /* case 2:
            desX = ;
            desY = ; 
            break;
            case 3:
            desX = ;
            desY = ; 
            break;
            case 4:
            desX = ;
            desY = ; 
            break;
            case 5:
            desX = ;
            desY = ; 
            break;
            case 6:
            desX = ;
            desY = ; 
            break;
            case 7:
            desX = ;
            desY = ; 
            break;
            case 8:
            desX = ;
            desY = ; 
            break;
            case 9:
            desX = ;
            desY = ; 
            break;
            case 10:
            desX = ;
            desY = ; 
            break;

        }
        */
        
    }


private void OnTriggerStay(Collider other)
    {
        if(uberThere == false)
        {
            if(Mathf.Abs(cc.speed) <= 20)
            { 
                Debug.Log("Es funktioniert jetzt");
                
                GetComponent<MeshRenderer>().enabled = false;
                transform.position = new Vector3(desX, -3, desY); 
                uberThere = true;
                Boden.transform.position = transform.position + new Vector3(0,5,0);
                
                
            }
        }
        else
        if(Mathf.Abs(cc.speed) <= 20)
            { 
                Debug.Log("Es funktioniert");
                arrived = true;
                GetComponent<Collider>().enabled = false;
                Destroy(Boden);
                transform.position = new Vector3(desX, 3, desY); 
            }
    }

    // Update is called once per frame
    void Update()
    {

            if(cc==null)
            cc=FindObjectOfType<CarController>();
    }
}
