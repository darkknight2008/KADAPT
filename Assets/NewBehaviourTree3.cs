using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using UnityEngine;
using TreeSharpPlus;

public class NewBehaviourTree3 : MonoBehaviour
{
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
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected Node move( GameObject people, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node moveToBall(GameObject people, GameObject ball)
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node hitGround(GameObject ball)
    {
        Func<RunStatus> IsHitGround =
        delegate ()
        {
            if (ball.transform.position.y == 0.1 && ball.GetComponent<Rigidbody>().velocity.magnitude < 1)
            {
                return RunStatus.Success;
            }
            return RunStatus.Running;
        };
        return new LeafInvoke(IsHitGround);
    }

    protected Node outOfYard(GameObject ball)
    {
        Func<RunStatus> IsoutOfYard =
        delegate ()
        {
            if (ball.transform.position.x <18.937)
            {
                return RunStatus.Success;
            }
            return RunStatus.Running;
        };
        return new LeafInvoke(IsoutOfYard);
    }

    protected Node BuildTreeRoot()
    {
        Node roaming = new DecoratorLoop
                        (
                        new Sequence
                        (
                            new Sequence
                            (
                                this.move(peopleA, this.tablepoint),
                                this.catchBall(peopleA, ball),
                                this.move(peopleA, this.A0),
                                new Sequence
                                (
                                    this.putBall(peopleA, ball, this.thrower),
                                    new LeafWait(1000)
                                    //this.throwBall(ball)
                                 )
                             ),
                            this.hitGround(ball),
                            new Selector
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
                                    new Sequence
                                    (
                                        this.move(peopleB, this.meetpoint),
                                        new SequenceParallel
                                        (
                                            //this.sayHi(peopleB, peopleC),
                                            //this.sayHi(peopleC, peopleB)
                                        )
                                    )
                               )
                            ),
                            new Sequence
                            (
                                this.moveToBall(peopleB, ball),
                                //this.squat(peopleB),
                                this.catchBall(peopleB, ball),
                                this.move(peopleB, tablepoint),
                                this.putBall(peopleB, ball, this.table),
                                this.move(peopleB, this.B0)
                            )
                        )
                    );
        return roaming;
    }

    protected Node catchBall(GameObject people, GameObject ball)
    {
        return new Grab(people, ball, 2000);
    }
    public class Grab : Node
    {
        protected Stopwatch stopwatch;
        protected long actionTime;
        protected GameObject participant;
        protected GameObject ball;
        protected GameObject Dummy;

        public Grab(GameObject participant, GameObject ball, Val<long> actionTime)
        {
            this.participant = participant;
            this.actionTime = actionTime.Value;
            this.stopwatch = new Stopwatch();
            this.ball = ball;
            this.Dummy = new GameObject();
            this.Dummy.transform.position = ball.transform.position;
            this.Dummy.transform.rotation = ball.transform.rotation;
        }

        public override void Start()
        {
            base.Start();
            this.stopwatch.Reset();
            this.stopwatch.Start();
        }

        public override void Stop()
        {
            base.Stop();
            this.stopwatch.Stop();
        }

        public override sealed IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                // Count down the wait timer
                // If we've waited long enough, succeed
                if (this.stopwatch.ElapsedMilliseconds >= 2 * this.actionTime)
                {
                    participant.GetComponent<IKtest>().time = 0.0f;
                    participant.GetComponent<BodyMecanim>().BodyAnimation("PICKUPRIGHT", false);
                    yield return RunStatus.Success;
                    yield break;
                }
                //if (ball.transform.position.y < 0.2)
                //{
                //    participant.GetComponent<BodyMecanim>().BodyAnimation("PICKUPRIGHT", true);
                //}
                else if (this.stopwatch.ElapsedMilliseconds <= this.actionTime)
                {
                    participant.GetComponent<IKtest>().rightHandObj = ball.transform;
                    participant.GetComponent<IKtest>().lookObj = ball.transform;
                    participant.GetComponent<IKtest>().time = (float)this.stopwatch.ElapsedMilliseconds / (float)this.actionTime;
                }
                else
                {
                    ball.GetComponent<BallController>().isHold = true;
                    ball.GetComponent<BallController>().holdBy = participant;

                    participant.GetComponent<IKtest>().rightHandObj = Dummy.transform;
                    participant.GetComponent<IKtest>().lookObj = null;
                    participant.GetComponent<IKtest>().time = (float)(2 * this.actionTime - this.stopwatch.ElapsedMilliseconds) / (float)this.actionTime;
                    //participant.GetComponent<IKtest>().time = 0.0f;
                }
                yield return RunStatus.Running;
            }
        }
    }

    protected Node putBall(GameObject people, GameObject ball, Transform target)
    {
        return new Put(people, ball, target, 2000);
    }
    public class Put : Node
    {
        protected Stopwatch stopwatch;
        protected long actionTime;
        protected GameObject participant;
        protected GameObject ball;
        protected Transform targetLocation;

        public Put(GameObject participant, GameObject ball, Transform targetLocation, Val<long> actionTime)
        {
            this.participant = participant;
            this.ball = ball;
            this.actionTime = actionTime.Value;
            this.stopwatch = new Stopwatch();
            this.targetLocation = targetLocation;
        }

        public override void Start()
        {
            base.Start();
            this.stopwatch.Reset();
            this.stopwatch.Start();
        }

        public override void Stop()
        {
            base.Stop();
            this.stopwatch.Stop();
        }

        public override sealed IEnumerable<RunStatus> Execute()
        {
            while (true)
            {
                // Count down the wait timer
                // If we've waited long enough, succeed
                if (this.stopwatch.ElapsedMilliseconds >= 2 * this.actionTime)
                {
                    participant.GetComponent<IKtest>().time = 0.0f;
                    yield return RunStatus.Success;
                    yield break;
                }
                else if (this.stopwatch.ElapsedMilliseconds <= this.actionTime)
                {
                    participant.GetComponent<IKtest>().rightHandObj = this.targetLocation;
                    participant.GetComponent<IKtest>().lookObj = this.targetLocation;
                    participant.GetComponent<IKtest>().time = (float)this.stopwatch.ElapsedMilliseconds / (float)this.actionTime;
                }
                else
                {
                    ball.GetComponent<BallController>().isHold = false;

                    participant.GetComponent<IKtest>().rightHandObj = this.targetLocation;
                    participant.GetComponent<IKtest>().lookObj = null;
                    participant.GetComponent<IKtest>().time = (float)(2 * this.actionTime - this.stopwatch.ElapsedMilliseconds) / (float)this.actionTime;
                }
                yield return RunStatus.Running;
            }
        }
    }



}
