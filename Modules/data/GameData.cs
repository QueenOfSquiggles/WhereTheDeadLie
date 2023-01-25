namespace data;


public struct GameData
{
    // helpful constants
    public const int PHASE_GENERATORS = 0;
    public const int PHASE_PUZZLE = 1;
    public const int PHASE_KEYS = 2;
    

    // changing game data
    public int phase = PHASE_KEYS;
    public int aggression_level = 0;
    public int active_generators = 0;
    public bool puzzle_solved = false;
    public int found_keys = 0;

    // reference information
    public const int MIN_AGGRESSION = 0;
    public const int MAX_AGGRESSION = 10;
    public const int REQUIRED_GENERATORS = 3;
    public const int REQUIRED_KEYS = 3;

    public GameData(){}
}