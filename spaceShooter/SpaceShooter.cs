using GameFramework.Component;
using GameFramework.Core;
using GameFrameWork;
using GameFramework.Environment;
using GameFramework.Environment.Behaviours;
using NAudio.Wave;
using System.Windows.Forms;
namespace spaceShooter
{
    public partial class SpaceShooter : Form
    {
       List<GameObject> gameObjects = new List<GameObject>();
        Audio audio;
        //GameObject player;
        //GameObject bird;
        //Animator birdAnimator;
        //SpriteAnimationTrack birdFlyTrack;

        System.Windows.Forms.Timer gameTimer;
        float deltaTime = 0.016f;
        public SpaceShooter()
        {
            InitializeComponent();



            this.DoubleBuffered = true;

            // 🐦 Create bird object
            //bird = new GameObject();
            //bird.Position = new PointF(200, 150);
            //bird.Size = new SizeF(64, 64);
            //bird.Velocity = new PointF(2, 0);
            //bird.Sprite = Properties.Resources.initalBird;
            
            //// 🎬 Animator
            //birdAnimator = new Animator();
            //birdAnimator.Owner = bird;

            //// 🎞 Animation Track

            //var Frames = new List<Image>()
            //{
            //    Properties.Resources.initalBird,
            //    Properties.Resources.flyingBird
            //};
            //birdFlyTrack = new SpriteAnimationTrack(Frames, 0.2f, true);


            //// ▶ Play animation
            //birdAnimator.PlayAnimation(birdFlyTrack);

            // ⏱ Game Timer
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 16;
            gameTimer.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var tree = new EnvironmentObject(
           "Tree",
           new SolidBehavious(),
           new PointF(150, 200),
           new SizeF(60, 80),
           Properties.Resources.flyingBird
       );
            var orange = new EnvironmentObject(
                "orange",
                            new SolidBehavious(),
                            new PointF(300, 200),
                            new SizeF(60, 80),
                            Properties.Resources.initalBird


                );
            gameObjects.Add(orange);
            gameObjects.Add(tree);


            //audio.AddSound(new AudioTrack
            //{
            //    Name = "hit",
            //    FilePath = "Assets/hit.wav",
            //    Volume = 1f,
            //    Loop = false
            //});

            //audio.AddSound(new AudioTrack
            //{
            //    Name = "bg",
            //    FilePath = "Assets/bg.mp3",
            //    Volume = 0.3f,
            //    Loop = true
            //});

            // background test
        }

        private void TimerEvent_Tick(object sender, EventArgs e)
        {
            //bird.Update(null);
            //birdAnimator.Update(deltaTime);
            Invalidate();
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);



        //    if (tree.Sprite != null)
        //    {
        //        e.Graphics.DrawImage(
        //            bird.Sprite,
        //            bird.Position.X,
        //            bird.Position.Y,
        //            bird.Size.Width,
        //            bird.Size.Height
        //        );
        //    }
        //}
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(e.Graphics);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            audio = new Audio();
            //var newAudio=new AudioTrack("bg", "Assets/backgroundMusic.mp3", false);
            audio.AddSound(
       new AudioTrack(
           "bg",
           "Assets/backgrounMusic.mp3",
           false
       )
   );

            audio.PlaySound("bg");

        }
    }
}
