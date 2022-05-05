using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Task_7
{
    public enum Type { shape1,
    shape2}
    
    public class Shape
    {   
        PointF TL;//TopLeft
        PointF DR;//DownRight
        private  int timesSaved ;
        private Type Type;
        private String Color;
        private Color PColor;
        private float PWidth;
        public float  Width { get { return DR.X - TL.X; } }
        public float Height { get { return DR.Y - TL.Y; } }
        public PointF TL_ { set { TL = value; }  get { return TL; } }
        public PointF DR_ { set { DR = value; } get { return DR; } }

        public Color PColor_ { get => PColor; set => PColor = value; }
        public float PWidth_ { get => PWidth; set => PWidth = value; }
        public Type Type1 { get => Type; set => Type = value; }
        public string Color1 { get => Color; set => Color = value; }
        public  int TimesSaved { get => timesSaved; set => timesSaved = value; }

        public Shape()
        { }
        public Shape(PointF SP,PointF EP,Type T,Color C,float Width)
        {
            Type = T;
            if (SP.X > EP.X)
            {
                DR.X = SP.X;
                TL.X = EP.X;
            }
            else 
            {
                DR.X = EP.X;
                TL.X = SP.X;
            }
            if (SP.Y > EP.Y)
            {
                TL.Y = EP.Y;
                DR.Y = SP.Y;
            }
            else 
            {
                TL.Y = SP.Y;
                DR.Y = EP.Y;
            }
            this.PColor = C;
            this.PWidth = Width;
            Color = PColor.Name;
            TimesSaved = 0;
           
        }
       public bool isInside(PointF MousePosition)
        {
            double S1 = (DR.Y - TL.Y) / (DR.X - TL.X);
            double C1 = TL.Y - S1 * TL.X;
            double Y = S1 * MousePosition.X + C1;
            double S2 = (TL.Y - DR.Y) / (DR.X - TL.X);
            double C2 = DR.Y - S2 * TL.X;
            double Y2= S2 * MousePosition.X + C2;
            if (Type.Equals(Type.shape1))
            { if (MousePosition.Y < TL.Y||MousePosition.Y > DR.Y)
                    return false;
                if (Y > DR.Y || Y < TL.Y || MousePosition.X < TL.X || MousePosition.X > DR.X)
                {
                    return false;
                }
                if (MousePosition.X < (TL.X + DR.X) / 2)
                {
                    if (S2 * MousePosition.X + C2 > MousePosition.Y && S1 * MousePosition.X + C1 < MousePosition.Y)
                    {
                        return false;
                    }

                }
                else 
                {
                    if (S1 * MousePosition.X + C1 > MousePosition.Y && S2 * MousePosition.X + C2 < MousePosition.Y)
                    { return false;}
                }
                return true;
            }
            if (Type.Equals(Type.shape2))
            {
                float S21 = 2*(TL.Y - DR.Y) / (TL.X - DR.X);
                float C21 = TL.Y - S21 * TL.X;
                float Y1 = S21 * MousePosition.X + C21;
                float S22 = 2 * (TL.Y - DR.Y) / (DR.X - TL.X);
                float C22 = TL.Y - S22 * DR.X;
                float Y22 = S22 * MousePosition.X + C22;
                if (MousePosition.X < TL.X || MousePosition.X > DR.X)
                    return false;
                if (MousePosition.Y > DR.Y || MousePosition.Y < TL.Y)
                    return false;
                if (MousePosition.X > TL.X && MousePosition.X < (TL.X + DR.X) / 2)
                {
                    if (MousePosition.Y < S21 * MousePosition.X + C21)
                        return false;

                }
                if (MousePosition.X >= (TL.X + DR.X) / 2 && MousePosition.X <= DR.X)
                {
                    if (MousePosition.Y < S22 * MousePosition.X + C22)
                        return false;
                }
                
                return true;
            }

            return false;
        }
        public void DRAW(Graphics G, Pen p)
        {
            if (Type.Equals(Type.shape1))
            {
                G.DrawLine(p, TL, DR);
                G.DrawLine(p, TL,new PointF(DR.X,TL.Y));
                G.DrawLine(p,new PointF(TL.X,DR.Y),DR);
                G.DrawLine(p, new PointF(TL.X, DR.Y ), new PointF(DR.X, TL.Y));
            }
            else
            if (Type.Equals(Type.shape2))
            {
                G.DrawLine(p,new PointF(TL.X, DR.Y), DR);//1
                G.DrawLine(p, TL, new PointF(((DR.X + TL.X)/2), DR.Y));//2
                G.DrawLine(p, TL, new PointF(TL.X, DR.Y));//3
                G.DrawLine(p, new PointF(DR.X, TL.Y), DR);//4
                G.DrawLine(p, new PointF((DR.X + TL.X) / 2, DR.Y),new PointF(DR.X,TL.Y));

            }
        }

        public void move(float Dx,float Dy)
        {
            TL.X = TL.X+Dx;
            TL.Y = TL.Y+ Dy;
            DR.X = DR.X+Dx;
            DR.Y = DR.Y+Dy;
        }

        
       

        
    }
}
