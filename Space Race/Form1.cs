using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Space_Race
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();


        }



        List<Rectangle> player1 = new List<Rectangle>();
        List<Rectangle> player2 = new List<Rectangle>();
        Rectangle clock = new Rectangle(325, 0, 5, 400);
        List<Rectangle> ball1List = new List<Rectangle>();
        List<Rectangle> ball2List = new List<Rectangle>();

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush orangeBrush = new SolidBrush(Color.Orange);

        bool upKeyDown = false;
        bool downKeyDown = false;
        bool wKeyDown = false;
        bool sKeyDown = false;
        bool p1offScreen = false;
        bool p2offScreen = false;


        int speed = 6;
        int count = 0;
        int randVal;
        int p1Score = 0;
        int p2Score = 0;

        SoundPlayer Death = new SoundPlayer(Properties.Resources.Boom);
        SoundPlayer point = new SoundPlayer(Properties.Resources.Score);

        string state = "title";

        Random rng = new Random();

        private void gameStart()
        {
            ball1List.Clear();
            ball2List.Clear();

            player1.Add(new Rectangle(157, 365, 20, 20));
            player2.Add(new Rectangle(482, 365, 20, 20));

            clock = new Rectangle(325, 0, 5, 400);

            Rectangle ball1 = new Rectangle(-51, rng.Next(5, 345), 20, 3);
            Rectangle ball2 = new Rectangle(this.Width + 51, rng.Next(5, 345), 20, 3);

            ball1List.Add(ball1);
            ball2List.Add(ball2);

            state = "playing";
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = true;
                    break;
                case Keys.S:
                    sKeyDown = true;
                    break;
                case Keys.Up:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                    downKeyDown = true;
                    break;
                case Keys.Space:
                    if (state == "title" || state == "gameover")
                    {
                        gameStart();
                    }
                    break;
                case Keys.Escape:
                    if (state == "title" || state == "gameover")
                    {
                        this.Close();
                    }
                    break;

            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wKeyDown = false;
                    break;
                case Keys.S:
                    sKeyDown = false;
                    break;
                case Keys.Up:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                    downKeyDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {

            if (state == "playing")
            {

                count++;

                //moves clock down
                if (count % 3 == 0)
                {
                    clock.Y++;
                }

                //ends game when clock is gone
                if (clock.Y > this.Height)
                {
                    state = "gameover";
                }

                randVal = rng.Next(0, 101);

                //spawns balls at random 
                if (randVal < 10)
                {
                    Rectangle ball1 = new Rectangle(-51, rng.Next(5, 345), 20, 3);
                    Rectangle ball2 = new Rectangle(this.Width + 51, rng.Next(5, 345), 20, 3);

                    ball1List.Add(ball1);
                    ball2List.Add(ball2);
                }

                // moves player 1
                for (int i = 0; i < player1.Count; i++)
                {

                    if (wKeyDown == true)
                    {
                        int y = player1[i].Y - speed;
                        player1[i] = new Rectangle(player1[i].X, y, 20, 20);

                    }

                    if (sKeyDown == true && player1[0].Y > 0 && player1[i].Y + player1[i].Height < this.Height - 3)
                    {
                        int y = player1[i].Y + speed;
                        player1[i] = new Rectangle(player1[i].X, y, 20, 20);
                    }
                }

                //moves player 2
                for (int i = 0; i < player2.Count; i++)
                {


                    if (upKeyDown == true)
                    {
                        int y = player2[i].Y - speed;
                        player2[i] = new Rectangle(player2[i].X, y, 20, 20);

                    }

                    if (downKeyDown == true && player2[0].Y > 0 && player2[i].Y + player2[i].Height < this.Height - 3)
                    {
                        int y = player2[i].Y + speed;
                        player2[i] = new Rectangle(player2[i].X, y, 20, 20);

                    }
                }

                leftSide();
                RightSide();

                // kills player if they hit a ball
                for (int i = 0; i < ball1List.Count; i++)
                {

                    if (ball1List[i].IntersectsWith(player1[0]) || ball2List[i].IntersectsWith(player1[0]))
                    {
                        player1.Add(new Rectangle(157, 365, 20, 20));
                        player1.RemoveAt(0);
                        Death.Play();
                    }



                    if (ball1List[i].IntersectsWith(player2[0]) || ball2List[i].IntersectsWith(player2[0]))
                    {
                        player2.Add(new Rectangle(482, 365, 20, 20));
                        player2.RemoveAt(0);
                        Death.Play();
                    }

                }

                // gives player 1 a point and makes a new one at the bottem when they reach the top
                if (player1[0].Y < 0 && p1offScreen == false)
                {
                    p1Score++;
                    point.Play();
                    player1.Add(new Rectangle(157, this.Height + 1, 20, 20));
                    p1offScreen = true;
                }


                // gets rid of the player 1 at thats offscreen 
                if (player1[0].Y + player1[0].Height < -1)
                {
                    player1.RemoveAt(0);
                    p1offScreen = false;
                }


                // gives player 2 a point and makes a new one at the bottem when they reach the top
                if (player2[0].Y < 0 && p2offScreen == false)
                {
                    p2Score++;
                    point.Play();
                    player2.Add(new Rectangle(482, this.Height + 1, 20, 20));
                    p2offScreen = true;
                }

                // gets rid of the player 2 at thats offscreen 
                if (player2[0].Y + player2[0].Height < -1)
                {
                    player2.RemoveAt(0);
                    p2offScreen = false;
                }

            }

            Refresh();
        }


        //Moves and gets rid of astroroids 
        private void leftSide()
        {
            for (int i = 0; i < ball1List.Count; i++)
            {

                if (ball1List[i].X > this.Width + 10)
                {
                    ball1List.RemoveAt(i);
                }

                int x = ball1List[i].X + speed;
                ball1List[i] = new Rectangle(x, ball1List[i].Y, 20, 3);
            }
        }

        private void RightSide()
        {
            for (int i = 0; i < ball1List.Count; i++)
            {
                int x2 = ball2List[i].X - speed;
                ball2List[i] = new Rectangle(x2, ball2List[i].Y, 20, 3);

                if (ball2List[i].X < -51)
                {
                    ball2List.RemoveAt(i);
                }


            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (state == "title")
            {
                titleLabel.Visible = true;
                subtitleLabel.Visible = true;
            }

            if (state == "playing")
            {

                titleLabel.Visible = false;
                subtitleLabel.Visible = false;

                for (int i = 0; i < player1.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, player1[i]);
                }

                for (int i = 0; i < player2.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, player2[i]);
                }


                e.Graphics.FillRectangle(whiteBrush, clock);

                for (int i = 0; i < ball1List.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, ball1List[i]);
                    e.Graphics.FillRectangle(whiteBrush, ball2List[i]);
                }


                p2Label.Text = $"{p2Score}";
                p1Label.Text = $"{p1Score}";
            }

            if (state == "gameover")
            {
                p2Label.Text = $"";
                p1Label.Text = $"";

                titleLabel.Visible = true;
                subtitleLabel.Visible = true;

                if (p1Score > p2Score)
                {
                    titleLabel.Text = "Player 1 Wins";
                }
                else if (p1Score < p2Score)
                {
                    titleLabel.Text = "Player 2 Wins";
                }
                else
                {
                    titleLabel.Text = "Tie";
                }
            }
        }
    }
}
