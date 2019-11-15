using System;
using ARngryBirdsBackend.Models;

namespace ARngryBirdsBackend
{
    public class Engine
    {
        private static Random _random = new Random();
        
        private static readonly TimeSpan FullRound = new TimeSpan(0, 3, 0);
        private const int VisibleAngle = 120;
        private const int DamageToStrengthRatio = 1;
        private const int LowerLongitude = 60;
        private const int HigherLongitude = 120;

        private readonly State _state;
        private DateTime _last;
        private DateTime _nextZone = DateTime.MinValue;
        
        public Engine(State state)
        {
            _state = state;
            _last = DateTime.Now;
        }

        public void Tick(in DateTime now)
        {
            if (_state.Health <= 0) return;
            
            var diff = (now - _last).TotalMilliseconds;
            _state.PlanetRotation += (int)Math.Round(diff / FullRound.TotalMilliseconds * 360);
            _state.PlanetRotation = _state.PlanetRotation.ToAngle();

            RemoveZones();
            GenerateZone();
            
            _last = now;
        }

        private void GenerateZone()
        {
            if (_nextZone < DateTime.Now)
            {
                var lowerGenAngle = _state.PlanetRotation;
                var higherGenAngle = (_state.PlanetRotation + VisibleAngle / 2).ToAngle();
                _state.Zones.Add(new Zone
                {
                    Latitude = _random.Next(lowerGenAngle, higherGenAngle),
                    Longitude = _random.Next(LowerLongitude, HigherLongitude),
                    Strength = GetZoneStrength(),
                    Type = GetRandomZoneType()
                });
                _nextZone = GetNextZonePeriod();
            }
        }

        private void RemoveZones()
        {
            // remove from left
            var lowerAngle = (_state.PlanetRotation - VisibleAngle / 2).ToAngle();
            var higherAngle = (_state.PlanetRotation + VisibleAngle / 2).ToAngle();

            var i = 0;
            while (i < _state.Zones.Count)
            {
                var zone = _state.Zones[i];
                if (!zone.Longitude.AreWithinAngles(lowerAngle, higherAngle))
                {
                    ExplodeZone(zone);
                    continue;
                }
                i++;
            }
        }

        private void ExplodeZone(Zone zone)
        {
            _state.Zones.Remove(zone);
            _state.Health -= (int)Math.Round(zone.Strength * DamageToStrengthRatio);
        }

        public void Reset()
        {
            _last = DateTime.Now;
            _state.Reset();
        }
        
        private double GetZoneStrength()
        {
            // TODO by score
            return _random.Next(30, 100) / 100.0;
        }

        private DateTime GetNextZonePeriod()
        {
            // TODO by score
            return DateTime.Now + new TimeSpan(0, 0, _random.Next(10, 30));
        }

        private ZoneType GetRandomZoneType()
        {
            // TODO by score
            return ZoneType.Fire;
        }
    }
}