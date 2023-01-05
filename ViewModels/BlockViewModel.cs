using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawNassiOpenGL.Models;

namespace DrawNassiOpenGL.ViewModels
{
    /// <summary>
    /// Главный класс взаимодействия логики GUI интерфейса и классов Blocks
    /// </summary>
    public class BlockViewModel
    {
        #region Fields

        /// <summary>
        /// Список блоков
        /// </summary>
        private List<Block> blocks;

        /// <summary>
        /// Активный блок
        /// </summary>
        private Block activeBlock;

        /// <summary>
        /// Смещение X-координаты
        /// </summary>
        private float deltaX;

        /// <summary>
        /// Смещение y-координаты
        /// </summary>
        private float deltaY;

        #endregion
        #region Properties

        /// <summary>
        /// Свойство списка блоков
        /// </summary>
        public List<Block> Blocks
        {
            get 
            { 
                return blocks; 
            }
            set 
            {
                if (value != null && blocks != value)
                {
                    blocks = value;
                }
            }
        }

        #endregion
        #region Methods

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public BlockViewModel()
        {
            blocks = new List<Block>();
            activeBlock = null;
            deltaX = 0;
            deltaY = 0;
        }

        /// <summary>
        /// Добавление блока
        /// </summary>
        /// <param name="block">Добавляемый блок</param>
        public void AddBlock(Block block)
        {
            blocks.Add(block);
        }


        /// <summary>
        /// Отрисовка блоков
        /// </summary>
        public void Draw()
        {
            foreach (Block item in blocks)
            {
                item.Draw();
            }
        }

        /// <summary>
        /// Переинициализация всех блоков 
        /// </summary>
        public void ReInit()
        {
            if (blocks != null)
            {
                for (int i = 0; i < this.blocks.Count; i++)
                {
                    blocks[i].ReInit();
                }
            }
        }

        /// <summary>
        /// Движение активного блока
        /// </summary>
        /// <param name="x">x-координата мыши</param>
        /// <param name="y">y-координата мыши</param>
        /// <param name="clRectW">Ширина области отрисовки</param>
        /// <param name="clRectH">Высота области отрисовки</param>
        /// <returns>Успешность движения блока</returns>
        public bool Move(float x, float y, float clRectW, float clRectH)
        {
            if (activeBlock != null)
            {
                activeBlock.X = activeBlock.NormalizeX(x, clRectW) - deltaX;
                activeBlock.Y = activeBlock.NormalizeY(y, clRectH) - deltaY;
                activeBlock.TextRenderer.Move(activeBlock.X, activeBlock.Y);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Скрепление блоков между собой
        /// </summary>
        /// <param name="block">Скрепляемый блок</param>
        public void Stick(Block block)
        {
            if (block != null)
            {
                foreach (Block rblock in blocks)
                {
                    block.Stick(rblock);
                }
            }
        }

        /// <summary>
        /// Выбор активного блока
        /// </summary>
        /// <param name="x">x-координата мыши</param>
        /// <param name="y">y-координата мыши</param>
        /// <param name="clRectW">Ширина области отрисовки</param>
        /// <param name="clRectH">Высота области отрисовки</param>
        /// <returns>Успешность выбора активного блока</returns>
        public bool Down(float x, float y, float clRectW, float clRectH)
        {
            foreach (Block item in blocks)
            {
                if (item.IsCoolisionCursor(x, y, clRectW, clRectH))
                {
                    deltaX = item.NormalizeX(x, clRectW) - item.X;
                    deltaY = item.NormalizeY(y, clRectH) - item.Y;
                    activeBlock = item;
                    Swap(blocks.IndexOf(activeBlock), blocks.Count - 1);
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Обмен местами по индексу
        /// </summary>
        /// <param name="firstIndex">Первый индекс</param>
        /// <param name="secondIndex">Второй индекс</param>
        private void Swap(int firstIndex, int secondIndex)
        {
            if (CheckIndex(firstIndex) && CheckIndex(secondIndex))
            {
                Block tempBlock = blocks[firstIndex];
                blocks[firstIndex] = blocks[secondIndex];
                blocks[secondIndex] = tempBlock;
            }
        }

        /// <summary>
        /// Проверка индекса
        /// </summary>
        /// <param name="index">Проверяемый индекс</param>
        /// <returns>Успешность проверки индекса</returns>
        private bool CheckIndex(int index)
        {
            if (index >= 0 && index < blocks.Count)
                return true;
            return false;
        }

        /// <summary>
        /// Деактивация блока
        /// </summary>
        /// <returns>Успешность деактивации блока</returns>
        public bool Up(float clRectW, float clRectH)
        {
            if (activeBlock != null)
            {

                Stick(activeBlock);
                activeBlock = null;
                return true;
            }
            return false;
        }

        #endregion
    }
}
