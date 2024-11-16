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
        instancedPlayerIcon = Instantiate(
                playerIcon, transform.position,
                Quaternion.identity, minimapIconsContainer.transform
                );
    }

    // Update is called once per frame
    void Update()
    {
        if (instancedPlayerIcon == null)
        {
            if (minimapIconsContainer == null) 
                minimapIconsContainer = GameObject.FindGameObjectWithTag("MinimapIconsContainer");
            if(minimapIconsContainer == null) {
                Debug.LogWarning("minimapIconsContainer es null, saliendo de la función");
                return;
            }
            instancedPlayerIcon = Instantiate(
                    playerIcon, transform.position, 
                    Quaternion.identity, minimapIconsContainer.transform
                    );
        }
        instancedPlayerIcon.transform.position = transform.position;  
    }
}
