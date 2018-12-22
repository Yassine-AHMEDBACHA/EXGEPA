using System;

namespace CORESI.Data
{
    public class ObjectValidator<T>
    {
        public Predicate<T> Predicate { get; set; }
        public string ErrorMessage { get; set; }
    }
}