using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Lattice;
using Threading;

namespace StellarSimulation
{

    /// <summary>
    /// Определяет выбранную систему симуляции
    /// </summary>
    enum SystemType { None, SlowStars, FastStars, MassiveBlackHole, OrbitalSystem, DoubleSystem, PlanetarySystem, DistribTest };

    /// <summary>
    /// "Мир" симуляции 
    /// </summary>
    class World
    {

        /// <summary>
        /// Количество миллисекунд между обновлением кадров
        /// </summary>
        private const int FrameInt = 33;

        /// <summary>
        /// Погрешность измерения FPS (кадров в секунду) 
        /// </summary>
        private const double FpsEasing = 0.2;

        /// <summary>
        /// Максимальное количество кадров в секунду  
        /// </summary>
        private const double FpsMax = 999.9;

        /// <summary>
        /// Обзор камеры (POV) 
        /// </summary>
        private const double CameraPOV = 9e8;

        /// <summary>
        /// Начальное положение камеры по оси Z
        /// </summary>
        private const double CameraZStart = 1e6;

        /// <summary>
        /// Константа, задающее "ускорение" камеры при вращении колеса мыши 
        /// </summary>
        private const double CameraZBoosting = -2e-4;

        /// <summary>
        /// Сглаживание масштабирования
        /// </summary>
        private const double CameraZEasing = 0.94;

        /// <summary>
        /// Гравитационная постоянная 
        /// </summary>
        public static double G = 6.67;

        /// <summary>
        /// Максимальная скорость
        /// </summary>
        public static double C = 3e6;

        /// <summary>
        /// Экземпляр "мира" 
        /// </summary>
        public static World Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new World();
                }
                return _model;
            }
        }
        private static World _model = null;

        /// <summary>
        /// Количество тел, задействованных в симуляции
        /// </summary>
        public int BodyAllocNumber
        {
            get
            {
                return _bodies.Length;
            }
            set
            {
                if (_bodies.Length != value)
                {
                    lock (_bodyBan)
                        _bodies = new Body[value];
                    Frames = 0;
                }
            }
        }

        /// <summary>
        /// Количество тел, существующих в симуляции 
        /// </summary>
        public int BodyNumber
        {
            get
            {
                return _tree == null ? 0 : _tree.BodyNumber;
            }
        }

        /// <summary>
        /// Общая масса всех тел, существующих в симуляции
        /// </summary>
        public double TotalWeight
        {
            get
            {
                return _tree == null ? 0 : _tree.Mass;
            }
        }

        /// <summary>
        /// Количество кадров, прошедших с начала симуляции 
        /// </summary>
        public long Frames
        {
            get;
            private set;
        }

        /// <summary>
        /// Определяет - приостановлена симуляция или нет. 
        /// </summary>
        public Boolean Active
        {
            get;
            set;
        }

        /// <summary>
        /// Кадров в секунду 
        /// </summary>
        public double FramePerSecond
        {
            get;
            private set;
        }

        /// <summary>
        /// Определяет - необходимо ли отображать древовидную структуру расчёта сил. 
        /// </summary>
        public Boolean DrawTree
        {
            get;
            set;
        }

        /// <summary>
        /// Массив тел в симуляции 
        /// </summary>
        private Body[] _bodies = new Body[1000];

        /// <summary>
        /// Запрет на создание новых тел
        /// </summary>
        private readonly Object _bodyBan = new Object();

        /// <summary>
        /// Дерево для расчёта сил
        /// </summary>
        private Octree _tree;

        /// <summary>
        /// Экземпляр визуализации, служащий для отображения 3D графики 
        /// </summary>
        private Renderer _renderer = new Renderer();

        /// <summary>
        /// Вспомогательный компонент, служащий для вычисления FPS (кадров в секунду)
        /// </summary>
        private Stopwatch _stopwatch = new Stopwatch();

        /// <summary>
        /// Позиция камеры относительно оси Z
        /// </summary>
        private double _cameraZ = CameraZStart;

        /// <summary>
        /// Скорость камеры относительно оси Z 
        /// </summary>
        private double _cameraZSpeed = 0;

        /// <summary>
        /// Создаёт "мир" и начинает симуляцию
        /// </summary>
        private World()
        {

            // Инициализация начальных значений 
            Active = true;
            Frames = 0;
            _renderer.Camera.Z = _cameraZ;
            _renderer.FOV = CameraPOV;

            // Запуск потока симуляции 
            new Thread(new ThreadStart(() =>
            {
                while (true)
                    Simulator();
            }))
            {
                IsBackground = true
            }.Start();
        }

        /// <summary>
        /// Производит вычисления и отрисовывает следующий кадр
        /// </summary>
        private void Simulator()
        {
            if (Active)
                lock (_bodyBan)
                {

                    // Обновление состояния тел и определение необходимой ширины дерева
                    double halfWidth = 0;
                    foreach (Body body in _bodies)
                        if (body != null)
                        {
                            body.Refresh();
                            halfWidth = Math.Max(Math.Abs(body.Position.X), halfWidth);
                            halfWidth = Math.Max(Math.Abs(body.Position.Y), halfWidth);
                            halfWidth = Math.Max(Math.Abs(body.Position.Z), halfWidth);
                        }

                    // Инициализация главного дерева и добавление к нему тел.  
                    // Главное дерево должно быть более чем в два раза  
                    _tree = new Octree(2.1 * halfWidth);
                    foreach (Body body in _bodies)
                        if (body != null)
                            _tree.Add(body);

                    // Параллельно ускоряет тела
                    Parallel.ForEach(_bodies, body =>
                    {
                        if (body != null)
                            _tree.Boost(body);
                    });

                    // Обновляет счётчик кадров, прошедших с начала симуляции 
                    if (_tree.BodyNumber > 0)
                        Frames++;
                }

            // Обновляет камеру 
            _cameraZ += _cameraZSpeed * _cameraZ;
            _cameraZ = Math.Max(1, _cameraZ);
            _cameraZSpeed *= CameraZEasing;
            _renderer.Camera.Z = _cameraZ;

            // Сон на определённое время
            int elapsed = (int)_stopwatch.ElapsedMilliseconds;
            if (elapsed < FrameInt)
                Thread.Sleep(FrameInt - elapsed);

            // Обновляет количество кадров в секунду симуляции
            _stopwatch.Stop();
            FramePerSecond += (1000.0 / _stopwatch.Elapsed.TotalMilliseconds - FramePerSecond) * FpsEasing;
            FramePerSecond = Math.Min(FramePerSecond, FpsMax);
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        /// <summary>
        /// Генерирует определённую гравитационную систему 
        /// </summary>
        /// <param name="type">Тип гравитационной системы</param>
        public void GenerateGravitySystem(SystemType type)
        {

            // Сброс кадров, прошедших с начала симуляции
            Frames = 0;

            lock (_bodyBan)
            {
                switch (type)
                {

                    // Сброс количества тел
                    case SystemType.None:
                        Array.Clear(_bodies, 0, _bodies.Length);
                        break;

                    // Генерация типа гравитационной системы "Медленные частицы" 
                    case SystemType.SlowStars:
                        {
                            for (int i = 0; i < _bodies.Length; i++)
                            {
                                double distance = PseudoRandom.Double(1e6);
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                Vector location = new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-2e5, 2e5), Math.Sin(angle) * distance);
                                double mass = PseudoRandom.Double(1e6) + 3e4;
                                Vector velocity = PseudoRandom.Vector(5);
                                _bodies[i] = new Body(location, mass, velocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Быстрые частицы"  
                    case SystemType.FastStars:
                        {
                            for (int i = 0; i < _bodies.Length; i++)
                            {
                                double distance = PseudoRandom.Double(1e6);
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                Vector location = new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-2e5, 2e5), Math.Sin(angle) * distance);
                                double mass = PseudoRandom.Double(1e6) + 3e4;
                                Vector velocity = PseudoRandom.Vector(5e3);
                                _bodies[i] = new Body(location, mass, velocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Массивные частицы" 
                    case SystemType.MassiveBlackHole:
                        {
                            _bodies[0] = new Body(Vector.Zero, 1e10);

                            Vector location1 = PseudoRandom.Vector(8e3) + new Vector(-3e5, 1e5 + _bodies[0].Radius, 0);
                            double mass1 = 1e6;
                            Vector velocity1 = new Vector(2e3, 0, 0);
                            _bodies[1] = new Body(location1, mass1, velocity1);

                            for (int i = 2; i < _bodies.Length; i++)
                            {
                                double distance = PseudoRandom.Double(2e5) + _bodies[1].Radius;
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                double vertical = Math.Min(2e8 / distance, 2e4);
                                Vector location = (new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-vertical, vertical), Math.Sin(angle) * distance) + _bodies[1].Position);
                                double mass = PseudoRandom.Double(5e5) + 1e5;
                                double speed = Math.Sqrt(_bodies[1].Weight * _bodies[1].Weight * G / ((_bodies[1].Weight + mass) * distance));
                                Vector velocity = Vector.Cross(location, Vector.YAxis).Unit() * speed + velocity1;
                                location = location.Rotate(0, 0, 0, 1, 1, 1, Math.PI * 0.1);
                                velocity = velocity.Rotate(0, 0, 0, 1, 1, 1, Math.PI * 0.1);
                                _bodies[i] = new Body(location, mass, velocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Орбитальная система" 
                    case SystemType.OrbitalSystem:
                        {
                            _bodies[0] = new Body(1e10);

                            for (int i = 1; i < _bodies.Length; i++)
                            {
                                double distance = PseudoRandom.Double(1e6) + _bodies[0].Radius;
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                Vector location = new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-2e4, 2e4), Math.Sin(angle) * distance);
                                double mass = PseudoRandom.Double(1e6) + 3e4;
                                double speed = Math.Sqrt(_bodies[0].Weight * _bodies[0].Weight * G / ((_bodies[0].Weight + mass) * distance));
                                Vector velocity = Vector.Cross(location, Vector.YAxis).Unit() * speed;
                                _bodies[i] = new Body(location, mass, velocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Двойная система" 
                    case SystemType.DoubleSystem:
                        {
                            double mass1 = PseudoRandom.Double(9e9) + 1e9;
                            double mass2 = PseudoRandom.Double(9e9) + 1e9;
                            double angle0 = PseudoRandom.Double(Math.PI * 2);
                            double distance0 = PseudoRandom.Double(1e5) + 3e4;
                            double distance1 = distance0 / 2;
                            double distance2 = distance0 / 2;
                            Vector location1 = new Vector(Math.Cos(angle0) * distance1, 0, Math.Sin(angle0) * distance1);
                            Vector location2 = new Vector(-Math.Cos(angle0) * distance2, 0, -Math.Sin(angle0) * distance2);
                            double speed1 = Math.Sqrt(mass2 * mass2 * G / ((mass1 + mass2) * distance0));
                            double speed2 = Math.Sqrt(mass1 * mass1 * G / ((mass1 + mass2) * distance0));
                            Vector velocity1 = Vector.Cross(location1, Vector.YAxis).Unit() * speed1;
                            Vector velocity2 = Vector.Cross(location2, Vector.YAxis).Unit() * speed2;
                            _bodies[0] = new Body(location1, mass1, velocity1);
                            _bodies[1] = new Body(location2, mass2, velocity2);

                            for (int i = 2; i < _bodies.Length; i++)
                            {
                                double distance = PseudoRandom.Double(1e6);
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                Vector location = new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-2e4, 2e4), Math.Sin(angle) * distance);
                                double mass = PseudoRandom.Double(1e6) + 3e4;
                                double speed = Math.Sqrt((mass1 + mass2) * (mass1 + mass2) * G / ((mass1 + mass2 + mass) * distance));
                                speed /= distance >= distance0 / 2 ? 1 : (distance0 / 2 / distance);
                                Vector velocity = Vector.Cross(location, Vector.YAxis).Unit() * speed;
                                _bodies[i] = new Body(location, mass, velocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Планетарная система"  
                    case SystemType.PlanetarySystem:
                        {
                            _bodies[0] = new Body(1e10);
                            int planets = PseudoRandom.Int32(10) + 5;
                            int planetsWithRings = PseudoRandom.Int32(1) + 1;
                            int k = 1;
                            for (int i = 1; i < planets + 1 && k < _bodies.Length; i++)
                            {
                                int planetK = k;
                                double distance = PseudoRandom.Double(2e6) + 1e5 + _bodies[0].Radius;
                                double angle = PseudoRandom.Double(Math.PI * 2);
                                Vector location = new Vector(Math.Cos(angle) * distance, PseudoRandom.Double(-2e4, 2e4), Math.Sin(angle) * distance);
                                double mass = PseudoRandom.Double(1e8) + 1e7;
                                double speed = Math.Sqrt(_bodies[0].Weight * _bodies[0].Weight * G / ((_bodies[0].Weight + mass) * distance));
                                Vector velocity = Vector.Cross(location, Vector.YAxis).Unit() * speed;
                                _bodies[k++] = new Body(location, mass, velocity);

                                // Генерация колец
                                const int RingParticles = 100;
                                if (--planetsWithRings >= 0 && k < _bodies.Length - RingParticles)
                                {
                                    for (int j = 0; j < RingParticles; j++)
                                    {
                                        double ringDistance = PseudoRandom.Double(1e1) + 1e4 + _bodies[planetK].Radius;
                                        double ringAngle = PseudoRandom.Double(Math.PI * 2);
                                        Vector ringLocation = location + new Vector(Math.Cos(ringAngle) * ringDistance, 0, Math.Sin(ringAngle) * ringDistance);
                                        double ringMass = PseudoRandom.Double(1e3) + 1e3;
                                        double ringSpeed = Math.Sqrt(_bodies[planetK].Weight * _bodies[planetK].Weight * G / ((_bodies[planetK].Weight + ringMass) * ringDistance));
                                        Vector ringVelocity = Vector.Cross(location - ringLocation, Vector.YAxis).Unit() * ringSpeed + velocity;
                                        _bodies[k++] = new Body(ringLocation, ringMass, ringVelocity);
                                    }
                                    continue;
                                }

                                // Генерация лун
                                int moons = PseudoRandom.Int32(4);
                                while (moons-- > 0 && k < _bodies.Length)
                                {
                                    double moonDistance = PseudoRandom.Double(1e4) + 5e3 + _bodies[planetK].Radius;
                                    double moonAngle = PseudoRandom.Double(Math.PI * 2);
                                    Vector moonLocation = location + new Vector(Math.Cos(moonAngle) * moonDistance, PseudoRandom.Double(-2e3, 2e3), Math.Sin(moonAngle) * moonDistance);
                                    double moonMass = PseudoRandom.Double(1e6) + 1e5;
                                    double moonSpeed = Math.Sqrt(_bodies[planetK].Weight * _bodies[planetK].Weight * G / ((_bodies[planetK].Weight + moonMass) * moonDistance));
                                    Vector moonVelocity = Vector.Cross(moonLocation - location, Vector.YAxis).Unit() * moonSpeed + velocity;
                                    _bodies[k++] = new Body(moonLocation, moonMass, moonVelocity);
                                }
                            }

                            // Генерация пояса астероидов
                            while (k < _bodies.Length)
                            {
                                double asteroidDistance = PseudoRandom.Double(4e5) + 1e6;
                                double asteroidAngle = PseudoRandom.Double(Math.PI * 2);
                                Vector asteroidLocation = new Vector(Math.Cos(asteroidAngle) * asteroidDistance, PseudoRandom.Double(-1e3, 1e3), Math.Sin(asteroidAngle) * asteroidDistance);
                                double asteroidMass = PseudoRandom.Double(1e6) + 3e4;
                                double asteroidSpeed = Math.Sqrt(_bodies[0].Weight * _bodies[0].Weight * G / ((_bodies[0].Weight + asteroidMass) * asteroidDistance));
                                Vector asteroidVelocity = Vector.Cross(asteroidLocation, Vector.YAxis).Unit() * asteroidSpeed;
                                _bodies[k++] = new Body(asteroidLocation, asteroidMass, asteroidVelocity);
                            }
                        }
                        break;

                    // Генерация типа гравитационной системы "Тест дистрибуции" 
                    case SystemType.DistribTest:
                        {
                            Array.Clear(_bodies, 0, _bodies.Length);
                            double distance = 4e4;
                            double mass = 5e6;

                            int side = (int)Math.Pow(_bodies.Length, 1.0 / 3);
                            int k = 0;
                            for (int a = 0; a < side; a++)
                                for (int b = 0; b < side; b++)
                                    for (int c = 0; c < side; c++)
                                        _bodies[k++] = new Body(distance * (new Vector(a - side / 2, b - side / 2, c - side / 2)), mass);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Поворачивает "мир" 
        /// </summary>
        /// <param name="point">Начальная точка оси вращения</param>
        /// <param name="direction">Направление оси вращения</param>
        /// <param name="angle">Угол, на который необходимо повернуть</param>
        public void Turn(Vector point, Vector direction, double angle)
        {
            lock (_bodyBan)
                Parallel.ForEach(_bodies, body =>
                {
                    if (body != null)
                        body.Turn(point, direction, angle);
                });
        }

        /// <summary>
        /// Перемещает камеру 
        /// </summary>
        /// <param name="delta">Количество перемещения</param>
        public void MoveCamera(int delta)
        {
            _cameraZSpeed += delta * CameraZBoosting;
        }

        /// <summary>
        /// Сбрасывает вид камеры 
        /// </summary>
        public void ResetCamera()
        {
            _cameraZ = CameraZStart;
            _cameraZSpeed = 0;
        }

        /// <summary>
        /// Отрисовывает тела в "мире" 
        /// </summary>
        /// <param name="g">Графическое пространство, на котором необходима отрисовка</param>
        public void Draw(Graphics g)
        {
            for (int i = 0; i < _bodies.Length; i++)
                if (_bodies[i] != null)
                {
                    Body body = _bodies[i];
                    _renderer.FillCircle2D(g, Brushes.White, body.Position, body.Radius);
                }

            if (DrawTree)
                _tree.Draw(g, _renderer);
        }
    }
}
