using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProducerConsumer
{
    public partial class Collections : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Slot> SlotsCollection { get; } = new(); // this is the collection for all the slots. each slot has an emoji and slot_number property.

        public Collections()
        {
            
        }
    }

}