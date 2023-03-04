using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobile : MonoBehaviour
{
    [SerializeField]
    private GameObject _mobile;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        _mobile.SetActive(true);
#else
        _mobile.SetActive(false);
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
