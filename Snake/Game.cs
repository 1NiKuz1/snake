using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Snake
{
    public class Game : Control
    {
        protected int _posFructX, _posFructY;
        protected int _lives = 1;
        protected bool _snakeStatus = true;
        protected bool _gameStatus = false;
        protected int _currentDurationGhost = 0;
        protected PictureBox _fruit;
        protected PictureBox[] _snake = new PictureBox[225];
        protected Label _gameStatusLabel;
        protected int _dirX = -1;
        protected int _dirY = 0;
        protected int _mapSize = 301;
        protected int _sizeOfSides = 20;
        protected int _score = 1;
        protected int _bestScore = 0;
        protected Color _mapColor;
        protected Color _fructColor;
        protected Color _headColor;
        protected Color _tailColor;
        protected Color _currentHeadColor;
        protected Color _currentTailColor;
        protected System.Timers.Timer _timer;
        protected event EventHandler _recordScore;
        protected event EventHandler _recordBestScore;
        protected event EventHandler _recordLives;



        public event EventHandler recordScore
        {
            add { _recordScore += value; }
            remove { _recordScore -= value; }
        }

        protected void onRecordScore()
        {
            _recordScore?.Invoke(this, new EventArgs());
        }

        public event EventHandler recordBestScore
        {
            add { _recordBestScore += value; }
            remove { _recordBestScore -= value; }
        }

        protected void onRecordBestScore()
        {
            _recordBestScore?.Invoke(this, new EventArgs());
        }

        public event EventHandler recordLives
        {
            add { _recordLives += value; }
            remove { _recordLives -= value; }
        }

        protected void onRecordLives()
        {
            _recordLives?.Invoke(this, new EventArgs());
        }
        public int score
        {
            get { return _score; }
            private set
            {
                if (_score != value)
                {
                    _score = value;
                    onRecordScore();
                    bestScore = (bestScore < _score) ? _score : bestScore;
                }
            }
        }

        public int bestScore
        {
            get { return _bestScore; }
            private set
            {
                if (_bestScore != value)
                {
                    _bestScore = value;
                }
            }
        }

        public int lives
        {
            get { return _lives; }
            set
            {
                if (_lives != value)
                {
                    _lives = value;
                    onRecordLives();
                }
            }
        }

        public Color mapColor
        {
            get
            {
                return _mapColor;
            }
            set
            {
                if (_mapColor != value)
                {
                    _mapColor = value;
                    Invalidate();
                }
            }
        }

        public Color fructColor
        {
            get
            {
                return _fructColor;
            }
            set
            {
                if (_fructColor != value)
                {
                    _fructColor = value;
                    Invalidate();
                }
            }
        }

        public Color headColor
        {
            get
            {
                return _headColor;
            }
            set
            {
                if (_headColor != value)
                {
                    _headColor = value;
                    Invalidate();
                }
            }
        }

        public Color tailColor
        {
            get
            {
                return _tailColor;
            }
            set
            {
                if (_tailColor != value)
                {
                    _tailColor = value;
                    Invalidate();
                }
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            int oldSizeOfSides = _sizeOfSides;
            if (width < 301 || height < 301)
            {
                width = 301;
                height = 301;
                _mapSize = height;
                _sizeOfSides = 20;
                settingAdaptiveValuesForObjects(oldSizeOfSides);
            }
            if (Created)
            {
                height = (width > height) ? width : height;
                width = (height > width) ? height : width;
                if (height % 15 == 0)
                {
                    _mapSize = height;
                    _sizeOfSides = height / 15;
                    settingAdaptiveValuesForObjects(oldSizeOfSides);
                }
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        public Game() : base()
        {
            mapColor = Color.Black;
            fructColor = Color.Red;
            headColor = Color.DarkGreen;
            tailColor = Color.Green;
            _currentHeadColor = headColor;
            _currentTailColor = tailColor;
            setStartPositionObjects();
            generateFruit();
            _timer = new System.Timers.Timer(150);
            _timer.AutoReset = true;
            _timer.Elapsed += update;
            _timer.Enabled = false;
            this.KeyDown += new KeyEventHandler(OKP);                 
        }

        protected void setStartPositionObjects()
        {
            _snake[0] = new PictureBox();
            _snake[0].Location = new Point(_sizeOfSides * 7 + 1, _sizeOfSides * 7 + 1);
            _snake[0].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _snake[0].BackColor = _headColor;
            this.Controls.Add(_snake[0]);
            _snake[1] = new PictureBox();
            _snake[1].Location = new Point(_sizeOfSides * 7 + 1 + _sizeOfSides, _sizeOfSides * 7 + 1);
            _snake[1].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _snake[1].BackColor = _tailColor;
            this.Controls.Add(_snake[1]);
            _fruit = new PictureBox();
            _fruit.BackColor = _fructColor;
            _fruit.Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
        }     

        private void settingAdaptiveValuesForObjects(int oldSizeOfSides)
        {
            int fuitLocX = _fruit.Location.X / oldSizeOfSides;
            int fuitLocY = _fruit.Location.Y / oldSizeOfSides;
            _fruit.Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _fruit.Location = new Point(_sizeOfSides * fuitLocX + 1, _sizeOfSides * fuitLocY + 1);
            _snake[0].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _snake[0].Location = new Point(_sizeOfSides * 7 + 1, _sizeOfSides * 7 + 1);
            _snake[1].Location = new Point(_sizeOfSides * 7 + 1 + _sizeOfSides, _sizeOfSides * 7 + 1);
            _snake[1].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
        }       

        protected void generateFruit()
        {
            Random r = new Random();

            _posFructX = r.Next(0, _mapSize - _sizeOfSides);
            _posFructX = _posFructX / _sizeOfSides * _sizeOfSides;
            _posFructX++;

            _posFructY = r.Next(0, _mapSize - _sizeOfSides);
            _posFructY = _posFructY / _sizeOfSides * _sizeOfSides;
            _posFructY++;

            for (int i = 0; i <= score; i++)
            {
                while (_snake[i].Location.X == _posFructX && _snake[i].Location.Y == _posFructY)
                {
                    _posFructX = r.Next(0, _mapSize - _sizeOfSides);
                    _posFructX = _posFructX / _sizeOfSides * _sizeOfSides;
                    _posFructX++;

                    _posFructY = r.Next(0, _mapSize - _sizeOfSides);
                    _posFructY = _posFructY / _sizeOfSides * _sizeOfSides;
                    _posFructY++;
                    i = 0;
                }
            }          
            _fruit.Location = new Point(_posFructX, _posFructY);
            this.Controls.Add(_fruit);
        }

        protected void clearSnake()
        {
            for (int i = 0; i <= score; i++)
                this.Controls.Remove(_snake[i]);          
        }

        protected void checkBorders()
        {
            if ((_snake[0].Location.X < 0) || (_snake[0].Location.X > _mapSize - _sizeOfSides) ||
                (_snake[0].Location.Y < 0) || (_snake[0].Location.Y > _mapSize - _sizeOfSides)) 
            {
                endProcess();
            }                    
        }

        protected void eatItself()
        {
            for (int i = 2; i < score; i++)
            {
                if (_snake[0].Location == _snake[i].Location && _snakeStatus)
                {
                    if (lives > 0)
                    {
                        _snakeStatus = false;                                             
                        --lives;
                    } else
                    {
                        endProcess();
                    }
                }
            }
            durationGhost();
            ghostSnake();
        }

        protected void generateItemTailSnake()
        {
            _snake[score] = new PictureBox();
            _snake[score].Location = new Point(_snake[score - 1].Location.X - _sizeOfSides * _dirX,
                                              _snake[score - 1].Location.Y - _sizeOfSides * _dirY);
            _snake[score].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _snake[score].BackColor = _currentTailColor;
            this.Controls.Add(_snake[score]);
        }

        protected void eatFruit()
        {
            if (_snake[0].Location.X == _posFructX && _snake[0].Location.Y == _posFructY)
            {
                score++;
                generateItemTailSnake();
                generateFruit();
            }
        }

        protected void inverseDirection()
        {
            if (_dirX != 0)
            {
                _dirX = (_dirX == 1) ? -1 : 1;
            }
            if (_dirY != 0)
            {
                _dirY = (_dirY == 1) ? -1 : 1;
            }
        }

        protected void createNewSnakePosition()
        {
            for (int i = score; i >= 1; i--)
            {
                _snake[i].Location = _snake[i - 1].Location;

            }
            var snakeLocationX = _snake[0].Location.X + _dirX * _sizeOfSides;
            var snakeLocationY = _snake[0].Location.Y + _dirY * _sizeOfSides;
            _snake[0].Location = new Point(snakeLocationX, snakeLocationY);         
        }


        protected void moveSnake()
        {
            if ((_snake[0].Location.X + _dirX * _sizeOfSides != _snake[1].Location.X) || 
                (_snake[0].Location.Y + _dirY * _sizeOfSides != _snake[1].Location.Y))
            {
                createNewSnakePosition();
            }            
            else
            {
                inverseDirection();
                createNewSnakePosition();
            }
        }

        protected void durationGhost()
        {
            if (!_snakeStatus && _currentDurationGhost <= 20)
            {
                ++_currentDurationGhost;
            }
            else
            {
                _currentDurationGhost = 0;              
                _snakeStatus = true;               
            }
        }

        protected void changeSnakeColor(Color head, Color tail)
        {
            for (int i = 0; i <= _score; i++)
            {
                _snake[i].BackColor = tail;
            }
            _snake[0].BackColor = head;
        }

        protected void ghostSnake()
        {          
            if (!_snakeStatus && _currentDurationGhost == 1)
            {
                _currentHeadColor = Color.DarkBlue;
                _currentTailColor = Color.Blue;
                changeSnakeColor(_currentHeadColor, _currentTailColor);
               
            } else if (_currentDurationGhost > 20)
            {
                _currentHeadColor = headColor;
                _currentTailColor = tailColor;
                changeSnakeColor(headColor, tailColor);
            }
        }

        protected void update(Object source, ElapsedEventArgs e)
        {
            try 
            {
                if (this.IsHandleCreated)
                {
                    Thread thread = new Thread(() =>
                    {
                        Invoke((Action)(() => { moveSnake(); checkBorders(); eatFruit(); eatItself(); }));
                    });
                    thread.Start();
                    Invalidate();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                      
        }

        public void startProcess()
        {
            _timer.Enabled = true;
            this.Focus();
            if (_gameStatus)
            {
                _gameStatus = false;
                setStartPositionObjects();
                generateFruit();
                lives = 1;
            }
        }

        public void endProcess()
        {
            _timer.Stop();
            onRecordBestScore();
            _currentHeadColor = headColor;
            _currentTailColor = tailColor;         
            clearSnake();
            Controls.Remove(_fruit);
            _gameStatus = true;
            Invalidate();
        }

        protected void OKP(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "D":
                    _dirX = 1;
                    _dirY = 0;
                    break;
                case "A":
                    _dirX = -1;
                    _dirY = 0;
                    break;
                case "W":
                    _dirY = -1;
                    _dirX = 0;
                    break;
                case "S":
                    _dirY = 1;
                    _dirX = 0;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush b = new SolidBrush(BackColor);
            e.Graphics.FillRectangle(b, ClientRectangle);
            b = new SolidBrush(mapColor);

            Pen p = new Pen(mapColor, 1);
            for (int i = 0; i <= _mapSize / _sizeOfSides; i++)
            {
                e.Graphics.DrawLine(p, 0, _sizeOfSides * i, _mapSize - 1, _sizeOfSides * i);
                e.Graphics.DrawLine(p, _sizeOfSides * i, 0, _sizeOfSides * i, _mapSize - 1);
            }
            if (_gameStatus)
            {
                string s = (_score != 225) ? "Game Over" : "Victory";
                int fontSize = (int)(Math.Round((_mapSize - 6) * 10 / e.Graphics.DpiY));
                Font font = new Font("Arial", fontSize);
                SolidBrush fontB = (_score != 225) ? new SolidBrush(Color.Red) : new SolidBrush(Color.Green);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                RectangleF rect = new RectangleF(0, 0, _mapSize, _mapSize);
                e.Graphics.DrawString(s, font, fontB, rect, sf);
                score = 1;
            }
            b.Dispose();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

    }
}
