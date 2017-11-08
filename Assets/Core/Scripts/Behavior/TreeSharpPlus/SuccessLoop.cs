using System;
using System.Collections.Generic;

namespace TreeSharpPlus
{
    /// <summary>
    /// Loops will continue executing their child definitely unless that child reports
    /// failure or success. If the child reports success, the proceed loop will end.
    /// </summary>
    public class SuccessLoop : Decorator
    {
        /// <summary>
        ///     The number of iterations to run (-1 is infinite)
        /// </summary>
        public int Iterations { get; set; }

        public SuccessLoop(Node child)
            : base(child)
        {
            this.Iterations = -1;
        }

        public SuccessLoop(int iterations, Node child)
            : base(child)
        {
            this.Iterations = iterations;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            // Keep track of the running iterations
            int curIter = 0;

            while (true)
            {
                this.DecoratedChild.Start();

                RunStatus result;
                while ((result = this.TickNode(this.DecoratedChild)) == RunStatus.Running)
                    yield return RunStatus.Running;

                this.DecoratedChild.Stop();

                // If the child failed, break and report the failure
                if (result == RunStatus.Failure)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }

                // Increase the iteration count and see if we're done
                curIter++;
                //If the chld failed, break and report the success
                if (result == RunStatus.Success && (curIter >= Iterations))
                {
                    yield return RunStatus.Success;
                    yield break;
                }

                // Take one tick to prevent infinite loops
                yield return RunStatus.Running;
            }
        }
    }
}