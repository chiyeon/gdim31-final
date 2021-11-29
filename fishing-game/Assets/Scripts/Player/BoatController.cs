using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField]
    private bool isNavigating = false;

    [Header("Player Search")]
    [SerializeField]
    private Transform playerSearch;
    [SerializeField]
    private float playerSearchRadius = 5.0f;
    [SerializeField]
    private LayerMask playerLayer;
    private bool isPlayerNear = false;

    [Header("References")]
    [SerializeField]
    private GameObject hitboxes;
    [SerializeField]
    private Transform playerNavPosition;


    void Update() {
        isPlayerNear = Physics.CheckSphere(playerSearch.position, playerSearchRadius, playerLayer);

        if(Input.GetButtonDown("Interact") && isPlayerNear) {
            SetNavigating(!isNavigating);
        }
    }

    void SetNavigating(bool _isNavigating) {
        isNavigating = _isNavigating;
        PlayerController.instance.SetMovementDisabled(isNavigating);
        
        if(isNavigating) {
            PlayerController.instance.transform.position = playerNavPosition.position;
        }

    }
}
