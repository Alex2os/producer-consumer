using Avalonia.Controls;
using System.Collections.Generic;

namespace ProducerConsumer
{
    public partial class SimulationPage : UserControl
    {
        public SimulationPage()
        {
            InitializeComponent();

            InitializeSlots(); // we use this function to generate the numbers of the slots. in this case, by using this function we also generate the slot borders.
        }

        public void InitializeSlots() // in this function we create a list and insert values from 0 to 21 in it, and pass it as item source to the slotspanel. this generates all the slots we need.
        {
            List<int> slot_numbers = new List<int>();

            for (int i = 0; i < 22; i++)
            {
                slot_numbers.Add(i);
            }

            SlotsPanel.ItemsSource = slot_numbers;
            

        }
    }
}