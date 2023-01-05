using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;
using DrawNassiOpenGL.DNDEngine.TextRenderer;

namespace DrawNassiOpenGL.Models
{
    /// <summary>
    /// Операционный блок
    /// </summary>
    public class OperationBlock : Block
    {
        #region Constructors

        /// <summary>
        /// Базовый конструктор
        /// </summary>
        public OperationBlock()
        {
            DoStandartValues();
            Init();
        }

        /// <summary>
        /// Конструктор (полный)
        /// </summary>
        /// <param name="x">x-координата</param>
        /// <param name="y">y-координата</param>
        /// <param name="width">ширина блока</param>
        /// <param name="height">высота блока</param>
        /// <param name="text">текст блока</param>
        /// <param name="blockColor">цвет блока</param>
        /// <param name="fontColor">цвет текста</param>
        /// <param name="contourColor">цвет контура</param>
        public OperationBlock(float x, float y, float width, float height, string text, Color4 blockColor, Color4 fontColor, Color4 contourColor, FontFamily fontFamily, FontStyle fontStyle, float contourWidth)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            BlockColor = blockColor;
            FontColor = fontColor;
            ContourColor = contourColor;
            ContourWidth = contourWidth;
            TextRenderer = new TextRenderer(Width, Height, X, Y, fontColor, fontFamily, fontStyle, text, this);
            Init();
        }

        #endregion
        #region Methods

        protected override void GenVertices()
        {
            Vertices = new float[] {
                // vertices                                                                                 //colors
                 0,                                                  Height/MINIMIZATIONY,                                0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B,
                 Width/MINIMIZATIONX,                                Height/MINIMIZATIONY,                                0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B,
                 Width/MINIMIZATIONX,                                0,                                                   0.0f,  BlockColor.R,   BlockColor.B,   BlockColor.B,
                 0,                                                  0,                                                   0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B,

                 0,                                                  0,                                                   0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX,                                0,                                                   0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX,                                ContourWidth/MINIMIZATIONY/2,                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 0,                                                  ContourWidth/MINIMIZATIONY/2,                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,

                 0,                                                  Height/MINIMIZATIONY,                                0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX,                                Height/MINIMIZATIONY,                                0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX,                                Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2, 0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 0,                                                  Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2, 0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,

                 ContourWidth/MINIMIZATIONX/2,                       Height/MINIMIZATIONY,                                0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 ContourWidth/MINIMIZATIONX/2,                       0,                                                   0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 

                 Width/MINIMIZATIONX - ContourWidth/MINIMIZATIONX/2, Height/MINIMIZATIONY,                                0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX - ContourWidth/MINIMIZATIONX/2, 0,                                                   0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
            };

            Indices = new uint[]
            {
                0,  1,  3,
                1,  2,  3,

                4,  5,  7,
                5,  6,  7,

                8,  9,  11,
                9,  10, 11,
                
                4, 8, 13,
                8, 12, 13,
                
                5, 9, 15,
                9, 14, 15,
            };
        }

        public override void Draw()
        {
            BindVAO();
            
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);

            UnbindVAO();
            base.Draw();
        }

        public override string GetName()
        {
            return "Operation Block";
        }

        #endregion
    }
}
