using System;
using System.Security.Cryptography;

PartySystem hero = new PartySystem(new HumanPlayer());
PartySystem monster = new PartySystem(new ComputerAi());

Console.Write("ENTER YOUR NAME: ");
string? name = Console.ReadLine();

hero.Characters.Add(new TrueProgrammer(name));
monster.Characters.Add(new Skeleton());

Game Game = new Game(hero,monster);

Game.Run();

public class Game
{
    private PartySystem hero;
    private PartySystem monster;
    public Character? currentPlayer { get; private set; }

    public Game(PartySystem hero, PartySystem monster)
    {
        this.hero = hero;
        this.monster = monster;
    }

    public void Run()
    {
        bool _herosTurn = true;
        int round = 0;
        currentPlayer = hero.Characters[0];
        while (round < 5)
        {
            Thread.Sleep(500);

            if (_herosTurn)
            {
                currentPlayer = hero.Characters[0];
                Console.WriteLine($"It is {currentPlayer}'s turn...");
                hero.Player.DoSelectedAction(this, currentPlayer).Excute(this);
                _herosTurn = false;
            }
            else if (!_herosTurn)
            {

                currentPlayer = monster.Characters[0];
                 Console.WriteLine($"It is {currentPlayer}'s turn...");
                monster.Player.DoSelectedAction(this, currentPlayer).Excute(this);
                _herosTurn = true;
            }

            Console.WriteLine();
            round++;
        }
    }
}
public interface IAction 
{
    public void Excute(Game game);
}

public interface IPlayer 
{
    public IAction DoSelectedAction(Game game, Character character);
}

public class HumanPlayer : IPlayer
{
    public IAction DoSelectedAction(Game game, Character character) {

        foreach (var action in character.Actions)
        {
            Console.WriteLine($"Select your action: ");
            Console.WriteLine($"1. {action}");
        }
       
        
        int.TryParse(Console.ReadLine(), out int choice);

        return choice switch
        {
            1 => new Skip(),
            _ => new Skip()
        };
    }
}

public class ComputerAi : IPlayer 
{
    public IAction DoSelectedAction(Game game, Character character) {
       Random r = new Random();
       
        int choice = r.Next(0, 1);
        
        return choice switch
        {
            1 => new Skip(),
            _ => new Skip()
        };
    }
}


public class Skip : IAction
{
    public void Excute(Game game)
    {
        Console.WriteLine($"{game.currentPlayer?.Name} did NOTHING.");
    }
}

public abstract class Character 
{
    public List<IAction> Actions = new List<IAction>();
    public string Name { get; private set; }

    public Character(string name, List<IAction> actions) 
    {
        Name = name;
        Actions = actions;
    }
}


public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name, new List<IAction> { new Skip()})
    {
    }
}


public class Skeleton : Character
{
    public Skeleton() : base ("Skeleton", new List<IAction> {new Skip()}) 
    {        
    }
}

public class PartySystem
{
    public IPlayer Player { get; }
    public List<Character> Characters { get; } = new List<Character>();
    
    public PartySystem(IPlayer player)
    {
        Player = player;
    }
}