public static class DatosSeleccionCombate
{
    public static DatosPersonaje[] equipoP1;
    public static DatosPersonaje[] equipoP2;

    public static bool HaySeleccionCompleta
    {
        get
        {
            return EquipoCompleto(equipoP1) && EquipoCompleto(equipoP2);
        }
    }

    public static void GuardarEquipos(DatosPersonaje[] nuevoEquipoP1, DatosPersonaje[] nuevoEquipoP2)
    {
        equipoP1 = CopiarEquipo(nuevoEquipoP1);
        equipoP2 = CopiarEquipo(nuevoEquipoP2);
    }

    public static void Limpiar()
    {
        equipoP1 = null;
        equipoP2 = null;
    }

    private static DatosPersonaje[] CopiarEquipo(DatosPersonaje[] equipo)
    {
        if (equipo == null)
        {
            return null;
        }

        DatosPersonaje[] copia = new DatosPersonaje[equipo.Length];
        for (int i = 0; i < equipo.Length; i++)
        {
            copia[i] = equipo[i];
        }

        return copia;
    }

    private static bool EquipoCompleto(DatosPersonaje[] equipo)
    {
        if (equipo == null || equipo.Length < 3)
        {
            return false;
        }

        for (int i = 0; i < 3; i++)
        {
            if (equipo[i] == null)
            {
                return false;
            }
        }

        return true;
    }
}
