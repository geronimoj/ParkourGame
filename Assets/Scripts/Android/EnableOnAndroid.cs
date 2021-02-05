using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Disables then deletes the GameObject this script is attached to if
/// we are setup for a windows build
/// </summary>
public class EnableOnAndroid : MonoBehaviour
{
#if UNITY_STANDALONE_WIN
    /// <summary>
    /// Called the moment the program starts to delete the GameObject if
    /// this is a windows build
    /// </summary>
    void Awake()
    {
        //If we are not in Android mode, disable this gameObject then destroy it
        gameObject.SetActive(false);
        Destroy(this);
}
#endif
}
