using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoIcon : MonoBehaviour
{
    public GameObject playerIcon;
    private GameObject instancedPlayerIcon;
    private GameObject minimapIconsContainer;
    void Start()
    {
        minimapIconsContainer = GameObject.FindGameObjectWithTag("MinimapIconsContainer");
        instancedPlayerIcon=Instantiate(playerIcon,transform.position,Quaternion.identity,minimapIconsContainer.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (instancedPlayerIcon == null)
        {
            instancedPlayerIcon = Instantiate(playerIcon, transform.position, Quaternion.identity, minimapIconsContainer.transform);
        }
        instancedPlayerIcon.transform.position=transform.position;  
    }
}
