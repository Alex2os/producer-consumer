using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProducerConsumer
{
    public partial class Collections : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Slot> EmojisCollection { get; } = new();

        public Collections()
        {
            
        }
    }

}