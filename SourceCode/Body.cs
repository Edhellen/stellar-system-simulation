using System;
using System.Drawing;
using Lattice;

namespace StellarSimulation
{

    /// <summary>
    /// Массивное тело, используемое в симуляции
    /// </summary>
    class Body
    {

        /// <summary>
        /// Возвращает радиус, определенный для данного значения массы.
        /// </summary>
        /// <param name="mass">Масса тела, для которого необходимо рассчитать радиус</param>
        /// <returns>Радиус, определённый для данного значения массы</returns>
        public static double GetRadiusOfBody(double mass)
        {

            /* Мы предполагаем, что у всех тел одинаковая плотность, таким образом 
             * объём пропорционален массе. Тогда мы используем инверсию уравнения для объёма сферы, 
             * чтобы рассчитать радиус. Конечный результат произвольно измерен и добавлен к константе 
             * так что тела, как правило, видны */
            return 10 * Math.Pow(3 * mass / (4 * Math.PI), 1 / 3.0) + 10;
        }

        /// <summary>
        /// Пространственное расположение тела 
        /// </summary>
        public Vector Position = Vector.Zero;

        /// <summary>
        /// Скорость тела 
        /// </summary>
        public Vector Velocity = Vector.Zero;

        /// <summary>
        /// Ускорение, накопленное телом в течение одного шага моделирования.
        /// </summary>
        public Vector Boosting = Vector.Zero;

        /// <summary>
        /// Масса тела 
        /// </summary>
        public double Weight;

        /// <summary>
        /// Радиус тела
        /// </summary>
        public double Radius
        {
            get
            {
                return GetRadiusOfBody(Weight);
            }
        }

        /// <summary>
        /// Создаёт тело с заданной массой. Все остальные значения равны 0 
        /// </summary>
        /// <param name="mass">Масса нового тела</param>
        public Body(double mass)
        {
            Weight = mass;
        }

        /// <summary>
        /// Создаёт тело с заданными местоположением, массой и скоростью 
        /// Неуказанным свойствам присвоены назначенные значения по умолчанию - ноль, за исключением
        /// массы, которая принимает значение 1000000.
        /// </summary>
        /// <param name="location">Местоположение тела</param>
        /// <param name="mass">Масса тела</param>
        /// <param name="velocity">Скорость тела</param>
        public Body(Vector location, double mass = 1e6, Vector velocity = new Vector())
            : this(mass)
        {
            Position = location;
            Velocity = velocity;
        }

        /// <summary>
        /// Обновляет свойства тела, такие как местоположение скорость и 
        /// приобретённое ускорение. Этот метод вызвается каждый шаг симуляции. 
        /// </summary>
        public void Refresh()
        {
            double rapidity = Velocity.Magnitude();
            if (rapidity > World.C)
            {
                Velocity = World.C * Velocity.Unit();
                rapidity = World.C;
            }

            if (rapidity == 0)
                Velocity += Boosting;
            else
            {

                // Добавление релятивистской скорости 
                Vector AcPar = Vector.Projection(Boosting, Velocity);
                Vector AcOrt = Vector.Rejection(Boosting, Velocity);
                double alpha = Math.Sqrt(1 - Math.Pow(rapidity / World.C, 2));
                Velocity = (Velocity + AcPar + alpha * AcOrt) / (1 + Vector.Dot(Velocity, Boosting) / (World.C * World.C));
            }

            Position += Velocity;
            Boosting = Vector.Zero;
        }

        /// <summary>
        /// Вращает тело вдоль произвольной оси 
        /// </summary>
        /// <param name="point">Отправная точка оси вращения</param>
        /// <param name="direction">Направление оси вращения</param>
        /// <param name="angle">Угол, на который нужно повернуться</param>
        public void Turn(Vector point, Vector direction, double angle)
        {
            Position = Position.Rotate(point, direction, angle);

            /* 
             * Чтобы повернуть вектор скорости и ускорения мы должны 
             * установить отправную точку оси вращения.
             * Таким образом векторы вращаются вокруг своих отправных точек.
             */
            Velocity += point;
            Velocity = Velocity.Rotate(point, direction, angle);
            Velocity -= point;
            Boosting += point;
            Boosting = Boosting.Rotate(point, direction, angle);
            Boosting -= point;
        }
    }
}
