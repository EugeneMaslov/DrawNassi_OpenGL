using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using StbImageSharp;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using System;

namespace DrawNassiOpenGL.DNDEngine.TextRenderer
{
    /// <summary>
    /// Отдельный класс текстуры с готовыми методами
    /// </summary>
    public class Texture : IDisposable
    {
        #region Fields
        
        /// <summary>
        /// Заголовок текстуры
        /// </summary>
        public readonly int Handle;

        #endregion
        #region Methods

        #region Constructors

        /// <summary>
        /// Конструктор по уже имеющимся ID
        /// </summary>
        /// <param name="glHandle">ID-Handle текстуры</param>
        public Texture(int glHandle) => Handle = glHandle;

        /// <summary>
        /// Загружает текстуру из файла по пути
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>Текстура загруженного файла</returns>
        public static Texture LoadFromFile(string path)
        {
            //Гененируем ID
            int handle = InitTexture();

            // Используем библиотеку специально для загрузки в OpenGL текстур, ничего страшного, ведь она и так основана на System.Drawing
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Используем открытый файл из памяти
            using (Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                // Создаем текстуру
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, image.Data);
            }

            IstallParameters();

            return new Texture(handle);
        }

        /// <summary>
        /// Генерирует текстуру текста используя GDI+ и TextRenderer
        /// </summary>
        /// <param name="textRenderer">Объект отрисовки</param>
        /// <returns>Сгенерированная текстура с текстом</returns>
        public static Texture FromGDIText(TextRenderer textRenderer)
        {
            //Гененируем ID
            int handle = InitTexture();

            // For this example, we're going to use .NET's built-in System.Drawing library to load textures.
            // Load the image
            using (var image = new Bitmap((int)textRenderer.Width, (int)textRenderer.Height))
            {
                const int maxLen = 20;
                float emSize;

                Graphics g = Graphics.FromImage(image);
                g.SmoothingMode = SmoothingMode.HighQuality;
                Rectangle rectangle = new Rectangle();

                SolidBrush brush = new SolidBrush(
                    Color.FromArgb(
                        (int)(textRenderer.TextColor.R * 255),
                        (int)(textRenderer.TextColor.G * 255),
                        (int)(textRenderer.TextColor.B * 255)
                        )
                    );

                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };


                switch (textRenderer.Block.GetName())
                {
                    case "Branch Block":
                        {
                            if (textRenderer.Text.Length > maxLen)
                            {
                                rectangle = new Rectangle((int)textRenderer.Width / 4, 0, (int)textRenderer.Width / 2, (int)textRenderer.Height / 4);
                                emSize = textRenderer.Height / 2 / textRenderer.Text.Length;
                            }
                            else
                            {
                                rectangle = new Rectangle((int)textRenderer.Width / 4, 0, (int)textRenderer.Width / 2, (int)textRenderer.Height / 2);
                                emSize = textRenderer.Height / 2 / textRenderer.Text.Length;
                            }
                        }
                        break;

                    default:
                        {
                            if (textRenderer.Text.Length > maxLen)
                            {
                                rectangle = new Rectangle(0, 0, (int)textRenderer.Width, (int)(textRenderer.Height / 2f));
                                emSize = textRenderer.Height / textRenderer.Text.Length * 2;
                            }
                            else
                            {
                                rectangle = new Rectangle(0, 0, (int)textRenderer.Width, (int)textRenderer.Height);
                                emSize = textRenderer.Height / textRenderer.Text.Length;
                            }
                        }
                        break;
                }
                
                Font font = new Font(
                    textRenderer.FontFamily,
                    emSize*4,
                    textRenderer.FontStyle);

                if (textRenderer.Text.Length > maxLen)
                {
                    DrawString(g, textRenderer.Text.Substring(0, maxLen), font, stringFormat, brush, rectangle, 0);
                    DrawString(g, textRenderer.Text.Substring(maxLen, textRenderer.Text.Length - maxLen), font, stringFormat, brush, rectangle, rectangle.Height);
                }
                else
                {
                    
                    DrawString(g, textRenderer.Text, font, stringFormat, brush, rectangle, 0);
                }

                // Очищаем графику GDI+ из памяти
                g.Dispose();

                // Переворачиваем изображение так как оно загружается по стандарту наоборот
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);

                // Загружаем image в BitmapData
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Создаём текстуру
                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }

            IstallParameters();

            return new Texture(handle);
        }

        #endregion
        #region Destructors

        /// <summary>
        /// Стандартный деструктор
        /// </summary>
        ~Texture()
        {
            Dispose();
        }

        /// <summary>
        /// Диспозиция объекта
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }

        #endregion
        #region GDIPlus

        /// <summary>
        /// Рисует текст с помощью GDI+ для
        /// </summary>
        /// <param name="g">Графика отрисовки</param>
        /// <param name="text">Необходимый текст</param>
        /// <param name="font">Формат текста</param>
        /// <param name="stringFormat">Форматирование</param>
        /// <param name="brush">Кисть</param>
        /// <param name="rectangle">Поле отрисовки</param>
        /// <param name="rectPad">Отступ отрисовки по y</param>
        private static void DrawString(Graphics g, string text, Font font, StringFormat stringFormat, Brush brush, Rectangle rectangle, int rectPad)
        {
            g.DrawString(
                text,
                font,
                brush,
                new Rectangle(rectangle.X, rectangle.Y + rectPad, rectangle.Width, rectangle.Height),
                stringFormat);
        }

        #endregion
        #region GL

        /// <summary>
        /// Инициализация текстуры с генерацией заголовка, и активирует сразу
        /// </summary>
        /// <returns>Заголовок созданной и активной текстуры</returns>
        private static int InitTexture()
        {
            // Генерация заголовка текстуры, по сути как ID в VBO, VAO
            int handle = GL.GenTexture();

            // Биндим текстуру для дальнейших манипуляций
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            return handle;
        }

        /// <summary>
        /// Устанавливает стандартные параметры для текстуры и генерирует мип-мапы
        /// </summary>
        private static void IstallParameters()
        {
            // Указываем параметры
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

            // Генерируем мип-мапы
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        /// <summary>
        /// Активирует текстуру
        /// </summary>
        /// <param name="unit">Текстурный тип</param>
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        #endregion

        #endregion
    }
}
