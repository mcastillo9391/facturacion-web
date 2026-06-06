export default function PendienteForm({
  clientes,
  clienteId,
  setClienteId,
  crearPendiente,
}) {
  return (
    <div>
      <select
        value={clienteId}
        onChange={(e) =>
          setClienteId(
            e.target.value
          )
        }
      >
        <option value="">
          Seleccione cliente
        </option>

        {clientes.map((c) => (
          <option
            key={c.idCli}
            value={c.idCli}
          >
            {c.nombre}
          </option>
        ))}
      </select>

      <button
        onClick={
          crearPendiente
        }
      >
        Nuevo Pendiente
      </button>
    </div>
  );
}