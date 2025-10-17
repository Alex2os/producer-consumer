using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;
using Avalonia.Interactivity;

namespace ProducerConsumer
{
    public partial class SimulationPage : UserControl
    {
        private Collections collections_vm;
        private int time_delay = 2000;
        private Random random = new Random();
        private LinkedList list;
        private DispatcherTimer main_timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000)
        };

        public string[] emojis = new string[]
        {
    "🍎","🍞","🍕","🍔","🍣","🍩","🍪","🍫","🍿","🧀",
    "☕","🍵","🥤","🍇","🌽","🥑","🍊","🍌","🍍","🌶️",
    "🌲","🌵","🍁","🌸","🍀","🍂","🌙","⭐","☀️","⛅",
    "☁️","🌧️","⛈️","❄️","🌈","🔥","🌊","💡","🔧","⚙️",
    "🔒","🔑","🧪","🔭","💻","🎧","🎮","🎲","🧩","📚",
    "📎","✏️","🖊️","📐","📏","⚽","🏀","🏈","⚾", "🎾"
    };

        public SimulationPage()
        {
            InitializeComponent();

            main_timer.Tick += (_, __) => Process();
            main_timer.Start();

            collections_vm = new Collections(); // this is the collections_vm for the emojis list, so we can display them in the ui.
            list = new LinkedList(collections_vm);

            Console.OutputEncoding = System.Text.Encoding.UTF8;

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

            bool consumed_all = true;

            ActionString.Content = "Ganador: consumidor";
            await Task.Delay(time_delay);

            int eat_amount = random.Next(3, 7);
            ActionString.Content = "El consumidor obtendrá " + eat_amount.ToString() + " objetos para agarrar.";
            ProgressRing.IsActive = true;
            await Task.Delay(time_delay);

            for (int i = 0; i < eat_amount; i++)
            {
                if (list.CheckConsumerVertex()) ConsumeBuffer();
                else
                {
                    if (i == 0) ActionString.Content = "No hay objetos en el búffer para agarrar.";
                    else ActionString.Content = "No pudo agarrar más. Se acabaron los objetos en el búffer. Objetos que agarró: " + (i).ToString();
                    consumed_all = false;
                    break;
                }
            }

            ProgressRing.IsActive = false;

            if(consumed_all) ActionString.Content = "Se consumieron los " + eat_amount.ToString() + " objetos.";
            await Task.Delay(time_delay);

            main_timer.Start();
        }

        private void ConsumeBuffer()
        {
            list.DeAssignVertexValue();
        }

        async private void StartProducer()
        {
            main_timer.Stop();

            bool produced_all = true;

            ActionString.Content = "Ganador: productor";
            await Task.Delay(time_delay);

            int produce_amount = random.Next(3, 7);
            ActionString.Content = "El productor pondrá " + produce_amount.ToString() + " objetos en el búffer.";
            ProgressRing.IsActive = true;
            await Task.Delay(time_delay);

            for (int i = 0; i < produce_amount; i++)
            {
                if (list.CheckProducerVertex()) ProduceBuffer();
                else
                {
                    if (i == 0) ActionString.Content = "No hay espacio para producir objetos.";
                    else ActionString.Content = "Ya no hay espacio para poner más objetos.";
                    produced_all = false;
                    break;
                }
            }
            ProgressRing.IsActive = false;
            if (produced_all) ActionString.Content = "Se produjeron los " + produce_amount.ToString() + " objetos.";
            await Task.Delay(time_delay);

            main_timer.Start();
        }

        private void ProduceBuffer()
        {
            string emoji_chosen = emojis[random.Next(0, 60)];
            list.AssignVertexValue(emoji_chosen);
        }

        private void InitializeSlots() // in this function we create a list and insert values from 0 to 21 in it, and pass it as item source to the slotspanel. this generates all the slots we need.
        {
            for (int i = 0; i < 22; i++)
            {
                collections_vm.EmojisCollection.Add(new Slot(i)); // we also add this to the emojis collection, as we need to fill it out with empty strings to then fill it up with emojis.
                collections_vm.EmojisCollection[i].AssignEmoji("");
            }

            SlotsPanel.ItemsSource = collections_vm.EmojisCollection;
        }

        public void ExitApp(object sender, RoutedEventArgs args)
        {
            Environment.Exit(0);
        }
    }
}