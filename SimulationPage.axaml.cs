using Avalonia.Controls;
using System.Collections.Generic;
using Avalonia.Threading;
using System;

namespace ProducerConsumer
{
    public partial class SimulationPage : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000) 
        };

        public SimulationPage()
        {
            InitializeComponent();

            timer.Tick += (_, __) => Process();
            timer.Start();

            LinkedList list = new LinkedList();

            for (int i = 0; i <= 21; i++) list.AppendVertex("");

            list.PrintList();

            InitializeSlots(); // we use this function to generate the numbers of the slots. in this case, by using this function we also generate the slot borders.
        }

        private void Process()
        {
            ChooseRandom();
        }

        private void ChooseRandom()
        {
            Random random = new Random();
            int winner = random.Next(0, 2);

            // in this part we should add a timer to be started when the winner is whether zero or one. this timer will be the condition for the process function to choose a random or not. in the process function also we will do all the animations and other things.
            if (winner == 0) ;
            else;
        }

        private void InitializeSlots() // in this function we create a list and insert values from 0 to 21 in it, and pass it as item source to the slotspanel. this generates all the slots we need.
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