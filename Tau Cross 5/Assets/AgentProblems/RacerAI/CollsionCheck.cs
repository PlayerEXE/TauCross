using UnityEngine;
using System.Collections;

public class CollsionCheck : MonoBehaviour
{


    // = = = // = = = Checks for Collisions, for communicating to the rest of the AI's Body to activate other Functions = = = // = = = //


    void OnCollisionEnter2D(Collision2D coll) // This calls only when Start Touch
    {
        if (coll.collider.name.Contains("D")) // D Acts at the kill switch here, any Object or Prehab that starts with the letter Captial G, is turned into a kill object. If the Ai dares tocuh it, they will die
            transform.parent.SendMessage("KillActivity", (object)0);

        if (coll.collider.name.Contains("X"))
            transform.parent.SendMessage("FAlock1", (object)0);
    }

    void OnTriggerEnter2D(Collider2D coll) // This Calls only when Continue to Touch
    {
        if (coll.GetComponent<Collider2D>().name.Contains("B"))
            transform.parent.SendMessage("SAlock1", (object)0);
    }


    // = = = // = = = 0 = = = // = = = ////


    void OnCollisionExit2D(Collision2D coll) // This Calls only when Continue to Touch
    {
        if (coll.collider.name.Contains("X"))
            transform.parent.SendMessage("FAlock0", (object)0);
    }

    void OnTriggerExit2D(Collider2D coll) // This Calls only when Continue to Touch
    {
        if (coll.GetComponent<Collider2D>().name.Contains("B"))
            transform.parent.SendMessage("SAlock0", (object)0);
    }

}


/*    public void SwarmActivity(int type) // check collisions
    {   
        Debug.Log ("Triggered!!!");

        }*/