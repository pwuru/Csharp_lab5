using WinFormsApp5.Objects;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        List<BaseObject> objects = new();
        Player player;
        Marker marker;
        GreenCircle greenCircle;
        Random random = new Random();
        int score = 0;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "Очки: 0";
            player = new Player(pbMain.Width / 2, pbMain.Height / 2, 0);
            player.OnOverlap += (p, obj) =>
            {
                txtLog.Text = $"[{DateTime.Now:HH:mm:ss:ff}] Игрок пересекся с {obj}\n" + txtLog.Text;
                if (obj is GreenCircle)
                {
                    objects.Remove(obj);
                    score += 1;
                    label1.Text = "Очки: " + score;
                    SpawnGreenCircle();
                }
            };
            player.OnMarkerOverlap += (m) =>
            {
                objects.Remove(m);
                marker = null;
            };
            marker = new Marker(pbMain.Width / 2 + 50, pbMain.Height / 2 + 50, 0);
            SpawnGreenCircle();
            objects.Add(marker);
            objects.Add(player);
            //objects.Add(new MyRectangle(50, 50, 0));
            //objects.Add(new MyRectangle(100, 100, 45));
        }

        private void SpawnGreenCircle()
        {
            greenCircle = new GreenCircle(
                random.Next(20, pbMain.Width - 20),
                random.Next(20, pbMain.Height - 20),
                0);

            greenCircle.OnLifeEnd += GreenCircle_OnLifeEnd;
            objects.Add(greenCircle);
        }

        private void GreenCircle_OnLifeEnd(GreenCircle gc)
        {
            // Перемещаем кружок и сбрасываем таймер
            gc.X = random.Next(20, pbMain.Width - 20);
            gc.Y = random.Next(20, pbMain.Height - 20);
            gc.ResetLife();
            pbMain.Invalidate(); // сразу перерисовать
        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            // сюда, теперь сначала вызываем пересчет игрока
            updatePlayer();

            // пересчитываем пересечения
            foreach (var obj in objects.ToList())
            {
                if (obj != player && player.Overlaps(obj, g))
                {
                    player.Overlap(obj);
                }
            }

            // рендерим объекты
            foreach (var obj in objects)
            {
                g.Transform = obj.GetTransform();
                obj.Render(g);
            }
        }

        private void updatePlayer()
        {
            if (marker != null)
            {
                float dx = marker.X - player.X;
                float dy = marker.Y - player.Y;
                float length = MathF.Sqrt(dx * dx + dy * dy);
                dx /= length;
                dy /= length;

                player.vX += dx * 0.5f;
                player.vY += dy * 0.5f;

                player.Angle = 90 - MathF.Atan2(player.vX, player.vY) * 180 / MathF.PI;
            }
            player.vX *= 0.9f;
            player.vY *= 0.9f;

            player.X += player.vX;
            player.Y += player.vY;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var obj in objects.OfType<GreenCircle>())
                obj.UpdateLife();

            pbMain.Invalidate();
        }


        private void pbMain_MouseClick(object sender, MouseEventArgs e)
        {
            // тут добавил создание маркера по клику если он еще не создан
            if (marker == null)
            {
                marker = new Marker(0, 0, 0);
                objects.Add(marker); // и главное не забыть пололжить в objects
            }

            // а это так и остается
            marker.X = e.X;
            marker.Y = e.Y;
        }
    }
}
