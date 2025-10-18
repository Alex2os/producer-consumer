using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Styling;
using Avalonia.VisualTree;
using System.Linq;
using System.Collections.Generic; // this is used for lists.

namespace ProducerConsumer
{
    public partial class SimulationPage : UserControl
    {
        // move distance for both the consumer and producer
        private double producer_consumer_move_distance = 50;
        // time delays and times
        private int time_delay_messages = 2000; // this is for each message, how much we will wait until a new message appears or to procced the program after a message.
        private int timer_tick_delay = 1000; // the main timer tick delay.
        private double time_animation_speed = 1; // animation speed in seconds.
        // items produced and consumed.
        private int total_items_produced = 0;
        private int total_items_consumed = 0;
        // string for the simulation_speed
        private string actual_simulation_speed = "X1";
        private List<Border> borders; // this is the list for our borders that we generate in the constructor.

        // we declare the transltetransform for each consumer and producer, so whenever we change the consumer_transform or producer_transform, then the changes also apply for the transforms of the producer and consumer visually.
        private readonly TranslateTransform consumer_transform = new();
        private readonly TranslateTransform producer_transform = new();
        private Collections collections_vm; // our collections_vm
        private Random random = new Random(); // this is the random variable so we can use it without declaring a new one. the whole class can use it.
        private LinkedList list; // our main list
        private DispatcherTimer main_timer; // we declare our main timer

        // this is our list of emojis that we use in this program. each time the producer puts objects in the slots, an emoji from this list is chosen randomly.
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

            main_timer = new DispatcherTimer // we assign our timer here, as we use a variable for the value of the timer tick or interval.
            {
                Interval = TimeSpan.FromMilliseconds(timer_tick_delay)
            };

            Console.OutputEncoding = System.Text.Encoding.UTF8; // this is for the encoding of the console. this is just to output emojis in the console, not directly in the ui.

            main_timer.Tick += (_, __) => Process();
            main_timer.Start();

            collections_vm = new Collections(); // this is the collections_vm for the emojis list, so we can display them in the ui.
            list = new LinkedList(collections_vm); // we create our linkedlist with the collections_vm so it can have the list we create then.



            for (int i = 0; i <= 21; i++) list.AppendVertex("");

            InitializeSlots(); // we use this function to generate the numbers of the slots. in this case, by using this function we also generate the slot borders.

            // this code below is to get a list (borders) once we have generated them. now we can animate each border individually, even if we created those borders with a template and in the code side.
            // basically we search the visual descendants of the type border, where the tag of those borders is "SlotBorder", as that is the tag they're created with, and convert them to a list which we assign to the borders variable.
            borders = SlotsPanel
            .GetVisualDescendants()
            .OfType<Border>()
            .Where(b => (string?)b.Tag == "SlotBorder")
            .ToList();

            // we update both the consumer and producer transforms. this is so we can change the consumer and producer render transform here in the code, and make animations and stuff.
            consumer.RenderTransform = consumer_transform;
            producer.RenderTransform = producer_transform;

            // this is just an adjustment for the consumer and producer visually in the ui, so they are aligned properly.
            consumer_transform.X = 15;
            producer_transform.X = 15;

        }

        private void Process() // each time the timer ticks we chooserandom. remember the timer freezes when starting a producer or consumer. also we put the progressring to false.
        {
            ProgressRing.IsActive = false;
            ChooseRandom();
        }

        private void ChooseRandom() // here we choose a random winner and also assign the winner to work.
        {
            ActionString.Content = "Escogiendo el turno al azar...";
            int winner = random.Next(0, 2);

            // we check for the winner.
            if (winner == 0) StartConsumer();
            else StartProducer();
        }

        async private void StartConsumer()
        {
            // we have to stop the timer when entering consumer or producer. this is so the timer doesn't tick again when those functions are working. then when everything is done, start the timer again, at the bottom of this function.
            main_timer.Stop();

            bool consumed_all = true; // this is just a variable to know if the consumer has consumed all or not. there's a variable like this in the producer too.

            ActionString.Content = "Ganador: consumidor";
            await Task.Delay(time_delay_messages); // we use the task.delay to delay the messages of the app. we use await, as if we use sleep, then the whole program freezes, not just this function.

            // for this part, we choose an amount to eat from 3 to 6, remember that 7 is not inclusive. 
            int eat_amount = random.Next(3, 7);
            ActionString.Content = "El consumidor obtendrá " + eat_amount.ToString() + " objetos para agarrar.";

            ProgressRing.IsActive = true;

            await Task.Delay(time_delay_messages);

            for (int i = 0; i < eat_amount; i++) // we do a for to check the consumer vertex. if it returns true, then we consume the buffer. otherwise means that the buffer is empty. if the buffer is empty and is not the last iteration, then a message for this will appear, in the else condition.
            {
                if (list.CheckConsumerVertex()) await ConsumeBuffer();
                else
                {
                    if (i == 0) ActionString.Content = "No hay objetos en el búffer para agarrar.";
                    else ActionString.Content = "No pudo agarrar más. Se acabaron los objetos en el\nbúffer. Objetos que agarró: " + (i).ToString();
                    consumed_all = false;
                    break;
                }
            }

            ProgressRing.IsActive = false;

            // here we just check the consumed_all variable to see if everything was eaten.
            if (consumed_all) ActionString.Content = "Se consumieron los " + eat_amount.ToString() + " objetos.";
            await Task.Delay(time_delay_messages);

            main_timer.Start();
        }

        async private Task ConsumeBuffer() // this is the function to consume the buffer. this function is called once a condition has been proved that there's something to consume in the buffer.
        {
            list.DeAssignVertexValue(); // we deassign the value of the actual vertex of the consumer. in this case, from an emoji it would return to "" as string,

            total_items_consumed++; // we add to the consumed items and also update the label
            consumed_items_label.Content = "Objetos consumidos: " + total_items_consumed.ToString();

            // animations
            // we do the animation, just like the producer. same as producer, but for consumer.
            await MoveConsumer(new Point(producer_consumer_move_distance * list.actual_consumer_vertex + 1, 0), time_animation_speed);
        }

        async private void StartProducer() // this function is to start the producer. just like the consumer, but for the producer.
        {
            main_timer.Stop();

            bool produced_all = true;

            ActionString.Content = "Ganador: productor";
            await Task.Delay(time_delay_messages);

            // we choose an amount to produce. 3-6 inclusive, the 7 doesn't count for being inclusive.
            int produce_amount = random.Next(3, 7);
            ActionString.Content = "El productor pondrá " + produce_amount.ToString() + " objetos en el búffer.";

            ProgressRing.IsActive = true;

            await Task.Delay(time_delay_messages);

            for (int i = 0; i < produce_amount; i++) // here we check if the slot is empty, if so then produce. if not, then show a message for this in the else.
            {
                if (list.CheckProducerVertex()) await ProduceBuffer();
                else
                {
                    if (i == 0) ActionString.Content = "No hay espacio para producir objetos.";
                    else ActionString.Content = "Ya no hay espacio para poner más objetos.";
                    produced_all = false;
                    break;
                }
            }

            ProgressRing.IsActive = false;

            // if the produced all variable is true, then show the message for when all the items were produced.
            if (produced_all) ActionString.Content = "Se produjeron los " + produce_amount.ToString() + " objetos.";
            await Task.Delay(time_delay_messages);

            main_timer.Start();
        }

        async private Task ProduceBuffer() // in this function we "produce" to the buffer. we have to use async for the await that we use inside here, and also we return a "Task". this is so the function that calls this one will wait until this one finishes, so everything inside here can be done without problems.
        {
            string emoji_chosen = emojis[random.Next(0, 60)]; // whe choose a random emoji
            list.AssignVertexValue(emoji_chosen); // and then assign that emoji to the vertex the producer is in.


            total_items_produced++; // we increase the amount of items produced and also update the produced_items_label
            produced_items_label.Content = "Objetos producidos: " + total_items_produced.ToString();

            // animations
            // this is the animation to move the producer. basically the same as the consumer. we do a calculation to know where, in this case, the producer is going to go, multiplying the move distance fr both consumer and producer by the actual vertex plus one. the second parameter is zero, as it would be for the Y axis, which is not used here.
            await MoveProducer(new Point(producer_consumer_move_distance * list.actual_producer_vertex + 1, 0), time_animation_speed); // the +1 in calculating the X is because it could be that the actual vertex is zero. so we just use +1 to fix this. the other is just to calculate automatically the position where the consumer/producer has to go.
        }

        private void InitializeSlots() // in this function we create a list and insert values from 0 to 21 in it, and pass it as item source to the slotspanel. this generates all the slots we need.
        {
            for (int i = 0; i < 22; i++)
            {
                collections_vm.EmojisCollection.Add(new Slot(i)); // we also add this to the emojis collection, as we need to fill it out with empty strings and vertex so we have our circular linked list.
                collections_vm.EmojisCollection[i].AssignEmoji(""); // we assign the emoji to "", as the list will be empty when starting the program.
            }

            SlotsPanel.ItemsSource = collections_vm.EmojisCollection; // we assign that the source of the slotspanel will be the collection that we created, this then wil create the 22 borders we want.
        }

        // buttons for the app
        public void ExitApp(object sender, RoutedEventArgs args) { Environment.Exit(0); } // this is to exit the app. we can use Environment.Exit(0); to close the app whenever we want.

        public void SpeedOne(object sender, RoutedEventArgs args) // this functions (speedone through speedfour) are for controlling the speed of the program when the user presses the buttons respectively.
        {
            // normal speeds are this ones. every time the speed increments, we divide the speed by 2.
            time_delay_messages = 2000;
            timer_tick_delay = 1000;
            time_animation_speed = 1;

            // we update the label for the speed, and also the string for the actual_simulation_speed.
            actual_simulation_speed = "X1";
            speed_label.Content = "Velocidad: " + actual_simulation_speed;
            Console.WriteLine("setting speed to: 1");
        }

        public void SpeedTwo(object sender, RoutedEventArgs args)
        {
            time_delay_messages = 1000;
            timer_tick_delay = 500;
            time_animation_speed = 0.5;
            actual_simulation_speed = "X2";
            speed_label.Content = "Velocidad: " + actual_simulation_speed;
            Console.WriteLine("setting speed to: 2");
        }

        public void SpeedThree(object sender, RoutedEventArgs args)
        {
            time_delay_messages = 500;
            timer_tick_delay = 250;
            time_animation_speed = 0.25;
            actual_simulation_speed = "X3";
            speed_label.Content = "Velocidad: " + actual_simulation_speed;
            Console.WriteLine("setting speed to: 3");
        }

        public void SpeedFour(object sender, RoutedEventArgs args)
        {
            time_delay_messages = 250;
            timer_tick_delay = 125;
            time_animation_speed = 0.12;
            actual_simulation_speed = "X4";
            speed_label.Content = "Velocidad: " + actual_simulation_speed;
            Console.WriteLine("setting speed to: 4");
        }

        // below are the moveconsumer and moveproducer functions. we can fusion this two functions into one, should do this later.
        private async Task MoveConsumer(Point target, double seconds) // we obtain the point as the target, and also the seconds that the animation will last.
        {
            var animX = new Animation() // we create a new animation for the X. in this case we just need an animation for the X axis, as they will move horizontally.
            {
                Duration = TimeSpan.FromSeconds(seconds), // we set the duration of our seconds
                Easing = new CubicEaseOut(), // we set the easing. in this case we use this one, but we can change this later if we want.
                Children =
                {
                    // we then create the keyframes for the animation itself. in this case we choose the canvas leftproperty. we use canvas to use absolute positioning, because when using the consumer or producer itself it caused trouble with the coordinates.
                    new KeyFrame { Cue = new Cue(0d), Setters = { new Setter(Canvas.LeftProperty, Canvas.GetLeft(consumer)) } }, // we first obtain the starting value, which would be the control is before the animation begins. this control is related to the consumer in this case.
                    new KeyFrame { Cue = new Cue(1d), Setters = { new Setter(Canvas.LeftProperty, target.X) } }, // then this is the target, where we want the animation to end and where we want our consumer to go, in this case.
                }
            };

            Canvas.SetLeft(consumer, target.X); // with this we can keep the position of the icon (pathicon) as the animation goes, so it doesn't keep restarting when the animation is over.

            await Task.WhenAll(animX.RunAsync(consumer, CancellationToken.None)); // we do the animation here and also wait for it to end. in this function we don't have to worry about the function calling because we return a Task here too, as we explained previously.
        }

        // same as the consumer function, but for the producer.
        private async Task MoveProducer(Point target, double seconds)
        {
            var animX = new Animation()
            {
                Duration = TimeSpan.FromSeconds(seconds),
                Easing = new CubicEaseOut(),
                Children =
                {
                    new KeyFrame { Cue = new Cue(0d), Setters = { new Setter(Canvas.LeftProperty, Canvas.GetLeft(producer)) } },
                    new KeyFrame { Cue = new Cue(1d), Setters = { new Setter(Canvas.LeftProperty, target.X) } },
                }
            };

            Canvas.SetLeft(producer, target.X);

            await Task.WhenAll(animX.RunAsync(producer, CancellationToken.None));
        }
    }
}