using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhotonEvents
{
    // Standard codes
	public const byte SELECTION_PLAYER_CHANGE = 1,
                      SELECTION_PLAYER_READY  = 2,
                      SELECTION_PLAYER_LEFT   = 11,
                      GAME_CHANGE_TURN        = 3,
                      PROPERTY_BOUGHT         = 4,
                      METER_CHANGED           = 5,
                      MONEY_CHANGED           = 6,
                      PLAYER_HAS_WON          = 7,
                      UPDATE_ACTIVE_PLAYER    = 8,
                      PAY_RENT                = 9,
                      MESSAGE                 = 10,
                      GIVE_PROPERTY           = 12;

    // Chance function codes
    public const byte TAKE_MONEY_FROM_OTHERS = 101,
                      GIVE_MONEY_TO_OTHERS   = 102,
                      COLLECT_FROM_WEALTHY   = 103,
                      PROPERTY_TAX           = 104,
                      SWAP_POSITIONS1        = 105,
                      ADD_SPACES             = 106,
                      NEW_POSITION           = 107,
                      SWAP_PLANETS1          = 108,
                      SWAP_PLANETS2          = 109,
                      LOSE_PLANETS           = 110,
                      CHANCE_SELECT          = 111,
                      SWAP_POSITIONS2        = 112,
                      LOSE_YOUR_PLANET       = 113,
                      SYNC_CARDS             = 114;
}
