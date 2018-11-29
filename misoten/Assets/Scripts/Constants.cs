using UnityEngine;
// 定数

namespace Constants
{
    public static class Game
    {
        public const float FPS = 60;
    }

    public static class Tutorial
    {
        public const float WAIT_TIME = 2.0f;
        public const int NO_1 = 0;
        public const int NO_2 = 1;
        public const int NO_3 = 2;
        public const int NO_4 = 3;
        public const int NO_5 = 4;
        public const int NO_6 = 5;
        public const int ERROR = 6;
    }

    public static class MyColor
    {
        public static readonly Color BLACK = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        public static readonly Color WHITE = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public static readonly Color ALPHA_0 = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

}