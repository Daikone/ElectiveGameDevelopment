﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Resources.Scripts.Stijn
{
    public class WanderingState : State
    {

        private Transform target;
        private float timer;
        private Vector3 newPos;

        public override void Enter(StijnGhost owner)
        {
            timer = 0;
            Debug.Log("Wandering");

            newPos = RandomNavSphere(owner.transform.position, 10, -1);
            owner.navMeshAgent.SetDestination(newPos);
        }


        public override void LogicUpdate(StijnGhost owner)
        {
            float dist = owner.navMeshAgent.remainingDistance;
            timer += Time.deltaTime;

            if (timer <= 10)
            {
                timer = 0;
                Debug.Log(newPos);
            }

            if (dist != Mathf.Infinity && owner.navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete && owner.navMeshAgent.remainingDistance == 0)
            {
                Debug.Log("Getting a new destination");
                newPos = RandomNavSphere(owner.transform.position, 10, -1);
                owner.navMeshAgent.SetDestination(newPos);
            }


        }

        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

            return navHit.position;
        }
    }
}