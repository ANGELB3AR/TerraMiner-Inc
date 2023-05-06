using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    bool buildComplete = false;
    float buildProgress = 0f;

    public bool GetBuildCompleteStatus()
    {
        return buildComplete;
    }
}
