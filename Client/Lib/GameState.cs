namespace QuizFoot.Client.Lib
{
    public enum GameState
    {
        NoCode,
        NoLobby,
        FailedToJoin,
        GettingInfo,
        InLobby,
        InGame,

        RoundDisplay,

        WaitingForNextQuestion,
        AnsweringQuestion,

        PreviewingQuestion,
    }
}
