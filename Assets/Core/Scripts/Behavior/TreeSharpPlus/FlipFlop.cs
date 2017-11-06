#region License

// A simplistic Behavior Tree implementation in C#
// Copyright (C) 2010-2011 ApocDev apocdev@gmail.com
// 
// This file is part of TreeSharp
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using UnityEngine;
using System.Collections.Generic;

namespace TreeSharpPlus
{
    /// <summary>
    ///   The base sequence class. This will execute each branch of logic, in order.
    ///   If all branches succeed, this composite will return a successful run status.
    ///   If any branch fails, this composite will return a failed run status.
    /// </summary>
    public class FlipFlop : NodeGroup
    {
        private bool state;

        public FlipFlop(params Node[] children)
            : base(children)
        {
            this.state = true;
        }

        public override IEnumerable<RunStatus> Execute()
        {
            Node node = (this.state)? this.Children[0] : this.Children[1];
            this.Selection = node;
            node.Start();

            // If the current node is still running, report that. Don't 'break' the enumerator
            RunStatus result;
            while ((result = this.TickNode(node)) == RunStatus.Running)
                yield return RunStatus.Running;

            // Call Stop to allow the node to clean anything up.
            node.Stop();

            // Clear the selection
            this.Selection.ClearLastStatus();
            this.Selection = null;

            if (result == RunStatus.Failure)
            {
                yield return RunStatus.Failure;
                yield break;
            }
            this.state = !this.state;
            yield return RunStatus.Success;
        }
    }
}