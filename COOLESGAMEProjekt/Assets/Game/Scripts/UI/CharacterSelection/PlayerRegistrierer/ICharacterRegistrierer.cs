using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.CharacterSelection.CharacterRegistrierer
{
    public interface ICharacterRegistrierer
    {
        List<UIPlayerInfo> FindNewPlayers();
    }
}