using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class HelloAgent : Agent
{
    public Transform player;
    public Transform goal;
    public Transform death;

    Vector3 startPosition;

    void Start() {
        startPosition = player.transform.position;
        Restart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {        
        base.CollectObservations(sensor);
        sensor.AddObservation(player.transform.position.x);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        var actions = actionsOut.ContinuousActions;
        actions[0] = Input.GetKey(KeyCode.RightArrow) ? 1f : Input.GetKey(KeyCode.LeftArrow) ? -1f : 0;        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        Move(actions.ContinuousActions[0]);
    }

    void Move(float delta) {
        player.position += new Vector3(delta, 0f, 0f);
    }

    void Restart() {
        player.position = startPosition;
    }

    void Update() {
        if (hasWon) {
            AddReward(1f);
            EndEpisode();
            Restart();
        } else if (hasLost) {
            AddReward(-1f);
            EndEpisode();
            Restart();
        } else {
            RequestDecision();
        }
    }

    bool hasWon => player.position.x >= goal.position.x;
    bool hasLost => player.position.x <= death.position.x;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
    }

}
