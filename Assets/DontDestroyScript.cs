using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyScript : MonoBehaviour
{
    
    void Start()
    {
        if(GameObject.FindGameObjectsWithTag("BgMusicOpenWorld").Count() == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

}
