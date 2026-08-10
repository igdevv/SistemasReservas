import type { Reserva } from "../services/reservasService";

interface Props {
  reserva: Reserva;
  onConfirmar: (id: number) => Promise<void>;
}

function formatearFecha(fecha: string) {
  return new Intl.DateTimeFormat("es-DO", {
    dateStyle: "medium",
    timeStyle: "short",
  }).format(new Date(fecha));
}

export default function ReservaCard({
  reserva,
  onConfirmar,
}: Props) {
  const estaPendiente = reserva.estado === "Pendiente";

  return (
    <article className="reserva-card">
      <div className="reserva-card__header">
        <h2>Reserva #{reserva.id}</h2>

        <span className={`estado estado--${reserva.estado.toLowerCase()}`}>
          {reserva.estado}
        </span>
      </div>

      <div className="reserva-card__info">
        <p>
          <strong>Inicio:</strong> {formatearFecha(reserva.fechaInicio)}
        </p>

        <p>
          <strong>Fin:</strong> {formatearFecha(reserva.fechaFin)}
        </p>
      </div>

      <div className="reserva-card__actions">
        <button
          onClick={() => onConfirmar(reserva.id)}
          disabled={!estaPendiente}
        >
          {estaPendiente ? "Confirmar" : "Ya confirmada"}
        </button>

        <button>
          Cancelar
        </button>
      </div>
    </article>
  );
}