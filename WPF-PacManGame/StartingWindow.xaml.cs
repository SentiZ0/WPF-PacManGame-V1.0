using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

namespace WPF_PacManGame
{
    /// <summary>
    /// Interaction logic for StartingWindow.xaml
    /// </summary>
    public partial class StartingWindow : Window
    {
        public StartingWindow()
        {
            InitializeComponent();

            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\MainScreen.gif");
            image.EndInit();

            ImageBehavior.SetAnimatedSource(screen, image);

            // Below it needed to start animation. If not, it is only make visible but animation does not start.
            ImageBehavior.SetAutoStart(screen, true);
            ImageBehavior.SetRepeatBehavior(screen, RepeatBehavior.Forever);

            var image2 = new BitmapImage();
            image2.BeginInit();
            image2.UriSource = new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\MainScreen2.gif");
            image2.EndInit();

            ImageBehavior.SetAnimatedSource(screen2, image2);

            // Below it needed to start animation. If not, it is only make visible but animation does not start.
            ImageBehavior.SetAutoStart(screen2, true);
            ImageBehavior.SetRepeatBehavior(screen2, RepeatBehavior.Forever);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            MainWindow window1 = new MainWindow(1);
            window1.ShowDialog();

            window1.Close();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            this.Hide();

            MainWindow window1 = new MainWindow(2);
            window1.ShowDialog();

            window1.Close();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
