using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CrowdManager : MonoBehaviour
{
    [SerializeField] protected Image crowdCheer1;
    [SerializeField] protected Image crowdCheer2;
    [SerializeField] protected Image crowdCheer3;
    public void CrowdCheer()
    {
        StartCoroutine(Crowd());
    }
    protected virtual IEnumerator Crowd()
    {
        crowdCheer1.enabled = true;
        crowdCheer2.enabled = true;
        crowdCheer3.enabled = true;
        yield return new WaitForSeconds(1f);
        crowdCheer1.enabled = false;
        crowdCheer2.enabled = false;
        crowdCheer3.enabled = false;



    }
}
