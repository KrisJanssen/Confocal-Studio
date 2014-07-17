#region License

// --------------------------------------------------------------------------- 
// <copyright file="SysInfoResult.cs" company="Kris Janssen">
//   Copyright (c) 2014-2014 Kris Janssen
// </copyright>
//  This file is part of CFStudio.
//  CFStudio is an open source tool foor confocal microscopy.
//  Parts of the CFStudio codebase were re-used, with permission from
//  the SambaPOS project <https://github.com/emreeren/SambaPOS-3>.
//  CFStudio is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// --------------------------------------------------------------------------- 
#endregion


#pragma warning disable 1591

namespace CFStudio.Infrastructure.ExceptionReporter.SystemInfo
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The sys info result.
    /// </summary>
    public class SysInfoResult
    {
        #region Fields

        /// <summary>
        ///     The _child results.
        /// </summary>
        private readonly List<SysInfoResult> _childResults = new List<SysInfoResult>();

        /// <summary>
        ///     The _name.
        /// </summary>
        private readonly string _name;

        /// <summary>
        ///     The _nodes.
        /// </summary>
        private readonly List<string> _nodes = new List<string>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SysInfoResult"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public SysInfoResult(string name)
        {
            this._name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the child results.
        /// </summary>
        public List<SysInfoResult> ChildResults
        {
            get
            {
                return this._childResults;
            }
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        ///     Gets the nodes.
        /// </summary>
        public List<string> Nodes
        {
            get
            {
                return this._nodes;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add children.
        /// </summary>
        /// <param name="children">
        /// The children.
        /// </param>
        public void AddChildren(IEnumerable<SysInfoResult> children)
        {
            this.ChildResults.AddRange(children);
        }

        /// <summary>
        /// The add node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        public void AddNode(string node)
        {
            this._nodes.Add(node);
        }

        /// <summary>
        /// The filter.
        /// </summary>
        /// <param name="filterStrings">
        /// The filter strings.
        /// </param>
        /// <returns>
        /// The <see cref="SysInfoResult"/>.
        /// </returns>
        public SysInfoResult Filter(string[] filterStrings)
        {
            List<string> filteredNodes =
                (from node in this.ChildResults[0].Nodes
                 from filter in filterStrings
                 where node.Contains(filter + " = ")
                 select node).ToList();

            this.ChildResults[0].Clear();
            this.ChildResults[0].AddRange(filteredNodes);
            return this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="nodes">
        /// The nodes.
        /// </param>
        private void AddRange(IEnumerable<string> nodes)
        {
            this._nodes.AddRange(nodes);
        }

        /// <summary>
        ///     The clear.
        /// </summary>
        private void Clear()
        {
            this._nodes.Clear();
        }

        #endregion
    }
}

#pragma warning restore 1591