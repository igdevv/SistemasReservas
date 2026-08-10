import { useEffect, useState } from "react";
import "./App.css";

import {
  confirmarReserva,
  obtenerReservas,
  type Reserva,
} from "./services/reservasService";

import ReservaCard from "./components/ReservaCard";

function App() {
  const [reservas, setReservas] = useState<Reserva[]>([]);
  const [cargando, setCargando] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    cargarReservas();
  }, []);

  async function cargarReservas() {
    try {
      setError("");

      const data = await obtenerReservas();

      setReservas(data);
    } catch {
      setError("No se pudieron cargar las reservas.");
    } finally {
      setCargando(false);
    }
  }

  async function confirmar(id: number) {
    try {
      await confirmarReserva(id);

      await cargarReservas();
    } catch {
      setError("No se pudo confirmar la reserva.");
    }
  }

  if (cargando) {
    return <p>Cargando reservas...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <main className="app-container">
      <header className="app-header">
        <div>
          <p className="app-eyebrow">Dashboard</p>

          <h1>Sistema de Reservas</h1>

          <p>
            Gestiona tus reservas desde un solo lugar.
          </p>
        </div>

        <button className="primary-button">
          + Nueva reserva
        </button>
      </header>

      {reservas.length === 0 ? (
        <p>No hay reservas.</p>
      ) : (
        <div className="reservas-grid">
          {reservas.map((reserva) => (
            <ReservaCard
              key={reserva.id}
              reserva={reserva}
              onConfirmar={confirmar}
            />
          ))}
        </div>
      )}
    </main>
  );
}

export default App;