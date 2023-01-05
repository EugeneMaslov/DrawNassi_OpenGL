using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;
using DrawNassiOpenGL.DNDEngine.TextRenderer;

namespace DrawNassiOpenGL.Models
{
    public class BlockFactory
    {
        private static Block MakeBlock(string name)
        {
            switch (name)
            {
                case "Operation":
                    return new OperationBlock();
                case "Branch":
                    return new BranchBlock();
                default:
                    return new OperationBlock();
            }
        }

        private static Block MakeBlock(string name, float x, float y)
        {
            Block newBlock = MakeBlock(name);
            newBlock.X = x;
            newBlock.Y = y;
            return newBlock;
        }

        private static Block MakeBlock(string name, float x, float y, float width, float height)
        {
            Block newBlock = MakeBlock(name, x, y);
            newBlock.Width = width;
            newBlock.Height = height;
            return newBlock;
        }

        private static Block MakeBlock(string name, float x, float y, float width, float height, string text)
        {
            Block newBlock = MakeBlock(name, x, y, width, height);
            return newBlock;
        }

        public static Block MakeBlock(string name, float x, float y, float width, float height, string text, Color4 blockColor, Color4 fontColor, Color4 contourColor, float contourWidth)
        {
            Block newBlock = MakeBlock(name, x, y, width, height, text);
            newBlock.BlockColor = blockColor;
            newBlock.FontColor = fontColor;
            newBlock.ContourColor = contourColor;
            newBlock.ContourWidth = contourWidth;
            newBlock.ReInit();
            newBlock.TextRenderer = new TextRenderer(width, height, x, y, contourColor, System.Drawing.FontFamily.GenericSansSerif, System.Drawing.FontStyle.Regular, text, newBlock);
            newBlock.TextRenderer.Init();
            return newBlock;
        }
    }
}
