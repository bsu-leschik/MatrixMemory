namespace MatrixMemory.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public bool StartMenu => true;
        public bool Registration => false;
        public bool MainGame => false;
    }
}