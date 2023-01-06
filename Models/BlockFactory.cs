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
    /// <summary>
    /// Фабрика блоков Насси-Шнейдермана
    /// </summary>
    public class BlockFactory
    {
        /// <summary>
        /// Создание блока на базе имени
        /// </summary>
        /// <param name="name">Имя блока</param>
        /// <returns>Блок нужного типа по имени</returns>
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

        /// <summary>
        /// Создание блока на базе имени и координат
        /// </summary>
        /// <param name="name">Имя блока</param>
        /// <param name="x">Координата x</param>
        /// <param name="y">Координата y</param>
        /// <returns>Блок нужного типа по имени</returns>
        private static Block MakeBlock(string name, float x, float y)
        {
            Block newBlock = MakeBlock(name);
            newBlock.X = x;
            newBlock.Y = y;
            return newBlock;
        }

        /// <summary>
        /// Создание блока на базе имени, координат и параметров ширины и высоты
        /// </summary>
        /// <param name="name">Имя блока</param>
        /// <param name="x">Координата x</param>
        /// <param name="y">Координата y</param>
        /// <param name="width">Ширина блока</param>
        /// <param name="height">Высота блока</param>
        /// <returns>Блок нужного типа по имени</returns>
        private static Block MakeBlock(string name, float x, float y, float width, float height)
        {
            Block newBlock = MakeBlock(name, x, y);
            newBlock.Width = width;
            newBlock.Height = height;
            return newBlock;
        }

        /// <summary>
        /// Создание блока на базе имени, координат, параметров ширины и высоты и других параметров
        /// </summary>
        /// <param name="name">Имя блока</param>
        /// <param name="x">Координата x</param>
        /// <param name="y">Координата y</param>
        /// <param name="width">Ширина блока</param>
        /// <param name="height">Высота блока</param>
        /// <param name="text">Текст блока</param>
        /// <param name="blockColor">Цвет блока</param>
        /// <param name="fontColor">Цвета текста блока</param>
        /// <param name="contourColor">Цвет контура блока</param>
        /// <param name="contourWidth">Ширина контура</param>
        /// <returns>Блок нужного типа по имени</returns>
        public static Block MakeBlock(string name, float x, float y, float width, float height, string text, Color4 blockColor, Color4 fontColor, Color4 contourColor, float contourWidth)
        {
            Block newBlock = MakeBlock(name, x, y, width, height);
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
