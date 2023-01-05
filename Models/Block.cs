using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using DrawNassiOpenGL.DNDEngine.TextRenderer;
using DrawNassiOpenGL.DNDEngine.Objects;
using DrawNassiOpenGL.DNDEngine.Shaders;

namespace DrawNassiOpenGL.Models
{
    [Serializable]
    /// <summary>
    /// Общий класс блоков
    /// </summary>
    public abstract class Block : SimpleObject
    {
        #region Fields

        /// <summary>
        /// Цвет блока (поле)
        /// </summary>
        private Color4 blockColor;

        /// <summary>
        /// Цвет текста (поле)
        /// </summary>
        private Color4 fontColor;

        /// <summary>
        /// Цвет контура (поле)
        /// </summary>
        private Color4 contourColor;

        /// <summary>
        /// Ширина контура блока (поле)
        /// </summary>
        private float contourWidth;

        /// <summary>
        /// Текст (поле)
        /// </summary>
        private TextRenderer text;

        #endregion
        #region Properties

        /// <summary>
        /// Ширина контура (свойство)
        /// </summary>
        public float ContourWidth
        {
            get { return contourWidth; }
            set
            {
                if (value >= 0.0f && value <= float.MaxValue)
                {
                    contourWidth = value;
                }
            }
        }

        /// <summary>
        /// Цвет блока (свойство)
        /// </summary>
        public Color4 BlockColor
        {
            get { return blockColor; }
            set
            {
                if (value != blockColor && value != null)
                {
                    blockColor = value;
                }
            }
        }

        /// <summary>
        /// Цвет текста (свойство)
        /// </summary>
        public Color4 FontColor
        {
            get { return fontColor; }
            set
            {
                if (value != fontColor && value != null)
                {
                    fontColor = value;
                }
            }
        }

        /// <summary>
        /// Цвет контура (свойство)
        /// </summary>
        public Color4 ContourColor
        {
            get { return contourColor; }
            set
            {
                if (value != contourColor && value != null)
                {
                    contourColor = value;
                }
            }
        }

        /// <summary>
        /// Текст (свойство)
        /// </summary>
        public TextRenderer TextRenderer
        {
            get { return text; }
            set
            {
                if (value != text && value != null)
                {
                    if (value != null)
                    {
                        text = value;
                    }
                    else
                    {
                        text = new TextRenderer(this);
                    }
                }
            }
        }

        #endregion
        #region Methods

        /// <summary>
        /// Задает вертексы блока
        /// </summary>
        protected override abstract void GenVertices();

        /// <summary>
        /// Инициализация VBO-VAO для объекта
        /// </summary>
        protected override void GLInitialization()
        {
            if (shader == null) shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

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
        /// Устанавливает стандартные значения блоков
        /// </summary>
        protected override void DoStandartValues()
        {
            Width = 200;
            Height = 100;
            blockColor = Color4.White;
            fontColor = Color4.Black;
            contourColor = Color4.Black;
            contourWidth = 5f;
        }

        public override void ReInit()
        {
            base.ReInit();
            if (text != null)
            {
                text.ReInit();
            }
        }

        /// <summary>
        /// Отрисовка блока
        /// </summary>
        public override void Draw()
        {
            if (text != null)
            {
                text.Draw();
            }
        }

        public virtual void Stick(Block block)
        {
            if (block != null)
            {
                if (IsColisionForSticking(block))
                {
                    Move(block.X, block.Y - Height);
                    text.Move(X, Y);
                }
            }
        }

        public override string GetName()
        {
            return "Block";
        }

        #endregion
    }
}
