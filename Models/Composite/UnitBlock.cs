using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawNassiOpenGL.Models;
using DrawNassiOpenGL.DNDEngine.Objects;

namespace DrawNassiOpenGL.Models.Composite
{
    /// <summary>
    /// Блок, который может иметь всего одно вложение
    /// </summary>
    public abstract class UnitBlock : NoComposeBlock
    {
        private List<Block> childrenList = new List<Block>();

        public override void Add(Block block)
        {
            childrenList.Add(block);
        }

        public override void Remove(Block block)
        {
            childrenList.Remove(block);
        }

        public override void Draw()
        {
            base.Draw();

            foreach (Block block in childrenList)
            {
                block.Draw();
            }
        }

        protected override abstract void GenVertices();
    }
}
