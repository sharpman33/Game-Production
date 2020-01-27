using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MB_Canvas : MonoBehaviour
{
    public static MB_Canvas instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
