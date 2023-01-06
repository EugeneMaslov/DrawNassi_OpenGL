using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using DrawNassiOpenGL.DNDEngine.Shaders;

namespace DrawNassiOpenGL.DNDEngine.Objects
{
    public abstract class SimpleObject
    {
        #region Fields

        /// <summary>
        /// Размер минимизации отрисовки по x-координате
        /// </summary>
        static public int MINIMIZATIONX;
        /// <summary>
        /// Размер минимизации отрисовки по y-координате
        /// </summary>
        static public int MINIMIZATIONY;

        /// <summary>
        /// Ширина объекта (поле)
        /// </summary>
        private float width;

        /// <summary>
        /// Высота объекта (поле) 
        /// </summary>
        private float height;

        /// <summary>
        /// x-координата (поле)
        /// </summary>
        private float _x;

        /// <summary>
        /// y-координата (поле)
        /// </summary>
        private float _y;

        /// <summary>
        /// Вершины (поле)
        /// </summary>
        private float[] _vertices;

        /// <summary>
        /// Поле массива хранения индексов для EBO
        /// </summary>
        private uint[] _indices;

        /// <summary>
        /// Путь к шейдеру вертексов
        /// </summary>
        protected string _shaderVertPath;

        /// <summary>
        /// Путь к шейдеру фрагментов
        /// </summary>
        protected string _shaderFragPath;

        /// <summary>
        /// Шейдер
        /// </summary>
        protected Shader shader;

        /// <summary>
        /// VBO
        /// </summary>
        protected int _vertexBufferObject;

        /// <summary>
        /// VAO
        /// </summary>
        protected int _vaoModel;

        /// <summary>
        /// EBO - Element Buffer Object - улучшение скорости отрисовки
        /// </summary>
        protected int _elementBufferObject;

        /// <summary>
        /// Projection
        /// </summary>
        protected Matrix4 _position;

        #endregion
        #region Properties

        /// <summary>
        /// Ширина объекта (свойство)
        /// </summary>
        public float Width
        {
            get { return width; }
            set
            {
                if (value != width && value >= 0)
                {
                    width = value;
                }
            }
        }

        /// <summary>
        /// Высота объекта (свойство)
        /// </summary>
        public float Height
        {
            get { return height; }
            set
            {
                if (value != height && value >= 0)
                {
                    height = value;
                }
            }
        }

        /// <summary>
        /// Вершины (свойство)
        /// </summary>
        public float[] Vertices
        {
            get { return _vertices; }
            set
            {
                if (value != _vertices && value != null)
                {
                    _vertices = value;
                }
            }
        }

        /// <summary>
        /// Индексы для EBO
        /// </summary>
        public uint[] Indices
        {
            get { return _indices; }
            set
            {
                if (value != _indices && value != null)
                {
                    _indices = value;
                }
            }
        }

        /// <summary>
        /// x-координата (свойство)
        /// </summary>
        public float X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                }
            }
        }

        /// <summary>
        /// y-координата (свойство)
        /// </summary>
        public float Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                }
            }
        }

        #endregion
        #region Methods

        /// <summary>
        /// Инициализация VBO-VAO для объекта
        /// </summary>
        protected virtual void GLInitialization()
        {
            if (shader == null) shader = new Shader(_shaderVertPath, _shaderFragPath);

            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);

            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            var positionLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var aColor = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(aColor);
            GL.VertexAttribPointer(aColor, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableVertexAttribArray(positionLocation);
            GL.DisableVertexAttribArray(aColor);

            shader.Use();
        }

        /// <summary>
        /// Бинд VAO-объекта
        /// </summary>
        protected virtual void BindVAO()
        {
            GL.BindVertexArray(_vaoModel);

            
            var model = Matrix4.CreateTranslation(new Vector3(X / MINIMIZATIONX, Y / MINIMIZATIONY, 0));

            _position = Matrix4.Identity;

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("position", _position);
        }

        /// <summary>
        /// Дебинд VAO-объекта
        /// </summary>
        protected virtual void UnbindVAO()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Инициализация блоков
        /// </summary>
        public virtual void Init()
        {
            GenVertices();
            GLInitialization();
        }

        /// <summary>
        /// Реинециализация блоков
        /// </summary>
        public virtual void ReInit()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vaoModel);
            Init();
        }

        /// <summary>
        /// Перемещение объекта
        /// </summary>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        public virtual void Move(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Нормализует стандартную x-координату к координате OpenGL
        /// </summary>
        /// <param name="x">стандратная x-координата</param>
        /// <param name="clRectW">Ширина текущей отрисовки</param>
        /// <returns>x-координата OpenGL</returns>
        public virtual float NormalizeX(float x, float clRectW)
        {
            float width = clRectW / 2.0f;
            if (x < width)
            {
                x = -(width - x);
            }
            else
            {
                x -= width;
            }
            return x * 2;
        }

        /// <summary>
        /// Нормализует стандартную y-координату к координате OpenGL
        /// </summary>
        /// <param name="y">стандартная y-координата</param>
        /// <param name="clRectH">Высота текущей отрисовки</param>
        /// <returns>y-координата OpenGL</returns>
        public virtual float NormalizeY(float y, float clRectH)
        {
            float height = clRectH / 2.0f;
            if (y <= height)
            {
                y = height - y;
            }
            else
            {
                y = -(y - height);
            }
            return y * 2;
        }

        /// <summary>
        /// Отрисовка объекта
        /// </summary>
        public virtual void Draw()
        {
            BindVAO();

            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            UnbindVAO();
        }

        /// <summary>
        /// Вычисляет коллизию курсора
        /// </summary>
        /// <param name="x">Текущая x-координата мыши</param>
        /// <param name="y">Текущая x-координата мыши</param>
        /// <param name="clRectW">Ширина текущей отрисовки</param>
        /// <param name="clRectH">Высота текущей отрисовки</param>
        /// <returns>совпадение коллизии блока и мыши</returns>
        public virtual bool IsCoolisionCursor(float x, float y, float clRectW, float clRectH)
        {
            x = NormalizeX(x, clRectW);
            y = NormalizeY(y, clRectH);
            if (x >= X && y >= Y && X + Width >= x && Y + Height >= y)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Проверяет колизию для слепления объектов снизу
        /// </summary>
        /// <param name="simp"></param>
        /// <returns></returns>
        public virtual bool IsColisionForSticking(SimpleObject simp)
        {
            if (Y + Height <= simp.Y && Y >= simp.Y - Height * 1.2 
                && X * 0.85 >= simp.X && X + Width <= simp.X + simp.Width * 1.15
                && this != simp)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Слепление объекта к другому
        /// </summary>
        /// <param name="simp"></param>
        /// <returns></returns>
        public virtual bool Stick(SimpleObject simp)
        {
            if (simp != null)
            {
                if (IsColisionForSticking(simp))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Устанавливает стандартные значения объекта
        /// </summary>
        protected abstract void DoStandartValues();

        /// <summary>
        /// Возвращает имя объекта
        /// </summary>
        /// <returns>Возвращает имя объекта</returns>
        public abstract string GetName();

        /// <summary>
        /// Задает вертексы блока
        /// </summary>
        protected abstract void GenVertices();

        #endregion
    }
}
