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
    public int level_generators {get; set;} = 10;
    public int level_chests {get; set;} = 10;

    public GameData(){}

    // other game related data

    public float VolumeMain {get; set;} = 0.0f;
    public float VolumeVO {get; set;} = 0.0f;
    public float VolumeSFX {get; set;} = 0.0f;
    public float VolumeCreature {get; set;} = 0.0f;
    public float VolumeDrones {get; set;} = -20.0f; // default -20 because it balances better
}

public class GamePreset
{
    public static GamePreset DEFAULT = new(3, 5, 3);
    public static GamePreset MAX_UTILIZATION = new(10, 5, 10); // uses all available chests & generators
    public static GamePreset RNG_HELL = new(1, 5, 1); // only one each of generators and chests will be needed. RNG determines which
    public static GamePreset MAX_DIFFICULTY = new(10, 10, 10);
    public static GamePreset INCREDIBLY_EASY = new(1, 1, 1);
    public static GamePreset FRUSTRATION_STATION = new(1, 10, 1);

    public int required_generators;
    public int aggression_level;
    public int required_keys;

    public GamePreset(int generators, int aggression, int keys)
    {
        required_generators = generators;
        aggression_level = aggression;
        required_keys = keys;
    }

    public override bool Equals(object obj)
    {
        if (obj is GamePreset other)
        {
            if (other.required_generators != required_generators) return false;
            if (other.aggression_level != aggression_level) return false;
            if (other.required_keys != required_keys) return false;
            return true;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        var value = aggression_level;
        value += required_keys * 100;
        value += required_generators * 10000;
        return value; // generate a digit code for GGKKAA.
    }
}