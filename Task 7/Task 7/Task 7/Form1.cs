using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace Task_7
{
    public partial class Form1 : Form
    {
        Graphics G;
        static Color C=Color.Green;
        static float Width = 1;
        Pen Pencile = new Pen(C,Width);
        PointF SP;//StartPoint
        PointF EP;//EndPoint
        Type T;
        int IDX;
        Shape MovedShape;
        bool flag = false;
        List<Shape> AllShapes = new List<Shape>();
       
        public Form1()
        {
            InitializeComponent();
            G = this.CreateGraphics();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            SP = e.Location;
            for (int i=0;i<AllShapes.Count;i++)
            {
                if (AllShapes[i].isInside(e.Location))
                {
                    IDX = i;
                    flag = true;
                    Pen Temp = new Pen(Color.Red,Width);
                    Redraw();
                    MovedShape = AllShapes[i];
                    AllShapes[i].DRAW(G, Temp);
                    Temp = new Pen(Color.Blue);
                    RectangleF[] tmp = new RectangleF[1];
                    tmp[0] = new RectangleF(AllShapes[i].TL_, new SizeF(AllShapes[i].Width, AllShapes[i].Height));
                    G.DrawRectangles(Temp, tmp);
                }
            }
            if (!flag)
            {
                if (e.Button == MouseButtons.Left)
                    T = Type.shape1;
                else
                    if (e.Button == MouseButtons.Right)
                    T = Type.shape2;
              
            }

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            EP = e.Location;
            if (flag)
            {if (e.Button == MouseButtons.Right)
                {
                    AllShapes.RemoveAt(IDX);
                    
                }
            
                float Dx = EP.X - SP.X;
                float Dy = EP.Y - SP.Y;
                G.Clear(Color.White);
                MovedShape.move(Dx, Dy);
                Redraw();

                flag = false;
            }
            else
            {
              
                Shape Drawn = new Shape(SP, EP, T,C,Width);
                AllShapes.Add(Drawn);
                Drawn.DRAW(G, Pencile);
            }
            


        }
        void Redraw()
        {
            foreach (Shape i in AllShapes)
            {
                i.DRAW(G,new Pen(i.PColor_,i.PWidth_));
            }
        }
       
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Redraw();
        }

        private void saveASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XMLFILE|*.xml";
            if ( AllShapes.Count != 0&&dlg.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                XmlSerializer se = new XmlSerializer(typeof(List<Shape>));

                se.Serialize(writer, AllShapes);
               
                AllShapes[0].TimesSaved ++;

                writer.Close();
            }
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        { ToolStripMenuItem X = (ToolStripMenuItem)sender;
           
            if (X.Text.Equals("Green"))
            {
                C = Color.Green;
                Pencile = new Pen(Color.Green,Width);
            }
            else if (X.Text.Equals("Blue"))
            {
                C = Color.Blue;
                Pencile = new Pen(Color.Blue,Width);
            }
           else  if (X.Text.Equals("Yellow"))
            {
                Pencile = new Pen(Color.Yellow,Width);
                C = Color.Yellow;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem X = (ToolStripMenuItem)sender;

            Width = int.Parse(X.Text);
            Pencile = new Pen(C, Width);

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "xmlfile|*.xml";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    StreamReader reader = new StreamReader(dlg.FileName);
                    XmlSerializer des = new XmlSerializer(typeof(List<Shape>));
                    AllShapes = (List<Shape>)des.Deserialize(reader);
                    G.Clear(Color.White);
                    foreach (Shape i in AllShapes)
                    {
                        if (i.Color1 == "Green")
                        {
                            i.PColor_ = Color.Green;
                        }
                        if (i.Color1 == "Blue")
                        {
                            i.PColor_ = Color.Blue;
                        }
                        if (i.Color1 == "Yellow")
                        {
                            i.PColor_ = Color.Yellow;
                        }
                        
                    }
                    this.Text = dlg.FileName;
                    reader.Close();
                    Redraw();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {if (AllShapes.Count!= 0&&AllShapes[0].TimesSaved==0)
            {
                saveASToolStripMenuItem_Click(sender, e);
                return;
            }
            if (AllShapes.Count != 0)
            {
                StreamWriter writer = new StreamWriter(this.Text);

                XmlSerializer se = new XmlSerializer(typeof(List<Shape>));

                se.Serialize(writer, AllShapes);

                AllShapes[0].TimesSaved++;
                writer.Close();
            }

            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult D = MessageBox.Show("You Are About To Open A new File Do you want to save the changes to this file?", "", MessageBoxButtons.YesNoCancel);
            if(D.Equals(DialogResult.Cancel))
            {
                return;
            }
            if(D.Equals(DialogResult.Yes))
            {
                saveToolStripMenuItem_Click(sender,e);
            
            }
            if (D.Equals(DialogResult.No))
            {
                if (this.Text.Equals("Form1"))
                { }
                else {
                    loadthisfile(this.Text);
                    saveToolStripMenuItem_Click(sender, e);
                }
            }
            G.Clear(Color.White);
            AllShapes = new List<Shape>();
            this.Text = "Form1";




        }

        private void loadthisfile(string text)
        {
            StreamReader reader = new StreamReader(text);
            XmlSerializer des = new XmlSerializer(typeof(List<Shape>));
            AllShapes = (List<Shape>)des.Deserialize(reader);
            G.Clear(Color.White);
            foreach (Shape i in AllShapes)
            {
                if (i.Color1 == "Green")
                {
                    i.PColor_ = Color.Green;
                }
                if (i.Color1 == "Blue")
                {
                    i.PColor_ = Color.Blue;
                }
                if (i.Color1 == "Yellow")
                {
                    i.PColor_ = Color.Yellow;
                }

            }
            reader.Close();
        }
    }
}
