using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : Interactive
{
    public NPCController npc;
    public Events.InteractionStart onInteractionStart;
    public Events.InteractionComplete onInteractionComplete;

    // Use this for initialization
    void Start()
    {
        InUse = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Npc")
        {
            npc = other.GetComponentInParent<NPCController>();
            Debug.Log(npc.NPCName + " Entered");
            CancelInvoke();
            StopCoroutine(UseBathroom());
            Interact();
        }
    }
    public void FaceInteractable()
    {
        npc.LookAtTarget(gameObject);
    }
    public override void Interact()
    {
        StartCoroutine(UseBathroom());
    }
    IEnumerator UseBathroom()
    {
        InvokeRepeating("FaceInteractable", 0.0f, 0.1f);
        InUse = true;
        yield return new WaitForSeconds(InteractTime);
        InUse = false;
        CancelInvoke();
    }
}
