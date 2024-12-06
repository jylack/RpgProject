using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2024Project
{
    struct Point
    {
        public int x;
        public int y;

        public void SetPosition(int num1, int num2)
        {
            x = num1;
            y = num2;
        }
    }
    enum MapsName
    {
        World, Dungeon, Village,
        Shop, Inn
    }

    //시작과 끝만 내뚜고 다 쓸지도? Inn은 여관
    enum TileStyle
    {
        start = 0,
        //제일 바깥 타일들
        Village, Dungeon, Field, Mountain, Sea,
        //마을 내부 타일들
        Shop, Inn, Wall, Road,
        //동작? 타일들
        Monster, RandomBox, Player, NPC,
        //상세메뉴 타일들
        Shop_menu, Inn_menu, Battle_menu,
        //작동
        Eat, Rest, Gate,
        //던전 내부 타일들       
        end
    }

    struct Tile
    {
        public string Image;
        public Point point;
        public TileStyle style;
        public int index;
        //쓸줄알았는데 아직안씀.
        //public ConsoleColor color;
    }

    //멥별 테두리 박스설정
    struct RectCase
    {
        public int[,] backGroundIndex;      //각 배열 인덱스값
        public string[] backGround;         //인덱스별로 가지고있을 특수문자.
        public int width;                   //각 박스별 너비
        public int height;                  //각 박스별 높이
        public int value;                   //value x value 제작할 사이즈 조절용        
    }

    internal class Program
    {


        static void Main(string[] args)
        {
            //이모지 사용 가능하게 해주는 코드
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //0번 월드 박스
            //1번 던전 박스
            //2번 마을 박스
            //3번 샵 박스
            //4번 여관 박스
            RectCase[] rect = new RectCase[5];


            int mapIndex = 0;
            //int backMapIndex = 0;

            //0번 월드 박스
            //value x value 사이즈 멥크기 생성하기위한 값
            rect[0].value = 7;
            //칸마다 너비조절.
            rect[0].width = 10;
            //칸마다 높이조절
            rect[0].height = 3;
            //박스생성
            //RectCreate(rect[0]);

            //1번 던전박스
            rect[1].value = 7;
            rect[1].width = 10;
            rect[1].height = 3;

            //2번 마을 박스
            rect[2].value = 4;
            rect[2].width = 10;
            rect[2].height = 3;

            //3번 샵 박스
            rect[3].value = 2;
            rect[3].width = 10;
            rect[3].height = 3;

            //4번 여관 박스
            rect[4].value = 2;
            rect[4].width = 10;
            rect[4].height = 3;


            //전체멥 정보
            List<Tile[,]> Maps = new List<Tile[,]>();

            //플레이어에게 보여줄 멥들
            Tile[,] worldTile = new Tile[rect[0].value, rect[0].value];
            Tile[,] dugeonTile = new Tile[rect[1].value, rect[1].value];
            Tile[,] villageTile = new Tile[rect[2].value, rect[2].value];
            Tile[,] shopTile = new Tile[rect[3].value, rect[3].value];
            Tile[,] innTile = new Tile[rect[4].value, rect[4].value];

            //플레이어에게 보여줄 멥들 전체멥에 추가
            Maps.Add(worldTile);
            Maps.Add(dugeonTile);
            Maps.Add(villageTile);
            Maps.Add(shopTile);
            Maps.Add(innTile);


            //뭔가 값을 안넣어서 터지고있음
            //createMap 함수 세번째 인자는 한칸씩 올라가서 순서대로 세팅을 해줘야하는데 mapIndex 를 넣어줘서 같은그림을 계속 그렸었음..ㅜㅜ
            //멥정보들 기본세팅

            for (int i = 0; i < Maps.Count; i++)
            {
                //처음에 리스트 안에 안들어가길래 temp로 임시저장후 넣었다가 생각해보니 그냥 반환값으로 가져오면 쉬웠음.
                //Tile[,] temp = Maps[i];
                //멥정보 생성.
                Maps[i] = CreateMap(Maps[i], rect[i], i);
                //Maps[i] = temp;

                //테두리 생성
                RectCreate(ref rect[i]);

                //이거없으면 멥이 너무 작을경우 이미지값이 겹침.
                //random 함수떄문인듯.
                Thread.Sleep(1);
            }

            /*좌표 확인코드
            //
            //for (int i = 0; i < tile.GetLength(0); i++)
            //{
            //    for (int j = 0; j < tile.GetLength(1); j++)
            //    {
            //        //tile[i, j].point.x + tile[i, j].point.y
            //        Console.Write($"{tile[i, j].point.x} {tile[i, j].point.y}\t");                    
            //    }
            //    Console.WriteLine();
            //}
            */

            //플레이어 타일 세팅
            Tile player = new Tile();
            PlayerCreate(ref player);
            Point backPoint = new Point();
            backPoint.SetPosition(0, 0);

            while (true)
            {
                Console.Clear();

                //백그라운드 만들어줌 - 테두리 선만 말하는것.
                //RectCreate(rect[mapIndex]);
                RectDrawing(ref rect[mapIndex]);

                /* 테스트용 코드
                //Console.WriteLine("🎪🧱🧱🏔️⛰️🌋🗻🛤️🏕️🏞️🛣️🏖️🏚️🏠🏡🏘️🏨🏪🏬🏢♨️😀😁😄▥♤♧◎⊙▣◈▒▤▦▩〓────━⊙🛺🧙🧚🧛🦄🧝‍♂️🧟🧖");

                //백그라운드 위에 월드 타일 이미지 올림.
                //for (int i = 0; i < worldTile.GetLength(0); i++)
                //{
                //    for (int j = 0; j < worldTile.GetLength(1); j++)
                //    {
                //        Console.SetCursorPosition(worldTile[i, j].point.x, worldTile[i, j].point.y);
                //        Console.Write(worldTile[i, j].Image);
                //    }
                //}
                */

                //백그라운드 위에 지정된 멥 타일 이미지 올림.
                for (int i = 0; i < Maps[mapIndex].GetLength(0); i++)
                {
                    for (int j = 0; j < Maps[mapIndex].GetLength(1); j++)
                    {
                        Console.SetCursorPosition(Maps[mapIndex][i, j].point.x, Maps[mapIndex][i, j].point.y);
                        Console.Write(Maps[mapIndex][i, j].Image);

                    }
                }

                //플레이어 그려주기
                Console.SetCursorPosition(player.point.x, player.point.y);
                Console.WriteLine(player.Image);

                //플레이어 현재 어디쯤있나 인덱스 찍어줌
                Console.SetCursorPosition(rect[mapIndex].width * rect[mapIndex].value + 1,
                                            rect[mapIndex].height * rect[mapIndex].value + 1);

                //현재 위치 멥 몇번 테스트용
                Console.WriteLine($"현재 인덱스 : [{player.index}]번 ");

                //현재 멥 뭔지 테스트용
                Console.SetCursorPosition(rect[mapIndex].width * rect[mapIndex].value + 1,
                                            rect[mapIndex].height * rect[mapIndex].value + 3);
                //멥 인덱스 호출용 0~5번 맨 위에 리스트에 넣은 순서대로임.
                Console.WriteLine((MapsName)mapIndex);


                //현재 그리드 좌표 알수있나 테스트
                Point point = new Point();
                //플레이어가 있는 인덱스값을 받아서 멥의 몇번째 배열칸인지 알아내는 코드             
                point = TileIndexSerch(Maps[mapIndex], player.index);

                Console.SetCursorPosition(rect[mapIndex].width * rect[mapIndex].value + 1,
                rect[mapIndex].height * rect[mapIndex].value + 5);



                Console.WriteLine($"jx = {point.x} ,iy = {point.y} " +
                    $"현재 있는 멥타일 이름 {Maps[mapIndex][point.y, point.x].style}");



                Console.WriteLine($"jx = {backPoint.x} ,iy = {backPoint.y} ");



                //키입력
                ConsoleKeyInfo myKey = Console.ReadKey(true);

                //index 값        rect 행열값. 
                //01 02 03 04     00 01 02 03
                //05 06 07 08     10 11 12 13
                //09 10 11 12     20 21 22 23
                //13 14 15 16     30 31 32 33

                TileStyle style = new TileStyle();


                //입력받은 키로 좌표이동
                switch (myKey.Key)
                {
                    /*멥이동 테스트
                    ////
                    //case ConsoleKey.Spacebar:
                    //    //멥 인덱스 최대치 못넘게 함.
                    //    if (mapIndex + 1 < Maps.Count)
                    //    {
                    //        mapIndex++;
                    //        player.point.SetPosition(1, 1);
                    //        player.index = 1;

                    //        //아래 주석들은 테스트용으로 썼었음
                    //        //Console.Clear();
                    //        //RectDrawing(ref rect[mapIndex]);
                    //    }
                    //    else
                    //    {
                    //        mapIndex = 0;
                    //        player.point.SetPosition(1, 1);
                    //        player.index = 1;
                    //    }
                    //    break;
                    */

                    case ConsoleKey.W:

                        if (player.point.y - rect[mapIndex].height > 0)
                        {
                            //TileStyle style = new TileStyle();
                            //가독성을 위해 임시로 현재 플레이어가 있는 위치 바로 위 타일의 타일스타일 넣어줌.
                            style = Maps[mapIndex][point.y - 1, point.x].style;
                            //Console.WriteLine(style);
                            //backMapIndex = mapIndex;
                            bool isEvent = EventTileSerch(ref mapIndex, ref player, ref backPoint, style);

                            if (style != TileStyle.Wall)
                            {
                                //벽이 아니고,이벤트타일일때.

                                //player.point.SetPosition(1, 1);
                                //player.index = 1;
                                if (isEvent)
                                {
                                    //player.point.SetPosition(1, 1);
                                    player.index = 1;
                                }
                                player.point.y -= rect[mapIndex].height;
                                player.index -= rect[mapIndex].value;

                            }


                            //if (style != TileStyle.Wall)
                            //{
                            //    //Player.point 의 포인터는 배열의 ij값이 아니라 콘솔창의 콘솔좌표값이다.
                            //    //이거 햇갈려서 해결에 오래걸림..
                            //    player.point.y -= rect[mapIndex].height;
                            //    player.index -= rect[mapIndex].value;
                            //}


                        }
                        break;

                    case ConsoleKey.S:
                        if (player.point.y + rect[mapIndex].height < rect[mapIndex].value * rect[mapIndex].height)
                        {

                            //TileStyle style = new TileStyle();
                            //가독성을 위해 임시로 현재 플레이어가 있는 위치 바로 위 타일의 타일스타일 넣어줌.
                            style = Maps[mapIndex][point.y + 1, point.x].style;
                            bool isEvent = EventTileSerch(ref mapIndex, ref player, ref backPoint, style);

                            if (style != TileStyle.Wall)
                            {
                                if (isEvent)
                                {
                                    player.point.SetPosition(1, 1);
                                    player.index = 1;
                                }

                                player.point.y += rect[mapIndex].height;
                                player.index += rect[mapIndex].value;




                            }

                            //if (style != TileStyle.Wall)
                            //{


                            //}
                        }

                        break;

                    case ConsoleKey.A:
                        if (player.point.x - rect[mapIndex].width > 0)
                        {
                            //TileStyle style = new TileStyle();
                            //가독성을 위해 임시로 현재 플레이어가 있는 위치 바로 위 타일의 타일스타일 넣어줌.
                            style = Maps[mapIndex][point.y, point.x - 1].style;

                            bool isEvent = EventTileSerch(ref mapIndex, ref player, ref backPoint, style);

                            if (style != TileStyle.Wall)
                            {
                                if (isEvent)
                                {
                                    player.point.SetPosition(1, 1);
                                    player.index = 1;
                                }


                                player.point.x -= rect[mapIndex].width;
                                player.index -= 1;

                            }

                        }

                        break;

                    case ConsoleKey.D:

                        if (player.point.x + rect[mapIndex].width < rect[mapIndex].value * rect[mapIndex].width)
                        {

                            //TileStyle style = new TileStyle();
                            //가독성을 위해 임시로 현재 플레이어가 있는 위치 바로 위 타일의 타일스타일 넣어줌.
                            style = Maps[mapIndex][point.y, point.x + 1].style;
                            bool isEvent = EventTileSerch(ref mapIndex, ref player, ref backPoint, style);

                            if (style != TileStyle.Wall)
                            {
                                if (isEvent)
                                {
                                    player.point.SetPosition(1, 1);
                                    player.index = 1;
                                }


                                player.point.x += rect[mapIndex].width;
                                player.index += 1;


                            }

                        }
                        break;
                }



            }

        }

        //이벤트 타일 인지 아닌지 체크 후 이동할수 있는 타일이면 이동.
        static bool EventTileSerch(ref int index, ref Tile ply, ref Point backPoint, TileStyle moveStyle)
        {
            //index = mapindex =즉 멥이 지금 몇번쨰 던전이냐 월드냐 상점이냐 판별

            //이벤트 타일일 경우 트루값
            bool result = true;
            //이전 멥인덱스 받아서 움직이고 싶었는데...잘모르겠음;
            //backindex = index;
            Random rnd = new Random();
            int rndNum;


            //Console.SetCursorPosition(30, 30);
            Console.WriteLine(moveStyle);



            //이동한 타일이 이벤트 타일인지 확인한뒤 진행
            switch (moveStyle)
            {
                case TileStyle.Wall:

                    break;

                case TileStyle.Village:
                    index = (int)MapsName.Village;
                    //backPoint = ply.point;
                    //ply.point.SetPosition(1, 1);
                    //ply.index = 1;
                    break;

                case TileStyle.Dungeon:
                    index = (int)MapsName.Dungeon;
                    //backPoint = ply.point;

                    //ply.point.SetPosition(1, 1);
                    //ply.index = 1;
                    break;

                case TileStyle.Shop:
                    index = (int)MapsName.Shop;
                    //backPoint = ply.point;

                    //ply.point.SetPosition(1, 1);
                    //ply.index = 1;
                    break;

                case TileStyle.Inn:
                    index = (int)MapsName.Inn;
                    //backPoint = ply.point;

                    //ply.point.SetPosition(1, 1);
                    //ply.index = 1;
                    break;

                case TileStyle.Gate:

                    switch ((MapsName)index)
                    {
                        case MapsName.Inn:
                            index = (int)MapsName.Village;
                            // ply.point = backPoint;
                            break;
                        case MapsName.Shop:
                            index = (int)MapsName.Village;
                            //ply.point = backPoint;
                            break;
                        case MapsName.Dungeon:
                            index = (int)MapsName.World;
                            //ply.point = backPoint;
                            break;
                        case MapsName.Village:
                            index = (int)MapsName.World;
                            //ply.point = backPoint;
                            break;
                        default:
                            Console.WriteLine("이거 오면 안되는디?");
                            break;
                    }

                    break;

                case TileStyle.NPC:
                    //상점 엔피시
                    if ((TileStyle)index == TileStyle.Shop)
                    {
                        //상점 이벤트 발동
                    }
                    //여관 엔피시
                    else if ((TileStyle)index == TileStyle.Inn)
                    {
                        //여관 이벤트 발동
                    }

                    break;

                //몬스터 조우
                case TileStyle.Monster:
                    //몬스터 배틀이벤트

                    break;

                case TileStyle.Field:
                    rndNum = rnd.Next(0, 10);

                    if (rndNum > 4) //50퍼 확률로 몬스터 만남.
                    {
                        //몬스터 배틀이벤트
                    }

                    break;
                case TileStyle.Mountain:
                    rndNum = rnd.Next(0, 10);

                    if (rndNum > 2) //70퍼 확률로 몬스터 만남.
                    {
                        //몬스터 배틀이벤트
                    }
                    break;
                case TileStyle.Sea:
                    rndNum = rnd.Next(0, 10);

                    if (rndNum > 7) //30퍼 확률로 몬스터 만남.
                    {
                        //몬스터 배틀이벤트
                    }
                    break;

                //보물박스 랜덤
                case TileStyle.RandomBox:
                    //아이템 흭득 랜덤박스 이벤트 발동
                    break;

                //이벤트 타일이 아닐경우 거짓
                default:
                    result = false;
                    break;
            }

            //if(TileStyle.Gate == )
            //Console.WriteLine(ply.index);
            //Console.WriteLine(ply.point.x);
            //Console.WriteLine(ply.point.y);


            return result;
        }

        //원하는 타일 인덱스값을 가지고 행열값 으로 검색해서 꺼내주기
        //이거로 이벤트 발생지점? 같은거 알아낼거임.
        static Point TileIndexSerch(Tile[,] tile, int index)
        {
            Point point = new Point();

            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    //바꾸길 원하는곳 찾았음.
                    if (tile[i, j].index == index)
                    {
                        point.x = j;
                        point.y = i;
                        break;
                    }
                }
            }

            return point;

        }

        //타일 중복시키기 싫은거 찾아서 변경
        static void FindAndReplaceDuplicateTiles(ref Tile[,] tile, TileStyle isStyle)
        {
            //마을 찾아서 마을 한개만 나오게 할거임.
            int count = 0;
            Random rnd = new Random();


            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {
                    if (tile[i, j].style == isStyle)
                    {
                        if (count != 0)
                        {
                            TileStyle style = new TileStyle();
                            //빌리지가 1번 인덱스임 //따로 뺴는 방법 못찾아서 순서 재설정해줌
                            style = (TileStyle)rnd.Next(2, 6);
                            TileImageSet(ref tile, tile[i, j].index, style);

                        }

                        count++;
                    }

                }
            }
        }

        //타일별 이미지 세팅
        //타일,변화를 원하는 인덱스,변화해줄 스타일
        static void TileImageSet(ref Tile[,] tile, int index, TileStyle style)
        {

            //Console.WriteLine(style);
            //터질까봐 인덱스맞춤
            //👨🏻‍🌾🧑🏻‍🍳👩🏻‍🔧
            //🎪🧱🧱🏔️⛰️🌋🗻🛤️🏕️🏞️🛣️🏖️🏚️🏠🏡🏘️🏨🏪🏬🏢♨️😀😁😄⩍🚪☠️💀
            string[] imogeImage = { " ",
                                    "🏡🏰🏘️","🧱🚪🧱","⛰️🛣️🏕️" , "🏔️🌋⛰️","🏞️🌅🏞️",
                                    "🏢🏪🏬","🏠♨️🏨","🧱🧱🧱","🛤️🛤️🛤️",
                                    "🧟‍🧌🧟‍",    "🎁🎁🎁",  "",    "😀😁😄",
                                    " ",    " ",    " ",
                                    " ",    " ", "🎞️🧿🎞️",
                                    " "
            };

            /* 이전에 임시로 사용하던 코드
            //원하는 칸의 배치를 꺼냄
            //for (int i = 0; i < tile.GetLength(0); i++)
            //{
            //    for (int j = 0; j < tile.GetLength(1); j++)
            //    {
            //        //바꾸길 원하는곳 찾았음.
            //        if (tile[i, j].index == index)
            //        {
            //            tile[i, j].Image = imogeImage[(int)style];
            //            tile[i, j].style = style;
            //        }
            //    }
            //}
            */

            Point point = TileIndexSerch(tile, index);
            tile[point.y, point.x].Image = imogeImage[(int)style];
            tile[point.y, point.x].style = style;
        }

        //타일 기본세팅
        static void TileSeting(ref Tile[,] tile, RectCase rect)
        {
            int temp = 1;
            for (int i = 0; i < tile.GetLength(0); i++)
            {
                for (int j = 0; j < tile.GetLength(1); j++)
                {

                    tile[i, j].point.SetPosition(2 + (j * rect.width), 2 + (i * rect.height));
                    tile[i, j].style = TileStyle.start;
                    tile[i, j].index = temp;
                    tile[i, j].Image = "　";
                    temp++;
                }
            }
            //함수 호출 잘되나 테스트 세팅
            //tile[0, 0].Image = "🏘️🏰🏘️";
        }

        //플레이어 생성
        static void PlayerCreate(ref Tile ply)
        {
            //플레이어 첫좌표
            ply.style = TileStyle.Player;
            ply.index = 1;
            ply.point.SetPosition(1, 1);
            ply.Image = "( '3')";
            //( '3')(❁´◡`❁)(●'◡'●)ᓚᘏᗢ(～￣▽￣)～(≧∀≦)ゞƪ(˘⌣˘)ʃ♪(´▽｀)ヾ(•ω•`)o(*￣3￣)╭)
            //Console.WriteLine("(`･Д･)ノ=☆");
            //Console.WriteLine("(⊃｡•́‿•̀｡)⊃━✿✿✿✿✿✿");
            //Console.WriteLine("(∩` ﾛ ´)⊃━炎炎炎炎炎");
            //Console.WriteLine("炎炎炎炎☆┣o(･ω･ )");
            //Console.WriteLine("(∩ᄑ_ᄑ)⊃━☆ﾟ*･｡*･:≡( ε:)");
            //Console.WriteLine("(ノ ˘_˘)ノ ζ|||ζ ζ|||ζ ζ|||ζ");
            //Console.WriteLine("(ﾉ≧∀≦)ﾉ ‥…━━━★");
            //Console.WriteLine("彡ﾟ◉ω◉ )つー☆*");
            //Console.WriteLine("༼つಠ益ಠ༽つ ─=≡ΣO))");
            //Console.WriteLine("(∩｀-´)⊃━☆ﾟ.*･｡ﾟ");



        }

        //만들어둔 멥 테두리를 그려준다.
        static void RectDrawing(ref RectCase rect)
        {
            //`Console.Clear();
            for (int i = 0; i < rect.backGround.GetLength(0); i++)
            {
                Console.WriteLine(rect.backGround[i]);
            }

        }

        //원하는 숫자를 입력하면 num x num 사이즈 타일(=박스)가 생성됨
        static void RectCreate(ref RectCase rect)
        {
            /* 잘나오나 테스트용
            //
            //string[] background = {　§§§§ ▥♤♧◎⊙▣◈▒▤▦▩〓────━⊙
            //                        "┏━━━━┳━━━━┳━━━━┳━━━━┓",
            //                        "┃    ┃    ┃    ┃    ┃",
            //                        "┣━━━━╋━━━━╋━━━━╋━━━━┫",
            //                        "┃    ┃    ┃    ┃    ┃",
            //                        "┣━━━━╋━━━━╋━━━━╋━━━━┫",
            //                        "┃    ┃    ┃    ┃    ┃",
            //                        "┣━━━━╋━━━━╋━━━━╋━━━━┫",
            //                        "┃    ┃    ┃    ┃    ┃",   
            //                        "┗━━━━┻━━━━┻━━━━┻━━━━┛"
            //};
            //가로 11 세로 11이 2x2 최소 사이즈 한칸씩 늘때마다 5씩 증가. 이유 빈공간 4칸에 숫자 4자리 넣을거임.
            //아.
            //가로 1+빈칸안에 들어갈 숫자 갯수. 한칸이 이렇고 늘어날때마다 들어갈 숫자갯수만큼 인덱스 늘려주고 설정하면됨.
            //세로는 1+2개당 방하나, 
            //int[,] backGroundIndex = new int[1+(5*num),1+(2*num)];

            //잘나오나 테스트용
            //foreach (string i in background) 
            //{
            //    Console.WriteLine(i);            
            //}
            */

            //이넘문 안되서 일케만듬.
            //인덱스 , ' '인 인덱스0은 나중에 숫자넣을수 있는 자리로 사용
            // 01234567891011 //끝에 두개만 좀밀려서 보임. 0~11까지 인덱스가 있음
            string ImageBackGrounds = " ┏┓┗┛━┃┣┳╋┻┫"; //┫ = 11 ┻ =10 ╋ = 9 ┳ = 8 ┣ = 7

            //나중에 값 입력받고 출력할 백그라운드


            //백그라운드에 이미지 몇번이 들어갔는지 정해줄변수.width
            rect.backGroundIndex = new int[1 + (rect.height * rect.value), 1 + (rect.width * rect.value)];

            rect.backGround = new string[rect.backGroundIndex.GetLength(0)];

            string str = "";
            char c = 'a';
            for (int i = 0; i < rect.backGroundIndex.GetLength(1); i++)
            {
                str += c;
            }

            for (int i = 0; i < rect.backGroundIndex.GetLength(0); i++)
            {
                rect.backGround[i] = str;
            }

            //백그라운드안에들어갈 요소들의 좌표 초기화
            for (int i = 0; i < rect.backGroundIndex.GetLength(0); i++)
            {
                for (int j = 0; j < rect.backGroundIndex.GetLength(1); j++)
                {

                    // 홀수 줄일때만 ㅡ 넣어줌width
                    if (i % rect.height == 0)
                    {
                        rect.backGroundIndex[i, j] = 5;

                    }
                    //숫자를 넣어줄 공간~ 즉 빈공간
                    else// if (i % 2 == 0)
                    {
                        rect.backGroundIndex[i, j] = 0;

                    }

                    //╋ 교차점
                    if (i % rect.height == 0 && j % rect.width == 0 && j > 0 && j < rect.backGroundIndex.GetLength(1) - 1)
                    {
                        rect.backGroundIndex[i, j] = 9;
                    }
                    //┫ = 11
                    //맨 오른쪽 가운데
                    else if (j == rect.backGroundIndex.GetLength(1) - 1 &&
                                i % rect.height == 0 && i < rect.backGroundIndex.GetLength(0))
                    {
                        rect.backGroundIndex[i, j] = 11;
                    }
                    //┣ = 7
                    //맨 왼쪽 가운데
                    else if (j == 0 && i % rect.height == 0 && i < rect.backGroundIndex.GetLength(0))
                    {
                        rect.backGroundIndex[i, j] = 7;

                    }//┃위에껄 다찍고 나머지중 좌표가 너비만큼 떨어졌을 경우 찍어줌
                    else if (j % rect.width == 0)
                    {
                        rect.backGroundIndex[i, j] = 6;
                    }


                    //맨왼쪽 맨위 ┏
                    if (i == 0 && j == 0)
                    {
                        rect.backGroundIndex[0, 0] = 1;
                    }
                    //맨오른쪽 맨위 ┓
                    else if (i == 0 && j == rect.backGroundIndex.GetLength(1) - 1)
                    {
                        rect.backGroundIndex[i, j] = 2;
                    }
                    //맨왼쪽 맨아래 ㄴ
                    else if (i == rect.backGroundIndex.GetLength(0) - 1 && j == 0)
                    {
                        rect.backGroundIndex[i, j] = 3;
                    }
                    //맨 오른쪽 맨아래 ┛
                    else if (i == rect.backGroundIndex.GetLength(0) - 1 && j == rect.backGroundIndex.GetLength(1) - 1)
                    {
                        rect.backGroundIndex[i, j] = 4;
                    }
                    //가로줄 5번째마다 ㅜ 하나씩 놓아줌-맨윗줄 일떄만 
                    else if (j % rect.width == 0 && (i == 0))
                    {
                        rect.backGroundIndex[i, j] = 8;

                    }
                    //가로줄 5번째마다 ㅗ 하나씩 놓아줌 - 맨아랫줄일때만
                    else if (j % rect.width == 0 && i == rect.backGroundIndex.GetLength(0) - 1)
                    {
                        rect.backGroundIndex[i, j] = 10;

                    }

                }
            }

            //캐릭터 배열을 string[]에 넣는법
            char[] chars = new char[rect.backGround[0].Length];

            for (int i = 0; i < rect.backGroundIndex.GetLength(0); i++)
            {
                for (int j = 0; j < rect.backGroundIndex.GetLength(1); j++)
                {
                    chars[j] = ImageBackGrounds[rect.backGroundIndex[i, j]];
                }

                //넣기전 초기화
                rect.backGround[i] = "";

                for (int j = 0; j < chars.Length; j++)
                {
                    rect.backGround[i] += chars[j];
                }
                //위와 같음. 
                //backGround[i] =  new string (chars);
            }

            //잘나왔나 그리기 테스트용
            //for (int i = 0; i < rect.backGround.GetLength(0); i++)
            //{
            //    Console.WriteLine(rect.backGround[i]);
            //}


        }

        //멥 세팅
        static Tile[,] CreateMap(Tile[,] tile, RectCase rect, int mapIndex)
        {
            //타일 좌표배정. 
            //타일들 기초세팅.
            TileSeting(ref tile, rect);

            //1~(num x num)까지  랜덤하게 타일 설정.
            //rect[0].value x rect[0].value
            //단.마을은 하나 
            Random rnd = new Random();

            //현재 만드는 타일들의 인덱스 최대값.
            int maxIndex = (rect.value * rect.value) + 1;


            /* 테스트용 기본값들.
            //for (int index = 1; index < maxIndex; index++)
            //{
            //    //지정해주고 싶은 타일스타일을 넣어줌.
            //    TileStyle style = new TileStyle();
            //    //TileStyle 1번은 마을이므로 1번제외 해야한다.
            //    //마을은 월드 전체에서 한개 마을안에 마을이라던지 던전안에 마을같은 이상한거 생기지 않게 하기위해 일단은 이렇게해둠.
            //    style = (TileStyle)rnd.Next(2, 6);
            //    //타일 이미지와 스타일 바꾸고싶을떄 씀
            //    //타일정보,바꿀타일 인덱스,타일 스타일 정보
            //    //이렇게 넣어줌.
            //    //TileImageSet(ref tile,index, style);
            //    TileImageSet(ref tile, index, style);
            //}
            */

            //MansName의 enum문 가독성 때문에 써줌
            //mapIndex는 현재 멥페이지 인덱스
            switch (mapIndex)
            {
                //월드타일일때만 설정 마을하나 내뚜고 마을들 다삭제
                //월드타일 한번 세팅해줌
                case (int)MapsName.World:

                    for (int index = 1; index < maxIndex; index++)
                    {
                        //지정해주고 싶은 타일스타일을 넣어줌.
                        TileStyle style = new TileStyle();
                        style = (TileStyle)rnd.Next(1, 6);

                        //타일 이미지와 스타일 바꾸고싶을떄 씀
                        //타일정보,바꿀타일 인덱스,타일 스타일 정보
                        //이렇게 넣어줌.
                        TileImageSet(ref tile, index, style);
                    }

                    //월드에서 생성된 마을을 1개만 남기고 다 삭제
                    FindAndReplaceDuplicateTiles(ref tile, TileStyle.Village);
                    FindAndReplaceDuplicateTiles(ref tile, TileStyle.Dungeon);
                    break;
                case (int)MapsName.Dungeon:

                    //index 1에는 게이트 넣을거임. 나가고 들어오고.                    
                    TileImageSet(ref tile, 1, TileStyle.Gate);

                    //나머지 부분 랜덤생성
                    for (int index = 2; index < maxIndex; index++)
                    {
                        TileStyle style = new TileStyle();
                        style = (TileStyle)rnd.Next(8, 12);
                        TileImageSet(ref tile, index, style);
                    }
                    break;
                case (int)MapsName.Village:

                    for (int index = 2; index < maxIndex; index++)
                    {

                        TileStyle style = new TileStyle();
                        int tileRandom = new Random().Next(0, 2);
                        Thread.Sleep(1);

                        if (tileRandom == 0)
                        {
                            // 벽과 길.
                            style = (TileStyle)rnd.Next(8, 10);
                        }
                        else
                        {
                            style = TileStyle.NPC;
                        }
                        TileImageSet(ref tile, index, style);

                    }

                    //처음 입구 출구.
                    TileImageSet(ref tile, 1, TileStyle.Gate);
                    //상점
                    TileImageSet(ref tile, 4, TileStyle.Shop);
                    //호텔
                    TileImageSet(ref tile, 13, TileStyle.Inn);

                    break;
                case (int)MapsName.Shop:
                    //처음 입구 출구.
                    TileImageSet(ref tile, 1, TileStyle.Gate);
                    //길 
                    TileImageSet(ref tile, 2, TileStyle.Road);
                    //길 
                    TileImageSet(ref tile, 3, TileStyle.Road);
                    //상점 npc
                    TileImageSet(ref tile, 4, TileStyle.NPC);

                    break;
                case (int)MapsName.Inn:
                    //처음 입구 출구.
                    TileImageSet(ref tile, 1, TileStyle.Gate);
                    //상점 npc
                    TileImageSet(ref tile, 2, TileStyle.NPC);
                    //길 
                    TileImageSet(ref tile, 3, TileStyle.Road);
                    //길 
                    TileImageSet(ref tile, 4, TileStyle.Road);

                    break;
                default:
                    Console.WriteLine("어어 여기 들오면 안된다.");
                    break;

            }

            return tile;
        }

    }
}
/*보물상자
"           _________________________________________________\n
        .' ____________________________________________ _.'|\n
      .' .'____________________________________________|_| |\n
    .' .'.'                                           .'.' |\n
  .' .'.'                                           .'.'  .'\n
 __.'.'___________________________________________.'.'  .'| \n
|  |'______.-.__________________________.-.____ __.'  .'| | \n
|  |    o--[]--o                     o--[]--o  |  | .'  | | \n
|__|____[.|  |.]____ ________ _______[.|  |.]__|__|' |  | | \n
  |  | |  ＼_/ _____|  ====  |  .'_____＼_/|  | |____|  | | \n
  |  | |.'          |        |.'           |  | |   . . | | \n
  |  | |            '--------'             |  | | .'.'__|.' \n
  |  | ____________________________________|  | |'.'        \n
  |  ||____________________________________|  | |'          \n
  |  | |                                   |  | |           \n
  |__|.'                                   |__|.'           \n"



 */

/*
 
"           _________________________________________________\n        .' ____________________________________________ _.'|\n      .' .'____________________________________________|_| |\n    .' .'.'                                           .'.' |\n  .' .'.'                                           .'.'  .'\n __.'.'___________________________________________.'.'  .'| \n|  |'______.-.__________________________.-.____ __.'  .'| | \n|  |    o--[]--o                     o--[]--o  |  | .'  | | \n|__|____[.|  |.]____ ________ _______[.|  |.]__|__|' |  | | \n  |  | |  ↖__/ _____|  ====  |  .'_____↘__/|  | |____|  | | \n  |  | |.'          |        |.'           |  | |   . . | | \n  |  | |            '--------'             |  | | .'.'__|.' \n  |  | ____________________________________|  | |'.'        \n  |  ||____________________________________|  | |'          \n  |  | |                                   |  | |           \n  |__|.'                                   |__|.'           \n"
 */