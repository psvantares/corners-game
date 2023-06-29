namespace Game.Gameplay.Board
{
    public enum PlayerType : byte
    {
        None,
        Black,
        White
    }

    public enum BoardMode : byte
    {
        Normal,
        Diagonal,
        VerticalHorizontal
    }
}