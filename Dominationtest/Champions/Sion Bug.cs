using EnsoulSharp;
using EnsoulSharp.SDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominationAIO.Champions
{
    internal class Sion
    {
        public static void ongameload()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.HasBuff("SionR")) return;

            var obj = new GameObject();

            var pos = ObjectManager.Player.Position.Extend(Game.CursorPos, 1000);

            Geometry.Line line = new Geometry.Line(ObjectManager.Player.Position, pos);

            line.Draw(Color.White, 200);

            foreach (var l in line.Points)
            {
                if (obj.Position.Distance(l) < 100)
                {
                    Orbwalker.Attack((AIBaseClient)obj);
                }
            }
        }
    }
}
