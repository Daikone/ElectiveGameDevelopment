using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AlexAISpace
{
    //All the AI States 
    public enum STATE
    {
        Idle,
        Patrol,
        Hunting,
        Dunking
    }

    public class AlexAI : BaseGhostAI
    {
        //General AI var

        private GameObject currentHuman;
        private Vector3 PatrolDestination;
        public int ResetTimer;

        private bool arrived;
        private Vector3 NewLocationForAI;
        public int StopHuntingTimer;
        public GameObject BloodPrefab;
        public GameObject Cauldren;
        public int SoulsOnMe;


        //Statemachine var
        private STATE currentState = STATE.Patrol;

        //Ability var
        //private ABILITY currentAbility;


        // Start is called before the first frame update
        void Start()
        {
            agent.speed = GetSpeed();
            //currentAbility = ABILITY.none;
            currentState = STATE.Patrol;
            arrived = true;
            NewLocationForAI = new Vector3(0,0,0);

        }

        // Update is called once per frame
        void Update()
        {
            if(SoulsOnMe >= 3)
                DunkSouls();
            else if (CheckHumanInfront())
                ChaseHuman();
            else if (currentState == STATE.Patrol && arrived == true)//Arrived is to make sure that it does not keep firing 24/7 and cause a huge preformance cost
            {
                Patrolling();
            }
            if(agent.transform.position == NewLocationForAI)
            {
                arrived = true;
            }
           
        }

        private void FixedUpdate()
        {
            //This is a reset system to prevent the agent from buggin out if was not able to go to a destination set by Patrolling
            if (currentState == STATE.Patrol)
            {
                ResetTimer++;
                if (NewLocationForAI != agent.transform.position)
                {
                    if (ResetTimer >= (6 * 60))
                    {
                        arrived = true;
                        Debug.Log("reseted death");
                        ResetTimer = 0;
                    }
                }
                else
                {
                    //reset the timer if agent is at the location
                    ResetTimer = 0;
                    Debug.Log("Not Needed to reset");
                }
            }
            else
            {
                ResetTimer = 0;
            }
            
        }
        void Patrolling()
        { 
                if (agent.destination == PatrolDestination || PatrolDestination == new Vector3())
                {
                    if (NavMesh.SamplePosition(new Vector3(Random.Range(-16, 16), 0, Random.Range(-16, 16)) + transform.position, out NavMeshHit EndLocation, 1.0f, NavMesh.AllAreas))
                    {
                        PatrolDestination = EndLocation.position;
                        agent.destination = PatrolDestination;
                        NewLocationForAI = PatrolDestination;
                        Debug.Log("New location added");
                        arrived = false;
                    }
                }
        }


        //Human Hunter system
        protected bool CheckHumanInfront()
        {

            Collider[] humansNearby = Physics.OverlapSphere(transform.position, 3);

            foreach (Collider human in humansNearby)
            {
                if (human.CompareTag("Human"))
                {
                    currentHuman = human.gameObject;
                    return true;
                }
            }
            return false;
        }
        protected void ChaseHuman()
        {
                currentState = STATE.Hunting;
                if (currentHuman != null)
                {
                    agent.SetDestination(currentHuman.transform.position);
                }
            }
        //Dunking souls
        protected void DunkSouls()
        {
            currentState = STATE.Dunking;
            agent.SetDestination(Cauldren.transform.position);
        }
        //play sound when hitted a human
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Human")
            {
                currentState = STATE.Patrol;
                arrived = true;
                SoulsOnMe ++;

                Instantiate(BloodPrefab, transform.position, Quaternion.identity);


            }
            //if dunked go back to patrol
            else if(collision.gameObject.tag == "Cauldron")
            {
                if (SoulsOnMe >= 3)
                {
                    SoulsOnMe = 0;
                    currentState = STATE.Patrol;
                    agent.ResetPath();
                    arrived = true;
                }

            }
        }

    }
}
