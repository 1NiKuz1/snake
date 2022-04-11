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
        protected int _lives = 1;
        protected bool _snakeStatus = true;
        protected bool _gameStatus = false;
        protected int _currentDurationGhost = 0;
        protected PictureBox _fruit;
        protected PictureBox[] _snake = new PictureBox[225];
        protected int _dirX = -1;
        protected int _dirY = 0;
        protected int _mapSize = 301;
        protected int _sizeOfSides = 20;
        protected int _score = 1;
        protected int _bestScore = 0;
        private int _oldWidth = 0;
        private int _oldHeight = 0;
        protected Color _mapColor = Color.Black;
        protected Color _fructColor = Color.Red;
        protected Color _headColor = Color.DarkGreen;
        protected Color _tailColor = Color.Green;
        protected Color _currentHeadColor;
        protected Color _currentTailColor;
        protected System.Timers.Timer _timer;
        protected event EventHandler _recordScore;
        protected event EventHandler _recordBestScore;
        protected event EventHandler _recordLives;

        public event EventHandler RecordScore
        {
            add { _recordScore += value; }
            remove { _recordScore -= value; }
        }

        protected void OnRecordScore()
        {
            _recordScore?.Invoke(this, new EventArgs());
        }

        public event EventHandler RecordBestScore
        {
            add { _recordBestScore += value; }
            remove { _recordBestScore -= value; }
        }

        protected void OnRecordBestScore()
        {
            _recordBestScore?.Invoke(this, new EventArgs());
        }

        public event EventHandler RecordLives
        {
            add { _recordLives += value; }
            remove { _recordLives -= value; }
        }

        protected void OnRecordLives()
        {
            _recordLives?.Invoke(this, new EventArgs());
        }
        public int Score
        {
            get { return _score; }
            private set
            {               
                if (_score != value)
                {
                    _score = value;
                    OnRecordScore();
                    BestScore = (BestScore < _score) ? _score : BestScore;
                }
                if (value == 224)
                {
                    EndProcess();
                }
            }
        }

        public int BestScore
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

        public int Lives
        {
            get { return _lives; }
            set
            {
                if (_lives != value)
                {
                    _lives = value;
                    OnRecordLives();
                }
            }
        }

        public Color MapColor
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

        public Color FructColor
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

        public Color HeadColor
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

        public Color TailColor
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
            //Запоминаем размеры одной клетки
            int oldSizeOfSides = _sizeOfSides;
            //Проверка минимальных значений высоты и ширины
            if (width < 301 || height < 301)
            {
                width = 301;
                height = 301;
                _mapSize = height;
                _sizeOfSides = 20;
                SettingAdaptiveValuesForObjects(oldSizeOfSides);
            }
            //Установка адаптивных значений
            if (Created)
            {
                if (_oldWidth == width || _oldHeight == height)
                {
                    if (_oldHeight != height)
                    {
                        width = height;
                    }
                    if (_oldWidth != width)
                    {
                        height = width;
                    }                   
                }
                if (_oldHeight < height && height % 15 == 1)
                {
                    _mapSize = height;
                    _sizeOfSides = height / 15;
                    SettingAdaptiveValuesForObjects(oldSizeOfSides);
                } else if(_oldHeight > height)
                {
                    _mapSize = height;
                    _sizeOfSides = height / 15;
                    SettingAdaptiveValuesForObjects(oldSizeOfSides);
                }
            }
            //Запоминаем значения ширины и высоты
            _oldWidth = width;
            _oldHeight = height;
            Invalidate();
            base.SetBoundsCore(x, y, width, height, specified);
        }

        public Game() : base()
        {
            _currentHeadColor = HeadColor;
            _currentTailColor = TailColor;
            SetStartPositionObjects();
            _timer = new System.Timers.Timer(150);
            _timer.AutoReset = true;
            _timer.Elapsed += Update;
            _timer.Enabled = false;
            this.KeyDown += new KeyEventHandler(OKP);                 
        }

        protected void SetStartPositionObjects()
        {
            _snake[0] = new PictureBox
            {
                Location = new Point(_sizeOfSides * 7 + 1, _sizeOfSides * 7 + 1),
                Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1),
                BackColor = HeadColor
            };
            this.Controls.Add(_snake[0]);
            _snake[1] = new PictureBox
            {
                Location = new Point(_sizeOfSides * 7 + 1 + _sizeOfSides, _sizeOfSides * 7 + 1),
                Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1),
                BackColor = TailColor
            };
            this.Controls.Add(_snake[1]);
            _fruit = new PictureBox
            {
                BackColor = FructColor,
                Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1)
            };
            GenerateFruit();
        }     

        private void SettingAdaptiveValuesForObjects(int oldSizeOfSides)
        {
            int fruitLocX = _fruit.Location.X / oldSizeOfSides;
            int fruitLocY = _fruit.Location.Y / oldSizeOfSides;
            _fruit.Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
            _fruit.Location = new Point(_sizeOfSides * fruitLocX + 1, _sizeOfSides * fruitLocY + 1);           
            if (_snake[Score] != null)
            {
                for (int i = 0; i <= Score; i++)
                {
                    _snake[i].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
                    _snake[i].Location = new Point(_sizeOfSides * (_snake[i].Location.X / oldSizeOfSides) + 1,
                        _sizeOfSides * (_snake[i].Location.Y / oldSizeOfSides) + 1);
                }
            }
            else //В случае если новый объект хвоста змейки не успел создаться, обновляем свойства змейки без его учета
            {
                for (int i = 0; i < Score; i++)
                {
                    _snake[i].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1);
                    _snake[i].Location = new Point(_sizeOfSides * (_snake[i].Location.X / oldSizeOfSides) + 1,
                        _sizeOfSides * (_snake[i].Location.Y / oldSizeOfSides) + 1);
                }
            }       
        }       

        protected void GenerateFruit()
        {
            int posFructX, posFructY;
            Random r = new Random();

            posFructX = r.Next(0, _mapSize - _sizeOfSides);
            posFructX = posFructX / _sizeOfSides * _sizeOfSides;
            posFructX++;

            posFructY = r.Next(0, _mapSize - _sizeOfSides);
            posFructY = posFructY / _sizeOfSides * _sizeOfSides;
            posFructY++;

            //Пробегаеся по всей змейки
            for (int i = 0; i <= Score; i++)
            {
                //Если поизиция фрукта совпадает с позициями змейки генерируем новые координаты и повторяем процедуру
                if (_snake[i].Location.X == posFructX && _snake[i].Location.Y == posFructY)
                {
                    posFructX = r.Next(0, _mapSize - _sizeOfSides);
                    posFructX = posFructX / _sizeOfSides * _sizeOfSides;
                    posFructX++;

                    posFructY = r.Next(0, _mapSize - _sizeOfSides);
                    posFructY = posFructY / _sizeOfSides * _sizeOfSides;
                    posFructY++;
                    i = 0;
                }
            }          
            _fruit.Location = new Point(posFructX, posFructY);
            this.Controls.Add(_fruit);
        }

        protected void ClearSnake()
        {
            for (int i = 0; i <= Score; i++)
                this.Controls.Remove(_snake[i]);          
        }

        protected void CheckBorders()
        {
            if ((_snake[0].Location.X < 0) || (_snake[0].Location.X > _mapSize - _sizeOfSides) ||
                (_snake[0].Location.Y < 0) || (_snake[0].Location.Y > _mapSize - _sizeOfSides)) 
            {
                EndProcess();
            }                    
        }

        protected void EatItself()
        {
            for (int i = 2; i < Score; i++)
            {
                if (_snake[0].Location == _snake[i].Location && _snakeStatus)
                {
                    if (Lives > 0)
                    {
                        _snakeStatus = false;                                             
                        --Lives;
                    } else
                    {
                        EndProcess();
                    }
                }
            }
            DurationGhost();
            GhostSnake();
        }

        protected void CreateSnakeTailElement()
        {
            _snake[Score] = new PictureBox
            {
                Location = new Point(_snake[Score - 1].Location.X - _sizeOfSides * _dirX,
                                              _snake[Score - 1].Location.Y - _sizeOfSides * _dirY),
                Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1),
                BackColor = _currentTailColor
            };
            this.Controls.Add(_snake[Score]);
        }

        protected void EatFruit()
        {
            if (_snake[0].Location.X == _fruit.Location.X && _snake[0].Location.Y == _fruit.Location.Y)
            {
                Score++;
                //Если игра закончена не нужно создавать новые объекты
                if (Score != 224)
                {
                    CreateSnakeTailElement();
                    GenerateFruit();
                }                   
            }
        }

        protected void InverseDirection()
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

        protected void SetNewSnakePosition()
        {
            for (int i = Score; i >= 1; i--)
            {
                _snake[i].Location = _snake[i - 1].Location;

            }
            var snakeLocationX = _snake[0].Location.X + _dirX * _sizeOfSides;
            var snakeLocationY = _snake[0].Location.Y + _dirY * _sizeOfSides;
            _snake[0].Location = new Point(snakeLocationX, snakeLocationY);         
        }


        protected void MoveSnake()
        {
            if ((_snake[0].Location.X + _dirX * _sizeOfSides != _snake[1].Location.X) || 
                (_snake[0].Location.Y + _dirY * _sizeOfSides != _snake[1].Location.Y))
            {
                SetNewSnakePosition();
            }            
            else
            {
                InverseDirection();
                SetNewSnakePosition();
            }
        }

        protected void DurationGhost()
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

        protected void ChangeSnakeColor(Color head, Color tail)
        {
            for (int i = 0; i <= _score; i++)
            {
                _snake[i].BackColor = tail;
            }
            _snake[0].BackColor = head;
        }

        protected void GhostSnake()
        {          
            if (!_snakeStatus && _currentDurationGhost == 1)
            {
                _currentHeadColor = Color.DarkBlue;
                _currentTailColor = Color.Blue;
                ChangeSnakeColor(_currentHeadColor, _currentTailColor);
               
            } else if (_currentDurationGhost > 20)
            {
                _currentHeadColor = HeadColor;
                _currentTailColor = TailColor;
                ChangeSnakeColor(HeadColor, TailColor);
            }
        }

        protected void Update(Object source, ElapsedEventArgs e)
        {
            try 
            {
                if (this.IsHandleCreated)
                {
                    Thread thread = new Thread(() =>
                    {
                        Invoke((Action)(() =>
                        {
                            //Если игра закончена не нужно делать лишних действий
                            if (Score != 224)
                            {
                                MoveSnake(); EatFruit(); CheckBorders(); EatItself();
                            }
                        }));
                    });
                    thread.Start();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                      
        }

        public void StartProcess()
        {
            _timer.Enabled = true;
            this.Focus();
            if (_gameStatus)
            {
                _gameStatus = false;
                Score = 1;
                Lives = 1;
                SetStartPositionObjects();                             
            }
            Invalidate();
        }

        public void EndProcess()
        {
            _timer.Stop();
            OnRecordBestScore();
            _currentHeadColor = HeadColor;
            _currentTailColor = TailColor;
            ClearSnake();
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
            b = new SolidBrush(MapColor);
            _fruit.BackColor = FructColor;           
            if (_snakeStatus)
            {
                _snake[0].BackColor = HeadColor;
                _snake[1].BackColor = TailColor;
                _currentHeadColor = HeadColor;
                _currentTailColor = TailColor;
            }

            //Прорисовка карты
            Pen p = new Pen(MapColor, 1);
            for (int i = 0; i <= _mapSize / _sizeOfSides; i++)
            {
                e.Graphics.DrawLine(p, 0, _sizeOfSides * i, _mapSize - 1, _sizeOfSides * i);
                e.Graphics.DrawLine(p, _sizeOfSides * i, 0, _sizeOfSides * i, _mapSize - 1);
            }
            //Вывод сообщение о результате игры
            if (_gameStatus)
            {
                string s = (_score != 224) ? "Game Over" : "Victory";
                int fontSize = (int)(Math.Round((_mapSize - 6) * 10 / e.Graphics.DpiY));
                Font font = new Font("Arial", fontSize);
                SolidBrush fontB = (_score != 224) ? new SolidBrush(Color.Red) : new SolidBrush(Color.Green);
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                RectangleF rect = new RectangleF(0, 0, _mapSize, _mapSize);
                e.Graphics.DrawString(s, font, fontB, rect, sf);
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
