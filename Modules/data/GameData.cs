namespace data;


public class GameData
{
    // phases
    public const int PHASE_GENERATORS = 0;
    public const int PHASE_PUZZLE = 1;
    public const int PHASE_KEYS = 2;
    public const int PHASE_END = 3;

    // reference information
    public const int MIN_AGGRESSION = 0;
    public const int MAX_AGGRESSION = 10;

    // progression game data
    public int phase = 0;
    public int active_generators = 0;
    public bool puzzle_solved  = false;
    public int found_keys = 0;

    
    // difficulty settings
    public int required_generators {get; set;}
    public int aggression_level {get; set;}
    public int required_keys {get; set;}

    // safety values
    public int level_generators {get; set;}
    public int level_chests {get; set;}

    public GameData(){}
}

public class GamePreset
{
    public static GamePreset DEFAULT = new(3, 5, 3);
    public static GamePreset MAX_UTILIZATION = new(999, 5, 999); // uses all available chests & generators
    public static GamePreset RNG_HELL = new(1, 5, 1); // only one each of generators and chests will be needed. RNG determines which

    public int required_generators;
    public int aggression_level;
    public int required_keys;

    public GamePreset(int generators, int aggression, int keys)
    {
        required_generators = generators;
        aggression_level = aggression;
        required_keys = keys;
    }

}