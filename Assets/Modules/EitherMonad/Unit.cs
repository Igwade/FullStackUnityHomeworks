using System;

namespace EitherMonad
{
    
    // Если в случае успеха/ошибки никаких данных не нужно возвращать, можно использовать структуру Unit
    [Serializable]
    public struct Unit : IEquatable<Unit>
    {
        static readonly Unit @default = new Unit();
        public static Unit Default => @default;
        public static bool operator ==(Unit first, Unit second) => true;
        public static bool operator !=(Unit first, Unit second) => false;
        public bool Equals(Unit other) => true;
        public override bool Equals(object obj) => obj is Unit;
        public override int GetHashCode() => 0;
        public override string ToString() => "()";
    }
}