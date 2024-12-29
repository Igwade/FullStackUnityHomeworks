using System;
using JetBrains.Annotations;
using SampleGame.Common;
using SampleGame.Gameplay;

namespace App.SaveLoad.Entities.ComponentSerializers
{
    [Serializable]
    public class TeamData
    {
        public string type;
    }

    [UsedImplicitly]
    public class TeamSerializer: ComponentSerializer<Team, TeamData>
    {
        public override TeamData Serialize(Team component) =>
            new()
            {
                type = component.Type.ToString()
            };

        public override void Deserialize(Team component, TeamData data)
        {
            if (Enum.TryParse<TeamType>(data.type, out var teamType))
            {
                component.Type = teamType;
            }
        }
    }
}