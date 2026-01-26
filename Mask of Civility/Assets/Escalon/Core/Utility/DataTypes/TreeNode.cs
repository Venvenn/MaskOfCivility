using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Escalon
{
    public class TreeNode<T> : IEqualityComparer, IEnumerable<T>, IEnumerable<TreeNode<T>>
    {
        public TreeNode<T> Parent { get; private set; }
        public T Value { get; set; }

        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            Value = value;
        }

        public TreeNode<T> this[int index] => _children[index];

        public TreeNode<T> Add(T value, int index = -1)
        {
            var childNode = new TreeNode<T>(value);
            Add(childNode, index);
            return childNode;
        }

        public void Add(TreeNode<T> childTreeNode, int index = -1)
        {
            if (index < -1)
            {
                throw new ArgumentException("The index can not be lower then -1");
            }

            if (index > Children.Count() - 1)
            {
                throw new ArgumentException(
                    "The index ({0}) can not be higher then index of the last iten. Use the AddChild() method without an index to add at the end"
                        .FormatInvariant(index));
            }

            if (!childTreeNode.IsRoot)
            {
                throw new ArgumentException(
                    "The child node with value [{0}] can not be added because it is not a root node.".FormatInvariant(
                        childTreeNode.Value));
            }

            if (Root == childTreeNode)
            {
                throw new ArgumentException(
                    "The child node with value [{0}] is the rootnode of the parent.".FormatInvariant(
                        childTreeNode.Value));
            }

            if (childTreeNode.SelfAndDescendants.Any(n => this == n))
            {
                throw new ArgumentException(
                    "The childnode with value [{0}] can not be added to itself or its descendants.".FormatInvariant(
                        childTreeNode.Value));
            }

            childTreeNode.Parent = this;
            if (index == -1)
            {
                _children.Add(childTreeNode);
            }
            else
            {
                _children.Insert(index, childTreeNode);
            }
        }

        public TreeNode<T> AddFirstChild(T value)
        {
            var childNode = new TreeNode<T>(value);
            AddFirstChild(childNode);
            return childNode;
        }

        public void AddFirstChild(TreeNode<T> childTreeNode)
        {
            Add(childTreeNode, 0);
        }

        public TreeNode<T> AddFirstSibling(T value)
        {
            var childNode = new TreeNode<T>(value);
            AddFirstSibling(childNode);
            return childNode;
        }

        public void AddFirstSibling(TreeNode<T> childTreeNode)
        {
            Parent.AddFirstChild(childTreeNode);
        }

        public TreeNode<T> AddLastSibling(T value)
        {
            var childNode = new TreeNode<T>(value);
            AddLastSibling(childNode);
            return childNode;
        }

        public void AddLastSibling(TreeNode<T> childTreeNode)
        {
            Parent.Add(childTreeNode);
        }

        public TreeNode<T> AddParent(T value)
        {
            var newNode = new TreeNode<T>(value);
            AddParent(newNode);
            return newNode;
        }

        public void AddParent(TreeNode<T> parentTreeNode)
        {
            if (!IsRoot)
            {
                throw new ArgumentException("This node [{0}] already has a parent".FormatInvariant(Value),
                    "parentNode");
            }

            parentTreeNode.Add(this);
        }

        public IEnumerable<TreeNode<T>> Ancestors
        {
            get
            {
                if (IsRoot)
                {
                    return Enumerable.Empty<TreeNode<T>>();
                }

                return Parent.ToIEnumarable().Concat(Parent.Ancestors);
            }
        }

        public IEnumerable<TreeNode<T>> Descendants
        {
            get { return SelfAndDescendants.Skip(1); }
        }

        public IEnumerable<TreeNode<T>> Children
        {
            get { return _children; }
        }

        public IEnumerable<TreeNode<T>> Siblings
        {
            get { return SelfAndSiblings.Where(Other); }
        }

        private bool Other(TreeNode<T> treeNode)
        {
            return !ReferenceEquals(treeNode, this);
        }

        public IEnumerable<TreeNode<T>> SelfAndChildren
        {
            get { return this.ToIEnumarable().Concat(Children); }
        }

        public IEnumerable<TreeNode<T>> SelfAndAncestors
        {
            get { return this.ToIEnumarable().Concat(Ancestors); }
        }

        public IEnumerable<TreeNode<T>> SelfAndDescendants
        {
            get { return this.ToIEnumarable().Concat(Children.SelectMany(c => c.SelfAndDescendants)); }
        }

        public IEnumerable<TreeNode<T>> SelfAndSiblings
        {
            get
            {
                if (IsRoot)
                {
                    return this.ToIEnumarable();
                }

                return Parent.Children;

            }
        }

        public IEnumerable<TreeNode<T>> All
        {
            get { return Root.SelfAndDescendants; }
        }


        public IEnumerable<TreeNode<T>> SameLevel
        {
            get { return SelfAndSameLevel.Where(Other); }
        }

        public int Level
        {
            get { return Ancestors.Count(); }
        }

        public IEnumerable<TreeNode<T>> SelfAndSameLevel
        {
            get { return GetNodesAtLevel(Level); }
        }

        public IEnumerable<TreeNode<T>> GetNodesAtLevel(int level)
        {
            return Root.GetNodesAtLevelInternal(level);
        }

        private IEnumerable<TreeNode<T>> GetNodesAtLevelInternal(int level)
        {
            if (level == Level)
            {
                return this.ToIEnumarable();
            }

            return Children.SelectMany(c => c.GetNodesAtLevelInternal(level));
        }

        public TreeNode<T> Root
        {
            get { return SelfAndAncestors.Last(); }
        }

        public void Disconnect()
        {
            if (IsRoot)
            {
                throw new InvalidOperationException("The root node [{0}] can not get disconnected from a parent."
                    .FormatInvariant(Value));
            }

            Parent._children.Remove(this);
            Parent = null;
        }

        public bool IsRoot => Parent == null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _children.Values().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static IEnumerable<TreeNode<T>> CreateTree<TId>(IEnumerable<T> values, Func<T, TId> idSelector, Func<T, TId?> parentIdSelector) where TId : struct
        {
            var valuesCache = values.ToList();
            if (!valuesCache.Any())
                return Enumerable.Empty<TreeNode<T>>();
            T itemWithIdAndParentIdIsTheSame =
                valuesCache.FirstOrDefault(v => IsSameId(idSelector(v), parentIdSelector(v)));
            if (itemWithIdAndParentIdIsTheSame != null) // Hier verwacht je ook een null terug te kunnen komen
            {
                throw new ArgumentException(
                    "At least one value has the samen Id and parentId [{0}]".FormatInvariant(
                        itemWithIdAndParentIdIsTheSame));
            }

            var nodes = valuesCache.Select(v => new TreeNode<T>(v));
            return CreateTree(nodes, idSelector, parentIdSelector);
        }

        public static IEnumerable<TreeNode<T>> CreateTree<TId>(IEnumerable<TreeNode<T>> rootNodes, Func<T, TId> idSelector, Func<T, TId?> parentIdSelector) where TId : struct
        {
            var rootNodesCache = rootNodes.ToList();
            var duplicates = rootNodesCache.Duplicates(n => n).ToList();
            if (duplicates.Any())
            {
                throw new ArgumentException(
                    "One or more values contains {0} duplicate keys. The first duplicate is: [{1}]".FormatInvariant(
                        duplicates.Count, duplicates[0]));
            }

            foreach (var rootNode in rootNodesCache)
            {
                var parentId = parentIdSelector(rootNode.Value);
                var parent = rootNodesCache.FirstOrDefault(n => IsSameId(idSelector(n.Value), parentId));

                if (parent != null)
                {
                    parent.Add(rootNode);
                }
                else if (parentId != null)
                {
                    throw new ArgumentException(
                        "A value has the parent ID [{0}] but no other nodes has this ID"
                            .FormatInvariant(parentId.Value));
                }
            }

            var result = rootNodesCache.Where(n => n.IsRoot);
            return result;
        }


        private static bool IsSameId<TId>(TId id, TId? parentId)
            where TId : struct
        {
            return parentId != null && id.Equals(parentId.Value);
        }

        #region Equals en ==

        public static bool operator ==(TreeNode<T> value1, TreeNode<T> value2)
        {
            if ((object) (value1) == null && (object) value2 == null)
            {
                return true;
            }

            return ReferenceEquals(value1, value2);
        }

        public static bool operator !=(TreeNode<T> value1, TreeNode<T> value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(Object anderePeriode)
        {
            var valueThisType = anderePeriode as TreeNode<T>;
            return this == valueThisType;
        }

        public bool Equals(TreeNode<T> value)
        {
            return this == value;
        }

        public bool Equals(TreeNode<T> value1, TreeNode<T> value2)
        {
            return value1 == value2;
        }

        bool IEqualityComparer.Equals(object value1, object value2)
        {
            var valueThisType1 = value1 as TreeNode<T>;
            var valueThisType2 = value2 as TreeNode<T>;

            return Equals(valueThisType1, valueThisType2);
        }

        public int GetHashCode(object obj)
        {
            return GetHashCode(obj as TreeNode<T>);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public int GetHashCode(TreeNode<T> value)
        {
            return base.GetHashCode();
        }

        #endregion
    }
}

