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

    public bool run;

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
        if (Input.GetKey(KeyCode.Space))
        {
            ball.transform.position = new Vector3(-1f, 0, -1f);
        }
    }

    protected Node move(GameObject people, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node moveToBall(GameObject people, GameObject ball)
    {
        Val<Vector3> position = Val.V(() => ball.transform.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node throwBall(GameObject ball)
    {
        return new LeafAssert(() => this.throw_ball(ball));
    }
    public bool throw_ball(GameObject  ball)
    {
        Vector3 w = new Vector3(-500 * Time.deltaTime, 0, 0);
        ball.GetComponent<Rigidbody>().AddForce(w * 50);
        return true;
    }

    public class isHitGround : Node
    {
        protected GameObject ball;
        public isHitGround(GameObject ball)
        {
            this.ball = ball;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            if (ball.transform.position.y < 0.6 && ball.GetComponent<Rigidbody>().velocity.magnitude < 1)
                yield return RunStatus.Success;
            else
                yield return RunStatus.Failure ;
        }
    }
    protected Node hitGround(GameObject ball)
    {
        return new isHitGround(ball);
    }

    public class isOutOfYard : Node
{
	protected GameObject ball;
	public isOutOfYard(GameObject ball)
	{
		this.ball=ball;
	}
	public override IEnumerable<RunStatus> Execute()
	{
		if (ball.transform.position.x < 18.937)
			yield return RunStatus.Success;
		else
			yield return RunStatus.Failure;
	}
}
    protected Node outOfYard(GameObject ball)
    {
        return new isOutOfYard(ball);
    }

    //private RunStatus Hold()
    //{
    //    if (ball.transform.position.y < 0.2)
    //    {
    //        //if (!peopleA.gameObject.GetComponent<Animator>().GetBool("B_PickupRight"))
    //        //{
    //        //    peopleA.gameObject.GetComponent<Animator>().SetTrigger("B_PickupRight");
    //        //}
    //        return RunStatus.Running;
    //    }
    //    peopleA.gameObject.GetComponent<Animator>().SetTrigger("B_PickupRight");
    //    return RunStatus.Success;
    //}

    protected bool StartSquat(GameObject people)
    {
        people.GetComponent<Animator>().SetBool("squat_down", true);
        return true;
    }

    protected Node squat(GameObject people)
    {
        return new SequenceParallel(
            new LeafAssert(() => StartSquat(people)),
            new LeafWait(542)
        );
    }

    protected bool StartStand(GameObject people)
    {
        people.GetComponent<Animator>().SetBool("squat_down", false);
        return true;
    }

    protected Node stand(GameObject people)
    {
        return new SequenceParallel(
            new LeafAssert(() => StartStand(people)),
            new LeafWait(1292)
        );
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
                                    new LeafWait(1000),
                                    this.throwBall(ball)
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
                                this.squat(peopleB), this.catchBall(peopleB, ball),
                                this.catchBall(peopleB, ball),
                                 this.stand(peopleB),
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
        return new Grab(people, ball, 1000);
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
                ball.GetComponent<BallController>().holdBy = participant;
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
        return new Put(people, ball, target, 1000);
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
