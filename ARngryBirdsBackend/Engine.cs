using System;
using ARngryBirdsBackend.Models;
using MoreLinq;

namespace ARngryBirdsBackend
{
    public class Engine
    {
        private static Random _random = new Random();
        
        private static readonly TimeSpan FullRound = new TimeSpan(0, 3, 0);
        private const int VisibleAngle = 120;
        private const int DamageToStrengthRatio = 50;
        private const int LowerLatitude = 60;
        private const int HigherLatitude = 120;
        private const double EliminateDistance = 3.0;
        private const double PassiveDistance = 10.0;
        private const int ScoreToStrengthRatio = 100;

        private int _x;
        private int _y;
        private DateTime _lastCoords;

        private BirdType _currentMode = BirdType.None;

        private readonly State _state;
        private DateTime _lastTick;
        private DateTime _nextZone = DateTime.MinValue;
        
        public Engine(State state)
        {
            _state = state;
            _lastTick = DateTime.Now;
        }

        public void Tick(in DateTime now)
        {
            if (_state.Health <= 0) return;
            
            var diff = (now - _lastTick).TotalMilliseconds;
            _state.PlanetRotation += (int)Math.Round(diff / FullRound.TotalMilliseconds * 360);
            _state.PlanetRotation = _state.PlanetRotation.ToAngle();

            RemoveZones();
            GenerateZone();
            
            _lastTick = now;
        }

        private void GenerateZone()
        {
            if (_nextZone >= DateTime.Now) return;
            
            var lowerGenAngle = _state.PlanetRotation;
            var higherGenAngle = (_state.PlanetRotation + VisibleAngle / 2).ToAngle();
            _state.Zones.Add(new Zone
            {
                Longitude = Utils.GenerateAngleWithin(lowerGenAngle, higherGenAngle),
                Latitude = _random.Next(LowerLatitude, HigherLatitude),
                Strength = GetZoneStrength(),
                Type = GetRandomZoneType()
            });
            _nextZone = GetNextZonePeriod();
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
            _lastTick = DateTime.Now;
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
            return DateTime.Now + new TimeSpan(0, 0, _random.Next(3, 10));
        }

        private ZoneType GetRandomZoneType()
        {
            // TODO by score
            return ZoneType.Fire;
        }

        public void SetCoords(int x, int y)
        {
            _x = x;
            _y = y;
            _lastCoords = DateTime.Now;
        }

        public void Hit(double strength)
        {
            var diff = DateTime.Now - _lastCoords;
            if (diff < new TimeSpan(0, 0, 2) && _currentMode != BirdType.None)
            {
                // generate bird
                var (lat, lng) = CalculateSpherePoint(strength);
                _state.Birds.Add(new Bird
                {
                    Id = _random.Next(),
                    Latitude = lat,
                    Longitude = lng,
                    X = _x,
                    Y = _y,
                    Type = _currentMode,
                    Strength = strength
                });
                
                // find closest point
                Zone closest = null;
                var minDist = double.MaxValue;
                foreach (var zone in _state.Zones)
                {
                    var dist = Utils.DistanceBetween2d(zone.Latitude, zone.Longitude, lat, lng);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = zone;
                    }
                }

                if (closest != null && minDist < PassiveDistance)
                {
                    if (minDist <= EliminateDistance)
                    {
                        _state.Zones.Remove(closest);
                        _state.Score += ScoreToStrengthRatio;
                    }
                    else
                    {
                        var scoreRatio = (PassiveDistance - minDist) / (PassiveDistance - EliminateDistance);
                        closest.Strength -= closest.Strength * scoreRatio;
                    }
                }

                _currentMode = BirdType.None;
            }
        }

        private (int, int) CalculateSpherePoint(in double strength)
        {
            // TODO
            return (
                (int)Math.Round((HigherLatitude - LowerLatitude) * strength + LowerLatitude),
                _state.PlanetRotation
            );
        }

        public void SetMode(int mode)
        {
            _currentMode = (BirdType)mode;
        }
    }
}