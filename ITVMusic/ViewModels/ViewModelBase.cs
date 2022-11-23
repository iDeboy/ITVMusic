using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ITVMusic.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {

        // Default fields
        protected bool m_IsViewVisible;
        protected Window? m_NextWindow;

        public bool IsViewVisible {
            get => m_IsViewVisible;
            set {
                m_IsViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }
        public Window? NextWindow {
            get => m_NextWindow;
            set {
                m_NextWindow = value;
                OnPropertyChanged(nameof(NextWindow));
            }
        }

        protected ViewModelBase() {
            IsViewVisible = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}
