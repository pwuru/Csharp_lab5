using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinFormsApp5.Objects
{
    internal class GreenCircle : BaseObject
    {
        public int LifeTicks = 0;
        public static int MaxLifeTicks = 99;

        // Событие, которое сработает при окончании таймера
        public event Action<GreenCircle> OnLifeEnd;

        public GreenCircle(float x, float y, float angle) : base(x, y, angle)
        {
            LifeTicks = MaxLifeTicks;
        }

        public void UpdateLife()
        {
            LifeTicks--;
            if (LifeTicks <= 0)
            {
                LifeTicks = 0;
                OnLifeEnd?.Invoke(this); // Сообщаем форме, что время вышло
            }
        }

        public void ResetLife()
        {
            LifeTicks = MaxLifeTicks;
        }

        public override void Render(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Lime), -15, -15, 30, 30);
            g.DrawEllipse(new Pen(Color.Lime, 2), -15, -15, 30, 30);

            // Таймер крупно, зелёным, справа и чуть ниже
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Green))
            {
                var text = LifeTicks.ToString();
                var size = g.MeasureString(text, font);
                g.ResetTransform();
                g.DrawString(text, font, brush, X + 20 - size.Width / 2, Y + 20 - size.Height / 2);
            }
        }

        public override GraphicsPath GetGraphicsPath()
        {
            var path = new GraphicsPath();
            path.AddEllipse(-15, -15, 30, 30);
            return path;
        }

        public override void Overlap(BaseObject obj)
        {
            base.Overlap(obj);
        }
    }
}
