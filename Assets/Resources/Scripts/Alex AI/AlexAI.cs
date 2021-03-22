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
        public bool haveWaited;
        public bool waiting;
        public ParticleSystem Blood;
        public float SoulsOnMe;


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
            ResetTimer = 0;
            haveWaited = false;
            waiting = false;
        }

        // Update is called once per frame
        void Update()
        {
            //SoulsOnMe = getSouls();
            //Need to get souls every frame to keep up to date 
            //Debug.Log();
            
            if (CheckHumanInfront())
                ChaseHuman();
            else if (currentState == STATE.Patrol)
            {
                Patrolling();
            }

            if (haveWaited == false)
            {
                WaitForReset();
            }
        }

        void Patrolling()
        {
            if (agent.destination == PatrolDestination || PatrolDestination == new Vector3())
            {
                if (NavMesh.SamplePosition(new Vector3(Random.Range(-12, 12), 0, Random.Range(-12, 12)), out NavMeshHit EndLocation, 1.0f, 1))
                {
                    PatrolDestination = EndLocation.position;
                    agent.destination = PatrolDestination;
                    Debug.Log("New location added");

                    //if het gets stuck well reset the path and timer
                    if (haveWaited == true)
                    {
                        if (agent.destination == EndLocation.position || ResetTimer >= 400000)
                        {
                            ResetTimer = 0;
                            agent.ResetPath();
                            Debug.Log("Path reseted");
                            haveWaited = false;
                        }
                    }
                }
            }
        }
        //Timer Functions to stop spamming
        protected IEnumerator WaitForReset()
        {
             Debug.Log("waiting has started");
             yield return new WaitForSeconds(5);
        }


        //Human Hunter system
        protected bool CheckHumanInfront()
        {
            //currentState = STATE.Scanning;

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
                //Debug.Log("human not nul");
                Vector3 direction = currentHuman.transform.position - transform.position;
                direction.Normalize();

                agent.SetDestination(currentHuman.transform.position);
                transform.forward = new Vector3(direction.x, 0, direction.z);
            } else
            {
                currentState = STATE.Patrol;
            }
        }
        //Dunking souls
        protected void DepositSouls()
        {
            //WIP
        }
        //play sound when hitted a human
        private void OnCollisionEnter(Collision collision)
        {
         if(collision.gameObject.tag == "Human")
            {
                Blood.Play();

            }
        }

    }
}
