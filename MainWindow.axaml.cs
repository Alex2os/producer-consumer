using Avalonia.Controls;

namespace ProducerConsumer;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new SimulationPage();
    }
}