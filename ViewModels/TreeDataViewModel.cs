using EasyCrawling.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyCrawling.ViewModels
{
    public class TreeDataViewModel : Base.ViewModelBase
    {
        #region Fields

        private TreeDataViewModel _parent;
        private TreeData _treeData;
        private ReadOnlyCollection<TreeDataViewModel> _children;
        private bool _isSelected;
        private bool _isExpanded;

        #endregion

        #region Constructors

        public TreeDataViewModel(TreeData treeData)
            : this(treeData, null)
        {
        }

        protected TreeDataViewModel(TreeData treeData, TreeDataViewModel parent)
        {
            _parent = parent;
            _treeData = treeData;

            _children = new ReadOnlyCollection<TreeDataViewModel>(
                (from child in _treeData.Children
                 select new TreeDataViewModel(child, this))
                 .ToList());
        }

        #endregion

        #region Properties

        public TreeDataViewModel Parent
        {
            get => _parent;
            set => Set(ref _parent, value);
        }
        public TreeData TreeData
        {
            get => _treeData;
            set => Set(ref _treeData, value);
        }
        public string Text
        {
            get => _treeData.Text;
        }

        public ReadOnlyCollection<TreeDataViewModel> Children
        {
            get => _children;
            set => Set(ref _children, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                Set(ref _isExpanded, value);

                if (_parent != null)
                    _parent.IsExpanded = true;
            }
        }

        #endregion
    }
}
