using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using DrawNassiOpenGL.DNDEngine.TextRenderer;

namespace DrawNassiOpenGL.Models
{
    public class BranchBlock : Block
    {
        public BranchBlock()
        {
            DoStandartValues();
            Init();
        }

        public BranchBlock(float x, float y, float width, float height, string text, Color4 blockColor, Color4 fontColor, Color4 contourColor, FontFamily fontFamily, FontStyle fontStyle)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            TextRenderer = new TextRenderer(Width, Height, X, Y, fontColor, fontFamily, fontStyle, text, this);
            BlockColor = blockColor;
            FontColor = fontColor;
            ContourColor = contourColor;
            Init();
        }

        protected override void GenVertices()
        {
            Vertices = new float[] {
                            // vertices                                                                                 //colors
                 0,                                                           Height/MINIMIZATIONY,                                                     0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B,
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY,                                                     0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B, 
                 Width/MINIMIZATIONX,                                         0,                                                                        0.0f,  BlockColor.R,   BlockColor.B,   BlockColor.B, //BLOCK
                 0,                                                           0,                                                                        0.0f,  BlockColor.R,   BlockColor.G,   BlockColor.B,

                 0,                                                           0,                                                                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX,                                         0,                                                                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //DOWN -
                 Width/MINIMIZATIONX,                                         ContourWidth/MINIMIZATIONY/2,                                             0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 0,                                                           ContourWidth/MINIMIZATIONY/2,                                             0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,

                 0,                                                           Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2,                      0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //UPPER -
                 0,                                                           Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2,                      0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,

                 ContourWidth/MINIMIZATIONX/2,                                Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //LEFT |
                 ContourWidth/MINIMIZATIONX/2,                                0,                                                                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,

                 Width/MINIMIZATIONX - ContourWidth/MINIMIZATIONX/2,          Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //RIGHT |
                 Width/MINIMIZATIONX - ContourWidth/MINIMIZATIONX/2,          0,                                                                        0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 

                 Width/MINIMIZATIONX - ContourWidth/MINIMIZATIONX/2,          Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX/2 + ContourWidth/MINIMIZATIONX/2,        Height/MINIMIZATIONY/2 + ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //RIGHT / 
                 Width/MINIMIZATIONX/2 - ContourWidth/MINIMIZATIONX/2,        Height/MINIMIZATIONY/2 - ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //RIGHT-LEFT 50/50
                 
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2,                      0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 

                 0,                                                           Height/MINIMIZATIONY - ContourWidth/MINIMIZATIONY/2,                      0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX/2 - ContourWidth/MINIMIZATIONX/2,        Height/MINIMIZATIONY/2 + ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //LEFT \
                 0 + ContourWidth/MINIMIZATIONX/2,                            Height/MINIMIZATIONY,                                                     0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 

                 0,                                                           Height/MINIMIZATIONY/2 + ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY/2 + ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //MIDDLE -
                 Width/MINIMIZATIONX,                                         Height/MINIMIZATIONY/2 - ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 0,                                                           Height/MINIMIZATIONY/2 - ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //26

                 Width/MINIMIZATIONX/2 - ContourWidth/MINIMIZATIONX/2,        Height/MINIMIZATIONY/2 - ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B,
                 Width/MINIMIZATIONX/2 - ContourWidth/MINIMIZATIONX/2,        0 + ContourWidth/MINIMIZATIONY,                                           0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //DOWN MIDDLE |
                 Width/MINIMIZATIONX/2 + ContourWidth/MINIMIZATIONX/2,        0 + ContourWidth/MINIMIZATIONY,                                           0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, 
                 Width/MINIMIZATIONX/2 + ContourWidth/MINIMIZATIONX/2,        Height/MINIMIZATIONY/2 + ContourWidth/MINIMIZATIONY/2,                    0.0f,  ContourColor.R, ContourColor.G, ContourColor.B, //30
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

                16, 17, 19,
                17, 18, 19,

                20, 18, 22,
                18, 21, 22,

                23, 24, 26,
                24, 25, 26,

                27, 28, 30,
                28, 29, 30
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
            return "Branch Block";
        }
    }
}
