using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading;

public class RacerAI : MonoBehaviour
{

    private NEATGeneticControllerV2 controller;
    public List<Rigidbody2D> bodies;
    private NEATNet net;
    private bool isActive = false;
    private const string ACTION_ON_FINISHED = "OnFinished";
    public delegate void TestFinishedEventHandler(object source, EventArgs args);
    public event TestFinishedEventHandler TestFinished;

    float ProFit = 1;
    float AntiFit = 0;
    float SwarmFit = 0;
    float KillerQueen = 100;

    bool finished = false;
    bool SAlock = false;
    bool FAlock = false;

    public GameObject linePrefab;
    public GameObject Tester;

    private int numberOfSensors = 7;
    private GameObject[] lineObjects;
    private LineRenderer[] lines;
    private float[] sightHit;
    private Rigidbody2D rBody;
    private Vector3 oldPosition;
    private float differencePos;


     // = = = // = = = Start up and Cycle Functions that keep the AI Working = = = // = = = //


    // Use this for initialization          // Make another sensor regarding anaylzing the 
    void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, -90);
        rBody = GetComponent<Rigidbody2D>();
        lineObjects = new GameObject[numberOfSensors];
        lines = new LineRenderer[numberOfSensors];
        sightHit = new float[numberOfSensors];

        for (int i = 0; i < numberOfSensors; i++) //all inputs
        {// color and properties of the sensor
            lineObjects[i] = (GameObject)Instantiate(linePrefab);
            lineObjects[i].transform.parent = transform;
            lines[i] = lineObjects[i].GetComponent<LineRenderer>();
            lines[i].startWidth = 0.1f; // Thiccness of the Sensors
            lines[i].endWidth = 0.2f; 
            lines[i].material = new Material(Shader.Find("Particles/Additive"));
            lines[i].startColor = Color.red;
            lines[i].endColor = Color.red;
        }
    }
    
    //Updates nerual net with new inputs from the agent
    private void UpdateNet()
    {
        UpdateOverTime();

        float angle = -135f; // This "CIRCLE" dictates the radius at which the Sensors start at
        float angleAdd = 312.181818f / (numberOfSensors - 0); // This "CIRCLE" dictates the radius at which the Sesnors will extend too, Keeping it at a smaller number will cluster the Sensors closer and closer together
        float distance = 7.5f; // Distance at which the Ai's Sensors can Reach
        if (distance <= 0)
            distance = 0f;
        float outDistance = 0f; // Distance at which the Sensors can reach from the base
        int ignoreLayers = ~((1 << 8) | (1 << 9));


        Vector3[] direction = new Vector3[numberOfSensors];
        Vector3[] relativePosition = new Vector3[numberOfSensors];
        RaycastHit2D[] rayHit = new RaycastHit2D[numberOfSensors];

        float redness = 1f - (100 / 100f); //Changes Color, If you affect the First 100 with a Float like AntiFit Then Sensors change when the NAI messes up.
        Color lineColor = new Color(1f, redness, redness);

        for (int i = 0; i < numberOfSensors; i++)
        {
            direction[i] = Quaternion.AngleAxis(angle, Vector3.forward) * bodies[0].transform.up;
            relativePosition[i] = bodies[0].transform.position + (outDistance * direction[i]);
            rayHit[i] = Physics2D.Raycast(relativePosition[i], direction[i], distance, ignoreLayers);
            lines[i].SetPosition(0, relativePosition[i]);
            sightHit[i] = -1f;

            if (rayHit[i].collider != null) // Dictates How exactly the Sensors will work (They stop at the nearest Ray Hit Point)
            {
                sightHit[i] = Vector2.Distance(rayHit[i].point, bodies[0].transform.position) / (distance);
                lines[i].SetPosition(1, rayHit[i].point);
            }
            else
            {
                lines[i].SetPosition(1, relativePosition[i]);
            }
            lines[i].endColor = lineColor;
            lines[i].endColor = lineColor;

            angle += angleAdd;
            }
    }

    //This keeps the AI in Motion, without it the Ai Will be a Stone, Dictates all the Movement in General
    public void UpdateOverTime() 
    {
        Vector2 dir = bodies[0].transform.up;

        // float distance = Vector3.Distance(bodies[0].transform.position, newPos);
        float[] inputValues = {
            sightHit[0], sightHit[1], sightHit[2]
        };

        float[] output = net.FireNet(inputValues);
        /*if (this.name.Equals("0_0"))
            Debug.Log(bodies[0].angularVelocity);*/

        if (output[0] > 0)
            bodies[0].velocity = 15f * dir * output[0];
        else
            bodies[0].velocity = -5f * dir * output[0];

        bodies[0].angularVelocity = 250f * output[1];
    }



    // = = = // = = = Fail Switch Kill Switch = = = // = = = //


    //fixedupdate function
    void FixedUpdate() 
    {
        if (isActive == true)
        {UpdateNet(); //update neural net
        CalculateFitnessOnUpdate(); //calculate fitness

    }}

    //action based on neural net faling/or Finishing the Test
    protected virtual void OnFinished()
    {
        if (TestFinished != null)
        {if (!finished)
            {finished = true;
            CalculateFitnessOnFinish();
            TestFinished(net.GetNetID(), EventArgs.Empty);
            TestFinished -= controller.OnFinished; //unsubscrive from the event notification
            Destroy(gameObject); //destroy this gameobject
        }}
    }


    // = = = // = = = Collision checkers Locks = = = // = = = //


    public void SAlock1(int type){SAlock = true;}

    public void SAlock0(int type){SAlock = false;}

    public void FAlock1(int type){FAlock = true;}

    public void FAlock0(int type){FAlock = false;}


    // = = = // = = = Collision checkers = = = // = = = //


    public void KillActivity(int type) // check collisions
    {
        {
        AntiFit = KillerQueen * 3; //Punishes AI Who hit wall to Avoid Approval of Long Lasting AI who Die from Wall. Reduces ANTIFIT by 75% of the KillerQueen
        OnFinished(); // Only Applies to Dome Objs
        }}


    // = = = // = = = Calculates ProFit and Schooling Fitness Varibles with the Calculation of Final Fitness = = = // = = = //


   //Fitness calculation Every Frame (actually just calculates the Fitness regarding Staying Alive)
   private void CalculateFitnessOnUpdate() 
    {
        if (AntiFit > KillerQueen)
        {OnFinished();} 

        if (FAlock == false)
        {ProFit += 0.40f;
            if (SAlock == true) // Swarm Activity
            {if (ProFit > 105)
            {SwarmFit += 0.20f;//Debug.Log (SwarmFit);
        }}}

        if (FAlock == true)
        {AntiFit += 0.60f;//Debug.Log (AntiFit);
        }

    }

    //Final fitness calculations
    private void CalculateFitnessOnFinish()
    {
        this.net.SetNetFitness(1 + SwarmFit + ProFit - AntiFit);
        float current = (1 + SwarmFit + ProFit - AntiFit);

        //Debug.Log (SwarmFit);

        Tester.GetComponent<Calculate>().TotalNum += 1;

        Tester.GetComponent<Calculate>().TotalAddedNum += current;
        if (Tester.GetComponent<Calculate>().highestNum < current)
        Tester.GetComponent<Calculate>().highestNum = current;
        if (Tester.GetComponent<Calculate>().lowestNum > current)
        Tester.GetComponent<Calculate>().lowestNum = current;
    }


    // = = = // = = = Other Functions Regarding Connections Activation and Other Stuff = = = // = = = //


    public void Activate(NEATNet net)
    {
        this.net = net;
        Invoke(ACTION_ON_FINISHED, net.GetTestTime());
        isActive = true;
    }
    public NEATNet GetNet()
    {
        return net;
    }
    public void SubscriveToEvent(NEATGeneticControllerV2 controller)
    {
        this.controller = controller;
        TestFinished += controller.OnFinished; //subscrive to an event notification
    }
    public void SetColor(Color color)
    {
        transform.GetComponentInChildren<Renderer>().material.color = color;
        transform.GetChild(0).GetComponentInChildren<Renderer>().material.color = color;
    }


}