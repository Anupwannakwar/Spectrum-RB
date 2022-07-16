using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEffect : MonoBehaviour
{
    public void disableForceField()
    {
        this.gameObject.SetActive(false);
    }
}
