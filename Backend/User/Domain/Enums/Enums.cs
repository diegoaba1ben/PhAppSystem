using System.ComponentModel;

namespace PhAppUser.Domain.Enums
{
    /// <summary>
    /// Enumerador para los tipos de identificación personal.
    /// </summary>
    public enum TipoId
    {   
        [Description("Cédula")]
        Cedula = 1,
        [Description("Cédula de extranjería")]
        CedulaExtranjeria = 2,
        [Description("Pasaporte")]
        Pasaporte = 3,
        [Description("Tarjeta de Identidad")]
        TarjetaIdentidad = 4
    }

    /// <summary>
    /// Enumerador para los tipos de usuario en el sistema 
    /// </summary>
    public enum TipoCuenta
    {
        [Description("Usuario común")]
        Comun = 1,
        [Description("Representante legal")]
        RepLegal = 2,
        [Description("Contador")]
        Contador = 3
    }

    /// <summary>
    /// Enumerador para el tipo de contrato.
    /// </summary>
    public enum TipoContrato
    {
        [Description("Tipo de contrato del usuario")]
        Empleado = 1,
        PrestadorDeServicios = 2
    }

    /// <summary>
    /// Enumerador para los tipos de identificación tributaria.
    /// </summary>
    public enum TipoIdTrib
    {
        [Description("NIT")]
        NIT = 1,
        [Description("RUT")]
        RUT = 2,
        [Description("No Aplicable")]
        NoAplica = 3
    }

    /// <summary>
    /// Enumerador para los tipos de afiliación a la seguridad social.
    /// </summary>
    public enum Afiliacion
    {
        [Description("Afiliación completa")]
        Completa = 1,

        [Description("Creación parcial")]
        Parcial = 2
    }
}