# RpgProject
 콘솔프로젝트 입니다.


#오늘 만든 프로그램

## Git Push 이런거 배웠음

### 더 작은 제목

####제목 4번째

**조영락**

```cs
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
```

![이미지 꺠젔을 떄 대신 나올 문구](https://file.notion.so/f/f/ecbfc15b-d9e9-421e-911d-a5bceae47cb4/85dd7b80-1579-4622-bad3-ef367d3b87c1/image.png?table=block&id=4d8f4821-7697-4ae0-b8b7-ee7d673b95e8&spaceId=ecbfc15b-d9e9-421e-911d-a5bceae47cb4&expirationTimestamp=1733551200000&signature=Ek-yvnM01KnUHi-HIKTCipn0t22GJzABQXdEmRgKVOw&downloadName=image.png)
