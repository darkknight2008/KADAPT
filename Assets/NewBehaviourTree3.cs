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
    public Transform ballpoint;
    public bool stWave1;
    public bool stWave2;
    public Transform thrower;
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

    }

    protected Node move(GameObject people, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(people.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node throwBall(GameObject ball)
    {
        return new LeafAssert(() => this.throw_ball(ball));
    }
    public bool throw_ball(GameObject ball)
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
            while (true)
            {
                if (ball.transform.position.y < 0.2 && ball.GetComponent<Rigidbody>().velocity.magnitude < 0.5)
                {
                    yield return RunStatus.Success;
                    yield break;
                }
                else
                    yield return RunStatus.Running;
            }
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
            this.ball = ball;
        }
        public override IEnumerable<RunStatus> Execute()
        {
            if (ball.transform.position.x >-5)
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
                                new Selector(
                                new SequenceParallel
                                (
                                    new Sequence
                                    (
                                        this.wander(peopleA, wander1, wander2),
                                        new LeafWait(6000)
                                     ),
                                    new Sequence
                                    (
                                        this.move(peopleB, meetpoint),
                                        this.sayHi(peopleB, peopleC)
                                    )
                                 ),
                                new LeafWait (100000)
                                )
                            ),
                            new Sequence
                            (
                                this.move(peopleB, ballpoint),
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

    protected Node wander(GameObject ppl0, Transform wander1, Transform wander2)
    {
        Animator animator0 = ppl0.GetComponent<Animator>();
        return new Sequence(ST_ApproachAndWait(ppl0, wander1), new LeafAssert(() => this.feelSad(animator0)), ST_ApproachAndWait(ppl0, wander2), new LeafAssert(() => this.crying(animator0)));
    }
    protected Node ST_ApproachAndWait(GameObject ppl, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(ppl.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(100));
    }
    public bool feelSad(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isSad = state.IsName("IdleSad");
        animator.SetTrigger("FeelSad");
        /*if (isSad)
        {
            animator.SetTrigger("Idle");
        }*/
        animator.SetTrigger("Idle");
        return true;
    }
    public bool crying(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isCry = state.IsName("Cry");
        animator.SetTrigger("H_Cry");
        animator.SetTrigger("Idle");
        return true;
    }

    protected Node sayHi(GameObject ppl1, GameObject ppl2)
    {
        Animator animator1 = ppl1.GetComponent<Animator>();
        Animator animator2 = ppl2.GetComponent<Animator>();
        return new Sequence(this.Greeting(animator1, animator2), new LeafWait(1000), this.Talking(animator1), this.response(animator2));
    }
    protected Node Greeting(Animator animator1, Animator animator2)
    {
        return new Sequence(new LeafAssert(() => this.Greet(animator1, 1)), new LeafWait(1000), new LeafAssert(() => this.Greet(animator2, 2)), new LeafWait(2000));
    }
    protected Node Talking(Animator animator1)
    {
        return new SelectorParallel(new LeafAssert(() => this.pointing(animator1)), new LeafWait(800));
        //new LeafInvoke(() => this.pointing(animator1))
    }
    protected Node response(Animator animator2)
    {
        return new Sequence(new LeafAssert(() => this.responsing(animator2)), new LeafWait(1000));
    }
    public bool Greet(Animator m_Animator, int index)
    {
        AnimatorStateInfo state = m_Animator.GetCurrentAnimatorStateInfo(0); ;
        if (index == 1)
        {
            // if (stWave1)
            // {
            m_Animator.SetTrigger("H_Wave");
            stWave1 = false;
            // }
        }
        else
        {
            // if (stWave2)
            // {
            m_Animator.SetTrigger("H_Wave");
            stWave2 = false;
            // }
        }
        return true;
    }
    public bool pointing(Animator m_Animator1)
    {
        AnimatorStateInfo state1 = m_Animator1.GetCurrentAnimatorStateInfo(0);
        bool isPoint = state1.IsName("LookUp");

        m_Animator1.SetTrigger("H_LookUp");
        //if (isPoint)
        //{
        m_Animator1.SetTrigger("Idle");
        //}
        return true;

    }
    public bool responsing(Animator animator)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isThink = state.IsName("Think");
        animator.SetTrigger("H_Think");
        if (isThink)
        {
            animator.SetTrigger("Idle");
        }
        return true;
    }

}