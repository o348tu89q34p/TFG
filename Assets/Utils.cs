using SFML.Graphics;

namespace Game {
    public static class FontUtils {
        private const string fontPath = "../Assets/Fonts/";
        public const string TitleFont = fontPath + "aAhaWow.ttf";
        //public const string TextFont1 = fontPath + "LinLibertine_aBS.ttf";
        public const string TextFont1 = fontPath + "Garute-Medium.ttf";
        public static Font StatusFont = new Font(fontPath + "Garute-Medium.ttf");
        public static Font ButtonFont = new Font(fontPath + "Bebas-Regular.ttf");
    }

    public static class TextureUtils {
        // Buttons.
        private const string ButtonTexturePath = "../Assets/Graphics/UI/Buttons/";

        private const string Background = "../Assets/Graphics/UI/bg1.png";
        public static Texture BackgroundTexture = new Texture(Background);

        private const string ActionIdle = ButtonTexturePath + "action_idle.png";
        private const string ActionHover = ButtonTexturePath + "action_hover.png";
        private const string ActionActive = ButtonTexturePath + "action_active.png";
        private const string ActionDisabled = ButtonTexturePath + "action_disabled.png";

        private const string Regular = ButtonTexturePath + "regular.png";
        private const string RegularHover = ButtonTexturePath + "regular_hover.png";
        private const string Square = ButtonTexturePath + "square_base.png";
        private const string SquareHover = ButtonTexturePath + "square_hover.png";
        private const string SortRun = ButtonTexturePath + "sort_run.png";
        private const string SortSet = ButtonTexturePath + "sort_set.png";

        public static Texture IdleTexture = new Texture(TextureUtils.ActionIdle);
        public static Texture HoverTexture = new Texture(TextureUtils.ActionHover);
        public static Texture ActiveTexture = new Texture(TextureUtils.ActionActive);
        public static Texture DisabledTexture = new Texture(TextureUtils.ActionDisabled);

        public static Texture RegularTexture = new Texture(Regular);
        public static Texture RegularHoverTexture = new Texture(RegularHover);
        public static Texture SquareTexture = new Texture(Square);
        public static Texture SquareHoverTexture = new Texture(SquareHover);
        public static Texture SortRunTexture = new Texture(SortRun);
        public static Texture SortSetTexture = new Texture(SortSet);

        // Cards.
        private const string CardPath = "../Assets/Graphics/Cards/";
        public const int CardWidth = 94;
        public const int CardHeight = 130;

        private const string EmptyCard = CardPath + "empty.png";
        public static Texture EmptyTexture = new Texture(EmptyCard);

        private const string FrenchFace = CardPath + "french.png";
        private const string FrenchWildFace = CardPath + "french_joker.png";
        private const string FrenchBack = CardPath + "french_back.png";
        public static Texture FrenchFaceTexture = new Texture(FrenchFace);
        public static Texture FrenchJokerTexture = new Texture(FrenchWildFace);
        public static Texture FrenchBackTexture = new Texture(FrenchBack);

        // Change the file names in this section.
        private const string SpanishFace = CardPath + "french.png";
        private const string SpanishWildFace = CardPath + "french_joker.png";
        private const string SpanishBack = CardPath + "french_back.png";
        public static Texture SpanishFaceTexture = new Texture(SpanishFace);
        public static Texture SpanishJokerTexture = new Texture(SpanishWildFace);
        public static Texture SpanishBackTexture = new Texture(SpanishBack);

        // Other elements
        private const string OtherPath = "../Assets/Graphics/UI/";

        private const string NamePlate = OtherPath + "name_tag.png";
        public static Texture NamePlateTexture = new Texture(NamePlate);
    }
}
