using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyCrawling.ViewModels
{
    public class HtmlViewModel : Base.ViewModelBase
    {

        #region Fields

        private string _html;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public string Html
        {
            get => _html;
            set => Set(ref _html, value);
        }

        #endregion

        #region Command

        #endregion

        #region method

        #endregion

        #region ExcuteCommand

        #endregion

        #region CanCommand

         #endregion
    }
}
