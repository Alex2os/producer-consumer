using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProducerConsumer
{
    public partial class Slot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int slot_number { get; set; }
        public string emoji { get; set; }
        public Slot(int _slot_number)
        {
            emoji = "";
            slot_number = _slot_number;
        }

        public void AssignEmoji(string _emoji)
        {
            emoji = _emoji;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(emoji)));
        }
    }

}