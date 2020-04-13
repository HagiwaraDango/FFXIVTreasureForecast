using System.ComponentModel;

namespace TreasureMakerV2.Enum
{
    public class FFXIVEnum
    {
        /// <summary>
        /// Dragon:飘龙、犬狮、鹦鹉、坟灵、普卡、坟灵、兔尾、仙子猪、刺猬
        /// Tree:树、蜜蜂、风花
        /// Bear:玫瑰熊、蜜蜂、蜜蜂,镇尼、兔尾、兔尾, 篮筐、刺猬、刺猬,  树、鹦鹉、鹦鹉
        /// ButterFly:血紫蝴蝶、鹰蜓、青蝴蝶、蜘蛛、青蝴蝶、蘑菇
        /// Moth:蛾、蘑菇、鹰蜓
        /// DragonBird:龙鸟、飘龙、飘龙、马、犬狮、犬狮
        /// Ghost:那伊阿得斯、坟灵、坟灵
        /// </summary>
        public enum MonsterTypeEnum
        {
            [Description("飘龙、犬狮、鹦鹉、坟灵、普卡、坟灵、兔尾、仙子猪、刺猬")]
            Dragon,
            [Description("树、蜜蜂、风花")]
            Tree,
            [Description("玫瑰熊、蜜蜂、蜜蜂,镇尼、兔尾、兔尾, 篮筐、刺猬、刺猬,  树、鹦鹉、鹦鹉")]
            Bear,
            [Description("血紫蝴蝶、鹰蜓、青蝴蝶、蜘蛛、青蝴蝶、蘑菇")]
            ButterFly,
            [Description("蛾、蘑菇、鹰蜓")]
            Moth,
            [Description("龙鸟、飘龙、飘龙、马、犬狮、犬狮")]
            DragonBird,
            [Description("那伊阿得斯、坟灵、坟灵")]
            Ghost,
        };
    }
}