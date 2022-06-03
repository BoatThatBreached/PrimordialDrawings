using System.Collections.Generic;
using System.Linq;

public static class PlayerInfo
{
    public static List<EnvType> Studied;
    public static Dictionary<EnvPaints, int> PaintsRemaining;
    public static int MaxLevel;
    public static EnvType ChosenType;
    public static int ChosenIndex;
    private static Dictionary<EnvType, Dictionary<EnvPaints, int>> Requirements;

    public static void Reset()
    {
        Studied = new List<EnvType>();
        PaintsRemaining = new Dictionary<EnvPaints, int>
        {
            [EnvPaints.Blood] = 4,
            [EnvPaints.Earth] = 6,
            [EnvPaints.Wood] = 10
        };
        MaxLevel = 1;
        Requirements = new Dictionary<EnvType, Dictionary<EnvPaints, int>>
        {
            [EnvType.Sprout] = CreateReq(0, 0, 2)
        };
        ChosenType = EnvType.Sprout;
        //ChosenIndex = -1;
    }

    public static bool EnoughResources()
        => Requirements[ChosenType].Keys.All(k => Requirements[ChosenType][k] <= PaintsRemaining[k]);

    private static Dictionary<EnvPaints, int> CreateReq(int blood, int earth, int wood) => new Dictionary<EnvPaints, int>
    {
        [EnvPaints.Blood] = blood,
        [EnvPaints.Earth] = earth,
        [EnvPaints.Wood] = wood
    };

    public static void Spend()
    {
        PaintsRemaining[EnvPaints.Blood] -= Requirements[ChosenType][EnvPaints.Blood];
        PaintsRemaining[EnvPaints.Wood] -= Requirements[ChosenType][EnvPaints.Wood];
        PaintsRemaining[EnvPaints.Earth] -= Requirements[ChosenType][EnvPaints.Earth];
    }
}