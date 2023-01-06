using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Platform.Windows;
using OpenTK.Graphics;
using System.Threading;
using DrawNassiOpenGL.Models;
using DrawNassiOpenGL.ViewModels;
using DrawNassiOpenGL.DNDEngine.TextRenderer;
using DrawNassiOpenGL.DNDEngine.Objects;

namespace DrawNassiOpenGL.Views
{
    public partial class DrawNassiForm : Form
    {

        private BlockViewModel blockView;
        public DrawNassiForm()
        {
            InitializeComponent();
        }


        private void glControl1_Load(object sender, EventArgs e)
        {
            
            glControl1.MakeCurrent();
            glControl1.VSync = true;
            blockView = new BlockViewModel();
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(169 / 255.0f, 169 / 255.0f, 169 / 255.0f, 1.0f);
            MinimizationUpdate();
            glControl1.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            
            MinimizationUpdate();
            
            if (blockView != null)
            {
                blockView.ReInit();
            }
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            glControl1.Invalidate();
            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            blockView.AddBlock(BlockFactory.MakeBlock(this.textBox1.Text, 0, 0, 200, 100, "Sample", Color4.White, Color4.Black, Color4.Black, 5f));
        }

        
        private void DrawNassiForm_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            if (blockView != null) blockView.Draw();

            glControl1.SwapBuffers();
        }

        public void MinimizationUpdate()
        {
            SimpleObject.MINIMIZATIONX = glControl1.ClientRectangle.Width;
            SimpleObject.MINIMIZATIONY = glControl1.ClientRectangle.Height;
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (blockView.Move(e.X, e.Y, glControl1.ClientRectangle.Width, glControl1.ClientRectangle.Height))
            {
                Draw();
            }
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (blockView.Down(e.X, e.Y, glControl1.ClientRectangle.Width, glControl1.ClientRectangle.Height)) 
                {
                    Cursor = Cursors.Hand;
                    Draw();
                }
            }
        }

        private void glControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (blockView.Up(glControl1.ClientRectangle.Width, glControl1.ClientRectangle.Height))
            {
                Draw();
                Cursor = Cursors.Default;
            }
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (glControl1.VSync)
            {
                glControl1.VSync = false;
            }
            else
            {
                glControl1.VSync = true;
            }
        }

    }
}
