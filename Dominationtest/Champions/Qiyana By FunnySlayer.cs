using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.MenuUI.Values;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominationAIO.Champions
{
    internal class qmenu
    {
        public class combo
        {
            public static MenuBool useq = new MenuBool("useq", "Use Combo Q");
            public static MenuBool usew = new MenuBool("usew", "Use Comno W");
            public static MenuBool usee = new MenuBool("usee", "Use Combo E");
            public static MenuBool user = new MenuBool("user", "Use Combo R");
        }
        public class harass
        {
            public static MenuBool useq = new MenuBool("useq", "Use Combo Q");
            public static MenuBool usew = new MenuBool("usew", "Use Comno W");
            public static MenuBool usee = new MenuBool("usee", "Use Combo E");
        }
        public class farm
        {
            public static MenuBool useq = new MenuBool("useq", "Use Combo Q");
            public static MenuBool usew = new MenuBool("usew", "Use Comno W");
            public static MenuBool usee = new MenuBool("usee", "Use Combo E");
        }
    }
    internal class Qiyana
    {
        private static Spell q, qrw, w, e, r;
        private static AIHeroClient Player = ObjectManager.Player;

        private static bool Qrock() => q.Name == "QiyanaQ_Rock";
        private static bool Qwater() => q.Name == ("QiyanaQ_Water");
        private static bool Qgrass() => q.Name == ("QiyanaQ_Grass");

        public static void ongameload()
        {
            q = new Spell(SpellSlot.Q, 470);
            q.SetSkillshot(0.2f, 140, float.MaxValue, false, EnsoulSharp.SDK.Prediction.SkillshotType.Line);

            qrw = new Spell(SpellSlot.Q, 710);
            qrw.SetSkillshot(0.2f, 150, 2000, true, EnsoulSharp.SDK.Prediction.SkillshotType.Line);

            w = new Spell(SpellSlot.W, 1250);
            w.SetSkillshot(0.2f, 360, 1200, false, EnsoulSharp.SDK.Prediction.SkillshotType.Circle);

            e = new Spell(SpellSlot.E, 650);

            r = new Spell(SpellSlot.R, 875);
            r.SetSkillshot(0.25f, 280, 2000, true, EnsoulSharp.SDK.Prediction.SkillshotType.Line);


            var qiyana = new Menu("qiyana", "Qiyana Settings", true);
            var combo = new Menu("combo", "Qicomboyana Settings");
            var harass = new Menu("harass", "harass Settings");
            var clear = new Menu("clear", "clear Settings");

            combo.Add(qmenu.combo.useq);
            combo.Add(qmenu.combo.usew);
            combo.Add(qmenu.combo.usee);
            combo.Add(qmenu.combo.user);

            harass.Add(qmenu.harass.useq);
            harass.Add(qmenu.harass.usew);
            harass.Add(qmenu.harass.usee);

            clear.Add(qmenu.farm.useq);
            clear.Add(qmenu.farm.usew);
            clear.Add(qmenu.farm.usee);

            qiyana.Add(combo);
            qiyana.Add(harass);
            qiyana.Add(clear);

            qiyana.Attach();

            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.OnAction += Orbwalker_OnAction;
            AIHeroClient.OnProcessSpellCast += AIHeroClient_OnProcessSpellCast;
            Dash.OnDash += Dash_OnDash;
        }

        private static void Dash_OnDash(AIBaseClient sender, Dash.DashArgs args)
        {
            if(sender.IsMe)
            {

            }else
            {
                if(sender.IsEnemy)
                {

                }
            }
        }

        private static void AIHeroClient_OnProcessSpellCast(AIBaseClient sender, AIBaseClientProcessSpellCastEventArgs args)
        {
            
        }

        private static void Orbwalker_OnAction(object sender, OrbwalkerActionArgs args)
        {
            
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead) return;
            switch (Orbwalker.ActiveMode)
            {
                case (OrbwalkerMode.Combo):
                    combo();
                    break;
                case OrbwalkerMode.Harass:
                    break;
                case OrbwalkerMode.LaneClear:
                    break;
                case OrbwalkerMode.LastHit:
                    break;
            }
        }

        private static void combo()
        {
            var target = TargetSelector.GetTarget(800 + 600);

            if (target == null) return;

            var rockobj = GameObjects.AllGameObjects.Where(r => r.DistanceToPlayer() < 1100 && r.Position.IsWall() || r.Position.IsBuilding());
            var grassobj = GameObjects.Get<GrassObject>().Where(g => g.DistanceToPlayer() < 1100);

            var water = new Vector2();

            Vector2 waterobj = Vector2.Zero;

            if (water.X >= 2500 && water.X <= 7000 && water.DistanceToPlayer() <= 1100)
            {
                if (water.Y >= 8000 && water.Y <= 12000)
                {
                    waterobj = water;
                }
            }
            if (water.X >= 8000 && water.X <= 12000 && water.DistanceToPlayer() <= 1100)
            {
                if (water.Y >= 3000 && water.Y <= 7000)
                {
                    waterobj = water;
                }
            }

            if (qmenu.combo.useq.Enabled && q.IsReady(0))
            {
                if(Qrock() || Qwater() || Qgrass())
                {
                    var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                    if(target.IsValidTarget(qrw.Range) && qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                    {
                        qrw.Cast(qpred.CastPosition);
                    }
                }else
                {
                    var qpred = q.GetPrediction(target);
                    if (target.IsValidTarget(q.Range) && qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                    {
                        q.Cast(qpred.CastPosition);
                    }
                }
            }
            if (qmenu.combo.usew.Enabled && w.IsReady(0))
            {
                bool canwater = false;
                bool canrock = false;
                //Geometry.Circle firstcircle = new Geometry.Circle(Player.Position, 360, 20);
                //Geometry.Circle secondcircle = new Geometry.Circle(Player.Position, 700, 20);
                //Geometry.Circle thirdcircle = new Geometry.Circle(Player.Position, 700, 20);

                if (waterobj != Vector2.Zero && waterobj.DistanceToPlayer() < 1100 && waterobj.Distance(target) < 700)
                {
                    var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                    if (qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                    {
                            canwater = true;
                    }
                    else { canwater = false; }
                }
                else { canwater = false; }
                if (rockobj != null)
                {
                    foreach (var rock in rockobj.Where(i => i.Distance(target) < 700))
                    {
                        qrw.UpdateSourcePosition(Player.Position.Extend(rock.Position, 300), rock.Position);
                        var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                        if (qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                        {
                                canrock = true;
                        }else
                        { canrock = false; }
                    }
                }else
                { canrock = false; }

                if (waterobj != null && waterobj.DistanceToPlayer() < 1100 && waterobj.Distance(target) < 700 && canwater == true)
                {
                    var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                    if(qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                    {
                        w.Cast(waterobj);
                    }                   
                }
                else
                {
                    if (rockobj != null && canrock == true)
                    {
                        foreach (var rock in rockobj.Where(i => i.Distance(target) < 700))
                        {
                            qrw.UpdateSourcePosition(Player.Position.Extend(rock.Position, 300), rock.Position);
                            var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                            if (qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                            {
                                    w.Cast(rock.Position);
                            }
                        }
                    }
                    else
                    {
                        if (grassobj != null)
                        {
                            foreach (var grass in grassobj.Where(i => i.Distance(target) < 700))
                            {
                                qrw.UpdateSourcePosition(Player.Position.Extend(grass.Position, 300), grass.Position);
                                var qpred = qrw.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall);
                                if (qpred.CastPosition != Vector3.Zero && qpred.Hitchance >= EnsoulSharp.SDK.Prediction.HitChance.High)
                                {
                                        w.Cast(grass.Position);
                                }
                            }
                        }
                    }
                }
                /*foreach (var fc in firstcircle.Points)
                {
                    foreach(var sc in secondcircle.Points)
                    {
                        foreach(var tc in thirdcircle.Points)
                        {
                            
                        }
                    }
                }*/
            }
            if (qmenu.combo.usee.Enabled && e.IsReady(0))
            {
                if(target.IsValidTarget(e.Range))
                {
                    if(!Player.IsDashing())
                    {
                        e.CastOnUnit(target);
                    }
                }
            }
            if (qmenu.combo.user.Enabled && r.IsReady(0))
            {
                var rpred = r.GetPrediction(target, false, -1, EnsoulSharp.SDK.Prediction.CollisionObjects.YasuoWall); 
                if (rpred.CastPosition == Vector3.Zero || rpred.CastPosition.DistanceToPlayer() >= r.Range) return;
                for (var i = 75; i < r.Range; i += 40)
                {              
                    var flags = NavMesh.GetCollisionFlags(rpred.CastPosition.ToVector2()
                        .Extend(Player.Position.ToVector2(), -i + target.Position.DistanceToPlayer()).ToVector3());
                    if (flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building))
                    {                        
                        r.Cast(rpred.CastPosition);
                    }

                    if(waterobj != null && waterobj.DistanceToPlayer() < i)
                    {
                        var rpos = rpred.CastPosition.ToVector2().Extend(Player.Position.ToVector2(), -i + target.Position.DistanceToPlayer());
                        if (rpos == waterobj)
                            r.Cast(rpred.CastPosition);
                    }
                }
            }
        }
    }
}
