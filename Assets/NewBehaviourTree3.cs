using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;

public class NewBehaviourTree3 : MonoBehaviour {
    public Transform A0;
    public Transform tablepoint;
    public Transform B0;
    public Transform meetpoint;
    public Transform table;
    public Transform wander1;
    public Transform wander2;
    public Transform wander3;
    public Transform thrower;
    public Transform wall;
    public GameObject ball;
    public GameObject peopleA;
    public GameObject peopleB;
    public GameObject peopleC;
    private BehaviorAgent behaviorAgent;

    // Use this for initialization
    void Start () {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }
        protected Node BuildTreeRoot()
    {
        Node roaming = new DecoratorLoop
                        (
                        new SequenceShuffle
                        (
                            new SequenceShuffle
                            (
                                this.move(peopleA, this.tablepoint),
                                this.catchBall(peopleA, ball),
                                this.move(peopleA, this.A0),
                                new SequenceShuffle
                                (
                                    this.putBall(peopleA, ball, this.thrower),
                                    new LeafWait(1000),
                                    this.throwBall(ball)
                                 )
                             ),
                            this.hitGround(ball),
                            new SelectorShuffle
                            (
                                this.outOfYard(ball),
                                new SelectorParallel
                                (
                                    new DecoratorLoop
                                    (
                                        new SequenceShuffle
                                        (
                                             this.move(peopleA, this.wander1),
                                             this.move(peopleA, this.wander2),
                                             this.move(peopleA, this.wander3)
                                        )
                                    ),
                                    new SequenceShuffle
                                    (
                                        this.move(peopleB, this.meetpoint),
                                        new SequenceParallel
                                        (
                                            this.sayHi(peopleB),
                                            this.sayHi(peopleC)
                                        )
                                    )
                               )
                            ),
                            new SequenceShuffle
                            (
                                this.moveToBall(peopleB, ball),
                                this.squat(peopleB),
                                this.catchBall(peopleB, ball),
                                this.move(peopleB, tablepoint),
                                this.putBall(peopleB, ball, this.table),
                                this.move(peopleB, this.B0)
                            )
                        )
                    );
        return roaming;
    }
}
