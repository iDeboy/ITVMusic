using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.ViewModels {
    public abstract class ViewModelBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
    }
}
