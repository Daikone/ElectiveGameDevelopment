using UnityEngine;
using UnityEngine.AI;

namespace AlexAISpace
{
    //All the AI States more states were not nessesarry  
    public enum STATE
    {
        Idle,
        Patrol,
        Hunting,
        Dunking
    }

    public class AlexAI : BaseGhostAI
    {
        //Human hunting Vars
        private GameObject currentHuman;

        //Patrolling and debugging for AI
        private Vector3 PatrolDestination;
        protected int ResetTimer;
        private bool arrived;
        private Vector3 NewLocationForAI;

        //Dunking System
        public GameObject Cauldren;
        protected int SoulsOnMe;

        //Statemachine var
        private STATE currentState = STATE.Patrol;


        // Start is called before the first frame update
        void Start()
        {
            agent.speed = GetSpeed();
            //currentAbility = ABILITY.none;
            currentState = STATE.Patrol;
            arrived = true;
            NewLocationForAI = new Vector3(0,0,0);

        }
        /// <summary>
        /// 
        /// General Description of how the AI works
        /// 1. First it is gonna check how many souls I have
        /// 2. Then it will look if there is any human nearby
        /// 3. If both are no then go a random posistion on the map
        /// 
        /// repeat from step one
        /// 
        /// </summary>

        // Update is called once per frame
        void Update()
        {
            // I'm pretty sure you knew, but what happens when three of the four cases are true?
            // If you didn't know why: first true has priority. It looks like this was the general idea however.
            if(SoulsOnMe >= 3)
                DunkSouls();
            else if (CheckHumanInfront())
                ChaseHuman();
            else if (currentState == STATE.Patrol && arrived == true)//Arrived is to make sure that it does not keep firing 24/7 and cause a huge preformance cost
            {
                Patrolling();
            }
            if(agent.transform.position == NewLocationForAI)//Has the AI arrived at its destination? yes good give him a new one
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
        //The patrolling AI works as following
        void Patrolling()
        {       
                //First check if he is at it's destination or if he has not yet have a destination
                if (agent.destination == PatrolDestination || PatrolDestination == new Vector3())
                {
                    //Give him a random destination
                    if (NavMesh.SamplePosition(new Vector3(Random.Range(-16, 16), 0, Random.Range(-16, 16)) + transform.position, out NavMeshHit EndLocation, 1.0f, NavMesh.AllAreas))
                    {
                    //Store this and give him a destination
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
        //Hunt the human down and get his/her soul!
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
                //put him back in a patrol mode
                currentState = STATE.Patrol;
                arrived = true;
                SoulsOnMe ++;



            }
            //if dunked go back to patrol. The reason for this course of action
            //is that I was unable to get the code from ghostbehavior script
            //that is why I made my own
            //(it does not intervere with the game)
            else if(collision.gameObject.tag == "Cauldron")
            {
                if (SoulsOnMe >= 3)
                {
                    SoulsOnMe = 0;
                    currentState = STATE.Patrol;
                    Patrolling();
                }

            }
        }

    }
}
