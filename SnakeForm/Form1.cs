using Snake;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            game.StartProcess();
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            game.EndProcess();
        }

        private void game_recordScore(object sender, EventArgs e)
        {
            int score = game.Score;
            scoreLabel.Text = "Score: " + --score;
        }

        private void game_recordBestScore(object sender, EventArgs e)
        {
            int bestScore = (game.BestScore == 0) ? 1 : game.BestScore;
            bestScoreLabel.Text = "Best score: " + --bestScore;
        }

        private void game_recordLives(object sender, EventArgs e)
        {
            lives.Text = "Lives: " + game.Lives;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !game.GameStatus;
        }
    }
}
