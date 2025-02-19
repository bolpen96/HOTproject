using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBMPanel : MonoBehaviour
{
    public GameObject Mine;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(Mine, 2f);
    }

}
