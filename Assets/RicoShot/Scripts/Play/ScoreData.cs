using RicoShot.Core;
using RicoShot.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Play
{
    public class ScoreData : INetworkSerializable, IDataChangedNotifiable, IEquatable<ScoreData>
    {
        public event Action OnDataChanged;

        public FixedString64Bytes UUID { get => uuid; private set => uuid = value; }
        public Team Team { get => team; private set => team = value; }
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnDataChanged?.Invoke();
            }
        }

        private FixedString64Bytes uuid;
        private Team team;
        private int score;
        
        public ScoreData()
        {

        }

        public ScoreData(FixedString64Bytes UUID, Team Team)
        {
            this.UUID = UUID;
            this.Team = Team;
            this.Score = 0;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref uuid);
            serializer.SerializeValue(ref team);
            serializer.SerializeValue(ref score);
        }

        public bool Equals(ScoreData other)
        {
            return this.UUID == other.UUID;
        }
    }
}
