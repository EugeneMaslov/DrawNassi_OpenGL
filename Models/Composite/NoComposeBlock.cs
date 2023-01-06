using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawNassiOpenGL.DNDEngine.Objects;
using DrawNassiOpenGL.Models;

namespace DrawNassiOpenGL.Models.Composite
{
    /// <summary>
    /// Блок, не имеющий вложенных блоков, но скрепляемый
    /// </summary>
    public abstract class NoComposeBlock : Block
    {
        private Block outBlock;

        public Block OutBlock
        {
            get { return outBlock; }
            set
            {
                outBlock = value;
                if (value != null)
                {
                    outBlock.Move(X, Y - Height);
                }
            }
        }

        public override void Add(Block block)
        {
            throw new NotImplementedException();
        }


        public override void Remove(Block block)
        {
            throw new NotImplementedException();
        }

        protected override abstract void GenVertices();

        public override bool Stick(SimpleObject simp)
        {
            if (base.Stick(simp))
            {
                (simp as NoComposeBlock).OutBlock = this;
                return true;
            }
            return false;
        }

        public override void Move(float x, float y)
        {
            base.Move(x, y);
            if (outBlock != null)
            {
                outBlock.Move(x, y - Height);
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (outBlock != null)
            {
                outBlock.Draw();
            }
        }

        public bool IsHaveThisBlock(Block block)
        {
            if (outBlock == block)
            {
                return true;
            }
            return false;
        }

        public void RemoveThisBlock(Block block)
        {
            if (IsHaveThisBlock(block))
            {
                OutBlock = null;
            }
        }
    }
}
