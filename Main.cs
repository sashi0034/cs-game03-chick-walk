using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DxLibDLL;
using System.Drawing;
using System.IO;

namespace ChickRun
{ 
    static class ProgramProperty
    {
        public const string version = "1.0.0";
        public const string published = "2021/07/22";
        public const string maker = "sashi";
    }




    static class MainGame
    {

        const int DRAW_WIDTH = 416;
        const int DRAW_HEIGHT = 240;

        static Random rand = new Random();


        static public string[,] map_matrix = new string[0,0];



        [STAThread]
        static void Main()
        {
            float draw_scale = 3;
            int loop_count = 0;



            // スクリーンモードを選択して起動
            DX.ChangeWindowMode(DX.FALSE);

            // フルスクリーンモード時の解像度モードをモニターの解像度を最大にするモードに設定
            DX.SetFullScreenResolutionMode(DX.DX_FSRESOLUTIONMODE_NATIVE);

            // フルスクリーンモード時の画面の拡大モードを最近点モードに設定
            DX.SetFullScreenScalingMode(DX.DX_FSSCALINGMODE_NEAREST);

            DX.SetGraphMode((int)(DRAW_WIDTH * draw_scale), (int)(DRAW_HEIGHT * draw_scale), 16);

            DX.SetAlwaysRunFlag(1); //非アクティブ状態でも動かす

            // ＤＸライブラリの初期化
            if (DX.DxLib_Init() < 0)
            {
                return;
            }
            Sprite.Init(); // スプライトの初期化


            DX.SetMainWindowText("Chick Walk");


            int Hndl_backScenery = DX.LoadGraph(@"Assets\Images\洞窟背景624x240.png");
            int Hndl_ground = DX.LoadGraph(@"Assets\Images\横スク角丸地面洞窟.png");
            int Hndl_groundBack = DX.LoadGraph(@"Assets\Images\横スク角丸足場洞窟.png");
            int Hndl_ground_Buf = DX.LoadGraph(@"Assets\Images\横スク角丸地面暗.png");
            int Hndl_groundBack_Buf = DX.LoadGraph(@"Assets\Images\横スク角丸足場暗.png");

            int Hndl_blocks = DX.LoadGraph(@"Assets\Images\ブロックセット16.png");
            int Hndl_Luna = DX.LoadGraph(@"Assets\Images\ルナ6x2f24x32.png");
            int Hndl_turtle = DX.LoadGraph(@"Assets\Images\亀ちゃん4f32.png");
            int Hndl_chick = DX.LoadGraph(@"Assets\Images\ひよこ4f16.png");
            int Hndl_slime = DX.LoadGraph(@"Assets\Images\スライム2f16.png");
            int Hndl_slimeR = DX.LoadGraph(@"Assets\Images\赤スライム2f16.png");

            int Hndl_lift = DX.LoadGraph(@"Assets\Images\足場48x8.png");
            int Hndl_rock = DX.LoadGraph(@"Assets\Images\岩4f48.png");
            int Hndl_rock16 = DX.LoadGraph(@"Assets\Images\岩4f16.png");
            int Hndl_bakugon = DX.LoadGraph(@"Assets\Images\赤バクゴン4f32.png");
            int Hndl_cloud = DX.LoadGraph(@"Assets\Images\雲96x16.png");
            int Hndl_smoke = DX.LoadGraph(@"Assets\Images\煙4f32.png");
            
            int Hndl_starY = DX.LoadGraph(@"Assets\Images\星4f24.png");
            int Hndl_starB = DX.LoadGraph(@"Assets\Images\青星4f24.png");
            int Hndl_ink = DX.LoadGraph(@"Assets\Images\紫インク4f48.png");

            int Hndl_bush1 = DX.LoadGraph(@"Assets\Images\ハリボテ茂み48x16.png");
            int Hndl_bush2 = DX.LoadGraph(@"Assets\Images\ハリボテ茂み128x64.png");

            int Hndl_mash = DX.LoadGraph(@"Assets\Images\背景きのこ2f16.png");

            int Hndl_menuBack = DX.LoadGraph(@"Assets\Images\ひよこスクロール背景512x320.png");
            int Hndl_stageNamePlate = DX.LoadGraph(@"Assets\Images\ステージ名札48x24.png");

            int Hndl_peke = DX.LoadGraph(@"Assets\Images\ペケ.png");

            int Hndl_titleBack = DX.LoadGraph(@"Assets\Images\ひよこタイトル.png");
            int Hndl_titleLogo = DX.LoadGraph(@"Assets\Images\ひよこタイトルロゴ.png");

            int Hndl_BackScreen = DX.MakeScreen(DRAW_WIDTH, DRAW_HEIGHT, 0);

            int Hndl_DrawScreen = DX.MakeScreen(DRAW_WIDTH, DRAW_HEIGHT, 0);

            int Hndl_font12 = DX.CreateFontToHandle(null, 12, 1, DX.DX_FONTTYPE_NORMAL);


            int se_scene = DX.LoadSoundMem(@"Assets\Sounds\シーン切り替え.mp3");
            int se_ok = DX.LoadSoundMem(@"Assets\Sounds\決定、ボタン押下22.mp3");
            int se_chick = DX.LoadSoundMem(@"Assets\Sounds\ヒヨコの鳴き声.mp3");
            int se_star = DX.LoadSoundMem(@"Assets\Sounds\きらーん.mp3");
            int se_jamp = DX.LoadSoundMem(@"Assets\Sounds\パッ.mp3");
            int se_jampChick = DX.LoadSoundMem(@"Assets\Sounds\決定、ボタン押下50.mp3");
            int se_bound = DX.LoadSoundMem(@"Assets\Sounds\ぷよん.mp3");
            int se_nyu = DX.LoadSoundMem(@"Assets\Sounds\ニュッ3.mp3");
            int se_atacked = DX.LoadSoundMem(@"Assets\Sounds\大パンチ.mp3");
            int se_kong = DX.LoadSoundMem(@"Assets\Sounds\試合終了のゴング.mp3");
            int se_cursor = DX.LoadSoundMem(@"Assets\Sounds\カーソル移動8.mp3");
            int se_rockBroken = DX.LoadSoundMem(@"Assets\Sounds\怪獣の足音.mp3");
            int se_rockBrokenMin = DX.LoadSoundMem(@"Assets\Sounds\石が砕ける.mp3");
            int se_voiceChick = DX.LoadSoundMem(@"Assets\Sounds\ヒヨコの鳴き声.mp3");
            int se_bubu = DX.LoadSoundMem(@"Assets\Sounds\クイズ不正解1.mp3");
            int bgm_utyu = DX.LoadSoundMem(@"Assets\Sounds\seishishitauchu.mp3");

            const uint MASK_TURTLE = 1 << 0;
            const uint MASK_CHARACTER = 1 << 1;
            const uint MASK_WALL = 1 << 2;
            const uint MASK_ROCK = 1 << 3;
            const uint MASK_ENEMY = 1 << 4;

            const string TABLE_ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int stage_select = 0;

            
            const int GAME_PLAYING = 0, GAME_OVER = 1, GAME_CLEAR = 2;


            int[] stage_clearTime = new int[25 + 1];
                for (int i = 0; i <= 25; i++) stage_clearTime[i] = 0;


            sav_load();

            DX.PlaySoundMem(bgm_utyu, DX.DX_PLAYTYPE_LOOP);
            DX.ChangeVolumeSoundMem(128, bgm_utyu);

            Title_Loop();


            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^ ループ地点
            Label_Main:
            //----------------------------


            int game_state = GAME_PLAYING;
            int finish_count = 0;
            int game_time = 0;


            //タイルマップレイヤーの作成
            int map_width = 0;
            int map_height = 0;


            int player_sp = 0;
            int player_jumpStartCount = 0;
            int scroll_sp = 0;
            float player_x = 0, player_y = 0;
            


            int frame_between = 30;
            int log_frame = frame_between * 5;
            List<float> player_LogX = new List<float>(0);
            List<float> player_LogY = new List<float>(0);

            int player_chick = 0;
            List<int> chick_sp = new List<int>(0);

            float scroll_x = 0, scroll_y = 0;



            Menu_Loop();

            if (stage_select > 0)//メインゲームを始める
            {
                Main_Loop();
                if (game_state != GAME_PLAYING) goto Label_Main; //Goto文
            }



            // ＤＸライブラリの後始末
            DX.DxLib_End();
            Sprite.End(); //スプライトの後始末
            return;
            // ===================================================================== プログラム終了
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^タイトルループ
            void Title_Loop()
            {
                Title_Start();

                bool f = false;
                Input.Start();
                while (DX.ProcessMessage() != -1)
                {
                    Input.Update();
                    if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 1) //escape押されたら終了
                    {
                        break;

                    }

                    if (Input.Button.A(Input.BUTTON_TRIGER))
                    {
                        f = true;
                        DX.PlaySoundMem(se_ok, DX.DX_PLAYTYPE_BACK);
                        break;
                    }

                    Title_Update();
                    Screen_Project();
                    DX.ScreenFlip();
                }
                //Sprite.Clear();

                if (f) Useful.Wait(10);


                return;
                //================== 終了


                //開始処理
                void Title_Start()
                {
                    DX.SetDrawScreen(Hndl_BackScreen);
                    DX.DrawGraph(0, 0, Hndl_titleBack, DX.TRUE);
                    DX.DrawGraph(0, 0, Hndl_titleLogo, DX.TRUE);

                    //Useful.DrawString_shadow(0, 0, "ver" + ProgramProperty.version + "(" + ProgramProperty.published + ")");
                    DX.DrawStringToHandle(0, 0, "ver" + ProgramProperty.version + "(" + ProgramProperty.published + ")", DX.GetColor(100, 100, 200), Hndl_font12);
                    //Useful.DrawString_shadow(0, 240 - 16, "a game by " + ProgramProperty.maker);
                    DX.DrawStringToHandle(0, 240 - 16,"a game by " + ProgramProperty.maker, DX.GetColor(255, 255, 255), Hndl_font12);


                    Useful.DrawString_bordered(128, 240 - 64, "plese push Enter");
                }

                //更新処理
                void Title_Update()
                {


                    DX.SetDrawScreen(Hndl_DrawScreen);
                    DX.DrawGraph(0, 0, Hndl_BackScreen, DX.TRUE);


                }

            }
            //-----------------------------------------------------------



            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^メニューループ
            void Menu_Loop()
            {
                int px0 = 56, py0 = 40; //paint X0 etc.
                int pw0 = 64, ph0 = 40;

                int cx = 0, cy = 0, csp;
                if (stage_select>0)
                {
                    int s = stage_select - 1;
                    cx = s % 5; cy = s / 5;
                }

                int count = 0;

                Menu_Start();
                Input.Start();

                while (DX.ProcessMessage() != -1)
                {
                    Input.Update();
                    if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 1) //escape押されたら終了
                    {
                        break;
                        stage_select = 0;
                    }

                    if (count>12 && Input.Button.A(Input.BUTTON_TRIGER))
                    {
                        stage_select = cx + cy * 5 + 1;
                        //Game_retry = true;
                        DX.PlaySoundMem(se_ok, DX.DX_PLAYTYPE_BACK);
                        break;
                    }

                    Menu_Update();


                    Screen_Project();
                    DX.ScreenFlip();// 裏画面の内容を表画面に反映する
                    count++;
                }
                Sprite.Clear();
                return;
                //================== 終了


                //開始処理
                void Menu_Start()
                {
                    //DX.SetDrawScreen(Hndl_BackScreen);
                    //DX.DrawGraph(0, 0, Hndl_MenuBack, DX.TRUE);
                    //DX.DrawGraph(0, 0, Hndl_numple, DX.TRUE);

                    {//タイトル背景
                        int sp = Sprite.Set(Hndl_menuBack, 0, 0, 512, 320);
                        Sprite.Offset(sp, 0, 0, 500);
                        Sprite.Anim(sp, Sprite.AnimType_XY,
                            -300, -64, -64, 0);
                    }


                    for (int i = 0; i < 25; i++)
                    {
                        int x1 = i % 5, y1 = i / 5;
                        int sp = Sprite.Set(Hndl_stageNamePlate, 0, 0, 48, 24);

                        Sprite.Offset(sp, px0 + x1 * pw0, py0 + y1 * ph0, 0);


                        
                        if (stage_clearTime[i + 1] > 0)
                        {
                            int sp1 = Sprite.Set(Hndl_peke, 0, 0, 32, 32);
                            Sprite.Offset(sp1, px0 + x1 * pw0, py0 + y1 * ph0 - 4, 0);
                        }
                        
                    }

                    csp = Sprite.Set(Hndl_chick, 0, 0, 16, 16);
                    Sprite.Attribution(csp, Sprite.Attribution_reverse);
                    Sprite.Anim(csp, Sprite.AnimType_UV,
                        30, 0, 0,
                        30, 16, 0,
                        0);
                    {
                        int sp1 = Sprite.Set(Hndl_chick, 0, 0, 16, 16);
                        Sprite.Offset(sp1, 8 + 32 + 8, 0);
                        Sprite.Link(sp1, csp);
                        Sprite.Anim(sp1, Sprite.AnimType_UV,
                            30, 16, 0,
                            30, 0, 0,
                            0);

                    }
                }


                //更新処理
                void Menu_Update()
                {
                    bool pf = false;

                    if (Input.Button.Left(Input.BUTTON_REPEAT)) { cx--; pf = true; }
                    if (Input.Button.Right(Input.BUTTON_REPEAT)) { cx++; pf = true; }
                    if (Input.Button.Up(Input.BUTTON_REPEAT)) { cy--; pf = true; }
                    if (Input.Button.Down(Input.BUTTON_REPEAT)) { cy++; pf = true; }

                    cx = cx % 5; if (cx < 0) cx = 4;
                    cy = cy % 5; if (cy < 0) cy = 4;

                    if (pf)//カーソル移動で音出す
                    {
                        DX.PlaySoundMem(se_cursor, DX.DX_PLAYTYPE_BACK);
                    }


                    Sprite.Offset(csp, px0 + cx * pw0-8, py0 + cy * ph0, -50);


                    DX.SetDrawScreen(Hndl_DrawScreen);
                    DX.DrawGraph(0, 0, Hndl_BackScreen, DX.TRUE);

                    Sprite.Drawing();

                    //UI系統
                    Useful.DrawString_bordered(144, 8, "STAGE SELECT");

                    for (int i = 0; i < 25; i++)
                    {
                        int x1 = i % 5, y1 = i / 5;

                        Useful.DrawString_bordered(px0 + x1 * pw0+20, py0 + y1 * ph0+2, TABLE_ABC[i].ToString());

                        if (stage_clearTime[i + 1] != 0)//ハイスコア
                        {
                            DX.DrawStringToHandle(px0 + x1 * pw0, py0 + y1 * ph0 + 24,
                                Useful.count_ToTime(stage_clearTime[i + 1]), DX.GetColor(200, 200, 255), Hndl_font12);

                        }

                    }


                }
            }
            //-----------------------------------------------------------
            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ メインループ
            void Main_Loop()
            {

                BackScreen_Set();
                Map_Load();
                Player_Set();

                for (int y = 0; y < 5; y++) Chick_Set();


                loop_count = 0;
                Input.Start();
                while (DX.ProcessMessage() != -1)
                {
                    Input.Update();

                    if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 1) //escape押されたら終了
                    {
                        break;
                    }

                    if (game_state != GAME_PLAYING)//ゲーム終わりへ
                    {
                        finish_count++;
                        {
                            int T = (game_state == GAME_CLEAR) ? 300 : 210;
                            if (finish_count > T)//ゲーム終了
                            {
                                Sprite.Clear();
                                break;
                            }
                        }
                    }

                    Rock_Pop();

                    Sprite.AllUpdate();

                    Screen_Update();
                    loop_count++;
                }

                
                if (game_state == GAME_CLEAR)//記録更新
                {
                    if (stage_clearTime[stage_select] == 0)
                    {
                        stage_clearTime[stage_select] = game_time;
                    }
                    else
                    {
                        stage_clearTime[stage_select] = Math.Min(stage_clearTime[stage_select], game_time);
                    }
                    sav_save();
                }


            }// --------------------------------------------------------- メインループ


            //背景描画
            void BackScreen_Set()
            {
                DX.SetDrawScreen(Hndl_BackScreen);
                DX.DrawBox(0, 0, DRAW_WIDTH, DRAW_HEIGHT, DX.GetColor(100, 240, 240), DX.TRUE);
            }



            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ バイナリファイル読み書き
            void sav_save()
            {
                string path = "data.sav";
                byte[] binary = new byte[2 * 26];

                for (int i = 0; i <= 25; i++)
                {
                    binary[i * 2 + 0] = (byte)(stage_clearTime[i] / 0xFF);
                    binary[i * 2 + 1] = (byte)(stage_clearTime[i] % 0xFF);
                }
                File.WriteAllBytes(path, binary);
            }
            //--------------------------------
            void sav_load()
            {
                string path = "data.sav";

                if (!File.Exists(path)) return;

                byte[] binary = File.ReadAllBytes(path);

                for (int i = 0; i <= 25; i++)
                {
                    stage_clearTime[i] = binary[i * 2 + 0] * 0xFF + binary[i * 2 + 1];

                }
                File.WriteAllBytes(path, binary);
            }









            //マップロード
            void Map_Load()
            {
                string stage = Useful.zero_padding(stage_select.ToString(), 2);

                string path = @"Assets\Maps\map"+stage+".csv";

                StreamReader sr = new StreamReader(path);

                int sx = 0, sy = 0;
                while (!sr.EndOfStream)
                {
                    // CSVファイルの一行を読み込む
                    string line = sr.ReadLine();

                    if (sy == 0)
                    {
                        string[] values = line.Split(',');// 読み込んだ一行をカンマ毎に分けて配列に格納する
                        sx = values.Length;
                    }
                    sy++;
                }

                sr = new StreamReader(path);

                map_matrix = new string[sx, sy];

                int i = 0;
                while (!sr.EndOfStream)
                {
                    // CSVファイルの一行を読み込む
                    string line = sr.ReadLine();
                    // 読み込んだ一行をカンマ毎に分けて配列に格納する
                    string[] values = line.Split(',');
                    // 格納
                    int j = 0;
                    foreach (string v in values)
                    {
                        map_matrix[j, i] = v;
                        j++;
                    }
                    i++;
                }


                map_width = sx;
                map_height = sy;


                //マップ初期化処理
                for (int x=0; x<sx; x++)
                {
                    for (int y=0; y<sy; y++)
                    {

                        if (map_matrix[x, y].Contains("始"))//スタート地点
                        {
                            player_x = x * 16-4;
                            player_y = y * 16 - 16;
                        }
                    }
                }



            }







            //プレイヤー処理
            void Player_Set()
            {
                int sp = Sprite.Set(Hndl_Luna, 0, 0, 32, 32);
                player_sp = sp;

                Properties.Player prop = new Properties.Player();
                Sprite.sprite[sp].Dict.Add("prop", prop);
                prop.x = player_x;
                prop.y = player_y;
                prop.vx = 0;
                prop.vy = 0;
                prop.anim_count = 0;


                //座標ログを初期化
                for (int i = 0; i < log_frame; i++)
                {
                    player_LogX.Add(prop.x);
                    player_LogY.Add(prop.y);
                }



                Sprite.Collider(sp, 4, 4, 16, 32 - 4, MASK_CHARACTER);

                Sprite.sprite[sp].Update += Player_Update;

                Sprite.Attribution(sp, Sprite.Attribution_reverse);

                //スクロール用スプライト
                int sp1 = Sprite.Set(-1,0,0,0,0);
                scroll_sp = sp1;
            }


            void Player_Update(int sp)
            {
                Properties.Player prop = (Properties.Player)(Sprite.sprite[player_sp].Dict["prop"]);

                float dx = prop.vx, dy = prop.vy;

                if (Input.Button.Right()) dx += +1;
                if (Input.Button.Left()) dx += -1;


                //画像内部キャラクター表示領域
                int x1 = 4, x2 = 19;
                int y1 = 4, y2 = 31;


                //移動処理
                if (dx < 0 && !player_moveCheckRect(prop.x + x1 + dx, prop.y + y1, 1, y2-y1+1)) prop.x += dx;
                if (dx > 0 && !player_moveCheckRect(prop.x + x2 + dx, prop.y + y1, 1, y2-y1+1)) prop.x += dx;

                if (dy < 0) 
                {
                    while (dy != 0)
                    {
                        float dy1 = -1;
                        if (Math.Abs(dy) < 1) dy1 = dy;

                        if (!player_moveCheckRect(prop.x + x1, prop.y + y1 + dy1, x2-x1+1, 1))//空中
                        {
                            prop.y += dy1;
                            dy -= dy1;
                        }
                        else//衝突
                        {
                            prop.y = (int)(prop.y);
                            prop.vy = 0;
                            dy = 0;
                        }
                    }
                }
                if (dy > 0)
                {
                    while (dy != 0)
                    {
                        float dy1 = 1;
                        if (Math.Abs(dy) < 1) dy1 = dy;

                        if (!player_moveFallCheckRect(prop.x + x1, prop.y + y2 + dy1, x2 - x1 + 1, 1))//空中
                        {
                            prop.y += dy1;
                            dy -= dy1;
                        }
                        else//着地
                        {
                            if (prop.y < 0) prop.y--;
                            prop.y = (int)(prop.y);
                            dy = 0;
                        }
                    }

                }
                


                bool hvr = false;
                if (player_moveFallCheckRect(prop.x + x1, prop.y + y2 + 1, x2 - x1 + 1, 1)) //地面に設置してるとき
                {
                    prop.vy = 0;
                    player_jumpStartCount = 0;

                    if (Input.Button.A(Input.BUTTON_TRIGER))//ジャンプ
                    {
                        DX.PlaySoundMem(se_jamp, DX.DX_PLAYTYPE_BACK);
                        prop.vy = -3.2f;
                        player_jumpStartCount = 12;
                    }
                }
                else //空中に浮いているとき
                {
                    prop.vy += 0.1f;
                    hvr = true;
                    player_jumpStartCount--;
                }


                if (player_jumpStartCount>0 && prop.vy<0)//小ジャンプ化
                {
                    if (!Input.Button.A(Input.BUTTON_STAY)) 
                    {
                        prop.vy /= 2;
                        player_jumpStartCount = 0;
                    }
                }


                {
                    bool atack = false;
                    {//敵踏んだら倒す
                        int h = Sprite.HitRectangle((int)prop.x + x1 + 2, (int)prop.y + 28, x2 - x1 + 1 - 4, 31 - 28 + 1, MASK_ENEMY);
                        if (h > -1)//踏んじゃった
                        {
                            Properties.Character p1 = (Properties.Character)(Sprite.sprite[h].Dict["prop"]);
                            Star_Effect((int)p1.x - 8, (int)p1.y - 8, Hndl_starY);
                            smoke_effect((int)p1.x - 8, (int)p1.y - 8);
                            Sprite.Clear(h);
                            prop.vy = Useful.between(Input.Button.push_A, 1, 6) ? -4 : -2;
                            DX.PlaySoundMem(se_nyu, DX.DX_PLAYTYPE_BACK);
                            atack = true;
                        }
                    }

                    {//敵との当たり判定
                        int h = Sprite.HitRectangle((int)prop.x + x1, (int)prop.y + y1, x2 - x1 + 1, y2 - y1 + 1, MASK_ENEMY | MASK_ROCK);
                        if (!atack && h > -1)//死んだ
                        {
                            Ink_Effect((int)(prop.x - scroll_x) - 12, (int)(prop.y - scroll_y) - 8);
                            Sprite.Clear(sp);
                            game_Overing();
                            DX.PlaySoundMem(se_atacked, DX.DX_PLAYTYPE_BACK);
                            return;
                        }
                    }
                    if (prop.y > DRAW_WIDTH)
                    {
                        game_Overing();
                    }
                }





                int w = 16 * 8;
                if (prop.x < w)//スクロール
                {
                    float sx = scroll_x - (prop.x - w);
                    float px = w;

                    if (sx < 0) //端まで行ってなかったらスクロール
                    {
                        scroll_x = sx;
                        prop.x = px;
                    }
                }
                if (prop.x>DRAW_WIDTH - w)//スクロール
                {
                    float sx = scroll_x - (prop.x - (DRAW_WIDTH - w));
                    float px = DRAW_WIDTH - w;

                    if (sx > (DRAW_WIDTH - map_width * 16)) //端まで行ってなかったらスクロール
                    {
                        scroll_x = sx;
                        prop.x = px;
                    }
                }




                //Console.WriteLine($"player_x = {prop.x}, player_y = {prop.y}");



                if (Input.Button.Right()) Sprite.Attribution(sp, Sprite.Attribution_reverse);
                if (Input.Button.Left()) Sprite.Attribution(sp, 0);

                if (Input.Button.Right() || (Input.Button.Left()))
                {
                    prop.anim_count++;
                    if (prop.anim_count >= 60) prop.anim_count = 0;

                    Sprite.Image(sp, ((int)(prop.anim_count / 10)) * 24, 32, 24, 32);

                }
                else
                {
                    Sprite.Image(sp, 0, 0, 24, 32);
                    prop.anim_count = 0;
                }
                if (hvr)//空中なら
                {
                    if (prop.vy < 0)
                    {
                        Sprite.Image(sp, 5 * 24, 32, 24, 32);
                    }
                    else
                    {
                        Sprite.Image(sp, 4 * 24, 32, 24, 32);
                    }
                }
                

                





                Sprite.Offset(sp,prop.x, prop.y);
                player_x = prop.x - scroll_x;
                player_y = prop.y - scroll_y;

                Sprite.Offset(scroll_sp, scroll_x, scroll_y);

                player_LogX.Add(prop.x - scroll_x);
                player_LogY.Add(prop.y - scroll_y);
                player_LogX.RemoveAt(0);
                player_LogY.RemoveAt(0);

                if (loop_count%6==0) chick_backNumber_reset();
            }


            void game_Overing()
            {
                if (game_state == GAME_PLAYING) game_state = GAME_OVER;
            }



            //壁との当たり判定
            bool player_moveCheckRect(float x0, float y0, int width, int height)
            {
                x0 -= scroll_x; y0 -= scroll_y;
                return character_moveCheckRect(x0, y0, width, height);
            }
            bool player_moveFallCheckRect(float x0, float y0, int width, int height)
            {
                x0 -= scroll_x; y0 -= scroll_y;
                return character_moveFallCheckRect(x0, y0, width, height);
            }


            bool character_moveCheckRect(float x0, float y0, int width, int height)
            {
                for (int x=0; x<width; x+=8)
                {
                    if (width - 1 - x < 8) x = width - 1;
                    for (int y=0; y<height; y+=8)
                    {
                        if (height - 1 - y < 8) y = height - 1;
                        if (character_moveCheck_map(x0 + x, y0 + y)) return true;
                    }
                }
                if (block_check_sprite((int)x0, (int)y0, width, height)) return true;
                return false;
            }
            bool character_moveFallCheckRect(float x0, float y0, int width, int height)
            {
                for (int x = 0; x < width; x+=8)
                {
                    if (width - 1 - x < 8) x = width - 1;
                    for (int y = 0; y < height; y+=4)
                    {
                        if (height - 1 - y < 8) y = height - 1;
                        if (character_moveFallCheck_map(x0 + x, y0 + y)) return true;
                    }
                }
                if (block_check_sprite((int)x0, (int)y0, width, height)) return true;
                return false;
            }
            bool character_moveCheck_map(float x0, float y0)
            {
                if (block_checkNormal_map((int)x0, (int)(y0))) return true;
                return false;
            }
            bool character_moveFallCheck_map(float x0, float y0)
            {
                if (!block_check_map((int)x0, (int)(y0 - 1), "b") && block_check_map((int)x0, (int)(y0), "b")) return true;
                if (!block_check_map((int)x0, (int)(y0 - 1), "B") && block_check_map((int)x0, (int)(y0), "B")) return true;
                if (block_checkNormal_map((int)x0, (int)(y0))) return true;
                return false;
            }


            //固体の当たり判定
            bool block_checkNormal(int x0, int y0)
            {
                return block_checkNormal_map(x0, y0) || block_check_sprite(x0, y0, 1, 1);
            }
            bool block_checkNormal_map(int x0, int y0)
            {
                return block_check_map(x0, y0, "g","角","岩");
            }
            bool block_check(int x0, int y0, params string[] c)
            {
                block_check_map(x0, y0, c);
                block_check_sprite(x0, y0, 1, 1);
                return false;
            }
            bool block_check_map(int x0, int y0, params string[] c)
            {
                if (x0 < 0) x0 -= 16; //負でも区間幅を統一
                if (y0 < 0) y0 -= 16; //負でも区間幅を統一
                if (map_check3(x0 / 16, y0 / 16, c)) return true;
                return false;
            }
            bool block_check_sprite(int x0, int y0, int width, int height)
            {
                if (Sprite.HitRectangle(x0 + (int)scroll_x, y0 + (int)scroll_y, width, height, MASK_WALL) != -1) return true;
                return false;
            }




            //キャラ重なったときの反発処理
            void chick_Repulsion(int sp, ref float dx, ref float dy)
            {
                Properties.Character p1 = (Properties.Character)Sprite.sprite[sp].Dict["prop"];
                float x1 = p1.x, y1 = p1.y;

                int h = Sprite.HitSprite(sp, MASK_TURTLE);
                if (h > -1)
                {
                    Properties.Character p2 = (Properties.Character)Sprite.sprite[h].Dict["prop"];
                    float x2 = p2.x, y2 = p2.y;

                    double r = Math.Atan2(y2-y1,x2-x1);
                    dx += (float)-Math.Cos(r)*4;
                    dy += (float)-Math.Sin(r)*4;



                    //Console.WriteLine($"{sp},{h}");
                }
            }







            //ひよこの処理
            void Chick_Set()
            {
                int sp = Sprite.Set(Hndl_chick, 0, 0, 16, 16);
                Sprite.Link(sp, scroll_sp);
            
                Properties.Chick prop = new Properties.Chick();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                player_chick++;
                prop.backNumber = player_chick;
                chick_sp.Add(sp);

                prop.x = player_LogX[prop.backNumber * frame_between-frame_between/2]+8;
                prop.y = player_LogY[prop.backNumber * frame_between- frame_between / 2] +16;
                prop.vx = 0;
                prop.vy = 0;
                prop.anim_count = 0;

                Sprite.Collider(sp, 2, 2, 16 - 2 * 2, 16 - 2, MASK_CHARACTER | MASK_TURTLE);

                Sprite.sprite[sp].Update += Chick_Update;

            }

            void Chick_Update(int sp)
            {
                Properties.Chick prop = (Properties.Chick)(Sprite.sprite[sp].Dict["prop"]);

                
                
                //アイディアル座標
                int ix = (int)player_LogX[prop.backNumber * frame_between - frame_between / 2] +8;
                int iy = (int)player_LogY[prop.backNumber * frame_between - frame_between / 2] +16;


                float dx = prop.vx, dy = prop.vy;
                bool jf = false;

                int spc = 4;
                //if (Useful.between(Useful.distance(ix - (player_x + 8),iy - (player_y + 16)), -12, 12)) spc = 32;
                if (Useful.between(ix - (player_x + 8), -12, 12)) spc = 32;

                if (prop.x+spc < ix) 
                {
                    dx += 1; //Sprite.Attribution(sp, Sprite.Attribution_reverse);
                    //if (dx < 0) jf = true;
                }
                if (prop.x-spc > ix) 
                {
                    dx += -1; //Sprite.Attribution(sp, 0);
                    //if (dx > 0) jf = true;
                }

                if (dx>0)
                {
                    Sprite.Attribution(sp, Sprite.Attribution_reverse);
                }
                else if (dx<0)
                {
                    Sprite.Attribution(sp, 0);
                }




                chick_Repulsion(sp, ref dx, ref dy);


                int x1 = 2, x2 = 13;
                int y1 = 2, y2 = 15;

                character_Move(prop, x1, y1, x2, y2, dx, dy);

                if (character_moveFallCheckRect(prop.x + x1, prop.y + y2 + 1, x2 - x1 + 1, 1)) //地面に設置してるとき
                {
                    prop.vy = 0;

                    if (prop.y - iy > 2)//ジャンプ
                    {
                        prop.vy = (prop.y-iy>48) ? -4 : -3;
                        //if (prop.y - iy > 48) DX.PlaySoundMem(se_jampChick, DX.DX_PLAYTYPE_BACK);
                        if (prop.backNumber == player_chick) DX.PlaySoundMem(se_jampChick, DX.DX_PLAYTYPE_BACK);
                    }
                    else if (loop_count % 300 == prop.backNumber * 60)
                    {
                        prop.vy = -1.5f;
                        //DX.PlaySoundMem(se_jampChick, DX.DX_PLAYTYPE_BACK);
                        if (rand.Next(0,5)==0) DX.PlaySoundMem(se_voiceChick, DX.DX_PLAYTYPE_BACK);
                        if (prop.backNumber == player_chick) DX.PlaySoundMem(se_jampChick, DX.DX_PLAYTYPE_BACK);
                    }
                }
                else //空中に浮いているとき
                {
                    prop.vy += 0.1f;
                }

                {//敵との当たり判定
                    int h = Sprite.HitRectangle((int)(prop.x + scroll_x) + x1, (int)(prop.y + scroll_y) + y1, x2 - x1 + 1, y2 - y1 + 1, MASK_ENEMY | MASK_ROCK);
                    if (h > -1)//死んだ
                    {
                        Ink_Effect((int)(prop.x) - 16, (int)(prop.y) - 16);
                        Sprite.Clear(sp);
                        chick_sp.Remove(sp);
                        player_chick--;
                        game_Overing();
                        DX.PlaySoundMem(se_atacked, DX.DX_PLAYTYPE_BACK);
                        return;
                    }
                    if (prop.y > DRAW_WIDTH)
                    {
                        game_Overing();
                    }
                }



                prop.anim_count++;
                if (prop.anim_count >= 40) prop.anim_count = 0;
                Sprite.Image(sp, (prop.anim_count/10) * 16, 0, 16, 16) ;
                Sprite.Offset(sp, prop.x, prop.y);
            }


            void chick_backNumber_reset()
            {
                Dictionary<int, int> d = new Dictionary<int, int>(0);

                for (int i = 0; i < player_chick; i++)
                {
                    int sp = chick_sp[i];
                    Properties.Chick prop = (Properties.Chick)(Sprite.sprite[sp].Dict["prop"]);
                    d.Add(chick_sp[i], (int)Useful.distance(player_x - prop.x, (player_y - prop.y)*4));
                }

                var ob = d.OrderByDescending((x) => x.Value);

                int c = 0;
                foreach (var v in ob)
                {
                    int sp = v.Key;
                    Properties.Chick prop = (Properties.Chick)(Sprite.sprite[sp].Dict["prop"]);

                    c++;
                    prop.backNumber = c;
                }
            }







                //キャラクター基本移動処理
                void character_Move(Properties.Character prop, int x1, int y1, int x2, int y2, float dx, float dy)
            {
                //移動処理
                if (dx < 0 && !character_moveCheckRect(prop.x + x1 + dx, prop.y + y1, 1, y2 - y1 + 1)) prop.x += dx;
                if (dx > 0 && !character_moveCheckRect(prop.x + x2 + dx, prop.y + y1, 1, y2 - y1 + 1)) prop.x += dx;

                if (dy < 0)
                {
                    while (dy != 0)
                    {
                        float dy1 = -1;
                        if (Math.Abs(dy) < 1) dy1 = dy;

                        if (!character_moveCheckRect(prop.x + x1, prop.y + y1 + dy1, x2 - x1 + 1, 1))//空中
                        {
                            prop.y += dy1;
                            dy -= dy1;
                        }
                        else//衝突
                        {
                            prop.y = (int)(prop.y);
                            prop.vy = 0;
                            dy = 0;
                        }
                    }
                }
                if (dy > 0) //次回からここを参考に処理を組む
                {
                    while (dy != 0)
                    {
                        float dy1 = 1;
                        if (Math.Abs(dy) < 1) dy1 = dy;

                        if (!character_moveFallCheckRect(prop.x + x1, prop.y + y2 + dy1, x2 - x1 + 1, 1))//空中
                        {
                            prop.y += dy1;
                            dy -= dy1;
                        }
                        else//着地
                        {
                            if (prop.y < 0) prop.y--;
                            prop.y = (int)(prop.y);
                            dy = 0;
                        }
                    }
                }

            }






            //リフト
            void Lift_Set(int x, int y, int move, int mode)
            {
                int sp = Sprite.Set(Hndl_lift, 0, 0, 48, 8);
                Sprite.Link(sp, scroll_sp);

                Properties.Lift prop = new Properties.Lift();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                prop.x0 = x; prop.x = prop.x0;
                prop.y0 = y; prop.y = prop.y0;
                prop.moveRange = 16 * move;
                prop.count = 0;
                prop.mode = mode;


                Sprite.Collider(sp, 0, 0, 48, 8, MASK_WALL);

                Sprite.sprite[sp].Update += Lift_Update;
                Sprite.Offset(sp, prop.x, prop.y);
            }


            void Lift_Update(int sp)
            {
                Properties.Lift prop = (Properties.Lift)(Sprite.sprite[sp].Dict["prop"]);

                prop.count++;


                int y1 = (int)prop.y, x1 = (int)prop.x;
                
                switch (prop.mode)
                {
                    case 0:
                        prop.y = prop.y0 + (int)(prop.moveRange * Math.Sin(Math.PI * prop.count / 180));
                        break;
                    case 1:
                        prop.x = prop.x0 + (int)(prop.moveRange * Math.Sin(Math.PI * prop.count / 180));
                        break;
                }

                int y2 = (int)prop.y, x2 = (int)prop.x;

                int dx = x2 - x1, dy = y2 - y1;

                //上に載っているものを動かす
                List<int> hl;
                
                if (prop.mode == 0)
                {
                    Useful.Sprite_HitRectangle_List(x1 + (int)scroll_x, y1 + (int)scroll_y - 1, 48, 9, MASK_CHARACTER, out hl);
                }
                else
                {
                    Useful.Sprite_HitRectangle_List(x1 + (int)scroll_x, y1 + (int)scroll_y - 1, 48, 1, MASK_CHARACTER, out hl);
                }
                
                
                
                for (int i = 0; i < hl.Count; i++)
                {
                    Properties.Character prop1 = (Properties.Character)(Sprite.sprite[hl[i]].Dict["prop"]);

                    string key = "LiftHitTime";
                    if (Sprite.sprite[hl[i]].Dict.ContainsKey(key)) //リフト上物体移の動処理が重複しないようにする
                    {
                        if ((int)Sprite.sprite[hl[i]].Dict[key] == loop_count) continue; //処理済み
                        Sprite.sprite[hl[i]].Dict[key] = loop_count;
                    }
                    else//キーの作成
                    {
                        Sprite.sprite[hl[i]].Dict.Add(key, loop_count);
                    }


                    if (prop.mode == 0)//動かす
                    {
                        prop1.y += dy;
                    }
                    else
                    {
                        prop1.x += dx;
                    }
                }





                Sprite.Offset(sp, prop.x, prop.y);
            }


            //ゴール
            void Goal_Set(int x, int y)
            {
                int sp = Sprite.Set(Hndl_cloud, 0, 0, 96,16);
                Sprite.Link(sp, scroll_sp);

                Properties.Goal prop = new Properties.Goal();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                prop.x = x; 
                prop.y = y;


                Sprite.Collider(sp, 0, 0, 96, 16, MASK_WALL);

                Sprite.sprite[sp].Update += Goal_Update;

            }


            void Goal_Update(int sp)
            {
                Properties.Goal prop = (Properties.Goal)(Sprite.sprite[sp].Dict["prop"]);

                if (loop_count % 6 == 0)
                {
                    //ゴール判定
                    List<int> hl;
                    Useful.Sprite_HitRectangle_List((int)(prop.x + scroll_x), (int)(prop.y + (int)scroll_y) - 16, 96, 16, MASK_CHARACTER, out hl);
                    for (int i = 0; i < hl.Count; i++)
                    {
                        bool f= true;

                        if (!hl.Contains(player_sp)) f = false;
                        for (int j=0; j<player_chick; j++)
                        {
                            if (!hl.Contains(chick_sp[j])) f = false;
                        }
                        
                        if (f)//クリア
                        {
                            //Console.WriteLine("クリア");
                            if (game_state == GAME_PLAYING)
                            {
                                game_state = GAME_CLEAR;
                                DX.PlaySoundMem(se_star, DX.DX_PLAYTYPE_BACK);
                            }
                        }

                    }
                    if (game_state == GAME_CLEAR)
                    {
                        if (loop_count % 12 == 0) Star_Effect((int)(prop.x + 36), (int)(prop.y), Hndl_starB);

                    }

                }




                Sprite.Offset(sp, prop.x, prop.y);
            }


            //スライム
            void Slime_Set(int x, int y, int type)
            {
                int sp = Sprite.Set((type==0) ? Hndl_slime : Hndl_slimeR, 0, 0, 16, 16);
                Sprite.Link(sp, scroll_sp);

                Properties.Slime prop = new Properties.Slime();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                prop.x = x; prop.xBefore = prop.x;
                prop.y = y; prop.yBefore = prop.y;
                prop.vx = 0;
                prop.vy = 0;
                prop.anim_count = 0;
                prop.type = type;

                prop.moveX = -1;

                Sprite.Collider(sp, 0, 0, 16, 16, MASK_CHARACTER | MASK_ENEMY);
                Sprite.Offset(sp, prop.x, prop.y);
                Sprite.sprite[sp].Update += Slime_Update;
            }

            void Slime_Update(int sp)
            {
                Properties.Slime prop = (Properties.Slime)(Sprite.sprite[sp].Dict["prop"]);

                float dx = prop.vx, dy = prop.vy;

                dx += prop.moveX;

                int x1 = 0, x2 = 15;
                int y1 = 0, y2 = 15;

                character_Move(prop, x1, y1, x2, y2, dx, dy);

                if (character_moveFallCheckRect(prop.x + x1, prop.y + y2 + 1, x2 - x1 + 1, 1)) //地面に設置してるとき
                {
                    prop.vy = 0;
                }
                else //空中に浮いているとき
                {
                    if (prop.vy==0) character_Move(prop, x1, y1, x2, y2, 0, 1); //ちょっとだけ落下させとく
                    prop.vy += 0.1f;
                }

                if (prop.x == prop.xBefore) prop.moveX *= -1;//方向転換
                if (prop.type == 1)
                {
                    if (prop.vy == 0 && !character_moveFallCheckRect(prop.x + 8 + prop.moveX * 4, prop.y + 16, 1, 1)) prop.moveX *= -1;//方向転換
                }





                if (prop.y>DRAW_HEIGHT || prop.x<scroll_x-32)
                {
                    Sprite.Clear(sp); return;
                }


                if (prop.moveX<0)
                {
                    Sprite.Attribution(sp, Sprite.Attribution_reverse);
                }
                else
                {
                    Sprite.Attribution(sp, 0);
                }

                prop.anim_count++;
                if (prop.anim_count >= 30) prop.anim_count = 0;
                Sprite.Image(sp, (prop.anim_count / 15) * 16, 0, 16, 16);
                Sprite.Offset(sp, prop.x, prop.y);

                prop.xBefore = prop.x;
                prop.yBefore = prop.y;
            }


            //煙エフェクト
            void smoke_effect(int x0, int y0)
            {
                for (int i = 0; i < 4; i++)
                {
                    int x1 = x0 - 16 + (i / 2) * 32;
                    int y1 = y0 - 16 + (i % 2) * 32;

                    int sp = Sprite.Set(Hndl_smoke, 0, 0, 32, 32);
                    Sprite.Offset(sp, x1, y1, -800);
                    Sprite.Link(sp, scroll_sp);

                    Sprite.Anim(sp, Sprite.AnimType_UV,
                        8, 0, 0,
                        8, 32, 0,
                        4, 32 * 2, 0,
                        4, 32 * 3, 0
                        );

                    Sprite.sprite[sp].Update += new SpriteCompornent.UpdateDelegate(Useful.Sprite_Effeectfade); //削除処理追加
                }
            }

            //星エフェクト
            void Star_Effect(int x0, int y0, int image)
            {
                for (int a = 0; a < 12; a++)
                {
                    int sp = Sprite.Set(image, 0, 0, 24, 24);
                    Sprite.Link(sp, scroll_sp);

                    int x1 = x0 + 0, y1 = y0 + 0;
                    int x2 = x1 + (int)(Math.Cos(Math.PI * (a * 30f / 180f)) * 128);
                    int y2 = y1 + (int)(Math.Sin(Math.PI * (a * 30f / 180f)) * 128);

                    Sprite.Offset(sp, x1, y1, -500);

                    Sprite.Anim(sp, Sprite.AnimType_XY
                        , -20, x2, y2
                        , 1);
                    Sprite.Anim(sp, Sprite.AnimType_UV
                        , 5, 24 * 0, 0
                        , 5, 24 * 1, 0
                        , 5, 24 * 2, 0
                        , 5, 24 * 3, 0
                        , 0
                        );

                    Sprite.sprite[sp].Update += new SpriteCompornent.UpdateDelegate(Useful.Sprite_EffeectfadeXY);
                }
            }


            //インクエフェクト
            void Ink_Effect(int x0, int y0)
            {
                for (int a = 0; a < 6; a++)
                {
                    int sp = Sprite.Set(Hndl_ink, 0, 0, 48, 48);
                    Sprite.Link(sp, scroll_sp);

                    int x1 = x0 + 0, y1 = y0 + 0;
                    int x2 = x1 + (int)(Math.Cos(Math.PI * (a * 60f / 180f)) * 128);
                    int y2 = y1 + (int)(Math.Sin(Math.PI * (a * 60f / 180f)) * 128);

                    Sprite.Offset(sp, x1, y1, -500);

                    /*
                    Sprite.Anim(sp, Sprite.AnimType_XY
                        , -20, x2, y2
                        , 1);
                    */
                    Useful.Sprite_ParabolaAnim(sp, x1, y1, x2, y2, 2, 2);

                    Sprite.Anim(sp, Sprite.AnimType_UV
                        , 5, 48 * 0, 0
                        , 5, 48 * 1, 0
                        , 5, 48 * 2, 0
                        , 5, 48 * 3, 0
                        , 0
                        );

                    Sprite.sprite[sp].Update += new SpriteCompornent.UpdateDelegate(Useful.Sprite_EffeectfadeXY);
                }
            }


            //落石
            void Rock_Pop()
            {
                if (loop_count % 30==0)
                {
                    Rock_Set();
                }
                if (loop_count % 30 == 0)
                {
                    Rock16_Set();
                }

            }

            void Rock_Set()
            {

                //落下x座標の計算
                float x0 = -scroll_x + rand.Next(0, DRAW_WIDTH);
                x0 = ((int)x0/16)*16;

                //if (!map_check((int)x0 / 16, 0, "r", "q")) return;
                if (!map_check((int)x0 / 16, 0, "r")) return;

                /*
                float fv = 1.5f;
                if (map_check((int)x0 / 16, 0, "q"))//qの場合は控えめで
                {
                    if (rand.Next(0, 2) == 0)
                    {
                        return;
                    }
                    else
                    {
                        fv = 0.6f;
                    }
                }
                */


                int sp = Sprite.Set(Hndl_rock, 0, 0, 48, 48);
                Sprite.Link(sp, scroll_sp);

                Properties.Rock prop = new Properties.Rock();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                prop.x = x0-16;
                prop.y = -48;
                prop.fall_velocity = 1.5f;

                Sprite.Collider(sp, 0, 0, 48, 48, MASK_ROCK);
                Sprite.Offset(sp, prop.x, prop.y);
                Sprite.sprite[sp].Update += Rock_Update;

            }

            void Rock_Update(int sp)
            {
                Properties.Rock prop = (Properties.Rock)(Sprite.sprite[sp].Dict["prop"]);

                prop.y += prop.fall_velocity;



                int cx = (int)(prop.x / 16), cy = (int)((prop.y + 48) / 16);
                bool cf = false;
                for (int x = 0; x <= 2; x++)//ブロックと接触するか調べてく
                {
                    int x1 = cx + x, y1 = cy;
                    if (map_check(x1, y1, "岩"))//岩ブロック破壊
                    {
                        map_matrix[x1, y1] = map_matrix[x1, y1].Replace("岩", ""); cf = true;
                        Useful.Clash_effect(Hndl_blocks, 16, 16, 16, 16, x1 * 16, y1 * 16, scroll_sp);
                    }
                    else if (character_moveFallCheckRect((int)prop.x + x * 16, (int)prop.y + 47, 16, 1))
                    {
                        cf = true;
                    }
                }

                if (cf) //落石破壊
                {
                    Useful.Clash_effect(Hndl_rock, 0, 0, 48, 48, (int)prop.x, (int)prop.y, scroll_sp);
                    Sprite.Clear(sp);
                    DX.PlaySoundMem(se_rockBroken, DX.DX_PLAYTYPE_BACK);
                    return;
                }




                if (prop.y > DRAW_HEIGHT) //画面外いったら
                {
                    Sprite.Clear(sp); return;
                }



                prop.anim_count++;
                
                {
                    int t = 10;//(prop.fall_velocity<1f) ? 30 : 10;
                    if (prop.anim_count >= t*4) prop.anim_count = 0;
                    Sprite.Image(sp, (prop.anim_count / t) * 48, 0);
                }




                Sprite.Offset(sp, prop.x, prop.y,-500);
            }


            void Rock16_Set()
            {

                //落下x座標の計算
                float x0 = -scroll_x + rand.Next(0, DRAW_WIDTH);
                x0 = ((int)x0 / 16) * 16;

                //if (!map_check((int)x0 / 16, 0, "r", "q")) return;
                if (!map_check((int)x0 / 16, 0, "q")) return;



                int sp = Sprite.Set(Hndl_rock16, 0, 0, 16, 16);
                Sprite.Link(sp, scroll_sp);

                Properties.Rock prop = new Properties.Rock();
                Sprite.sprite[sp].Dict.Add("prop", prop);

                prop.x = x0 - 16;
                prop.y = -16;
                prop.fall_velocity = 0.6f;

                Sprite.Collider(sp, 0, 0, 16, 16, MASK_ROCK);
                Sprite.Offset(sp, prop.x, prop.y);
                Sprite.sprite[sp].Update += Rock16_Update;

            }

            void Rock16_Update(int sp)
            {
                Properties.Rock prop = (Properties.Rock)(Sprite.sprite[sp].Dict["prop"]);

                prop.y += prop.fall_velocity;



                int cx = (int)(prop.x / 16), cy = (int)((prop.y + 16) / 16);
                bool cf = false;
                for (int x = 0; x <= 0; x++)//ブロックと接触するか調べてく
                {
                    int x1 = cx + x, y1 = cy;
                    if (map_check(x1, y1, "岩"))//岩ブロック破壊
                    {
                        map_matrix[x1, y1] = map_matrix[x1, y1].Replace("岩", ""); cf = true;
                        Useful.Clash_effect(Hndl_blocks, 16, 16, 16, 16, x1 * 16, y1 * 16, scroll_sp);
                    }
                    else if (character_moveFallCheckRect((int)prop.x + x * 16, (int)prop.y + 15, 16, 1))
                    {
                        cf = true;
                    }
                }

                if (cf) //落石破壊
                {
                    Useful.Clash_effect(Hndl_rock16, 0, 0, 16, 16, (int)prop.x, (int)prop.y, scroll_sp);
                    Sprite.Clear(sp);
                    DX.PlaySoundMem(se_rockBrokenMin, DX.DX_PLAYTYPE_BACK);
                    return;
                }




                if (prop.y > DRAW_HEIGHT) //画面外いったら
                {
                    Sprite.Clear(sp); return;
                }



                prop.anim_count++;

                {
                    int t = 20;
                    if (prop.anim_count >= t * 4) prop.anim_count = 0;
                    Sprite.Image(sp, (prop.anim_count / t) * 16, 0);
                }




                Sprite.Offset(sp, prop.x, prop.y, -500);
            }



            //張りぼて設置
            int Haribote_Set(int x, int y, int image, int u, int v, int w, int h)
            {
                int sp = Sprite.Set(image, u, v, w, h);
                Sprite.Link(sp, scroll_sp);
                Sprite.Offset(sp, x, y, 500);
                return sp;
            }





            //UI更新
            void UI_Update()
            {
                Useful.DrawString_bordered(0, 0, $"STAGE {TABLE_ABC[stage_select-1]}");

                if (game_state == GAME_PLAYING) game_time = Math.Min(0xFFFF, loop_count);
                Useful.DrawString_bordered(272, 0, $"TIME {Useful.count_ToTime(game_time)}");

                if (finish_count >= 60)
                {
                    
                    if (game_state == GAME_OVER)
                    {
                        Useful.DrawString_bordered(128, DRAW_HEIGHT / 2, "G A M E  O V E R", DX.GetColor(255, 120, 120));
                        if (finish_count == 60) DX.PlaySoundMem(se_bubu, DX.DX_PLAYTYPE_BACK);
                    }
                    else if (game_state == GAME_CLEAR)
                    {
                        Useful.DrawString_bordered(120, DRAW_HEIGHT / 2, "G A M E  C L E A R", DX.GetColor(255, 255, 0));
                        if (finish_count == 60) DX.PlaySoundMem(se_kong, DX.DX_PLAYTYPE_BACK);
                    }
                }

            }








            //画面更新
            void Screen_Update()
            {
                // 背景塗りつぶし

                DX.SetDrawScreen(Hndl_DrawScreen);
                //DX.DrawGraph(0, 0, Hndl_BackScreen, DX.TRUE);
                DX.DrawGraph((int)scroll_x/2, 0, Hndl_backScenery, DX.TRUE);
                DX.DrawGraph((int)scroll_x / 2+624, 0, Hndl_backScenery, DX.TRUE);


                // マップを描画
                Map_Drawing();

                //スプライト描画
                Sprite.Drawing();


                //UI描画
                UI_Update();


                Screen_Project();

                DX.ScreenFlip();// 裏画面の内容を表画面に反映する
            }


            //画面を拡大して表示
            void Screen_Project()
            {
                DX.SetDrawScreen(DX.DX_SCREEN_BACK);
                DX.DrawExtendGraph(0, 0, (int)(DRAW_WIDTH * draw_scale), (int)(DRAW_HEIGHT * draw_scale), Hndl_DrawScreen, DX.FALSE);


            }





            //マップ描画
            void Map_Drawing()
            {
                int xS = (int)((-scroll_x) / 16);
                for (int x = xS; x < xS+27; x++)
                {

                    for (int y = 0; y < 15; y++)
                    {//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ マップチップ描画
                        int x0 = x * 16, y0 = y * 16; //マップドット座標
                        int x1 = (int)scroll_x + x * 16, y1 = (int)(scroll_y) + y * 16; //スクリーンドット座標
                        string m = map_matrix[x, y];


                        //ハリボテ
                        if (m.Contains("茸1")) { Haribote_Set(x0, y0, Hndl_mash, 0, 0, 16, 16); map_matrix[x, y] = map_matrix[x, y].Replace("茸1", ""); }
                        if (m.Contains("茸2")) { Haribote_Set(x0, y0, Hndl_mash, 16, 0, 16, 16); map_matrix[x, y] = map_matrix[x, y].Replace("茸2", ""); }

                        if (m.Contains("茂1")) { Haribote_Set(x0, y0, Hndl_bush1, 0, 0, 48, 16); map_matrix[x, y] = map_matrix[x, y].Replace("茂1", ""); }
                        if (m.Contains("茂2")) { Haribote_Set(x0, y0 - 48, Hndl_bush2, 0, 0, 128, 64); map_matrix[x, y] = map_matrix[x, y].Replace("茂2", ""); }


                        //地面など
                        FormatMaptip_put(Hndl_groundBack, x, y, "B", "B");
                        FormatMaptip_put(Hndl_groundBack, x, y, "b", "b");
                        FormatMaptip_put(Hndl_ground, x, y, "g", "g");

                        //ブロックなど
                        if (m.Contains("角")) DX.DrawRectGraph(x1, y1, 16, 0, 16, 16, Hndl_blocks, 1);
                        if (m.Contains("岩")) DX.DrawRectGraph(x1, y1, 16, 16, 16, 16, Hndl_blocks, 1);




                        //リフト
                        for (int i=1; i < 6; i++) 
                        {
                            string c = "L" + i.ToString();
                            if (m.Contains(c))
                            {
                                map_matrix[x, y] = map_matrix[x, y].Replace(c, "");
                                Lift_Set(x0, y0, i, 0);
                            }
                            c = "R" + i.ToString();
                            if (m.Contains(c))
                            {
                                map_matrix[x, y] = map_matrix[x, y].Replace(c, "");
                                Lift_Set(x0, y0, i, 1);
                            }
                        }

                        //ゴール
                        if (m.Contains("完"))
                        {
                            map_matrix[x, y] = map_matrix[x, y].Replace("完", "");
                            Goal_Set(x0, y0);
                        }


                        //スライム
                        if (m.Contains("s"))
                        {
                            map_matrix[x, y] = map_matrix[x, y].Replace("s", "");
                            Slime_Set(x0, y0, 0);
                        }
                        if (m.Contains("S"))
                        {
                            map_matrix[x, y] = map_matrix[x, y].Replace("S", "");
                            Slime_Set(x0, y0, 1);
                        }


                    }//-------------------------------------------------------------------- マップチップ描画

                }

            }






            //map_matrix[x, y]に指定文字があるか
            bool map_check(int x, int y, params string[] c)
            {
                if (x < 0 || y < 0 || x >= map_width || y >= map_height) return false;
                for (int i = 0; i < c.Length; i++)
                {
                    if (map_matrix[x, y].Contains(c[i])) return true;
                }
                return false;
            }

            bool map_check2(int x, int y, params string[] c)
            {
                if (x < 0 || y < 0 || x >= map_width || y >= map_height) return true;
                for (int i = 0; i < c.Length; i++)
                {
                    if (map_matrix[x, y].Contains(c[i])) return true;
                }
                return false;
            }

            bool map_check3(int x, int y, params string[] c) //主にキャラ用
            {
                if (x < 0 || x >= map_width) return true;
                if (y < 0 || y >= map_height) return false;
                for (int i = 0; i < c.Length; i++)
                {
                    if (map_matrix[x, y].Contains(c[i])) return true;
                }
                return false;
            }


            //フォーマットマップチップを描画
            void FormatMaptip_put(int image48, int x, int y, string SubjectiveChar, params string[] ObjectiveString)
            {

                int x1 = (int)scroll_x, y1 = (int)(scroll_y);
                
                if (map_matrix[x, y].Contains(SubjectiveChar))
                {

                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 0, 8, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16 + 8, y1 + y * 16, 40, 0, 8, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16 + 8, 0, 40, 8, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16 + 8, y1 + y * 16 + 8, 40, 40, 8, 8, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 0, 8, 16, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16 + 8, y1 + y * 16, 40, 0, 8, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 32, 8, 16, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16 + 8, y1 + y * 16, 40, 32, 8, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 16, 8, 16, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16 + 8, y1 + y * 16, 40, 16, 8, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    // ++

                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 0, 16, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16 + 8, 0, 40, 16, 8, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 0, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 32, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    if (map_check2(x - 1, y, ObjectiveString) == false && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 0, 16, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    //++++

                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 32, 0, 16, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16 + 8, 32, 40, 16, 8, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 32, 0, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 32, 32, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == false &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 32, 16, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    // ++

                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 16, 0, 16, 8, image48, DX.TRUE, DX.FALSE);
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16 + 8, 16, 40, 16, 8, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == false && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 16, 0, 16, 16, image48, DX.TRUE, DX.FALSE);
                        //DX.DrawRectGraph(x1 + x * 16+8, y1 + y * 16, 32, 32, 8, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == false)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 16, 32, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }

                    if (map_check2(x - 1, y, ObjectiveString) == true && map_check2(x + 1, y, ObjectiveString) == true &&
                        map_check2(x, y - 1, ObjectiveString) == true && map_check2(x, y + 1, ObjectiveString) == true)
                    {
                        DX.DrawRectGraph(x1 + x * 16, y1 + y * 16, 16, 16, 16, 16, image48, DX.TRUE, DX.FALSE);
                        return;
                    }


                }
            }
        }

        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ class Properties
        class Properties
        {

            public class Character
            {
                public float x, y;
                public float vx, vy;
            }


            public class Player : Character
            {
                public int anim_count;
            }

            public class Chick : Character
            {
                public int anim_count;
                public int backNumber;
            }

            public class Slime : Character
            {
                public int anim_count;
                public float xBefore, yBefore;
                public float moveX;
                public int type = 0;
            }

            public class Lift
            {
                public float x, y;
                public float x0, y0;
                public int moveRange;
                public int mode;
                public int count;
            }
            public class Rock
            {
                public float x, y;
                public int anim_count = 0;
                public float fall_velocity;
            }

            public class Goal
            {
                public float x, y;
            }

        }
        //---------------------------------------- class Properties





    }




    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ class Sprote
    class SpriteCompornent
    {
        public bool used;
        public float X, Y;
        //public float Z;
        public int image;
        public int U, V;
        public int Width, Height;
        public int Attribution;
        public uint mask;
        public int link;

        public int colliderX, colliderY;
        public int colliderWidth, colliderHeight;



        public delegate void UpdateDelegate(int sp);
        public UpdateDelegate Update;

        //アニメ関連
        public static readonly int ANIM_MAX = 8;
        public List<long>[] AnimData;
        public bool[] AnimFrag;
        public bool[] AnimRelative;
        public int[] AnimStep;
        public int[] AnimCount;
        public bool[] AnimDelete;


        //内部変数
        public Dictionary<string, object> Dict;



        public SpriteCompornent()
        {
            used = false;
            X = 0;
            Y = 0;
            //Z = 0;
            image = -1;
            U = 0;
            V = 0;
            Width = 0;
            Height = 0;
            Attribution = 0;
            mask = 0;
            link = -1;

            colliderX = 0;colliderY = 0;
            colliderWidth = 0; colliderHeight = 0;

            AnimData = new List<long>[ANIM_MAX + 1]; //現状, アニメデータは最大8個まで追加予定
            AnimFrag = new bool[ANIM_MAX + 1];
            AnimRelative = new bool[ANIM_MAX + 1];
            AnimStep = new int[ANIM_MAX + 1];
            AnimCount = new int[ANIM_MAX + 1];
            AnimDelete = new bool[ANIM_MAX + 1];

            Dict = new Dictionary<string, object>();

            Update = delegate (int sp) { }; //デリゲート
        }

    }




    static class Sprite
    {
        public static readonly int SPRITE_MAX = 4096;

        public static SpriteCompornent[] sprite = new SpriteCompornent[SPRITE_MAX];
        public static Dictionary<int, short> sprite_Z = new Dictionary<int, short>();

        public static int NextNum = 0;

        public static int Attribution_reverse = 1 << 0;

        public static bool WasDisposed;

        public static int ThreadFPS = 60;


        //Spriteクラス初期化
        public static void Init()
        {
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                sprite[i] = new SpriteCompornent();
                sprite_Z.Add(i, 0);
            }

            WasDisposed = false;

            Thread thread1 = new Thread(new ThreadStart(Animation_Thread));
            thread1.Start();


            //アニメ1ステップ要素数
            Anim1step_Load();
        }


        //Spriteクラス終了処理
        public static void End()
        {
            WasDisposed = true;
        }



        public static int Set(int imageHndl, int u, int v, int width, int height)
        {
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                int n = (NextNum + i) % SPRITE_MAX;

                if (sprite[n].used == false)
                {
                    sprite[n] = new SpriteCompornent();
                    sprite_Z[n] = 0;

                    sprite[n].used = true;
                    sprite[n].image = imageHndl;
                    // imageHndlが-1で空スプライトの作成
                    sprite[n].U = u;
                    sprite[n].V = v;
                    sprite[n].Width = width;
                    sprite[n].Height = height;

                    sprite[n].colliderWidth = width;
                    sprite[n].colliderHeight = height;

                    NextNum = n + 1;
                    return n;
                }
            }

            return -1;
        }

        public static void Attribution(int n, int attribution)
        {
            sprite[n].Attribution = attribution;
        }

        public static void Image(int n, int imageHndl)
        {
            sprite[n].image = imageHndl;
        }

        public static void Image(int n, int U, int V)
        {
            sprite[n].U = U;
            sprite[n].V = V;
        }

        public static void Image(int n, int U, int V, int Width, int Heigth)
        {
            sprite[n].U = U;
            sprite[n].V = V;
            sprite[n].Width = Width;
            sprite[n].Height = Heigth;
        }

        public static void Image(int n, int imageHndl, int U, int V, int Width, int Heigth)
        {
            sprite[n].image = imageHndl;
            sprite[n].U = U;
            sprite[n].V = V;
            sprite[n].Width = Width;
            sprite[n].Height = Heigth;
            sprite[n].AnimDelete[AnimType_UV] = true;
        }


        public static void Offset(int n, float x, float y)
        {
            sprite[n].X = x;
            sprite[n].Y = y;
            sprite[n].AnimDelete[AnimType_XY] = true;
        }

        public static void Offset(int n, float x, float y, short z)
        {
            sprite[n].X = x;
            sprite[n].Y = y;
            sprite_Z[n] = z;
            sprite[n].AnimDelete[AnimType_XY] = true;
        }


        public static object Var(int n, string key)
        {
            return sprite[n].Dict[key];
        }


        public static void Link(int sp, int link)
        {
            sprite[sp].link = link;
            return;
        }

        public static float LinkDifference_X(int sp)
        {
            if (sprite[sp].link != -1)
            {
                int spL = sprite[sp].link;
                return sprite[spL].X + LinkDifference_X(spL);
            }
            else
            {
                return 0;
            }
        }
        public static float LinkDifference_Y(int sp)
        {
            if (sprite[sp].link != -1)
            {
                int spL = sprite[sp].link;
                return sprite[spL].Y + LinkDifference_Y(spL);
            }
            else
            {
                return 0;
            }
        }




        //使用済処理
        public static void Clear(int n)
        {
            sprite[n].used = false;
            NextNum = n + 1;
        }
        public static void Clear()
        {
            for (int n = 0; n < SPRITE_MAX; n++)
            {
                sprite[n].used = false;
                NextNum = n + 1;
            }
        }



        //^^^^^^^ アニメ情報
        //アニメ情報を登録する際はここに加えてAnimUpdate系統の追加も必要
        //そして、この下のAnimメソッドのswitch構文にも処理を追加
        //さらに, アニメでない通常処理を行ってパラメーターを変更した際, 自動でアニメのdeleteフラグをtrueにする必要がある
        public const int AnimType_XY = 1;
        public const int AnimType_UV = 2;

        private static void Anim1step_Load()
        {
            Anim1step[AnimType_XY] = 3;
            Anim1step[AnimType_UV] = 3;
        }
        //-------



        public static readonly int[] Anim1step = new int[SpriteCompornent.ANIM_MAX + 1]; //アニメの1ステップ要素数


        public static void Anim(int n, int Type, params int[] data)
        {
            if (Type < 0)
            {
                Type *= -1;
                sprite[n].AnimRelative[Type] = true;
            }
            else
            {
                sprite[n].AnimRelative[Type] = false;
            }
            sprite[n].AnimFrag[Type] = true;
            sprite[n].AnimDelete[Type] = false;


            //初期情報登録

            sprite[n].AnimData[Type] = new List<long>(0);

            switch (Type)
            {
                case AnimType_XY:
                    sprite[n].AnimData[Type].Add(0);
                    sprite[n].AnimData[Type].Add(sprite[n].AnimRelative[Type] ? 0 : (long)sprite[n].X);
                    sprite[n].AnimData[Type].Add(sprite[n].AnimRelative[Type] ? 0 : (long)sprite[n].Y);
                    break;
                case AnimType_UV:
                    sprite[n].AnimData[Type].Add(0);
                    sprite[n].AnimData[Type].Add(sprite[n].AnimRelative[Type] ? 0 : (long)sprite[n].U);
                    sprite[n].AnimData[Type].Add(sprite[n].AnimRelative[Type] ? 0 : (long)sprite[n].V);
                    break;
            }



            //アニメデータ引数を登録
            for (int i = 0; i < data.Length; i++)
            {
                sprite[n].AnimData[Type].Add(data[i]);
            }



            //回数が省略されていれば自動で1にする
            if (data.Length % Anim1step[Type] == 0) sprite[n].AnimData[Type].Add(1);


        }

        //アニメ動作中か調べる
        public static bool AnimCheck(int n)
        {
            for (int i = 1; i < SpriteCompornent.ANIM_MAX; i++)
            {
                if (sprite[n].AnimFrag[i]) return true;
            }
            return false;
        }




        public static void Collider(int n, int x, int y, int width, int height)
        {
            sprite[n].colliderX = x; sprite[n].colliderY = y;
            sprite[n].colliderWidth = width; sprite[n].colliderHeight = height;
        }

        public static void Collider(int n, int x, int y, int width, int height, uint mask)
        {
            sprite[n].colliderX = x; sprite[n].colliderY = y;
            sprite[n].colliderWidth = width; sprite[n].colliderHeight = height;
            sprite[n].mask = mask;
        }




        
        //スプライト当たり判定
        public static int HitSprite(int sp)
        {
            int x = (int)(sprite[sp].X + LinkDifference_X(sp)) + sprite[sp].colliderX, 
                y = (int)(sprite[sp].Y + LinkDifference_Y(sp)) + sprite[sp].colliderY;
            int width = sprite[sp].colliderWidth, height = sprite[sp].colliderHeight;
            uint mask = sprite[sp].mask;
            
            Rectangle r1 = new Rectangle(x, y, width, height);
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                if (i == sp) continue;
                if (sprite[i].used && (sprite[i].mask & mask) != 0)
                {
                    Rectangle r2 = new Rectangle(
                        (int)(sprite[i].X + LinkDifference_X(i)) + sprite[i].colliderX, 
                        (int)(sprite[i].Y + LinkDifference_Y(i)) + sprite[i].colliderY, 
                        sprite[i].colliderWidth, sprite[i].colliderHeight);

                    if (r1.IntersectsWith(r2))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        //マスクだけ変更
        public static int HitSprite(int sp, uint maskReplace)
        {
            int x = (int)(sprite[sp].X + LinkDifference_X(sp)) + sprite[sp].colliderX,
                y = (int)(sprite[sp].Y + LinkDifference_Y(sp)) + sprite[sp].colliderY;
            int width = sprite[sp].colliderWidth, height = sprite[sp].colliderHeight;
            uint mask = maskReplace;

            Rectangle r1 = new Rectangle(x, y, width, height);
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                if (i == sp) continue;
                if (sprite[i].used && (sprite[i].mask & mask) != 0)
                {
                    Rectangle r2 = new Rectangle(
                        (int)(sprite[i].X + LinkDifference_X(i)) + sprite[i].colliderX,
                        (int)(sprite[i].Y + LinkDifference_Y(i)) + sprite[i].colliderY,
                        sprite[i].colliderWidth, sprite[i].colliderHeight);

                    if (r1.IntersectsWith(r2))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }


        public static int HitRectangle(int x, int y, int width, int height, uint mask)
        {
            Rectangle r1 = new Rectangle(x, y, width, height);
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                if (sprite[i].used && (sprite[i].mask & mask) != 0)
                {
                    Rectangle r2 = new Rectangle(
                        (int)(sprite[i].X + LinkDifference_X(i)) + sprite[i].colliderX,
                        (int)(sprite[i].Y + LinkDifference_Y(i)) + sprite[i].colliderY,
                        sprite[i].colliderWidth, sprite[i].colliderHeight);

                    if (r1.IntersectsWith(r2)) return i;
                }
            }

            return -1;
        }

        public static int HitRectangle(int min, int max, int x, int y, int width, int height, uint mask)
        {
            Rectangle r1 = new Rectangle(x, y, width, height);
            for (int i = min; i < max; i++)
            {
                if (sprite[i].used && (sprite[i].mask & mask) != 0)
                {
                    Rectangle r2 = new Rectangle(
                        (int)(sprite[i].X + LinkDifference_X(i)) + sprite[i].colliderX,
                        (int)(sprite[i].Y + LinkDifference_Y(i)) + sprite[i].colliderY,
                        sprite[i].colliderWidth, sprite[i].colliderHeight);

                    if (r1.IntersectsWith(r2)) return i;
                }
            }

            return -1;
        }







        //スプライト一括更新処理
        public static void AllUpdate()
        {
            for (int i = 0; i < SPRITE_MAX; i++)
            {
                if (sprite[i].used) sprite[i].Update(i);
            }
        }






        //スプライト一括描画処理
        public static void Drawing()
        {
            var draws = sprite_Z.OrderByDescending((x) => x.Value);

            //for (int i=0; i<SPRITE_MAX; i++)
            foreach (var v in draws)
            {
                int i = v.Key;

                if (sprite[i].used)
                {
                    if (sprite[i].image == -1) continue;

                    int x, y;
                    if (sprite[i].link != -1)//リンクしてる
                    {
                        x = (int)(sprite[i].X + LinkDifference_X(i)); y = (int)(sprite[i].Y + LinkDifference_Y(i));
                    }
                    else //リンクしていない
                    { 
                        x = (int)sprite[i].X; y = (int)sprite[i].Y;
                    }
                    DX.DrawRectGraph(x, y, sprite[i].U, sprite[i].V,
                        sprite[i].Width, sprite[i].Height, sprite[i].image,
                        DX.TRUE, sprite[i].Attribution & Attribution_reverse);
                }
            }
        }



        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ アニメーションスレッド (完全に指定FPS通りに処理はしていない)
        private static void Animation_Thread()
        {
            int frame = 0;
            int before = Environment.TickCount;


            while (!WasDisposed)
            {
                int now = Environment.TickCount;
                int progress = now - before;

                int ideal = (int)(frame * 1000.0F / ThreadFPS);

                //^^^^^^^^^^ 処理
                for (int i = 0; i < SPRITE_MAX; i++)
                {
                    if (sprite[i].used)
                    {
                        for (int j = 0 + 1; j < SpriteCompornent.ANIM_MAX + 1; j++)
                        {
                            if (sprite[i].AnimFrag[j])
                            {
                                if (sprite[i].AnimDelete[j]) //アニメデータ強制削除
                                {
                                    sprite[i].AnimData[j] = new List<long>(0);
                                    sprite[i].AnimFrag[j] = false;
                                    continue;
                                }

                                AnimUpdateBase(i, j); //iが管理番号, jがアニメタイプ
                            }
                        }
                    }
                }
                //---------

                if (ideal > progress) Thread.Sleep(ideal - progress);

                frame++;
                if (progress >= 1000) //1sごとに更新
                {
                    before = now;
                    frame = 0;
                }
            }

            //^^^^^^^^^^ アニメ更新処理
            void AnimUpdateBase(int n, int type)
            {
                bool smooth = false;
                int count = sprite[n].AnimCount[type];
                int step = sprite[n].AnimStep[type];

                if (count >= Math.Abs((int)sprite[n].AnimData[type][step * Anim1step[type]])) //次のコマへ移行
                {
                    step++;
                    count = 0;
                    int datalen = sprite[n].AnimData[type].Count;
                    int stepmax = (datalen - 1) / Anim1step[type] - 1;

                    if (step > stepmax) //1ループ終了

                    {
                        if (sprite[n].AnimData[type][datalen - 1] > 0) //ループ数が有限なら
                        {
                            sprite[n].AnimData[type][datalen - 1]--;
                            if (sprite[n].AnimData[type][datalen - 1] == 0) //ループ終了したらアニメデータを削除
                            {
                                sprite[n].AnimData[type] = new List<long>(0);
                                sprite[n].AnimFrag[type] = false;
                                return;
                            }
                        }
                        step = 0;

                    }
                }
                else
                {
                    count++;
                }

                int nextc = (int)sprite[n].AnimData[type][step * Anim1step[type]];
                if (nextc < 0) //スムーズアニメ
                {
                    nextc *= -1;
                    smooth = true;
                }

                //Console.WriteLine(step);
                switch (type)
                {
                    case AnimType_XY:
                        AnimUpdate_XY(); break;
                    case AnimType_UV:
                        AnimUpdate_UV(); break;

                }



                sprite[n].AnimCount[type] = count;
                sprite[n].AnimStep[type] = step;
                //ここでAnimUpdateBaseの処理は終了



                //^^^^^^^^^ 各々アニメ型に応じて場合分け
                //メソッドを追加したら上のswitch構文でちゃんと飛んでいけるようにすること
                void AnimUpdate_XY()
                {
                    int rx = 0, ry = 0;
                    if (sprite[n].AnimRelative[type])
                    {
                        rx = (int)sprite[n].AnimData[type][1];
                        ry = (int)sprite[n].AnimData[type][2];

                    }


                    int x2 = (int)sprite[n].AnimData[type][step * 3 + 1] + rx;
                    int y2 = (int)sprite[n].AnimData[type][step * 3 + 2] + ry;

                    if (smooth) //スムーズアニメなら
                    {
                        int backstep = step - 1;
                        if (step == 0) backstep = 0;

                        int x1 = (int)sprite[n].AnimData[type][backstep * 3 + 1] + rx;
                        int y1 = (int)sprite[n].AnimData[type][backstep * 3 + 2] + ry;

                        sprite[n].X = x1 + (float)((x2 - x1) * ((double)count / nextc));
                        //Console.WriteLine(nextc);
                        sprite[n].Y = y1 + (float)((y2 - y1) * ((double)count / nextc));
                    }
                    else //ラフアニメなら
                    {
                        if (count == 0)
                        {
                            sprite[n].X = x2;
                            sprite[n].Y = y2;
                        }
                    }
                }

                void AnimUpdate_UV()
                {
                    int ru = 0, rv = 0;
                    if (sprite[n].AnimRelative[type])
                    {
                        ru = (int)sprite[n].AnimData[type][1];
                        rv = (int)sprite[n].AnimData[type][2];

                    }

                    //Console.WriteLine(step);
                    int u2 = (int)sprite[n].AnimData[type][step * 3 + 1] + ru;
                    int v2 = (int)sprite[n].AnimData[type][step * 3 + 2] + rv;

                    if (smooth) //スムーズアニメなら
                    {
                        int backstep = step - 1;
                        if (step == 0) backstep = 0;

                        int u1 = (int)sprite[n].AnimData[type][backstep * 3 + 1] + ru;
                        int v1 = (int)sprite[n].AnimData[type][backstep * 3 + 2] + rv;

                        sprite[n].U = u1 + (int)((u2 - u1) * ((double)count / nextc));
                        sprite[n].V = v1 + (int)((v2 - v1) * ((double)count / nextc));
                    }
                    else //ラフアニメなら
                    {
                        if (count == 0)
                        {
                            sprite[n].U = u2;
                            sprite[n].V = v2;
                        }
                    }
                }

                //-----------

            }

        }




        // ------------------------------------------ アニメーションスレッド

    }

    //-------------------------------------------- class Sprite

    //お役立ちクラス
    static class Useful
    {
        //2値の間にあるかどうか
        public static bool between(double a, double min, double max)
        {
            if (min <= a && a <= max)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //スプライトを放物運動アニメさせる
        public static void Sprite_ParabolaAnim(int sp, int x1, int y1, int x2, int y2, int interval, double curv)
        {
            int w = x2 - x1, h = y2 - y1;

            int[] AnimData = new int[16 * 3];

            for (int i = 0; i < 16; i++)
            {
                AnimData[i * 3 + 0] = -interval;
                AnimData[i * 3 + 1] = x1 + (int)(w * (double)(i + 1) / 16);
                AnimData[i * 3 + 2] = y1 + (int)(h * (double)(i + 1) / 16 + curv * ((i + 1 - 8) * (i + 1 - 8) - 64));
            }

            Sprite.Anim(sp, Sprite.AnimType_XY, AnimData);
        }

        //簡易破壊エフェクト
        public static void Clash_effect(int imageHndl, int u, int v, int width, int height, int x0, int y0, int spLink)
        {
            int sp;
            int w = width; int h = height;

            for (int i = 0; i < 4; i++)
            {
                int u1 = (i % 2) * (w / 2);
                int v1 = (int)(i / 2) * (w / 2);

                sp = Sprite.Set(imageHndl, u + u1, v + v1, w / 2, h / 2);
                if (spLink != -1) Sprite.Link(sp, spLink);
                Sprite.Offset(sp, x0 + u1, y0 + v1, -512);

                int x1 = x0 + u1 + (-w / 4 + u1) * 3;
                int y1 = y0 + v1 + (-h / 4 + v1) * 3 + h;
                Sprite_ParabolaAnim(sp, x0 + u1, y0 + v1, x1, y1, 2, 0.4);

                Sprite.sprite[sp].Update += new SpriteCompornent.UpdateDelegate(Sprite_Effeectfade); //削除処理追加
            }

        }




        public static void Sprite_Effeectfade(int sp)
        {
            if (!Sprite.AnimCheck(sp)) Sprite.Clear(sp);
        }

        public static void Sprite_EffeectfadeXY(int sp)
        {
            if (!Sprite.sprite[sp].AnimFrag[Sprite.AnimType_XY]) Sprite.Clear(sp);
        }



        public static void DrawString_shadow(int x, int y, string s)
        {
            DX.DrawString(x, y + 1, s, DX.GetColor(32, 32, 32));
            DX.DrawString(x, y, s, DX.GetColor(255, 255, 255));
        }

        public static void DrawString_bordered(int x, int y, string s)
        {
            uint c1 = DX.GetColor(255, 255, 255);
            uint c2 = DX.GetColor(32, 32, 32);
            DX.ChangeFontType(DX.DX_FONTTYPE_EDGE);
            DX.DrawString(x, y, s, c1, c2);
        }
        public static void DrawString_bordered(int x, int y, string s, uint c1)
        {
            uint c2 = DX.GetColor(32, 32, 32);
            DX.ChangeFontType(DX.DX_FONTTYPE_EDGE);
            DX.DrawString(x, y, s, c1, c2);
        }

        public static void DrawString_bordered(int x, int y, string s, uint c1, uint c2)
        {
            DX.ChangeFontType(DX.DX_FONTTYPE_EDGE);
            DX.DrawString(x, y, s, c1, c2);
        }





        //接触スプライトの中で一番手前にあるスプライトを返す
        public static int Sprite_HitRectangle_front(int x, int y, int width, int height, uint mask)
        {
            int ret = -1, next = 0;

            int h = 0;
            while (h != -1 && next < Sprite.SPRITE_MAX)
            {
                h = Sprite.HitRectangle(next, Sprite.SPRITE_MAX, x, y, width, height, mask);
                if (h != -1)
                {
                    next = h + 1;

                    if (ret == -1)
                    {
                        ret = h;
                        continue;
                    }

                    if (Sprite.sprite_Z[ret] > Sprite.sprite_Z[h]) ret = h;
                }
            }


            return ret;
        }


        //接触スプライトを全て返す
        public static void Sprite_HitRectangle_List(int x, int y, int width, int height, uint mask, out List<int> list)
        {
            list = new List<int>(0);
            int  next = 0;
            int h = 0;
            while (h != -1 && next < Sprite.SPRITE_MAX)
            {
                h = Sprite.HitRectangle(next, Sprite.SPRITE_MAX, x, y, width, height, mask);
                if (h != -1)
                {
                    next = h + 1;

                    list.Add(h);
                }
            }

            return;
        }



        //ユークリッド距離を返す
        public static double distance(double x, double y)
        {
            return Math.Sqrt(x * x + y + y);
        }










        public static void Wait(int frame)
        {
            int i = 0;
            DX.ScreenFlip();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);
            DX.DrawGraph(0, 0, DX.DX_SCREEN_FRONT, 0);

            while (DX.ProcessMessage() != -1)
            {
                DX.ScreenFlip();
                i++;
                if (i >= frame) return;
            }
        }



        /*
        public static void SpriteDict_Read(int sp, params object[] args)
        {
            for (int i=0; i<args.Length/2; i++)
            {
                
            }
        }
        */


        public static string zero_padding(string s, int order)
        {
            int o1 = s.Length;
            if (o1 < order)
            {
                for (int i = 0; i<order-o1; i++)
                {
                    s = "0" + s;
                }
            }
            return s;
        }

        public static string count_ToTime(int c)
        {
            int min, sec, dec;
            min = c / 3600;
            sec = (c % 3600) / 60;
            dec = (c % 60)*100 / 60;

            return $"{zero_padding(min.ToString(), 2)}\'{zero_padding(sec.ToString(), 2)}\'{zero_padding(dec.ToString(), 2)}";
        }


    }


    //入力クラス
    static class Input
    {

        public static uint ClickLeft_time;

        public static int button_repeatStart = 15;
        public static int button_repeat = 6;

        public const int BUTTON_STAY = 0, BUTTON_TRIGER = 1, BUTTON_REPEAT = 2;



        public static void Start()
        {
            ClickLeft_time = 0;
            Button.push_left = 0;
            Button.push_right = 0;
            Button.push_up = 0;
            Button.push_down = 0;
            Button.push_A = 0;
        }

        public static void Update()
        {
            if ((DX.GetMouseInput() & DX.MOUSE_INPUT_LEFT) != 0)
            {
                ClickLeft_time += 1;
            }
            else
            {
                ClickLeft_time = 0;
            }

            if (Button.Left()) Button.push_left++; else Button.push_left = 0;
            if (Button.Right()) Button.push_right++; else Button.push_right = 0;
            if (Button.Up()) Button.push_up++; else Button.push_up = 0;
            if (Button.Down()) Button.push_down++; else Button.push_down = 0;
            if (Button.A()) Button.push_A++; else Button.push_A = 0;
        }


        //ボタン入力
        public class Button
        {

            public static int push_left = 0;
            public static int push_right = 0;
            public static int push_up = 0;
            public static int push_down = 0;
            public static int push_A = 0;

            public static bool Left()
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_A) == DX.TRUE || DX.CheckHitKey(DX.KEY_INPUT_LEFT) == DX.TRUE) return true;
                return false;
            }
            public static bool Left(int type)
            {
                return button_check(type, push_left);
            }
            public static bool Right()
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_D) == DX.TRUE || DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == DX.TRUE) return true;
                return false;
            }
            public static bool Right(int type)
            {
                return button_check(type, push_right);
            }
            public static bool Up()
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_W) == DX.TRUE || DX.CheckHitKey(DX.KEY_INPUT_UP) == DX.TRUE) return true;
                return false;
            }
            public static bool Up(int type)
            {
                return button_check(type, push_up);
            }

            public static bool Down()
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_S) == DX.TRUE || DX.CheckHitKey(DX.KEY_INPUT_DOWN) == DX.TRUE) return true;
                return false;
            }
            public static bool Down(int type)
            {
                return button_check(type, push_down);
            }
            public static bool A()
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_RETURN) == DX.TRUE) return true;
                return false;
            }
            public static bool A(int type)
            {
                return button_check(type, push_A);
            }




            private static bool button_check(int type, int time)
            {
                switch (type)
                {
                    case BUTTON_STAY:
                        if (time > 0) return true; else return false;
                    case BUTTON_TRIGER:
                        if (time == 1) return true; else return false;
                    case BUTTON_REPEAT:
                        if (time == 1 || (time > button_repeatStart && ((time - button_repeatStart) % button_repeat) == 0)) return true; else return false;
                }
                return false;

            }
        }
    }

}




