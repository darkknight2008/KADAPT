using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using TreeSharpPlus;
using System.Collections.Generic;

public class GrabTest : MonoBehaviour {

    public Transform wander1;

    public GameObject ball;
    public GameObject participant;

    protected Animator animator;
    private BehaviorAgent behaviorAgent;

    public long t;
    // Use this for initialization
    void Start()
    {
        behaviorAgent = new BehaviorAgent(this.BuildTreeRoot());
        BehaviorManager.Instance.Register(behaviorAgent);
        behaviorAgent.StartBehavior();

        t = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        //t += Time.deltaTime;
    }

    protected Node ST_ApproachAndWait(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
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

    protected Node BuildTreeRoot()
    {
        Node roaming = new Sequence(
                        new Grab(participant, ball, 2000),
                        this.ST_ApproachAndWait(this.wander1),
                        new Put(participant, ball, wander1, 2000)
                        //new Grab(participant, ball, t)
                        );
        return roaming;
    }

}
