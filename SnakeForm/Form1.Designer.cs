namespace SnakeForm
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.endButton = new System.Windows.Forms.Button();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.bestScoreLabel = new System.Windows.Forms.Label();
            this.lives = new System.Windows.Forms.Label();
            this.game = new Snake.Game();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(613, 30);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // endButton
            // 
            this.endButton.Location = new System.Drawing.Point(613, 59);
            this.endButton.Name = "endButton";
            this.endButton.Size = new System.Drawing.Size(75, 23);
            this.endButton.TabIndex = 2;
            this.endButton.Text = "End";
            this.endButton.UseVisualStyleBackColor = true;
            this.endButton.Click += new System.EventHandler(this.endButton_Click);
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(511, 30);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(47, 13);
            this.scoreLabel.TabIndex = 3;
            this.scoreLabel.Text = "Score: 0";
            // 
            // bestScoreLabel
            // 
            this.bestScoreLabel.AutoSize = true;
            this.bestScoreLabel.Location = new System.Drawing.Point(513, 57);
            this.bestScoreLabel.Name = "bestScoreLabel";
            this.bestScoreLabel.Size = new System.Drawing.Size(69, 13);
            this.bestScoreLabel.TabIndex = 3;
            this.bestScoreLabel.Text = "Best score: 0";
            // 
            // lives
            // 
            this.lives.AutoSize = true;
            this.lives.Location = new System.Drawing.Point(513, 83);
            this.lives.Name = "lives";
            this.lives.Size = new System.Drawing.Size(44, 13);
            this.lives.TabIndex = 3;
            this.lives.Text = "Lives: 1";
            // 
            // game
            // 
            this.game.fructColor = System.Drawing.Color.Red;
            this.game.headColor = System.Drawing.Color.DarkGreen;
            this.game.lives = 1;
            this.game.Location = new System.Drawing.Point(29, 12);
            this.game.mapColor = System.Drawing.Color.Black;
            this.game.Name = "game";
            this.game.Size = new System.Drawing.Size(476, 476);
            this.game.TabIndex = 1;
            this.game.tailColor = System.Drawing.Color.Green;
            this.game.Text = "game";
            this.game.recordScore += new System.EventHandler(this.game_recordScore);
            this.game.recordBestScore += new System.EventHandler(this.game_recordBestScore);
            this.game.recordLives += new System.EventHandler(this.game_recordLives);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 529);
            this.Controls.Add(this.lives);
            this.Controls.Add(this.bestScoreLabel);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.endButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.game);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Snake.Game game;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button endButton;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label bestScoreLabel;
        private System.Windows.Forms.Label lives;
    }
}

