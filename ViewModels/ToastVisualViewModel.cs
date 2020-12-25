using EasyCrawling.Enums;
using EasyCrawling.Helpers;
using EasyCrawling.Models;

namespace EasyCrawling.ViewModels
{
    public class ToastVisualViewModel : NameViewModel
    {
        private readonly ToastVisual _toastVisual; 
        
        public ToastVisualViewModel()
        {
            _toastVisual = new ToastVisual();
            wordPointer = _toastVisual;
        }

        public ToastVisualViewModel(ToastVisual inVIsual)
        {
            _toastVisual = inVIsual;
            wordPointer = _toastVisual;
        }
        public ToastVisual ToastVisual
        {
            get => _toastVisual;
        }

        public VisualType Visual
        {
            get => _toastVisual.Visual;
            set
            {
                if (_toastVisual.Visual != value)
                {
                    _toastVisual.Visual = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string NameVisual
        {
            get => EnumHelper.StringValueOf(Visual);
        }      
    }
}
