using Chess.Core;
using Chess.Interaction;
using Chess.Moves;
using Chess.Pieces;
using Chess.UI;
using Zenject;

namespace Chess.Installers
{
	public class GameInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<GameManager>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<BoardManager>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<TileSelector>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<PieceMover>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<PawnPromotionPanelHandler>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<PieceSpawner>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<CameraController>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<PausePanelHandler>().FromComponentInHierarchy(true).AsSingle();
			Container.Bind<CaptureZone>().AsSingle();
			Container.Bind<MoveValidator>().To<KingMoveValidator>().WhenInjectedInto<King>();
			Container.Bind<MoveValidator>().To<QueenMoveValidator>().WhenInjectedInto<Queen>();
			Container.Bind<MoveValidator>().To<RookMoveValidator>().WhenInjectedInto<Rook>();
			Container.Bind<MoveValidator>().To<KnightMoveValidator>().WhenInjectedInto<Knight>();
			Container.Bind<MoveValidator>().To<BishopMoveValidator>().WhenInjectedInto<Bishop>();
			Container.Bind<MoveValidator>().To<PawnMoveValidator>().WhenInjectedInto<Pawn>();
		}
	}
}