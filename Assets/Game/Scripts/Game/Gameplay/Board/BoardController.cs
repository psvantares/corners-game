using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Data;
using Game.Utilities;
using UniRx;
using UnityEngine;

namespace Game.Gameplay
{
    public class BoardController : IDisposable
    {
        private readonly BoardContext context;
        private Board board;
        private BotGame bot;

        private List<Cell> availableToMoveCells;
        private List<CellHighlight> cellsHighlight;

        private readonly PlayerType selfPlayer;
        private PlayerType activePlayer;

        private readonly CompositeDisposable disposables = new();
        private CancellationTokenSource cts;
        private readonly ISubject<PlayerType> playerWinEvent = new Subject<PlayerType>();

        public IObservable<PlayerType> PlayerWinEvent => playerWinEvent;

        public BoardController(BoardContext context)
        {
            this.context = context;

            InitializeBoard(context);
            InitializeBot(context);
            InitializeHighlight(context);

            activePlayer = context.NetworkGameController.CurrentPlayer;

            selfPlayer = context.NetworkGameController.PlayerCount switch
            {
                0 => PlayerType.White,
                1 => PlayerType.Black,
                _ => selfPlayer
            };

            Subscribes();
        }

        public void Dispose()
        {
            Unsubscribes();

            board.Dispose();
            cts.CancelAndDisposeSafe();

            cellsHighlight.Clear();
            availableToMoveCells.Clear();
        }

        private void InitializeBoard(BoardContext context)
        {
            board = new Board(context);
            board.Initialize();
        }

        private void InitializeBot(BoardContext context)
        {
            if (context.GameModel.GameplayMode == GameplayMode.Bot)
            {
                bot = new BotGame(PlayerType.Black, board);
            }
        }

        private void InitializeHighlight(BoardContext context)
        {
            cellsHighlight = new List<CellHighlight>();
            availableToMoveCells = new List<Cell>();

            var max = context.Config.BoardSize.x + context.Config.BoardSize.y;
            var pool = context.Pool;

            for (var i = 0; i < max; i++)
            {
                var cellHighlight = pool.GetHighlightPrefab();
                cellHighlight.SetActive(false);
                cellsHighlight.Add(cellHighlight);
            }
        }

        private void Subscribes()
        {
            BoardInput.CellSelectedEvent.Subscribe(OnCellSelected).AddTo(disposables);
            context.NetworkGameController.SwitchPlayerEvent.Subscribe(OnNetworkActivePlayer).AddTo(disposables);
            context.NetworkGameController.UpdateCheckerPositionEvent.Subscribe(OnUpdateCheckerPosition).AddTo(disposables);
        }

        private void Unsubscribes()
        {
            disposables.Clear();
        }

        private void ShowHighlight(IReadOnlyList<Cell> cells)
        {
            HideHighlight();

            for (var i = 0; i < cells.Count; i++)
            {
                var position = cells[i].Position;
                var cellHighlight = cellsHighlight[i];
                cellHighlight.transform.localPosition = new Vector3(position.x, position.y, 0);
                cellHighlight.SetActive(true);
            }
        }

        private void HideHighlight()
        {
            foreach (var cellHighlight in cellsHighlight)
            {
                cellHighlight.SetActive(false);
            }
        }

        private void ProcessLogic(Cell cell)
        {
            if (IsAiTurn())
            {
                return;
            }

            var checker = board.GetChecker(cell);

            if (context.GameModel.GameplayMode == GameplayMode.Network)
            {
                if (selfPlayer != activePlayer)
                {
                    return;
                }
            }

            if (checker != null)
            {
                if (checker.PlayerType != activePlayer)
                {
                    return;
                }

                checker.SetSelected(true);
                availableToMoveCells = board.GetAvailableMoves(cell);
                ShowHighlight(availableToMoveCells);
            }
            else
            {
                if (availableToMoveCells == null || !availableToMoveCells.Contains(cell) || Checker.Selected == null)
                {
                    return;
                }

                HideHighlight();
                MakeMove(cell);

                if (!IsWinner(activePlayer))
                {
                    SwitchPlayer();

                    if (IsAiTurn())
                    {
                        cts.CancelAndDisposeSafe();
                        cts = new CancellationTokenSource();
                        MakeAiMove(cts.Token).Forget();
                    }
                }
                else
                {
                    Unsubscribes();
                    playerWinEvent?.OnNext(activePlayer);
                }
            }
        }

        private async UniTask MakeAiMove(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            MakeMove(bot.CalcTurn());
            SwitchPlayer();
        }

        private void MakeMove(Cell cell)
        {
            if (context.GameModel.GameplayMode == GameplayMode.Network)
            {
                var data = new NetworkCheckerData
                {
                    Index = Checker.Selected.Index,
                    Position = cell.Position
                };

                context.NetworkGameController.RPC_UpdateCheckerPosition(data);
            }
            else
            {
                Checker.Selected.SetPosition(cell.Position);
            }

            Checker.Selected.SetSelected(false);
        }

        private bool IsAiTurn()
        {
            return bot != null && bot.Type == activePlayer;
        }

        private void SwitchPlayer()
        {
            SetLocalActivePlayer(NextPlayer());
        }

        private PlayerType NextPlayer()
        {
            return activePlayer == PlayerType.White ? PlayerType.Black : PlayerType.White;
        }

        private void SetLocalActivePlayer(PlayerType playerType)
        {
            activePlayer = playerType;
            context.NetworkGameController.RPC_SwitchActivePlayer(activePlayer);
        }

        private bool IsWinner(PlayerType player)
        {
            return board.IsPlayerOnOpponentPositions(player == PlayerType.Black ? PlayerType.Black : PlayerType.White);
        }

        // Events

        private void OnCellSelected(Vector2Int position)
        {
            var cell = board.GetCell(position);

            if (cell == null)
            {
                return;
            }

            ProcessLogic(cell);
        }

        private void OnNetworkActivePlayer(PlayerType playerType)
        {
            activePlayer = playerType;
        }

        private void OnUpdateCheckerPosition(NetworkCheckerData data)
        {
            var checker = board.GetChecker(data.Index);

            if (checker == null)
            {
                return;
            }

            checker.SetPosition(data.Position);
        }
    }
}