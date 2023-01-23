namespace data;


public struct GameData
{
    // helpful constants
    public const int PHASE_GENERATORS = 0;
    public const int PHASE_PUZZLE = 1;
    public const int PHASE_KEYS = 2;
    

    // changing game data
    public int phase = 0;
    public int aggression_level = 0;
    public int active_generators = 0;
    public bool puzzle_solved = false;
    public int found_keys = 0;

    // reference information
    public int min_aggression = 0;
    public int max_aggression = 10;
    public int required_generators = 3;
    public int required_keys = 3;

    public GameData(){}
}