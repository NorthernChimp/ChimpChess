using System;

public class GameEvent
{
	//anything that alters the events of the game, on the board or in the various decks of the player or their hand. any change in the game is done through events in a queue
	public EventType theType;
	public List<Transform> theActor;
	public List<Transform> theTarget;
	public Player targetPlayer;
	public Event(EventType theTypeToApply,List<Transform> actors,List<Transform> targets,Player playerTargetted)
	{
		theType = theTypeToApply;
		theActor = actors;
		theTarget = targets;
		owningPlayer = playerTargetted;
	}
}
public enum EventType { moveUnit,attackUnit,playCard}
