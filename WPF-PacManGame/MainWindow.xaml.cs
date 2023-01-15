using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WpfAnimatedGif;

namespace WPF_PacManGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;

        public static bool redLeft, redRight, redDown, redUp;
        public static bool blueLeft, blueRight, blueDown, blueUp;
        public static bool greenLeft, greenRight, greenUp, greenDown;

        public static List<Rect> wallList = new List<Rect>();

        int speed = 4;

        Rect pacmanHitBox;

        public static Rect redGhostHitBox;
        public static Rect blueGhostHitBox;
        public static Rect greenGhostHitBox;

        public static Rect RedPreviousPosition;
        public static Rect BluePreviousPosition;
        public static Rect GreenPreviousPosition;

        public static int GameMode;


        public static Rect hitBox;

        int ghostSpeed = 4;
        int ghostMoveStep = 1000;
        int currentGhostStep;


        int hardGhostSpeed = 4;
        int hardMoveStep = 240;
        int hardCurrentGhostStep;
        int score = 0;

        public static int timer;

        public MainWindow(int id)
        {
            InitializeComponent();

            GameMode = id;

            GameSetUp();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false)
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);

            }

            if (e.Key == Key.Up && noUp == false)
            {
                noRight = noDown = noLeft = false;
                goRight = goDown = goLeft = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && noDown == false)
            {
                noUp = noLeft = noRight = false;
                goUp = goLeft = goRight = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }


        }

        private void GameSetUp()
        {
            MyCanvas.Focus();

            //Tiki gry
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            currentGhostStep = ghostMoveStep;
            hardCurrentGhostStep = hardMoveStep;

            redLeft = true;

            blueLeft = true;

            greenRight = true;

            timer = 0;

            //Ładowanie obrazów 
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\pacman.gif");
            image.EndInit();

            ImageBehavior.SetAnimatedSource(pacman, image);

            // Below it needed to start animation. If not, it is only make visible but animation does not start.
            ImageBehavior.SetAutoStart(pacman, true);
            ImageBehavior.SetRepeatBehavior(pacman, RepeatBehavior.Forever);
            //pacman.Fill = pacmanImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\red.jpg"));
            redGuy.Fill = redGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\orange.jpg"));
            blueGuy.Fill = orangeGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\pink.jpg"));
            greenGuy.Fill = pinkGhost;

            ImageBrush hardModeGhost = new ImageBrush();
            hardModeGhost.ImageSource = new BitmapImage(new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\hardMode.jpg"));
            leftGuy.Fill = hardModeGhost;
            rightGuy.Fill = hardModeGhost;

            ImageBrush coin = new ImageBrush();
            coin.ImageSource = new BitmapImage(new Uri("C:\\Users\\jakki\\source\\repos\\WPF-PacManGame\\WPF-PacManGame\\images\\coin.jpg"));

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "wall")
                {
                    hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    wallList.Add(hitBox);
                }
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "coin")
                {
                    x.Fill = coin;
                }
            }

            if(GameMode == 1)
            {
                leftGuy.Visibility = Visibility.Hidden;
                rightGuy.Visibility = Visibility.Hidden;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            timer++;

            txtScore.Content = "Score: " + score;
            
            //Ruch Pacmana w danym kierunku
            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);
            redGhostHitBox = new Rect(Canvas.GetLeft(redGuy), Canvas.GetTop(redGuy), redGuy.Width, redGuy.Height);
            blueGhostHitBox = new Rect(Canvas.GetLeft(blueGuy), Canvas.GetTop(blueGuy), blueGuy.Width, blueGuy.Height);
            greenGhostHitBox = new Rect(Canvas.GetLeft(greenGuy), Canvas.GetTop(greenGuy), greenGuy.Width, greenGuy.Height);

            //Sprawdzanie wszystkich obiektów z mapy
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    //Sprawdź czy pacman wchodzi w ściane, jeżeli tak wyłącz mu movement
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }
                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }

                    //Sprawdź czy różowy wchodzi w ściane
                    if (greenLeft == true && greenGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(greenGuy, Canvas.GetLeft(greenGuy) + 10);
                    }
                    if (greenRight == true && greenGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(greenGuy, Canvas.GetLeft(greenGuy) - 10);
                    }
                    if (greenDown == true && greenGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(greenGuy, Canvas.GetTop(greenGuy) - 10);
                    }
                    if (greenUp == true && greenGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(greenGuy, Canvas.GetTop(greenGuy) + 10);
                    }

                    //Sprawdź czy czerwony wchodzi w ściane
                    if (redLeft == true && redGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(redGuy, Canvas.GetLeft(redGuy) + 10);
                    }
                    if (redRight == true && redGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(redGuy, Canvas.GetLeft(redGuy) - 10);
                    }
                    if (redDown == true && redGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(redGuy, Canvas.GetTop(redGuy) - 10);
                    }
                    if (redUp == true && redGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(redGuy, Canvas.GetTop(redGuy) + 10);
                    }

                    //Sprawdzanie czy pomarańczowy wchodzi w ściane
                    if (blueLeft == true && blueGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(blueGuy, Canvas.GetLeft(blueGuy) + 10);
                    }
                    if (blueRight == true && blueGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(blueGuy, Canvas.GetLeft(blueGuy) - 10);
                    }
                    if (blueDown == true && blueGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(blueGuy, Canvas.GetTop(blueGuy) - 10);
                    }
                    if (blueUp == true && blueGhostHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(blueGuy, Canvas.GetTop(blueGuy) + 10);
                    }
                }

                if ((string)x.Tag == "coin")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        score++;
                    }

                    if (redGhostHitBox.IntersectsWith(hitBox))
                    {
                        if (timer % 7 == 0)
                        {
                            Canvas.SetTop(redGuy, Canvas.GetTop(x) - 2);
                            Canvas.SetLeft(redGuy, Canvas.GetLeft(x) - 2);

                            SelectNextMoveRed();
                        }
                    }

                    if (blueGhostHitBox.IntersectsWith(hitBox))
                    {
                        if (timer % 7 == 0)
                        {

                            Canvas.SetTop(blueGuy, Canvas.GetTop(x) - 2);
                            Canvas.SetLeft(blueGuy, Canvas.GetLeft(x) - 2);

                            SelectNextMoveBlue();

                        }
                    }

                    if (greenGhostHitBox.IntersectsWith(hitBox))
                    {
                        if (timer % 7 == 0)
                        {
                            Canvas.SetTop(greenGuy, Canvas.GetTop(x) - 2);
                            Canvas.SetLeft(greenGuy, Canvas.GetLeft(x) - 2);

                            SelectNextMoveGreen();
                        }
                    }
                }

                if ((string)x.Tag == "blueGhost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        this.Hide();

                        Lost window = new Lost(GameMode);
                        window.ShowDialog();
                    }

                    if (blueLeft == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }

                    if (blueRight == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    if (blueUp == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }

                    if (blueDown == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }
                }

                if ((string)x.Tag == "redGhost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        this.Hide();

                        Lost window = new Lost(GameMode);
                        window.ShowDialog();
                    }

                    if (redLeft == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }

                    if (redRight == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    if (redUp == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }

                    if (redDown == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }
                }

                if ((string)x.Tag == "greenGhost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        this.Hide();

                        Lost window = new Lost(GameMode);
                        window.ShowDialog();
                    }

                    if (greenLeft == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);
                    }

                    if (greenRight == true)
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    if (greenUp == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) - ghostSpeed);
                    }

                    if (greenDown == true)
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }              
                }

                if (GameMode == 2)
                {
                    if ((string)x.Tag == "hardGhost")
                    {
                        if (pacmanHitBox.IntersectsWith(hitBox))
                        {
                            this.Hide();

                            Lost window = new Lost(GameMode);
                            window.ShowDialog();
                        }

                        Canvas.SetTop(x, Canvas.GetTop(x) + hardGhostSpeed);

                        hardCurrentGhostStep--;

                        if (hardCurrentGhostStep < 1)
                        {
                            hardCurrentGhostStep = hardMoveStep;
                            hardGhostSpeed = -hardGhostSpeed;
                        }
                    }
                }
            }

            if (score == 204)
            {
                this.Hide();

                Win window = new Win(GameMode);
                window.ShowDialog();
            }
        }

        public void SelectNextMoveGreen()
        {
            var newGreenBottom = new Rect(Canvas.GetLeft(greenGuy), Canvas.GetTop(greenGuy) + 15, greenGuy.Width, greenGuy.Height);

            var newGreenTop = new Rect(Canvas.GetLeft(greenGuy), Canvas.GetTop(greenGuy) - 15, greenGuy.Width, greenGuy.Height);

            var newGreenRight = new Rect(Canvas.GetLeft(greenGuy) + 15, Canvas.GetTop(greenGuy), greenGuy.Width, greenGuy.Height);

            var newGreenLeft = new Rect(Canvas.GetLeft(greenGuy) - 15, Canvas.GetTop(greenGuy), greenGuy.Width, greenGuy.Height);

            var nextPossibleMovesList = new List<int>()
            {
                1, 2, 3, 4,
            };

            if (greenGhostHitBox.X > GreenPreviousPosition.X)
            {
                nextPossibleMovesList.Remove(1);
            }

            if (greenGhostHitBox.X < GreenPreviousPosition.X)
            {
                nextPossibleMovesList.Remove(2);
            }

            if (greenGhostHitBox.Y > GreenPreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(3);
            }

            if (greenGhostHitBox.Y < GreenPreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(4);
            }

            GreenPreviousPosition = greenGhostHitBox;

            //Legenda 1 - lewo, 2 - prawo, góra - 3, góra - 4

            foreach (var y in wallList)
            {
                if (newGreenLeft.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(1);
                }

                if (newGreenRight.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(2);
                }

                if (newGreenTop.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(3);
                }

                if (newGreenBottom.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(4);
                }
            }

            var rnd = new Random();

            var nextMove = nextPossibleMovesList.OrderBy(x => rnd.Next()).Take(1).FirstOrDefault();


            switch (nextMove)
            {
                case 1:
                    greenLeft = true;
                    greenRight = greenUp = greenDown = false;
                    break;
                case 2:
                    greenRight = true;
                    greenLeft = greenUp = greenDown = false;
                    break;
                case 3:
                    greenUp = true;
                    greenRight = greenLeft = greenDown = false;
                    break;
                case 4:
                    greenDown = true;
                    greenRight = greenUp = greenLeft = false;

                    break;
                default:
                    break;
            }
        }

        public void SelectNextMoveRed()
        {
            var newRedBottom = new Rect(Canvas.GetLeft(redGuy), Canvas.GetTop(redGuy) +15, redGuy.Width, redGuy.Height);

            var newRedTop = new Rect(Canvas.GetLeft(redGuy), Canvas.GetTop(redGuy) - 15, redGuy.Width, redGuy.Height);

            var newRedRight = new Rect(Canvas.GetLeft(redGuy) + 15, Canvas.GetTop(redGuy), redGuy.Width, redGuy.Height);

            var newRedLeft = new Rect(Canvas.GetLeft(redGuy) - 15, Canvas.GetTop(redGuy), redGuy.Width, redGuy.Height);

            var nextPossibleMovesList = new List<int>()
            {
                1, 2, 3, 4,
            };

            if(redGhostHitBox.X > RedPreviousPosition.X)
            {
                nextPossibleMovesList.Remove(1);
            }

            if (redGhostHitBox.X < RedPreviousPosition.X)
            {
                nextPossibleMovesList.Remove(2);
            }

            if (redGhostHitBox.Y > RedPreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(3);
            }

            if (redGhostHitBox.Y < RedPreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(4);
            }

            RedPreviousPosition = redGhostHitBox;

            //Legenda 1 - lewo, 2 - prawo, góra - 3, góra - 4

            foreach (var y in wallList)
            {
                if (newRedLeft.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(1);
                }

                if (newRedRight.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(2);
                }

                if (newRedTop.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(3);
                }

                if (newRedBottom.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(4);
                }
            }

            var rnd = new Random();

            var nextMove = nextPossibleMovesList.OrderBy(x => rnd.Next()).Take(1).FirstOrDefault();


            switch (nextMove)
            {
                case 1:
                    redLeft = true;
                    redRight = redUp = redDown = false;
                    break;
                case 2:
                    redRight = true;
                    redLeft = redUp = redDown = false;
                    break;
                case 3:
                    redUp = true;
                    redRight = redLeft = redDown = false;
                    break;
                case 4:
                    redDown = true;
                    redRight = redUp = redLeft = false;

                    break;
                default:
                    break;
            }
        }

        public void SelectNextMoveBlue()
        {
            var newBlueBottom = new Rect(Canvas.GetLeft(blueGuy), Canvas.GetTop(blueGuy) + 15, blueGuy.Width, blueGuy.Height);

            var newBlueTop = new Rect(Canvas.GetLeft(blueGuy), Canvas.GetTop(blueGuy) - 15, blueGuy.Width, blueGuy.Height);

            var newBlueRight = new Rect(Canvas.GetLeft(blueGuy) + 15, Canvas.GetTop(blueGuy), blueGuy.Width, blueGuy.Height);

            var newBlueLeft = new Rect(Canvas.GetLeft(blueGuy) - 15, Canvas.GetTop(blueGuy), blueGuy.Width, blueGuy.Height);

            var nextPossibleMovesList = new List<int>()
            {
                1, 2, 3, 4,
            };

            if (blueGhostHitBox.X > BluePreviousPosition.X)
            {
                nextPossibleMovesList.Remove(1);
            }

            if (blueGhostHitBox.X < BluePreviousPosition.X)
            {
                nextPossibleMovesList.Remove(2);
            }

            if (blueGhostHitBox.Y > BluePreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(3);
            }

            if (blueGhostHitBox.Y < BluePreviousPosition.Y)
            {
                nextPossibleMovesList.Remove(4);
            }

            BluePreviousPosition = blueGhostHitBox;

            //Legenda 1 - lewo, 2 - prawo, góra - 3, góra - 4

            foreach (var y in wallList)
            {
                if (newBlueLeft.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(1);
                }

                if (newBlueRight.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(2);
                }

                if (newBlueTop.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(3);
                }

                if (newBlueBottom.IntersectsWith(y))
                {
                    nextPossibleMovesList.Remove(4);
                }
            }

            var rnd = new Random();

            var nextMove = nextPossibleMovesList.OrderBy(x => rnd.Next()).Take(1).FirstOrDefault();


            switch (nextMove)
            {
                case 1:
                    blueLeft = true;
                    blueRight = blueUp = blueDown = false;
                    break;
                case 2:
                    blueRight = true;
                    blueLeft = blueUp = blueDown = false;
                    break;
                case 3:
                    blueUp = true;
                    blueRight = blueLeft = blueDown = false;
                    break;
                case 4:
                    blueDown = true;
                    blueRight = blueUp = blueLeft = false;
                    break;
                default:
                    break;
            }
        }

        private void GameOver(string message)
        {
            // inside the game over function we passing in a string to show the final message to the game
            gameTimer.Stop(); // stop the game timer
            MessageBox.Show(message, "The Pac Man Game WPF MOO ICT"); // show a mesage box with the message that is passed in this function

            // when the player clicks ok on the message box
            // restart the application
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
