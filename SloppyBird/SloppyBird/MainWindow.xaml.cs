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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace SloppyBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        double score;   // Är en double eftersom det finns två stycken pipes och man får poängen från att man åker igenom dem.
        int gravity = 8;
        bool gameover;
        Rect flappyBirdHitBox; //Recten är som en variabel som deffinerar om man har nuddat ett obejekt. Och innehåller höjden och bredden på fågeln.
        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;

            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width, flappyBird.Height);

            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity); // Gravity funktionen gör så att fågeln faller ner om man inte trycker eller håller in space-baren

            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 458) // Om fågeln rör sig för lågt i fönstret så avslutas spelet.
            {
                EndGame();
            }

            foreach (var x in MyCanvas.Children.OfType<Image>()) // Foreach-loopen gör så att tuberna åker in med en viss hastighet samt håller ett visst avstånd imellan dem tre.
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || ((string)x.Tag == "obs3"));
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5;

                    }

                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height); // En sats som håller koll på om fågeln har nuddat tuberna och om det är så körs end-game funktionen.

                    if (flappyBirdHitBox.IntersectsWith(pipeHitBox)) ;
                    {
                        EndGame();
                    }
                
               }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);

                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }

            }


        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) //När man trycker på space-baren så rör sig fågeln upp och ner
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);
                gravity = -8; // IStället för att fågeln rör sig neråt så rör den sig uppåt.
            }

            if (e.Key == Key.R && gameover == true) // Startar om spelet.
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            gravity = 8;
        }
        private void StartGame() //Klass som håller koll på om spelet ska startas och hur.
        {
            MyCanvas.Focus();

            int temp = 300; 

            score = 0; //Startar med score:0

            gameover = false;
            Canvas.SetTop(flappyBird, 190); //Startar med fågeln på en höjd av 190

            foreach (var x in MyCanvas.Children.OfType<Image>()) // En loop där alla eliment som är en bild går igenom
            {
                if((string)x.Tag == "obs1")   //Gör så att tuberna samt målnen rör sig in i bilden.
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2")   
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3")   
                {
                    Canvas.SetLeft(x, 1100);
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 300 + temp);
                    temp = 800;
                }

            }

            gameTimer.Start();

        }

        private void EndGame() //Denna klass är det som gör att spelet sluta, är kallad till ovan om man tex. stöter in i en av pipesen eller liknande, stoppar timern och visar ett meddelande.
        {
            gameTimer.Stop();
            gameover = true;
            txtScore.Content += " Game over, Press R to try again";
        }

    }


}
