using Avalonia.Controls;
using System.Collections.Generic;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public partial class SimulationPage : UserControl
    {
        private Random random = new Random();
        private LinkedList list;
        private DispatcherTimer main_timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };

        public SimulationPage()
        {
            InitializeComponent();

            main_timer.Tick += (_, __) => Process();
            main_timer.Start();

            list = new LinkedList();

            for (int i = 0; i <= 21; i++) list.AppendVertex("");

            list.PrintList();

            InitializeSlots(); // we use this function to generate the numbers of the slots. in this case, by using this function we also generate the slot borders.
        }

        private void Process()
        {
            ProgressRing.IsActive = false;
            ChooseRandom();
        }

        private void ChooseRandom()
        {
            ActionString.Content = "Escogiendo el turno al azar...";
            int winner = random.Next(0, 2);

            // in this part we should add a timer to be started when the winner is whether zero or one. this timer will be the condition for the process function to choose a random or not. in the process function also we will do all the animations and other things.
            if (winner == 0) StartConsumer();
            else StartProducer();
        }

        async private void StartConsumer()
        {
            main_timer.Stop();
            ActionString.Content = "Ganador: consumidor";
            await Task.Delay(4000);

            int eat_amount = random.Next(3, 7);
            ActionString.Content = "El consumidor obtendrá " + eat_amount.ToString() + " objetos para agarrar.";
            ProgressRing.IsActive = true;
            await Task.Delay(4000);

            for (int i = 0; i < eat_amount; i++)
            {
                if (list.CheckConsumerVertex()) ConsumeBuffer();
                else
                {

                    if (i == 0) ActionString.Content = "No hay objetos en el búffer para agarrar.";
                    else ActionString.Content = "No pudo agarrar más. Se acabaron los objetos en el búffer";
                    break;
                }
            }
            
            ProgressRing.IsActive = false;
            await Task.Delay(4000);
            main_timer.Start();
        }

        private void ConsumeBuffer()
        {
            list.DeAssignVertexValue();
        }

        async private void StartProducer()
        {
            main_timer.Stop();
            ActionString.Content = "Ganador: productor";
            await Task.Delay(4000);

            int produce_amount = random.Next(3, 7);
            ActionString.Content = "El productor pondrá " + produce_amount.ToString() + " objetos en el búffer.";
            ProgressRing.IsActive = true;
            await Task.Delay(4000);

            main_timer.Start();
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