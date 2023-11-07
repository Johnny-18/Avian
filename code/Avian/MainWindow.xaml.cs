using System.Linq;
using Application.Dal;

namespace Avian
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly AvianContext _context;
        
        public MainWindow(AvianContext context)
        {
            InitializeComponent();
            _context = context;
            GetPilots();
        }

        private void GetPilots()
        {
            var pilots = _context.Pilots.ToArray();
            Pilots.ItemsSource = pilots;
        }
    }
}