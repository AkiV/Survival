using UnityEngine;

interface AIState
{
    void OnStart(GameObject actor);
    void OnStop(GameObject actor);
}