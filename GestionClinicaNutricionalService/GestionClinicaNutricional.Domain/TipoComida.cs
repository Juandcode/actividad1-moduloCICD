namespace GestionClinicaNutricional.Domain
{
    public record TipoComida
    {
        public string Nombre { get; init; }
        public string Descripcion { get; init; }
        public CategoriaComida Categoria { get; init; }

        public TipoComida(string nombre, string descripcion, CategoriaComida categoria)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío");

            if (nombre.Length > 100)
                throw new ArgumentException("El nombre no puede superar 100 caracteres");

            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("La descripción no puede estar vacía");

            if (descripcion.Length > 500)
                throw new ArgumentException("La descripción no puede superar 500 caracteres");

            if (!Enum.IsDefined(typeof(CategoriaComida), categoria))
                throw new ArgumentException("La categoría no es válida");

            Nombre = nombre;
            Descripcion = descripcion;
            Categoria = categoria;
        }
    }
}