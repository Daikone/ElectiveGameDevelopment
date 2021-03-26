﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace HaniAISpace
{
    public enum STATE { Idle, Hunting, RoomChange, SoulDeposit } 
    public enum INTERACTABLE{ None, Ghost, Pickup, Human}
    
    public class HaniAI : BaseGhostAI
    {
        
        public List<Transform> rooms;
        
        private STATE currentState;
        private INTERACTABLE Interactable;
        private Vector3 targetRoomPos;
        private GameObject currentChasableObject;
        private GhostBehaviour gb;
        private int randomNumber;
        
        void Start()
        {
            gb = gameObject.GetComponent<GhostBehaviour>();
            currentState = STATE.Idle;
            //sheck if human in view
        }

       void Update()
       {
           CheckState();
           Debug.Log("HaniAI currentState = " + currentState);
       }

        private void CheckState()
        {
            if (gb.getSouls() >= 3)
                DepositSouls();
            else
                CheckCloseInteractable();
            
            switch (currentState)
            {
                case STATE.SoulDeposit:
                    if(Vector3.Distance(transform.position, targetRoomPos) < .1f)
                        currentState = STATE.Idle;
                    break;
            
                case STATE.Hunting:
                    if (currentChasableObject == null)
                        currentState = STATE.Idle;
                    else if (currentChasableObject != null && Vector3.Distance(transform.position, currentChasableObject.transform.position) >= 10f)
                        currentChasableObject = null;
                    break;
            
                case STATE.RoomChange:
                    if (Vector3.Distance(transform.position, targetRoomPos) <= 1f)
                        currentState = STATE.Idle;
                    break;
            
                case STATE.Idle: MoveToRandomPoint(); break;
            }
        }

        private void MoveToRandomPoint()
        {
            
            if(currentState == STATE.Idle)
              randomNumber = Random.Range(0, rooms.Count);
            
            currentState = STATE.RoomChange;
            targetRoomPos = rooms[randomNumber].position;
            agent.SetDestination(targetRoomPos);
        }
        
        private INTERACTABLE CheckObjectsInfront()
        {
            Collider[] objNearby = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Humans", "Pickups"));

            GameObject nearestObject = CheckClosestObject(objNearby);
            
            if (nearestObject != null)
            {
                if (gb.hasPickup == false && gb.isPickupActive == false && nearestObject.CompareTag("Pickup"))
                {
                    currentChasableObject = nearestObject;
                    return INTERACTABLE.Pickup;
                }
            
                if (nearestObject.CompareTag("Human"))
                {
                    currentChasableObject = nearestObject;
                    return INTERACTABLE.Human; 
                }
                    
            }

            

            return INTERACTABLE.None;
        }    

        void DepositSouls()
        {
            currentState = STATE.SoulDeposit;
            targetRoomPos = rooms[0].position;
            agent.SetDestination(targetRoomPos);
        }

        void ChaseObject()
        {
            currentState = STATE.Hunting;

            if (currentChasableObject != null)
                agent.SetDestination(currentChasableObject.transform.position);
        }

        private GameObject CheckClosestObject(Collider[] objList)
        {
            GameObject closeObj;

            if (objList.Length > 0)
            {
                closeObj = objList[0].gameObject;
                foreach (var obj in objList)
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) <
                        Vector3.Distance(transform.position, closeObj.transform.position))
                        closeObj = obj.gameObject;
                }
                
                return closeObj;
            }

            return null;
        }

        private void CheckCloseInteractable()
        {
            switch (CheckObjectsInfront())
            {
                case INTERACTABLE.Human:
                    RaycastHit hit;
                    if (!Physics.Linecast(transform.position, currentChasableObject.transform.position, out hit,
                        LayerMask.GetMask("Walls")))
                        ChaseObject();
                    else
                        currentState = STATE.Idle;
                    break;
                
                case INTERACTABLE.Pickup: 
                    ChaseObject();
                    break;
            }
        }
    }
}