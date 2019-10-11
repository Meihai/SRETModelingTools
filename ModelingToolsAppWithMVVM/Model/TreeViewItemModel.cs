using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace ModelingToolsAppWithMVVM.Model
{
    public class TreeViewItemModel:ObservableObject
    {
        #region 委托方法和事件
        public delegate void ViewSwitchEventHandler(object sender);
        public event ViewSwitchEventHandler evtViewSwitch;

        #endregion

        #region Data
        private bool _isExpanded;
        private bool _isSelected;
        static readonly TreeViewItemModel DummyChild = new TreeViewItemModel();
        private List<TreeViewItemModel> _children;
        private TreeViewItemModel _parent;

        #endregion //Data

        #region Constructors
        public TreeViewItemModel(TreeViewItemModel parent, bool lazyLoadChildren)
        {
            _parent = parent;
            _children = new List<TreeViewItemModel>();
            if (lazyLoadChildren)
            {
                _children.Add(DummyChild);
            }
        }

        /// <summary>
        ///  This is used to create the DummyChild instance.
        /// </summary>
        public TreeViewItemModel() { 
        }
        #endregion //Constructors

        #region Presentation Members

        #region Children
        public List<TreeViewItemModel> Children
        {
            get { return _children; }
            set { _children = value;
                  RaisePropertyChanged(() => Children);
            }
        }
        #endregion //Children

        #region HasLoadedChildren
        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get { return this.Children.Count == 1 && this.Children[0] == DummyChild; }
        }

        #endregion //HasLoadedChildren

        #region IsExpanded
        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    RaisePropertyChanged(() => IsExpanded);
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;

                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.Children.Remove(DummyChild);
                    this.LoadChildren();
                }
            }
        }
        #endregion //IsExpanded


        #region IsSelected
        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    RaisePropertyChanged(() => IsSelected);                  
                }
                if (null != evtViewSwitch && _isSelected == true)
                {
                    evtViewSwitch(this);
                }
            }
        }

        #endregion //IsSelected

        #region LoadChildren
        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }
        #endregion //LoadChildren

        #endregion //Presentation Members


    }
}
