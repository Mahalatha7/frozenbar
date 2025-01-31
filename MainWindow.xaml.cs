﻿using hackaton.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace hackaton
{
   
    public partial class MainWindow : Window
    {
        // truc cool pour paufiner sur la fin
        bool versus = false;
        DarkShip darkShip;


        bool running = true;
        double count = 0; // comteur de temps total passé
        double interval = 800;
        Random rdm = new Random();

        Ship ship;
        Map map = new Map(20);
        DispatcherTimer dispatcherTimerScroll = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
			//playSongBg();

            // scroll auto
            dispatcherTimerScroll.Tick += ScrollAuto;
            dispatcherTimerScroll.Interval = TimeSpan.FromMilliseconds(interval);
            dispatcherTimerScroll.Start();

            ship = new Ship(5, 10);
            map.SetInMap(ship, 5, 10);
            showMap(map.MapGame);
            DataContext = this;

            if (versus)
            {
                darkShip = new DarkShip(10, 0);
                map.SetInMap(darkShip, 10, 0);
            }
        }

        private void ScrollAuto(object sender, EventArgs e)
        {

            Boss boss = map.isBossOnMap();



            map.Scroll();


            // Test si le joueur est toujours sur la map, si non running = false et si running = false il faut stop le jeux et affiché un écran de game over

            if (map.isOnMap() == false)
            {
                running = false;
                dispatcherTimerScroll.Stop();
                ShowGameOver();
                return;
            }
            else if(IControlable.Hit == 10 && boss is null)
            {
                TimeToBoss();
            }
            count = count + interval;
            UpdateInterval();
            if (boss is null) map.AddMonster();
            else
            {
                if (map.isBossOnMap() is null) {
                    ShowWin();
                    return;
                }

                if (rdm.NextDouble() <= 0.5) Move.MoveEntity(boss, MoveDisponible.left, map);
                else Move.MoveEntity(boss, MoveDisponible.right, map);

                if (rdm.NextDouble() < 0.05) boss.WallInvo(map);
                else boss.Invo(map);
            }
            showMap(map.MapGame);
        }

        private void UpdateInterval()
        {   
            interval = interval * 0.999;
            dispatcherTimerScroll.Interval = TimeSpan.FromMilliseconds(interval);
            if(interval != 800)
            {
                Console.WriteLine();
            }
        }

        private void ShowGameOver()
        {
            myCanvas.Children.Clear();
            GameOver Gameover = new GameOver(count);
            Gameover.Show();
            this.Close();
        }
        private void ShowWin()
        {
            myCanvas.Children.Clear();
            Win win = new Win();
            win.Show();
            this.Close();
        }


        //truc qui fais que quand on appuis sur un bouton ça fais un truc
        private void Canvas_KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Q)
            {
                
                Move.MoveEntity(ship, MoveDisponible.left, map);

            }
            if (e.Key == Key.Right || e.Key == Key.D)
            {
               
                Move.MoveEntity(ship, MoveDisponible.right, map);


            }
            if (e.Key == Key.Up || e.Key == Key.Z)
            {
               
                Move.MoveEntity(ship, MoveDisponible.up, map);

            }
            if (e.Key == Key.Down || e.Key == Key.S)
            {
               
                Move.MoveEntity(ship, MoveDisponible.down, map);


            }
            if (e.Key == Key.Space)
            {
               //shoot dans le bébé
                ship.Shoot(map);
            }


            if (versus)
            {
                if (e.Key == Key.NumPad1)
                {
                    Move.MoveEntity(darkShip, MoveDisponible.left, map);
                }
                if (e.Key == Key.NumPad3)
                {

                    Move.MoveEntity(darkShip, MoveDisponible.right, map);


                }
                if (e.Key == Key.NumPad5)
                {

                    Move.MoveEntity(darkShip, MoveDisponible.up, map);

                }
                if (e.Key == Key.NumPad2)
                {

                    Move.MoveEntity(darkShip, MoveDisponible.down, map);


                }
                if (e.Key == Key.Enter)
                {
                    //shoot dans le bébé
                    darkShip.Shoot(map);
                }
            }


            //ça affiche ça
            showMap(map.MapGame);
        }

       //the fonction de la muerte elle affiche en gros
        private void showMap(Entity[][] map) 
        {//très important de bien le mettre le clear pour pas perdre 30 min à chercher(ça éfface derrière nous quand on se déplace)
            myCanvas.Children.Clear();
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[0].Length; j++)
                {
                    if (map[i][j]!=null) 
                    {
                        if (map[i][j] is Boss){
                            Rectangle newEntity = new Rectangle
                            {
                                Tag = "entity",
                                Height = 62,
                                Width = 62,
                                Fill = map[i][j].Sprite,
                            };
                            Canvas.SetTop(newEntity, j * 32);
                            Canvas.SetLeft(newEntity, i * 32);
                            myCanvas.Children.Add(newEntity);
                        }
                        else {


                            Rectangle newEntity = new Rectangle
                            {
                                Tag = "entity",
                                Height = 32,
                                Width = 32,
                                Fill = map[i][j].Sprite,

                            };

                            Canvas.SetTop(newEntity, j * 32);
                            Canvas.SetLeft(newEntity, i * 32);
                            myCanvas.Children.Add(newEntity);
                        }
                        
                    }
                }
            }
        }

        private void TimeToBoss()
        {
            Map bossMap = new Map(map.Lenght()[1], map.GetAllMap(),ship);
            map = bossMap;
            showMap(map.MapGame);
        }
		private void playSongBg() {

            Sound.PlayBgMusic();
        }

    }
}
