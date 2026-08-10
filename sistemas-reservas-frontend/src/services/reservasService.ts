export interface Reserva{
    id: number;
    fechaInicio: string;
    fechaFin: string;
    estado: string;
}

export async function obtenerReservas(): Promise<Reserva[]>{
    const response = await fetch("https://localhost:7179/api/Reservas");

    if(!response.ok){
        throw new Error("No se pudieron cargar las reservas.");
    }

    return await response.json();
}

export async function confirmarReserva(id: number): Promise<void> {
  const response = await fetch(
    `https://localhost:7179/api/Reservas/${id}/confirmar`,
    {
      method: "PATCH",
    }
  );

  if (!response.ok) {
    throw new Error("No se pudo confirmar la reserva.");
  }
}