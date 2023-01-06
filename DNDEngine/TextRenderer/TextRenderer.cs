using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using DrawNassiOpenGL.Models;
using DrawNassiOpenGL.DNDEngine.Shaders;
using DrawNassiOpenGL.DNDEngine.Objects;

namespace DrawNassiOpenGL.DNDEngine.TextRenderer
{
    /// <summary>
    /// Класс рендера текста
    /// </summary>
    public class TextRenderer : SimpleObject
    {
        #region Fields

        /// <summary>
        /// Поле с объектом текстуры
        /// </summary>
        private Texture _textMap;

        /// <summary>
        /// Поле с цветом текста
        /// </summary>
        private Color4 textColor;

        /// <summary>
        /// Поле с семейством шрифтов текста
        /// </summary>
        private FontFamily family;

        /// <summary>
        /// Поле со стилем шрифта текста
        /// </summary>
        private FontStyle style;

        /// <summary>
        /// Поле с текстом
        /// </summary>
        private string text;

        /// <summary>
        /// Ссылка на родительский блок
        /// </summary>
        private Block block;

        #endregion
        #region Properties

        /// <summary>
        /// Свойство для обращения к родительскому блоку
        /// </summary>
        public Block Block
        {
            get
            {
                return block;
            }
            set
            {
                block = value;
            }
        }

        /// <summary>
        /// Свойство для обращения к тексту
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        /// <summary>
        /// Свойство для обращения к цвету текста
        /// </summary>
        public Color4 TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }

        /// <summary>
        /// Свойство для обращения к семейству текста
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return family;
            }
            set
            {
                family = value;
            }
        }

        /// <summary>
        /// Свойство для обращения к стилю текста
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }

        #endregion
        #region Constructors

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public TextRenderer(Block block)
        {
            DoStandartValues();
            this.block = block;
        }

        /// <summary>
        /// Конструктор объекта рендерера текста
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        /// <param name="x">X-координата</param>
        /// <param name="y">Y-координата</param>
        /// <param name="textColor">Цвет текста</param>
        /// <param name="fontFamily">Семейство текста</param>
        /// <param name="fontStyle">Стиль текста</param>
        /// <param name="text">Текст</param>
        /// <param name="block">Блок к которому привязан текст</param>
        public TextRenderer(float width, float height, float x, float y, Color4 textColor, FontFamily fontFamily, FontStyle fontStyle, string text, Block block)
        {
            X = x;
            Y = y;
            style = fontStyle;
            Width = width;
            Height = height;
            family = fontFamily;
            this.block = block;
            this.textColor = textColor;
            this.text = text;
        }

        #endregion
        #region Methods

        /// <summary>
        /// Установка стандартных значений текста
        /// </summary>
        protected override void DoStandartValues()
        {
            Width = 200;
            Height = 100;
            X = 0;
            Y = 0;
            textColor = Color4.Black;
            family = FontFamily.GenericSansSerif;
            style = FontStyle.Regular;
            text = "Simple Text";
        }

        /// <summary>
        /// Бинд VAO-объекта
        /// </summary>
        protected override void BindVAO()
        {
            GL.BindVertexArray(_vaoModel);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _textMap.Use(TextureUnit.Texture0);
            shader.Use();

            var model = Matrix4.CreateTranslation(new Vector3(X / MINIMIZATIONX, Y / MINIMIZATIONY, 0));
            _position = Matrix4.Identity;

            shader.SetMatrix4("model", model);
            shader.SetMatrix4("position", _position);

        }

        /// <summary>
        /// Генерация вершин
        /// </summary>
        protected override void GenVertices()
        {
            Vertices = new float[]
            {
                 // vertices                                            //TexCoords
                 0,                      Height/MINIMIZATIONY,  0.0f,   0.0f, 1.0f,
                 Width/MINIMIZATIONX,    Height/MINIMIZATIONY,  0.0f,   1.0f, 1.0f,
                 Width/MINIMIZATIONX,    0,                     0.0f,   1.0f, 0.0f,
                 0,                      0,                     0.0f,   0.0f, 0.0f,
            };

            Indices = new uint[] {
                0, 1, 3,
                1, 2, 3
            };
        }

        /// <summary>
        /// Инициализация объъекта в OpenGL
        /// </summary>
        protected override void GLInitialization()
        {

            if (shader == null) shader = new Shader("Shaders/shaderT.vert", "Shaders/shaderT.frag");
            shader.Use();

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
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);


            var aTexCoords = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(aTexCoords);
            GL.VertexAttribPointer(aTexCoords, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            if (text.Length * (Height / text.Length * 2) * 2 > Width)
            {
                Width = text.Length * (Height / text.Length * 2) * 2.5f;
                if (Width > block.Width)
                {
                    block.Width = Width;
                    block.ReInit();
                }
            }

            _textMap = Texture.FromGDIText(this);
            _textMap.Use(TextureUnit.Texture0);

            GL.BindVertexArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DisableVertexAttribArray(positionLocation);
            GL.DisableVertexAttribArray(aTexCoords);
        }

        /// <summary>
        /// Пересоздание объекта
        /// </summary>
        public override void ReInit()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vaoModel);
            if (_textMap != null)
            {
                _textMap.Dispose();
            }
            Init();
        }
        
        /// <summary>
        /// Отрисовывает текст
        /// </summary>
        public override void Draw()
        {

            BindVAO();

            GL.DrawElements(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.Disable(EnableCap.Blend);

            UnbindVAO();
        }

        /// <summary>
        /// Возвращает имя объекта
        /// </summary>
        /// <returns>Возвращает имя объекта</returns>
        public override string GetName()
        {
            return "TextRenderer";
        }

        #endregion
    }
}
