using System;
using System.Drawing;
using Lattice;

namespace StellarSimulation
{

    /// <summary>
    /// Пространственная структура дерева алгоритма Barnes-Hut TreeCode algorithm. 
    /// </summary>
    class Octree
    {

        /// <summary>
        /// Допуск приближения группировки массы в симуляции 
        /// Тело только ускоряется, когда отношение ширины дерва  
        /// к расстоянию (от центра масс дерева до тела) меньше чем это
        /// </summary>
        private const double Admission = 0.5;

        /// <summary>
        /// Смягчающий фактор для уравнения ускорения
        /// Позволяет гасить рывки при близком взаимодействии тел
        /// </summary>
        private const double MitFactor = 700;

        /// <summary>
        /// Минимальная ширина дерева. Поддерева не создаются, когда их величина 
        /// меньше этого значения 
        /// </summary>
        private const double MinimumWidth = 1;

        /// <summary>
        /// Число тел в дереве 
        /// </summary>
        public int BodyNumber = 0;

        /// <summary>
        /// Общая масса тел, сосредоточенных в дереве
        /// </summary>
        public double Mass = 0;

        /// <summary>
        /// Массив поддеревьев в дереве
        /// </summary>
        private Octree[] _subTr = null;

        /// <summary>
        /// Расположение центра границ дерева 
        /// </summary>
        private Vector _position;

        /// <summary>
        /// Ширина границ дерева 
        /// </summary>
        private double _width = 0;

        /// <summary>
        /// Рсположение центра масс тел, содержащихся в дереве
        /// </summary>
        private Vector _centerOfMass = Vector.Zero;

        /// <summary>
        /// Первое тело добавленное к дереву. Оно используется, когда необходимо его добавить 
        /// в поддерево позже. 
        /// </summary>
        private Body _firstBody = null;

        /// <summary>
        /// Создаёт дерево с данной шириной, расположенное относительно начала.
        /// </summary>
        /// <param name="width">Ширина дерева</param>
        public Octree(double width)
        {
            _width = width;
        }

        /// <summary>
        /// Создаёт дерево с заданными положением и шириной
        /// </summary>
        /// <param name="location">Положение нового дерева</param>
        /// <param name="width">Ширина нового дерева</param>
        public Octree(Vector location, double width)
            : this(width)
        {
            _position = _centerOfMass = location;
        }

        /// <summary>
        /// Добавляет тело к дереву и поддеревьям при необходимости
        /// </summary>
        /// <param name="body">Тело, которое необходимо добавить к дереву</param>
        public void Add(Body body)
        {
            _centerOfMass = (Mass * _centerOfMass + body.Weight * body.Position) / (Mass + body.Weight);
            Mass += body.Weight;
            BodyNumber++;
            if (BodyNumber == 1)
                _firstBody = body;
            else
            {
                AddToSubtree(body);
                if (BodyNumber == 2)
                    AddToSubtree(_firstBody);
            }
        }

        /// <summary>
        /// Добавляет тело к соответствующему поддерева на основании его пространственного расположения. 
        /// Массив поддеревьев и индивидуальные поддеревья инициализируются по мере необходимости 
        /// </summary>
        /// <param name="body">Тело, которое необходимо добавить к поддереву</param>
        private void AddToSubtree(Body body)
        {
            double subTrWidth = _width / 2;

            // Не создаёт поддеревья, если возможен выход за пределы ширины
            if (subTrWidth < MinimumWidth)
                return;

            if (_subTr == null)
                _subTr = new Octree[8];

            // Определяет - к каким поддеревьям тело принадлежит и добавляет его в это поддерево
            int subtreeIndex = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                    {
                        Vector subTrPosition = _position + (subTrWidth / 2) * new Vector(i, j, k);

                        // Определяет - находится ли тело в пределах границ дерева 
                        if (Math.Abs(subTrPosition.X - body.Position.X) <= subTrWidth / 2
                         && Math.Abs(subTrPosition.Y - body.Position.Y) <= subTrWidth / 2
                         && Math.Abs(subTrPosition.Z - body.Position.Z) <= subTrWidth / 2)
                        {

                            if (_subTr[subtreeIndex] == null)
                                _subTr[subtreeIndex] = new Octree(subTrPosition, subTrWidth);
                            _subTr[subtreeIndex].Add(body);
                            return;
                        }
                        subtreeIndex++;
                    }
        }

        /// <summary>
        /// Обновляет ускорение тела
        /// </summary>
        /// <param name="body">Тело, к которому необходимо применить изменение ускорения</param>
        public void Boost(Body body)
        {
            double deltaX = _centerOfMass.X - body.Position.X;
            double deltaY = _centerOfMass.Y - body.Position.Y;
            double deltaZ = _centerOfMass.Z - body.Position.Z;
            double dSquared = deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;


            if ((BodyNumber == 1 && body != _firstBody) || (_width * _width < Admission * Admission * dSquared))
            {

                // Рассчитывает нормализованное значение ускорения и умножает его на 
                // смещение каждой координаты, чтобы получить координатную компоненту ускорения 
                double distance = Math.Sqrt(dSquared + MitFactor * MitFactor);
                double normBoost = World.G * Mass / (distance * distance * distance);

                body.Boosting.X += normBoost * deltaX;
                body.Boosting.Y += normBoost * deltaY;
                body.Boosting.Z += normBoost * deltaZ;
            }
 
            else if (_subTr != null)
                foreach (Octree subtree in _subTr)
                    if (subtree != null)
                        subtree.Boost(body);
        }

        /// <summary>
        /// Рисует дерево и его поддеревья
        /// </summary>
        /// <param name="g">Графическая поверхность, на которой необходима отрисовка</param>
        public void Draw(Graphics g, Renderer renderer)
        {
            renderer.DrawSquare2D(g, Pens.Red, _position, _width);

            if (_subTr != null)
                foreach (Octree subtree in _subTr)
                    if (subtree != null)
                        subtree.Draw(g, renderer);
        }
    }
}
